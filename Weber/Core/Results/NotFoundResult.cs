using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Results
{
    /// <summary>
    /// Represents Resource Not Found 404
    /// </summary>
    public class NotFoundResult : IMethodResult
    {
        private readonly string _content = "Resource Not Found";
        public NotFoundResult(string content = "")
        {
            _content += string.IsNullOrEmpty(content) ? "" : $": {content}";
        }

        public ResultType StatusCode()
        {
            return ResultType.NotFound;
        }

        public object Content()
        {
            return _content;
        }
    }
}
