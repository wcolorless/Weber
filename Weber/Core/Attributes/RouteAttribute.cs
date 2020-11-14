using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Attributes
{
    /// <summary>
    /// Configures the path to the controller method
    /// </summary>
    public class RouteAttribute : Attribute
    {
        public string Route { get; set; }
        public RouteAttribute(string route = "")
        {
            route = route.Replace(" ", "");
            Route = route;
        }
    }
}
