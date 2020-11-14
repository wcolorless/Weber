using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = true)]
    public class ControllerAttribute : Attribute
    {
        public string Name { get; private set; }
        public ControllerAttribute(string name = "")
        {
            Name = name;
        }
    }
}
