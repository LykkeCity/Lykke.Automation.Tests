using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TestsCore.RestRequests.Interfaces
{
    public interface IResponse
    {
        HttpStatusCode StatusCode { get; set; }
        string Content { get; set; }
    }

    public interface IResponse<T> : IResponse
    {
        T GetResponseObject();
    }
}
