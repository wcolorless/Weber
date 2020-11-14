using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TinyDI;
using Weber.Core.Container;
using Weber.Core.Content;
using Weber.Core.Controllers;
using Weber.Core.Host;
using Weber.Core.Results;
using Weber.Core.Routes;

namespace Weber.Core.Server
{
    public class HandlerDispatcher
    {
        private readonly Dictionary<string, RequestQueue> _queues = new Dictionary<string, RequestQueue>();
        private readonly Dictionary<string, Thread> _threads = new Dictionary<string, Thread>();
        private readonly Dictionary<string, Type> _controllers;
        private readonly TinyDependencyInjection _tinyDependencyInjection;
        private readonly List<ControllerInfo> _controllerInfos;
        private readonly List<RouteInfo> _routes;
        public HandlerDispatcher(Dictionary<string, Type> controllers, TinyDependencyInjection tinyDependencyInjection, List<ControllerInfo> controllerInfos, List<RouteInfo> routes)
        {
            _controllers = controllers;
            _tinyDependencyInjection = tinyDependencyInjection;
            _controllerInfos = controllerInfos;
            _routes = routes;
        }

        public void AcceptRequest(HttpListenerContext context)
        {
            try
            {
                var request = context.Request;
                var response = context.Response;
                var requestType = Enum.Parse<RequestType>(context.Request.HttpMethod);
                var urlParts = request.RawUrl.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);
                if (!urlParts.Any())
                {
                    ResultHelper.SendResult(response, new NotFoundResult());
                    return;
                }
                var controllerName = urlParts.FirstOrDefault();
                if (requestType == RequestType.GET)
                {
                    var cutStart = controllerName.IndexOf("?", StringComparison.Ordinal);
                    if (cutStart != -1) controllerName = controllerName.Substring(0, cutStart);
                }
                if (!_controllers.ContainsKey(controllerName))
                {
                    ResultHelper.SendResult(response, new NotFoundResult("No controller found with this name"));
                    return;
                }
                Console.WriteLine($"New request: time: {DateTime.UtcNow}; type: {request.HttpMethod}");
                // Thread check
                var threadExist = _threads.ContainsKey(controllerName);
                // Queue check
                var queueExist = _queues.ContainsKey(controllerName);
                RequestQueue queue;
                if (queueExist)
                {
                    queue = _queues[$"{controllerName}"];
                }
                else
                {
                    queue = new RequestQueue { ControllerName = controllerName };
                    _queues.Add(controllerName, queue);
                }
                queue.Requests.Enqueue(context);

                if (!threadExist)
                {
                    var newThread = new Thread(RequestHandler); // Create new Thread
                    _threads.Add(controllerName, newThread);
                    newThread.Start(new ThreadInfo
                    {
                        BornTime = DateTime.UtcNow,
                        Self = newThread,
                        ControllerName = controllerName,
                        Queue = queue,
                        Infos = _controllerInfos,
                        Threads = _threads
                    });
                }
            }
            catch (Exception e)
            {
                throw new Exception($"HandlerDispatcher AcceptRequest Error: {e.Message}");
            }
        }

        private void RequestHandler(object input)
        {
            var threadInfo = input as ThreadInfo;
            var controllerType = _controllers[threadInfo.ControllerName];
            var controller = CreateControllerInstance.Create(controllerType, _tinyDependencyInjection);
            var controllerInfo = threadInfo.Infos.FirstOrDefault(x => x.Name == threadInfo.ControllerName);
            if (controller == null || controllerInfo == null)
            {
                Console.WriteLine($"Weber Error: can't create controller object");
                return;
            }
            while (true)
            {
                var requestExist = threadInfo.Queue.Requests.Any();
                if (!requestExist && (DateTime.UtcNow > threadInfo.BornTime + TimeSpan.FromSeconds(WeberConstants.ThreadLifetime)))
                {
                    threadInfo.Threads.Remove(threadInfo.ControllerName);
                    break;
                }
                if (!requestExist)
                {
                    Thread.Sleep(10);
                    continue;
                }
                var context = threadInfo.Queue.Requests.Dequeue();
                var request = context.Request;
                var response = context.Response;
                var url = UrlHelper.ClearAndNormalize(request.RawUrl);
                try
                {
                    // Handling
                    var requestType = Enum.Parse<RequestType>(request.HttpMethod);
                    var bodyData = RequestContentLoader.ReadAsByteArray(context.Request);
                    var controllerMethodInfo = MethodRouteHelper.MatchPath(request, threadInfo.ControllerName, _controllerInfos);
                    if (controllerMethodInfo == null)
                    {
                        ResultHelper.SendResult(response, new NotFoundResult("Route not found"));
                        return;
                    }
                    var parametersDataForMethod = RequestAnalyzer.GetParameterData(requestType, bodyData, url, controllerMethodInfo);
                    var taskFromMethod = controllerMethodInfo.MethodInfo.Invoke(controller, parametersDataForMethod.ToArray()) as Task<IMethodResult>;
                    var result = taskFromMethod.Result;
                    response.StatusCode = (int)result.StatusCode();
                    var stream = response.OutputStream;
                    var serializationResult = ResponseSerializer.GetData(result);
                    if (serializationResult.IsOk)
                    {
                        stream.WriteAsync(serializationResult.Data);
                        stream.Close();
                    }
                    else
                    {
                        ResultHelper.SendResult(response, new InternalServerErrorResult("Result serialize error"));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Internal Server Error: {e.Message}\n{e.StackTrace}");
                    ResultHelper.SendResult(response, new InternalServerErrorResult($": {e.Message}\n{e.StackTrace}"));
                }
            }
        }
    }
}
