using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Weber.Core.Attributes;

namespace Weber.Core.Controllers
{
    public class RouteScriber
    {
        public static List<string> GetControllerRoutes(List<ControllerInfo> controllerInfos)
        {
            try
            {
                var result = new List<string>();
                result.AddRange(controllerInfos.SelectMany(x => x.Routes.Select(r => r.RawRoute)));
                result = result.Distinct().ToList();
                result = result.Select(x => x.Replace("{", "").Replace("}", "")).ToList();
                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"RouteScriber GetControllerRoutes Error: {e.Message}");
            }
        }
    }
}
