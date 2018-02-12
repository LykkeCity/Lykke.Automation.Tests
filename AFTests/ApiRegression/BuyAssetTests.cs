using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using AFTests.ApiRegression.Steps;
using Lykke.Client.AutorestClient.Models;
using LykkeAutomationPrivate.DataGenerators;
using NUnit.Framework;

namespace AFTests.ApiRegression
{
    class BuyAssetTests : ApiRegressionBaseTest
    {
        [Test]
        [Category("ApiRegression")]
        public void BuyAssetTest()
        {
            string email = "untest005@test.com";
            string password = "1234567";
            string pin = "1111";

            var stepHelper = new MobileSteps(walletApi);

            var loggedResult = stepHelper.Login(email, password, pin);
            var privateKey = AesUtils.Decrypt(loggedResult.encodedPrivateKey, password);

            var svt = walletApi.SignatureVerificationToken.GetKeyConfirmation(email, loggedResult.token)
                .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError();
            string m = svt.GetResponseObject().Result.Message;
            Assert.That(m, Is.Not.Null);

        }
    }
}
