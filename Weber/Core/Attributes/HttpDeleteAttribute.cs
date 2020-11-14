using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Attributes
{
    /// <summary>
    /// Configures the controller method as an HttpDelete operation
    /// </summary>
    public class HttpDeleteAttribute : Attribute
    {
        public static bool IsHttpDelete(object attribute)
        {
            try
            {
                return attribute is HttpDeleteAttribute;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
