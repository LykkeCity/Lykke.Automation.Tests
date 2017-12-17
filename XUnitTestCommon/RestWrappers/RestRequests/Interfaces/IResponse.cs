﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;

namespace TestsCore.RestRequests.Interfaces
{
    public interface IResponse
    {
        HttpStatusCode StatusCode { get; }
        string Content { get; }
        JObject JObject { get; }
    }

    public interface IResponse<T> : IResponse
    {
        T GetResponseObject();
    }
}
