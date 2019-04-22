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
        IList<RestSharp.Parameter> Headers { get; }
        List<RestSharp.Parameter> Cookies { get; }
        Uri ResponseURI { get; set; }
    }

    public interface IResponse<T> : IResponse
    {
        T ResponseObject { get; }
        T GetResponseObject();
        new IValidate<T> Validate { get; }
    }
}
