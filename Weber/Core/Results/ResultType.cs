using System;
using System.Collections.Generic;
using System.Text;

namespace Weber.Core.Results
{
    public enum ResultType
    {
        None,
        Ok = 200,
        BadRequest = 400,
        NotFound = 404,
        InternalServerError = 500
    }
}
