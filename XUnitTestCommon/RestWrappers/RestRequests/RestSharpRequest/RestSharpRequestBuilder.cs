﻿using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;
using Newtonsoft.Json;
using System.Linq;
using LykkeAutomation.TestsCore;
using NUnit.Framework;

namespace XUnitTestCommon.RestRequests.RestSharpRequest
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
        public IRequestBuilder Post(string resourse, bool redirect = true)
        {
            request = new RestSharpRequest(Method.POST, baseUrl, resourse, redirect);
            return this;
        }

        public IRequestBuilder Patch(string resourse, bool redirect = true)
        {
            request = new RestSharpRequest(Method.PATCH, baseUrl, resourse, redirect);
            return this;
        }

        public IRequestBuilder Get(string resourse, bool redirect = true)
        {
            request = new RestSharpRequest(Method.GET, baseUrl, resourse, redirect);
            return this;
        }

        public IRequestBuilder Delete(string resourse, bool redirect = true)
        {
            request = new RestSharpRequest(Method.DELETE, baseUrl, resourse, redirect);
            return this;
        }

        public IRequestBuilder Put(string resourse, bool redirect = true)
        {
            request = new RestSharpRequest(Method.PUT, baseUrl, resourse, redirect);
            return this;
        }

        public IRequestBuilder Post(string resourse)
        {
            request = new RestSharpRequest(Method.POST, baseUrl, resourse);
            return this;
        }

        public IRequestBuilder Patch(string resourse)
        {
            request = new RestSharpRequest(Method.PATCH, baseUrl, resourse);
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
        public IRequestBuilder WithHeaders(string name, string value)
        {
            request.AddHeader(name, value);
            return this;
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

        public IRequestBuilder AddObject(object body)
        {
            request.AddObject(body);
            return this;
        }

        public IRequestBuilder AddJsonBody(string json)
        {
            request.AddJsonBody(json);
            return this;
        }

        public IRequestBuilder AddQueryParameter(string name, object value)
        {
            request.AddQueryParameter(name, value);
            return this;
        }

        public IRequestBuilder AddQueryParameterIfNotNull(string name, object value)
        {
            if (value != null)
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

        public IRequestBuilder AddTextBody(string text)
        {
            request.AddTextBody(text);
            return this;
        }
        #endregion
    }
}
