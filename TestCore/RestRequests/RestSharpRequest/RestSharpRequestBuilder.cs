using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using TestsCore.RestRequests.Interfaces;
using Newtonsoft.Json;
using System.Linq;
using LykkeAutomation.TestsCore;
using NUnit.Framework;

namespace TestsCore.RestRequests.RestSharpRequest
{
    public class RestSharpRequestBuilder : IRequestBuilder
    {
        private string baseUrl;
        private RestSharpRequest request;

        public RestSharpRequestBuilder(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }

        public IRequest Build()
        {
            return request;
        }

        #region Methods
        public IRequestBuilder Post(string resourse)
        {
            request = new RestSharpRequest(Method.POST, baseUrl, resourse);
            return this;
        }

        public IRequestBuilder Get(string resourse)
        {
            request = new RestSharpRequest(Method.GET, baseUrl, resourse);
            return this;
        }

        public IRequestBuilder Delete(string resourse)
        {
            request = new RestSharpRequest(Method.DELETE, baseUrl, resourse);
            return this;
        }

        public IRequestBuilder Put(string resourse)
        {
            request = new RestSharpRequest(Method.PUT, baseUrl, resourse);
            return this;
        }
        #endregion

        #region Request
        public IRequestBuilder WithHeaders()
        {
            throw new NotImplementedException();
        }

        public IRequestBuilder WithBearerToken(string token)
        {
            request.AddHeader("Authorization", string.Format("Bearer {0}", token));
            return this;
        }

        public IRequestBuilder AddJsonBody(object json)
        {
            request.AddJsonBody(json);
            return this;
        }

        public IRequestBuilder AddQueryParameter(string name, object value)
        {
            request.AddQueryParameter(name, value);
            return this;
        }

        public IRequestBuilder ContentType(string contentType)
        {
            request.AddHeader("Content-Type", contentType);
            return this;
        }

        public IRequestBuilder Accept(string mediaType)
        {
            request.AddHeader("Accept", mediaType);
            return this;
        }
        #endregion
    }
}
