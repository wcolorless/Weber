using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Weber.Core.Server
{
    public class RequestQueue
    {
        public string ControllerName { get; set; }
        public Queue<HttpListenerContext> Requests { get; set; } = new Queue<HttpListenerContext>();
    }
}
