using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace XUnitTestCommon.RestRequests.Interfaces
{
    public interface IResponse
    {
        HttpStatusCode StatusCode { get; }
        string Content { get; }
        JObject JObject { get; }
        IValidate Validate { get; }
    }

    public interface IResponse<T> : IResponse
    {
        T ResponseObject { get; }
        T GetResponseObject();
        new IValidate<T> Validate { get; }
    }
}
