using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Attributes
{
    /// <summary>
    /// Configures the controller method as an HttpPut operation
    /// </summary>
    public class HttpPutAttribute : Attribute
    {
        public static bool IsHttpPut(object attribute)
        {
            try
            {
                return attribute is HttpPutAttribute;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
