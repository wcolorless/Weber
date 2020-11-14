using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Weber.Core.Controllers;
using Weber.Core.Inputs;
using Weber.Core.Routes;

namespace Weber.Core.Content
{
    public class RequestAnalyzer
    {
        /// <summary>
        /// Retrieves data from a request to pass to a controller method
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="body"></param>
        /// <param name="url"></param>
        /// <param name="controllerMethodInfo"></param>
        /// <returns></returns>
        public static List<object> GetParameterData(RequestType requestType, byte[] body, string url, ControllerMethodInfo controllerMethodInfo)
        {
            var parametersInfo = new List<(Attribute Attribute, ParameterInfo Parameter)>();
            var result = new List<object>();
            try
            {
                var parameters = controllerMethodInfo.MethodInfo.GetParameters();
                foreach (var parameter in parameters)
                {
                   var attribute = parameter.GetCustomAttributes().FirstOrDefault();
                   parametersInfo.Add((attribute, parameter));
                }
                switch (requestType)
                {
                    case RequestType.None:
                        throw new Exception($"RequestAnalyzer GetParameterData Error: RequestType.None for => url:{url}");
                    case RequestType.GET:
                    {
                        var variables = new List<GetRequestVariable>();
                        foreach (var parameterInfo in parametersInfo)
                        {
                            var attr = parameterInfo.Attribute;
                            // Extract from query
                            if (attr is FromQueryAttribute)
                            {
                                if (url.Contains("?"))
                                {
                                    var d = url.IndexOf('?');
                                    var clearUri = url.Substring(d + 1, (url.Length - d) - 1);
                                    var urlParts = clearUri.Split(new[] { '&' });
                                    variables.AddRange(urlParts.AsParallel().Select(x =>
                                    {
                                        var parts = x.Split(new[] { '=' });
                                        return new GetRequestVariable(x)
                                        {
                                            Name = parts[0],
                                            Value = parts[1]
                                        };
                                    }));
                                }
                                else variables.Add(null);
                            }

                            // Extract from route
                            if (attr is FromRouteAttribute)
                            {
                                var routeParts = controllerMethodInfo.Route.Parts;
                                var urlParts = UrlHelper.RemoveQueryParams(url).Split("/", StringSplitOptions.RemoveEmptyEntries);
                                foreach (var part in routeParts)
                                {
                                    if (part.PartType == RoutePartType.Parameter)
                                    {
                                        variables.Add(new GetRequestVariable(urlParts[part.Index])
                                        {
                                            Name = part.PartName,
                                            Value = urlParts[part.Index]
                                        });
                                    }
                                }
                            }
                        }

                        foreach (var variable in parametersInfo)
                        {
                            var name = variable.Parameter.Name;
                            var type = variable.Parameter.ParameterType;
                            var urlVariable = variables.FirstOrDefault(x => x.Name == name);
                            var varObj = Convert.ChangeType(urlVariable.Value, type);
                            result.Add(varObj);
                        }
                    } break;
                    case RequestType.POST:
                    {
                        var json = Encoding.UTF8.GetString(body);
                        var parameter = parameters.FirstOrDefault(x =>
                        {
                            return x.GetCustomAttributes().ToList().Any(s => s.GetType() == typeof(FromBodyAttribute));
                        });
                        var type = parameter.ParameterType;
                        var obj = JsonConvert.DeserializeObject(json, type);
                        result.Add(obj);
                    } break;
                    case RequestType.PUT:
                    {
                        if (body != null && body.Length > 0)
                        {
                            var json = Encoding.UTF8.GetString(body);
                            var parameter = parameters.FirstOrDefault(x =>
                            {
                                return x.GetCustomAttributes().ToList().Any(s => s.GetType() == typeof(FromBodyAttribute));
                            });
                            var type = parameter.ParameterType;
                            var obj = JsonConvert.DeserializeObject(json, type);
                            result.Add(obj);
                        }
                    } break;
                    case RequestType.DELETE:
                    {
                        var variables = new List<GetRequestVariable>();
                        foreach (var parameterInfo in parametersInfo)
                        {
                            var attr = parameterInfo.Attribute;
                            // Extract from route
                            if (attr is FromRouteAttribute)
                            {
                                var routeParts = controllerMethodInfo.Route.Parts;
                                var urlParts = UrlHelper.RemoveQueryParams(url).Split("/", StringSplitOptions.RemoveEmptyEntries);
                                foreach (var part in routeParts)
                                {
                                    if (part.PartType == RoutePartType.Parameter)
                                    {
                                        variables.Add(new GetRequestVariable(urlParts[part.Index])
                                        {
                                            Name = part.PartName,
                                            Value = urlParts[part.Index]
                                        });
                                    }
                                }
                            }
                        }

                        foreach (var variable in parametersInfo)
                        {
                            var name = variable.Parameter.Name;
                            var type = variable.Parameter.ParameterType;
                            var urlVariable = variables.FirstOrDefault(x => x.Name == name);
                            var varObj = Convert.ChangeType(urlVariable.Value, type);
                            result.Add(varObj);
                        }
                    } break;
                }
                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"RequestAnalyzer GetParameterData Error: {e.Message}");
            }
        }
    }
}
