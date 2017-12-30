using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using XUnitTestCommon.RestRequests.Interfaces;
using NUnit.Framework;

namespace AFTests.PrivateApiTests
{
    public class ResponseValidator
    {
        public void IsOK(IResponse response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK),
                "Expected 200 OK in response");
        }
    }
}
