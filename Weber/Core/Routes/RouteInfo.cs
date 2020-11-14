using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Routes
{
    /// <summary>
    /// Represents path information
    /// </summary>
    public class RouteInfo
    {
        public string Controller { get; set; }
        public string MethodName { get; set; }
        public string RawRoute { get; set; }
        public List<RoutePart> Parts { get; set; } = new List<RoutePart>();

        public string GetStringRoute()
        {
            return $"{RawRoute}";
        }

        public static bool Equal(RouteInfo a, RouteInfo b)
        {
            var result = true;
            try
            {
                if (a.Controller != b.Controller) return false;
                if (a.Parts.Count != b.Parts.Count) return false;
                for (var i = 0; i < a.Parts.Count; i++)
                {
                    if (!RoutePart.Equal(a.Parts[i], b.Parts[i])) return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception($"RouteInfo Equal Error: {e.Message}");
            }

            return result;
        }
    }
}
