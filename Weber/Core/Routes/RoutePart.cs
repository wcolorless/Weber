using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Routes
{
    /// <summary>
    /// Represents information about a path element
    /// </summary>
    public class RoutePart
    {
        public RoutePartType PartType { get; set; }
        public int Index { get; set; }
        public string PartName { get; set; }

        public static bool Equal(RoutePart a, RoutePart b)
        {
            var result = true;
            try
            {
                if (a.PartType != b.PartType) return false;
                if (a.Index != b.Index) return false;
                if (a.PartName != b.PartName) return false;
            }
            catch (Exception e)
            {
                throw new Exception($"RoutePart Equal Error: {e.Message}");
            }

            return result;
        }
    }
}
