using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Weber.Core.Attributes;
using Weber.Core.Routes;

namespace Weber.Core.Controllers
{
    public class MethodRouteHelper
    {
        public static (RouteInfo Route, object Attribute) GetRouteInfo(string controllerName, string methodName, object[] attributes)
        {
            try
            {
                var attr= attributes.FirstOrDefault(x => x.GetType() == typeof(RouteAttribute));
                if (attr != null)
                {
                    var routeAttr = attr as RouteAttribute;
                    if (routeAttr.Route.StartsWith("/")) routeAttr.Route = routeAttr.Route.TrimStart(new []{'/', '\\'});
                    var stringRoute = routeAttr.Route;
                    var newRoute = new RouteInfo()
                    {
                        Controller = controllerName, 
                        MethodName = methodName, 
                        RawRoute = $"{controllerName}/{methodName}/{stringRoute}"
                    };
                    var parts = GetRouteParts($"{controllerName}/{stringRoute}");
                    newRoute.Parts.AddRange(parts);
                    
                    return (newRoute, routeAttr);
                }
                return (null, null);
            }
            catch (Exception e)
            {
                throw new Exception($"MethodRouteHelper GetRouteInfo Error: {e.Message}");
            }
        }

        private static List<RoutePart> GetRouteParts(string stringRoute)
        {
            try
            {
                stringRoute = stringRoute.Replace("//", "/");
                stringRoute = stringRoute.Replace("\\", "/");
                var stringParts = stringRoute.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
                var parts = new List<RoutePart>();
                for (var i = 0; i < stringParts.Length; i++)
                {
                    var stringPart = stringParts[i];
                    var part = new RoutePart()
                    {
                        Index = i,
                        PartType = (stringPart.StartsWith("{") && stringPart.EndsWith("}")) ? RoutePartType.Parameter : RoutePartType.Text,
                        PartName = stringPart.Contains("{") ? stringPart.Replace("{", "").Replace("}", "") : stringPart
                    };
                    parts.Add(part);
                }
                return parts;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Searches for a controller method matching the request
        /// </summary>
        /// <param name="request"></param>
        /// <param name="controllerName"></param>
        /// <param name="controllers"></param>
        /// <returns></returns>
        public static ControllerMethodInfo MatchPath(HttpListenerRequest request, string controllerName, List<ControllerInfo> controllers)
        {
            try
            {
                var url = UrlHelper.RemoveQueryParams(UrlHelper.ClearAndNormalize(request.RawUrl));
                var controller = controllers.FirstOrDefault(x => x.Name == controllerName);
                var predictParts = GetRouteParts(url);
                var requestType = Enum.Parse<RequestType>(request.HttpMethod);
                var methods = controller.Methods.Where(x => x.RequestType == requestType).ToList();
                foreach (var methodInfo in methods)
                {
                    var route = methodInfo.Route;
                    var compare = true;
                    if (route.Parts.Count == predictParts.Count)
                    {
                        for (var i = 0; i < route.Parts.Count; i++)
                        {
                            var partFromInner = route.Parts[i];
                            var partFromUrl = predictParts[i];
                            if (partFromInner.PartType == RoutePartType.Text)
                            {
                                if (partFromInner.PartName != partFromUrl.PartName)
                                {
                                    compare = false;
                                    break;
                                }
                            } 
                        }
                    }
                    else continue;
                    if (compare) return methodInfo;
                }
                throw new Exception($"Route not found: {url}");
            }
            catch (Exception e)
            {
                throw new Exception($"MethodRouteHelper MatchPath Error: {e.Message}");
            }
        }
    }
}
