using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    class ApiV2PaymentsTests : ApiV2TokenBaseTest
    {
        [Test]
        [Category("ApiV2")]
        public void GetPaymentsMethod()
        {
            Step("Make GET /api/PaymentMethods and validate response", () => 
            {
                var response = apiV2.PaymentMethods.GetPaymentsMethods(token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                Assert.That(response.GetResponseObject().PaymentMethods, Is.Not.Null);
            });
        }
    }
}
