using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Results
{
    /// <summary>
    /// Represents Bad Request 400
    /// </summary>
    public class BadResult : IMethodResult
    {
        private readonly object _result;
        private BadResult(object result)
        {
            _result = result;
        }
        public static BadResult Create(object result)
        {
            return new BadResult(result);
        }

        public ResultType StatusCode()
        {
            return ResultType.BadRequest;
        }

        public object Content()
        {
            return _result;
        }
    }
}
