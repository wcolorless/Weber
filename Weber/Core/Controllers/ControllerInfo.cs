using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weber.Core.Attributes;
using Weber.Core.Routes;

namespace Weber.Core.Controllers
{
    public class ControllerInfo
    {
         public string Name { get; set; }
         public List<ControllerMethodInfo> Methods { get; set; } = new List<ControllerMethodInfo>();
         public List<RouteInfo> Routes { get; set; } = new List<RouteInfo>();

        /// <summary>
        /// Retrieves information about a controller
        /// </summary>
        /// <param name="controller"></param>
        /// <returns></returns>
        public static ControllerInfo GetControllerInfo(Type controller)
         {
             try
             {
                 var result = new ControllerInfo();
                 var controllerName = controller.Name.ToLower();
                 var attributes = Attribute.GetCustomAttributes(controller);
                 var controllerAttribute = attributes.First(x => x.GetType() == typeof(ControllerAttribute)) as ControllerAttribute;
                 var controllerBaseRoute = result.Name = string.IsNullOrWhiteSpace(controllerAttribute.Name)
                     ? controllerName
                     : controllerAttribute.Name;
                 var methods = controller.GetMethods().ToList();
                 methods = methods.Where(x => x.IsPublic).ToList();
                 foreach (var method in methods)
                 {
                     var methodName = method.Name.ToLower();
                     var methodAttributes = method.GetCustomAttributes(false);
                     var currentTypeAttribute = RequestTypeHelper.GetRequestType(controllerName, methodName, methodAttributes);
                     if (currentTypeAttribute.Item1 != RequestType.None)
                     { 
                        var methodRouteInfo = MethodRouteHelper.GetRouteInfo(controllerName, method.Name, methodAttributes);
                        if (methodRouteInfo.Route == null)
                        {
                            methodRouteInfo.Route = new RouteInfo
                            {
                                Controller = controllerName,
                                MethodName = method.Name,
                                RawRoute = string.Empty,
                                Parts = new List<RoutePart>
                                {
                                    new RoutePart
                                    {
                                        Index = 0,
                                        PartType = RoutePartType.Text,
                                        PartName = $"{controllerName}"
                                    }
                                }
                            };
                        }
                        var methodInfo = new ControllerMethodInfo
                        {
                            Name = methodName,
                            InitialName = method.Name,
                            RequestType = currentTypeAttribute.Item1,
                            Route = methodRouteInfo.Route,
                            MethodInfo = method
                        };
                        result.Methods.Add(methodInfo);
                        result.Routes.Add(methodRouteInfo.Route);
                        continue;
                     }
                 }

                 return result;
             }
             catch (Exception e)
             {
                 throw new Exception($"GetControllerInfo Error: {e.Message}");
             }
         }
    }
}
