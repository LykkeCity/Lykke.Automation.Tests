using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;
using NUnit.Framework;
using System.Reflection;

namespace XUnitTestCommon.RestRequests
{
    public class Validate : IValidate
    {
        private IResponse response;

        public Validate(IResponse response)
        {
            this.response = response;
        }

        public IResponse StatusCode(HttpStatusCode code, string message = null)
        {
            Assert.That(response.StatusCode, Is.EqualTo(code), message ?? "Unexpected status code");
            return response;
        }
    }

    public class Validate<T> : IValidate<T>
    {
        private IResponse<T> response;

        public Validate(IResponse<T> response)
        {
            this.response = response;
        }

        IResponse<T> IValidate<T>.StatusCode(HttpStatusCode code, string message)
        {
            Assert.That(response.StatusCode, Is.EqualTo(code), message ?? "Unexpected status code");
            return response;
        }

        public IResponse<T> NoApiError(string message = null)
        {
            var responseObject = response.GetResponseObject();
            if (responseObject == null)
            {
                throw new Exception("Response object is null");
            }

            Type responseType = responseObject.GetType();
            PropertyInfo property = responseObject.GetType().GetProperty("Error");
            if (property == null)
            {
                throw new Exception($"There is no Error for type {responseType}");
            }

            var propertyValue = property.GetValue(responseObject, null);
            Assert.That(propertyValue, Is.Null, message ?? $"The error is not null");

            return response;
        }
    }
}
