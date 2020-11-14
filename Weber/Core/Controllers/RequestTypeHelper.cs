using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Weber.Core.Attributes;

namespace Weber.Core.Controllers
{
    public class RequestTypeHelper
    {
        /// <summary>
        /// Defines the request type of a controller method
        /// </summary>
        /// <param name="controllerName"></param>
        /// <param name="methodName"></param>
        /// <param name="methodAttributes"></param>
        /// <returns></returns>
        public static (RequestType, object) GetRequestType(string controllerName, string methodName, object[] methodAttributes)
        {

            try
            {
                var httpGetAttrCount = methodAttributes.Count(x => x.GetType() == typeof(HttpGetAttribute));
                var httpPostAttrCount = methodAttributes.Count(x => x.GetType() == typeof(HttpPostAttribute));
                var httpPutAttrCount = methodAttributes.Count(x => x.GetType() == typeof(HttpPutAttribute));
                var httpDeleteAttrCount = methodAttributes.Count(x => x.GetType() == typeof(HttpDeleteAttribute));
                if ((httpGetAttrCount + httpPostAttrCount + httpPutAttrCount + httpDeleteAttrCount) > 1) throw new Exception($"Weber controller Error: Controller: {controllerName} => Method: {methodName} => Cannot have more than one request type attribute");
                foreach (var attribute in methodAttributes)
                {
                    if (HttpGetAttribute.IsHttpGet(attribute))
                    {
                        return (RequestType.GET, attribute);
                    }

                    if (HttpPostAttribute.IsHttpPost(attribute))
                    {
                        return (RequestType.POST, attribute);
                    }

                    if (HttpPutAttribute.IsHttpPut(attribute))
                    {
                        return (RequestType.PUT, attribute);
                    }

                    if (HttpDeleteAttribute.IsHttpDelete(attribute))
                    {
                        return (RequestType.DELETE, attribute);
                    }
                }


            }
            catch (Exception e)
            {
                throw new Exception($"RequestTypeHelper GetRequestType Error: {e.Message}");
            }
            return (RequestType.None, null);
        }
    }
}
