using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using XUnitTestCommon.TestsData;

namespace AFTests.ApiRegression
{
    class LoginTests : ApiRegressionBaseTest
    {
        [Test]
        [Category("ApiRegression")]
        public void LoginTest()
        {
            string email = "untest001@test.com"; //TODO: Register new user
            string password = "123456";
            string clientInfo = "<android>; Model:<Android SDK built for x86>; Os:<android>; Screen:<1080x1794>;";
            string pin = "1111";
            string code = "0000";
            string token = null;
            string accessToken = null;

            //STEP 1
            var getClientState = walletApi.ClientState
                .GetClientState(email, null)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            Assert.That(getClientState.GetResponseObject().Result
                .IsRegistered, Is.True, "Trying to login by unregistered account");

            //STEP 2
            var postAuth = walletApi.Auth
                .PostAuthResponse(new AuthenticateModel()
                {
                    ClientInfo = clientInfo,
                    Email = email,
                    Password = "" //TODO: Add password;
                })
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            var postAuthResult = postAuth.GetResponseObject().Result;
            Assert.That(postAuthResult.PersonalData.Email, Is.EqualTo(email));
            Assert.That(postAuthResult.Token, Is.Not.Null);
            token = postAuthResult.Token;

            //STEP 3
            var getCheckDocumentsToUpload = walletApi.CheckDocumentsToUpload
                .GetCheckDocumentsToUpload(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();

            //STEP 4
            var getPinSecurity = walletApi.PinSecurity
                .GetPinSecurity(pin, token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            Assert.That(getPinSecurity.GetResponseObject().Result
                .Passed, Is.True);

            //STEP 5
            var getClientCode = walletApi.Client
                .GetClientCodes(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();

            //STEP 6
            var postClientCodes = walletApi.Client
                .PostClientCodes(new SubmitCodeModel()
                {
                    Code = code
                }, token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            accessToken = postClientCodes.GetResponseObject().Result.AccessToken;
            Assert.That(accessToken, Is.Not.Null);
            
            //STEP 7

        }
    }
}
