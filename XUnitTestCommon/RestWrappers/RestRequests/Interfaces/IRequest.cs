﻿using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.RestRequests.Interfaces
{
    public interface IRequest
    {
        RestSharp.Method Method { get; }
        string BaseUrl { get; }
        string Resource { get; }
        object JsonBody { get; }

        Dictionary<string, string> Headers { get; }
        Dictionary<string, object> QueryParams { get; }
        

        void AddHeader(string name, string value);
        void AddQueryParameter(string name, object value);
        void AddJsonBody(object json);
        void AddTextBody(string text);

        IResponse Execute();
        IResponse<T> Execute<T>();
    }
}
