using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.ClientFullName
{
    class ClientFullNameTests
    {
        public class PostClientFullName : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostClientFullNameTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var getClient = walletApi.PersonalData.GetPersonalDataResponse(registrationresponse.Result.Token).GetResponseObject();
                Assert.That(newUser.FullName, Does.Contain(getClient.Result.FullName));

                var newFullName = TestData.GenerateLetterString(8) + " " + TestData.GenerateLetterString(6);

                var newName = new PostClientFullNameModel(){ FullName = newFullName };
                var postResponse = walletApi.ClientFullName.PostClientFullName(newName, registrationresponse.Result.Token);

                postResponse.Validate.StatusCode(HttpStatusCode.OK);
                var getNewClient = walletApi.PersonalData.GetPersonalDataResponse(registrationresponse.Result.Token).GetResponseObject();

                Assert.That(getNewClient.Result.FullName, Is.EqualTo(newFullName));
            }
        }

        public class PostClientFullNameInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%^&*(0")]
            [Category("WalletApi")]
            public void PostClientFullNameInvalidTokenTest(string token)
            {
                var newFullName = TestData.GenerateLetterString(8) + " " + TestData.GenerateLetterString(6);

                var newName = new PostClientFullNameModel() { FullName = newFullName };
                var postResponse = walletApi.ClientFullName.PostClientFullName(newName, token);

                postResponse.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
