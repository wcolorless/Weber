using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Attributes
{
    /// <summary>
    /// Configures the controller method as an HttpPost operation
    /// </summary>
    public class HttpPostAttribute : Attribute
    {
        public static bool IsHttpPost(object attribute)
        {
            try
            {
                return attribute is HttpPostAttribute;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
