using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Results
{
    public interface IMethodResult
    {
        ResultType StatusCode();
        object Content();
    }
}
