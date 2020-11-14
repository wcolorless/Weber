using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TinyDI;
using Weber.Core.Controllers;
using Weber.Core.Routes;

namespace Weber.Core.Server
{
    public class HttpServer
    {
        private readonly Dictionary<string, Type> _controllers;
        private readonly List<ControllerInfo> _controllerInfos;
        private readonly List<RouteInfo> _routes;
        private readonly HttpListener _httpListener;
        public bool IsActive { get; set; }
        private HandlerDispatcher _handlerDispatcher;
        private TinyDependencyInjection _tinyDependencyInjection;
        public HttpServer(string host, Dictionary<string, Type> controllers, TinyDependencyInjection tinyDependencyInjection, List<ControllerInfo> controllerInfos, List<RouteInfo> routes)
        {
            _controllers = controllers;
            _tinyDependencyInjection = tinyDependencyInjection;
            _controllerInfos = controllerInfos;
            _routes = routes;
            _httpListener = new HttpListener();
            _httpListener.Prefixes.Add(host);
            var stringRoutes = RouteScriber.GetControllerRoutes(_controllerInfos);
            foreach (var route in stringRoutes)
            {
                var prefixRoute = route.EndsWith("/") ? route.TrimEnd(new []{'/', '\\'}) : route;
                var prefix = $"{host}{prefixRoute}/";
                _httpListener.Prefixes.Add(prefix);
            }
            _handlerDispatcher = new HandlerDispatcher(_controllers, _tinyDependencyInjection, _controllerInfos, _routes);
        }

        public void StartServer()
        {
            try
            {
                Task.Run(() =>
                {
                    Console.WriteLine("Weber server start-up");
                    IsActive = true;
                    while (IsActive)
                    {
                        _httpListener.Start();
                        var context = _httpListener.BeginGetContext(ServerCycle, _httpListener);
                        context.AsyncWaitHandle.WaitOne();
                    }
                });
            }
            catch (Exception e)
            {
                throw new Exception($"HttpTransport StartServer Error: {e.Message}");
            }
        }

        public void StopServer()
        {
            try
            {
                Console.WriteLine("Weber server shutdown");
                IsActive = false;
                _httpListener?.Stop();
            }
            catch (Exception e)
            {
                throw new Exception($"HttpTransport StopServer Error: {e.Message}");
            }
        }

        private async void ServerCycle(IAsyncResult result)
        {
            var state = result.AsyncState;
            try
            {
                if (state == null) return;
                var listener = state as HttpListener;
                var context = listener.EndGetContext(result);
                _handlerDispatcher.AcceptRequest(context);
            }
            catch (HttpListenerException httpListenerException)
            {
                if(IsActive == false) return;
                throw new Exception($"HttpTransport ServerCycle Error: {httpListenerException.Message}");
            }
            catch (Exception e)
            {
                throw new Exception($"HttpTransport ServerCycle Error: {e.Message}");
            }
        }
    }
}
