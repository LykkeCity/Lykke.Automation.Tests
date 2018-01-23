using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.ClientKeys
{
    class ClientKeysTests
    {

        public class PostClientKeys : WalletApiBaseTest
        {
            [Test]
            [Description("Are here some limitations to keys? How to validate changes??")]
            [Category("WalletApi")]
            public void PostClientKeysTest()
            {
                var encodedKey = TestData.GenerateString(16);
                var privateKey = TestData.GenerateString(16);
                var pubKey = TestData.GenerateString(16);
                var tempKey = TestData.GenerateString(16);

                var keys = new ClientKeysModel()
                {
                    EncodedPrivateKey = encodedKey,
                    PrivateKey = privateKey,
                    PubKey = pubKey,
                    TempKey = tempKey
                };

                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.ClientKeys.PostClientKeys(keys, registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class PostClientKeysInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%^&*(")]
            [Category("WalletApi")]
            public void PostClientKeysInvalidTokenTest(string token)
            {
                var encodedKey = TestData.GenerateString(16);
                var privateKey = TestData.GenerateString(16);
                var pubKey = TestData.GenerateString(16);
                var tempKey = TestData.GenerateString(16);

                var keys = new ClientKeysModel()
                {
                    EncodedPrivateKey = encodedKey,
                    PrivateKey = privateKey,
                    PubKey = pubKey,
                    TempKey = tempKey
                };

                var response = walletApi.ClientKeys.PostClientKeys(keys, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
