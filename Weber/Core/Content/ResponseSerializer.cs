using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Weber.Core.Results;

namespace Weber.Core.Content
{
    public class ResponseSerializer
    {
        /// <summary>
        /// Serializes the result of a controller method
        /// </summary>
        /// <param name="methodResult"></param>
        /// <returns></returns>
        public static (bool IsOk, byte[] Data) GetData(IMethodResult methodResult)
        {
            try
            {
                var content = methodResult.Content();
                if (content is string)
                {
                    var data = Encoding.UTF8.GetBytes((string) content);
                    return (true, data);
                }

                if (content is int)
                {
                    return (true, Encoding.UTF8.GetBytes(((int)content).ToString()));
                }

                if (content is object)
                {
                    var json = JsonConvert.SerializeObject(content);
                    var data = Encoding.UTF8.GetBytes(json);
                    return (true, data);
                }
            }
            catch (Exception e)
            {
                throw new Exception($"ResponseSerializer GetData Error: {e.Message}");
            }
            return (false, null);
        }
    }
}
