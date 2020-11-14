using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Weber.Core.Routes;

namespace Weber.Core.Controllers
{
    /// <summary>
    /// Contains information about a controller method
    /// </summary>
    public class ControllerMethodInfo
    {
        public string Name { get; set; }
        public string InitialName { get; set; }
        public RequestType RequestType { get; set; }
        public RouteInfo Route { get; set; }
        public MethodInfo MethodInfo { get; set; }

    }
}
