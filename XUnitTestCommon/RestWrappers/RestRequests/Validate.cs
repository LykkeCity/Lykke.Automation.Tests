using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;
using NUnit.Framework;

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
    }
}
