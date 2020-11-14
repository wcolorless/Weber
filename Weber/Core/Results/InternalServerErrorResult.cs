using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Results
{
    /// <summary>
    /// Represents Internal Server Error 500
    /// </summary>
    public class InternalServerErrorResult : IMethodResult
    {
        private readonly string _content = "Internal Server Error";
        public InternalServerErrorResult(string content = "")
        {
            _content += string.IsNullOrEmpty(content) ? "" : $": {content}";
        }

        public ResultType StatusCode()
        {
            return ResultType.InternalServerError;
        }

        public object Content()
        {
            return _content;
        }
    }
}
