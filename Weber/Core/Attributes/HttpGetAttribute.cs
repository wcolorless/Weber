using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Attributes
{
    /// <summary>
    /// Configures the controller method as an HttpGet operation
    /// </summary>
    public class HttpGetAttribute : Attribute
    {
        public static bool IsHttpGet(object attribute)
        {
            try
            {
                return attribute is HttpGetAttribute;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
