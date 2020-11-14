using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Weber.Core.Content
{
    public class RequestContentLoader
    {
        /// <summary>
        /// Reads the content of the request into an array
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static byte[] ReadAsByteArray(HttpListenerRequest request)
        {
            try
            {
                byte[] buffer;
                var stream = request.InputStream;
                try
                {
                    var length = (int)request.ContentLength64;
                    buffer = new byte[length];
                    int count;
                    int sum = 0;
                    while ((count = stream.Read(buffer, sum, length - sum)) > 0)
                    {
                        sum += count;
                    }
                }
                finally
                {
                    stream.Close();
                }

                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception($"RequestContentLoader ReadAsByteArray Error: {e.Message}");
            }
        }
    }
}
