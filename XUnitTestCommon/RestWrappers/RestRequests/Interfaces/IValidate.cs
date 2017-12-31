using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace XUnitTestCommon.RestRequests.Interfaces
{
    public interface IValidate
    {
        IResponse StatusCode(HttpStatusCode code, string message = null);
    }

    public interface IValidate<T>
    {
        IResponse<T> StatusCode(HttpStatusCode code, string message = null);
    }
}
