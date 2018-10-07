using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using ApiV2Data.Models;
using Lykke.Client.ApiV2.Models;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    class ApiV2SecondFactorAuthTests : ApiV2TokenBaseTest
    {

        [Test]
        [Category("ApiV2")]
        public void Get2FATest()
        {
            Step("Make GET /api/2fa and validate response", () => 
            {
                var response = apiV2.SecondFactorAuth.Get2FA(token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                Assert.That(response.GetResponseObject(), Is.Not.Null);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void Post2FAOperations()
        {
            Step("Make POST /api/2fa/operation and validate response", () => 
            {
                var model = new OperationConfirmationModel
                {
                    OperationId = Guid.NewGuid().ToString(),
                    Signature = new OperationConfirmationSignature { Code = "1111"},
                    Type = ""
                };

                var response = apiV2.SecondFactorAuth.Post2FAOperation(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void Get2FAGoogleSessionTest()
        {
            Step("Make GET /api/2fa/setup/google and validate response", () => 
            {
                var response = apiV2.SecondFactorAuth.Get2FASetUpGoogle(token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                Assert.That(response.GetResponseObject().ManualEntryKey, Is.Not.Null.Or.Empty);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostSetupGoogleTest()
        {
            Step("Make POST /api/2fa/setup/google and validate response", () => 
            {
                var model = new GoogleSetupVerifyRequest
                {
                    Code = Guid.NewGuid().ToString()
                };

                var response = apiV2.SecondFactorAuth.Post2FASetUpGoogle(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.GetResponseObject().IsValid, Is.False);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void Post2FASessionTest()
        {
            Step("Make POST /api/2fa/session and validate response", () => 
            {
                var model = new TradingSessionConfirmModel
                {
                    Confirmation = "111111",
                    SessionId = Guid.NewGuid().ToString()
                };

                var response = apiV2.SecondFactorAuth.Post2FASession(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}
