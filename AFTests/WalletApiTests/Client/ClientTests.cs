using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.Client
{
    class ClientTests
    {
        public class GetClientCodes : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetClientCodesTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var responseNew = walletApi.Client.GetClientCodes(registrationresponse.Result.Token);
                responseNew.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(responseNew.GetResponseObject().Error, Is.Null);
            }
        }

        public class GetClientCodesInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%&(0")]
            [Category("WalletApi")]
            public void GetClientCodesInvalidTokenTest(string token)
            {
                var response = walletApi.Client.GetClientCodes(token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        public class PostClientCodesWrongCode : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostClientCodesWrongCodeTest()
            {
                var model = new SubmitCodeModel() {Code = TestData.GenerateString(6) };
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.Client.PostClientCodes(model, registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.Content, Does.Contain("Wrong confirmation code"));
            }
        }

        public class PostClientCodesValidCode : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostClientCodesValidCodeTest()
            {
                var model = new SubmitCodeModel() { Code = "0000" };
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var getResponse = walletApi.Client.GetClientCodes(registrationresponse.Result.Token);
                getResponse.Validate.StatusCode(HttpStatusCode.OK);

                var response = walletApi.Client.PostClientCodes(model, registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class PostClientCodesInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%&*(0")]
            [Category("WalletApi")]
            public void PostClientCodesInvalidTokenTest(string token)
            {
                var model = new SubmitCodeModel() { Code = "0000" };
                
                var response = walletApi.Client.PostClientCodes(model, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        public class PostClientEncodedMainKey : WalletApiBaseTest
        {
            [Test]
            [Description("Get logic")]
            [Category("WalletApi")]
            public void PostClientEncodedMainKeyTest()
            {
                var model = new SubmitCodeModel() { Code = "0000" };
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var getResponse = walletApi.Client.GetClientCodes(registrationresponse.Result.Token);
                getResponse.Validate.StatusCode(HttpStatusCode.OK);

                var response = walletApi.Client.PostClientCodes(model, registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);

                var accessTokenModel = new AccessTokenModel() {AccessToken = response.GetResponseObject().Result.AccessToken };

                var responseEncodedKey = walletApi.Client.PostClientEncodedMainKey(accessTokenModel, registrationresponse.Result.Token);
            }
        }
    }
}
