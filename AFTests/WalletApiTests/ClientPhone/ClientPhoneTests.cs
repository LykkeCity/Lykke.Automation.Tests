using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.ClientPhone
{
    class ClientPhoneTests
    {
        public class PostClientPhone : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostClientPhoneTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var getClient = walletApi.Registration.GetRegistrationResponse(registrationresponse.Result.Token).GetResponseObject();

                Assert.That(newUser.ContactPhone, Is.EqualTo(getClient.Result.PersonalData.Phone));

                var newNumber = TestData.GeneratePhone();
                var response = walletApi.ClientPhone.PostClientPhone(new PostClientPhoneModel() { PhoneNumber = newNumber }, registrationresponse.Result.Token);
                Assert.That(response.GetResponseObject().Error, Is.Null);

                var newGetClient = walletApi.Registration.GetRegistrationResponse(registrationresponse.Result.Token).GetResponseObject();

                Assert.That(newNumber, Is.EqualTo(newGetClient.Result.PersonalData.Phone));
            }
        }

        public class PostClientPhoneLetterPhone : WalletApiBaseTest
        {
            [Test]
            [Description("No back-end validation of phone number! Check that")]
            [Category("WalletApi")]
            public void PostClientPhoneLetterPhoneTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var getClient = walletApi.Registration.GetRegistrationResponse(registrationresponse.Result.Token).GetResponseObject();

                Assert.That(newUser.ContactPhone, Is.EqualTo(getClient.Result.PersonalData.Phone));

                var newNumber = TestData.GenerateLetterString(8);
                var response = walletApi.ClientPhone.PostClientPhone(new PostClientPhoneModel() { PhoneNumber = newNumber }, registrationresponse.Result.Token);
                Assert.That(response.GetResponseObject().Error, Is.Null);

                var newGetClient = walletApi.Registration.GetRegistrationResponse(registrationresponse.Result.Token).GetResponseObject();

                Assert.That(newNumber, Is.EqualTo(newGetClient.Result.PersonalData.Phone));
            }
        }

        public class PostClientPhoneInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@%^&*(0")]
            [Category("WalletApi")]
            public void PostClientPhoneInvalidTokenTest(string token)
            {
                var newNumber = TestData.GenerateLetterString(8);
                var response = walletApi.ClientPhone.PostClientPhone(new PostClientPhoneModel() { PhoneNumber = newNumber }, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
