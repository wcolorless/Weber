using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Weber.Core.Content;
using Weber.Core.Controllers;

namespace Weber.Core.Results
{
    /// <summary>
    /// Sends the result to the client
    /// </summary>
    public class ResultHelper
    {
        public static void SendResult(HttpListenerResponse response, IMethodResult result)
        {
            try
            {
                var outputStream = response.OutputStream;
                response.StatusCode = (int)result.StatusCode();
                var serializeResult = ResponseSerializer.GetData(result);
                if (serializeResult.IsOk)
                {
                    outputStream.Write(serializeResult.Data);
                    outputStream.Close();
                }
                else
                {
                    outputStream.Close();
                }
            }
            catch (Exception e)
            {
                throw new Exception($"ResultHelper SendResult Error: {e.Message}");
            }
        }
    }
}
