using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.ClientFirstNameLastName
{
    class ClientFirstNameLastNameTests
    {
        public class PostClientFirstNameLastName : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostClientFirstNameLastNameTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var getClient = walletApi.PersonalData.GetPersonalDataResponse(registrationresponse.Result.Token).GetResponseObject();
                Assert.That(newUser.FullName, Does.Contain(getClient.PersonalData.FullName));

                var newFirstName = TestData.GenerateLetterString(6);
                var newLastName = TestData.GenerateLetterString(6);

                var newName = new PostClientFirstNameLastNameModel() {FirstName = newFirstName, LastName = newLastName };
                var postResponse = walletApi.ClientFirstNameLastName.PostClientFirstNameLastName(newName, registrationresponse.Result.Token);

                postResponse.Validate.StatusCode(HttpStatusCode.OK);
                var getNewClient = walletApi.PersonalData.GetPersonalDataResponse(registrationresponse.Result.Token).GetResponseObject();

                Assert.That(getNewClient.PersonalData.FirstName, Is.EqualTo(newFirstName));
                Assert.That(getNewClient.PersonalData.LastName, Is.EqualTo(newLastName));
            }
        }

        public class PostClientFirstNameLastNameInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%^&*(")]
            [Category("WalletApi")]
            public void PostClientFirstNameLastNameInvalidTokenTest(string token)
            {
                var newFirstName = TestData.GenerateLetterString(6);
                var newLastName = TestData.GenerateLetterString(6);

                var newName = new PostClientFirstNameLastNameModel() { FirstName = newFirstName, LastName = newLastName };
                var postResponse = walletApi.ClientFirstNameLastName.PostClientFirstNameLastName(newName, token);

                postResponse.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
