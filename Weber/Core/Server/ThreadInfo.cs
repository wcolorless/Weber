using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Weber.Core.Controllers;

namespace Weber.Core.Server
{
    public class ThreadInfo
    {
        public DateTime BornTime { get; set; }
        public Thread Self { get; set; }
        public string ControllerName { get; set; }
        public RequestQueue Queue { get; set; }
        public Dictionary<string, Thread> Threads { get; set; }
        public List<ControllerInfo> Infos { get; set; }
    }
}
