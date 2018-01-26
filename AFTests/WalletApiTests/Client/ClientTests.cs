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
                var model = new SubmitCodeModel() { Code = TestData.GenerateString(6) };
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

                var accessTokenModel = new AccessTokenModel() { AccessToken = response.GetResponseObject().Result.AccessToken };

                var responseEncodedKey = walletApi.Client.PostClientEncodedMainKey(accessTokenModel, registrationresponse.Result.Token);
            }
        }

        public class GetClientBalancesBaseAsset : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetClientBalancesBaseAssetTest()
            {
                string baseAsset = "BTC";
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.Client.GetClientBalancesBaseAsset(baseAsset, registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }            
        }

        public class GetClientBalancesBaseAssetInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("123456")]
            [TestCase("testToken")]
            [TestCase("!@$%^&*(0")]
            [Category("WalletApi")]
            public void GetClientBalancesBaseAssetInvalidTokenTest(string token)
            {
                string baseAsset = "BTC";
                var response = walletApi.Client.GetClientBalancesBaseAsset(baseAsset, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        public class PostClientDialogOk : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostClientDialogOkTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.Client.PostClientDialogOk(registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class PostClientDialogOkInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@%^&*(")]
            [Category("WalletApi")]
            public void PostClientDialogOkInvalidTokenTest(string token)
            {
                var response = walletApi.Client.PostClientDialogOk(token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        public class DeleteClientKey : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void DeleteClientKeyTest()
            {
                string key = "key";

                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.Client.DeleteClientKey(key, registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class GetClientKey : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetClientKeyTest()
            {
                var key = "key";
                var value = "val1";

                var model = new KeyValue() { Key = key, Value = value };

                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var postResponse = walletApi.Client.PostClientDictionary(model, registrationresponse.Result.Token);

                var response = walletApi.Client.GetClientKey(key, registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
                Assert.That(response.GetResponseObject().Result.Key, Is.EqualTo(key), "Key is not expected");
                Assert.That(response.GetResponseObject().Result.Value, Is.EqualTo(value), "Value is not expected");

                var deleteResponse = walletApi.Client.DeleteClientKey(key, registrationresponse.Result.Token);
                deleteResponse.Validate.StatusCode(HttpStatusCode.OK);

                var newResponse = walletApi.Client.GetClientKey(key, registrationresponse.Result.Token);
                Assert.That(newResponse.GetResponseObject().Result, Is.Null);
            }
        }

        public class PostClientDictionary : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostClientDictionaryTest()
            {
                var model = new KeyValue(){Key = "key", Value = "val1" };
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.Client.PostClientDictionary(model, registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class PutClientDictionary : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PutClientDictionaryTest()
            {
                var key = "key";
                var value = "val1";
                var newValue = "val2";

                var model = new KeyValue() { Key = key, Value = value };

                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var postResponse = walletApi.Client.PostClientDictionary(model, registrationresponse.Result.Token);

                var response = walletApi.Client.GetClientKey(key, registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
                Assert.That(response.GetResponseObject().Result.Key, Is.EqualTo(key), "Key is not expected");
                Assert.That(response.GetResponseObject().Result.Value, Is.EqualTo(value), "Value is not expected");

                var putResponse = walletApi.Client.PutClientDictionary(new KeyValue { Key = key, Value = newValue }, registrationresponse.Result.Token);
                var newGetResponse = walletApi.Client.GetClientKey(key, registrationresponse.Result.Token);
                newGetResponse.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(newGetResponse.GetResponseObject().Error, Is.Null);
                Assert.That(newGetResponse.GetResponseObject().Result.Key, Is.EqualTo(key), "Key is not expected");
                Assert.That(newGetResponse.GetResponseObject().Result.Value, Is.EqualTo(newValue), "Value is not expected");
            }
        }

        public class GetClientIsFromUs : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetClientIsFromUsTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.Client.GetClientIsFromUs(registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
                Assert.That(response.GetResponseObject().Result.IsUserFromUS, Is.False);
            }
        }

        public class PostClientIsFromUs : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostClientIsFromUsTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.Client.GetClientIsFromUs(registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
                Assert.That(response.GetResponseObject().Result.IsUserFromUS, Is.False);

                var postResponse = walletApi.Client.PostClientIsFromUs(new SetIsUserFromUSModel() {IsUserFromUS = true }, registrationresponse.Result.Token);
                postResponse.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(postResponse.GetResponseObject().Error, Is.Null);

                var newGetResponse = walletApi.Client.GetClientIsFromUs(registrationresponse.Result.Token);
                newGetResponse.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(newGetResponse.GetResponseObject().Error, Is.Null);
                Assert.That(newGetResponse.GetResponseObject().Result.IsUserFromUS, Is.True, "IsUserFromUs didnt changed its value");
            }
        }

        public class GetClientPendingActions : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetClientPendingActionsTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.Client.GetClientPendingActions(registrationresponse.Result.Token);

                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class GetClientPendingActionsInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%^&*(")]
            [Category("WalletApi")]
            public void GetClientPendingActionsInvalidTokenTest(string token)
            {
                var response = walletApi.Client.GetClientPendingActions(token);

                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
