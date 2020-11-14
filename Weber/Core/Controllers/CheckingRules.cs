using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weber.Core.Routes;

namespace Weber.Core.Controllers
{
    public class CheckingRules
    {
        /// <summary>
        /// Warns about errors in controller method paths
        /// </summary>
        /// <param name="controllers"></param>
        public static void CheckRoutes(List<ControllerInfo> controllers)
        {
            try
            {
                foreach (var controller in controllers)
                {
                    var methods = controller.Methods;
                    foreach (var method in methods)
                    {
                        var searchMethods = methods.Where(x => x != method);
                        foreach (var smethod in searchMethods)
                        {
                            if (RouteInfo.Equal(method.Route, smethod.Route) && method.RequestType == smethod.RequestType)
                            {
                                throw new Exception($"Route duplicate error: In controller => {controller.Name}, Method => {method.InitialName}  ");
                            }
                        }
                        
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception($"CheckingRules CheckRoutes Error: {e.Message}");
            }
        }
    }
}
