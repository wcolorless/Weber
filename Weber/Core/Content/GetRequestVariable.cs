using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Content
{
    /// <summary>
    /// Represents the value of a variable from a Get request
    /// </summary>
    public class GetRequestVariable
    {
        public string Raw { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public GetRequestVariable(string raw)
        {
            Raw = raw;
        }
    }
}
