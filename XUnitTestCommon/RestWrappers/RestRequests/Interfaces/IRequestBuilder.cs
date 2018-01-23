using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestCommon.RestRequests.Interfaces
{
    public interface IRequestBuilder
    {
        IRequestBuilder Post(string resourse);
        IRequestBuilder Get(string resourse);
        IRequestBuilder Delete(string resourse);
        IRequestBuilder Put(string resourse);

        IRequestBuilder WithHeaders(string name, string value);
        IRequestBuilder AddObject(object body);
        IRequestBuilder AddJsonBody(object json);
        IRequestBuilder AddJsonBody(string json);
        IRequestBuilder AddQueryParameter(string name, object value);
        IRequestBuilder AddQueryParameterIfNotNull(string name, object value);
        IRequestBuilder WithBearerToken(string token);
        //IRequestBuilder WithProxy { get; }
        IRequestBuilder Accept(string mediaType);
        IRequestBuilder ContentType(string contentType);

        IRequest Build();
    }
}
