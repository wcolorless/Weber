using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Results
{
    /// <summary>
    /// Represents Ok 200
    /// </summary>
    public class OkResult : IMethodResult
    {
        private object _result;
        private OkResult(object result)
        {
            _result = result;
        }
        public static OkResult Create(object result)
        {
            return new OkResult(result);
        }

        public ResultType StatusCode()
        {
            return ResultType.Ok;
        }

        public object Content()
        {
            return _result;
        }
    }
}
