using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.CheckMobilePhone
{
    class CheckMobilePhoneTests
    {

        public class GetCheckMobilePhone : WalletApiBaseTest
        {
            [Test]
            [Description("Get test vald logic")]
            [Category("WalletApi")]
            public void GetCheckMobilePhoneTests()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var phoneNumber = newUser.ContactPhone;

                var response = walletApi.CheckMobilePhone.GetCheckMobilePhone(phoneNumber, "1234", registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class GetCheckMobilePhoneValidateCode : WalletApiBaseTest
        {
            [TestCase("123")]
            [TestCase("!@%^&")]
            [TestCase("testCode")]
            [Category("WalletApi")]
            public void GetCheckMobilePhoneValidateCodeTests(string code)
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var phoneNumber = newUser.ContactPhone;

                var response = walletApi.CheckMobilePhone.GetCheckMobilePhone(phoneNumber, code, registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.Content, Does.Contain("Code must contain exactly 4 digits"));
            }
        }

        public class GetCheckMobilePhoneCodeEmpty : WalletApiBaseTest
        {
            [TestCase("")]
            [Category("WalletApi")]
            public void GetCheckMobilePhoneCodeEmptyTests(string code)
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var phoneNumber = newUser.ContactPhone;

                var response = walletApi.CheckMobilePhone.GetCheckMobilePhone(phoneNumber, code, registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.Content, Does.Contain("Field should not be empty"));
            }
        }

        public class GetCheckMobilePhoneNumberEmpty : WalletApiBaseTest
        {
            [TestCase("")]
            [Category("WalletApi")]
            public void GetCheckMobilePhoneNumberEmptyTests(string phoneNumber)
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.CheckMobilePhone.GetCheckMobilePhone(phoneNumber, "1345", registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.Content, Does.Contain("Field should not be empty"));
            }
        }

        public class GetCheckMobilePhoneInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("testToken")]
            [TestCase("1234567")]
            [TestCase("!@$%^&*")]
            [Category("WalletApi")]
            public void GetCheckMobilePhoneInvalidTokenTests(string token)
            {
                var phoneNumber = "+1234567";
                var code = "1234";

                var response = walletApi.CheckMobilePhone.GetCheckMobilePhone(phoneNumber, code, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        public class PostCheckMobilePhone : WalletApiBaseTest
        {
            [Test]
            [Description("Get valid test logic")]
            [Category("WalletApi")]
            public void PostCheckMobilePhoneTest()
            {
                var model = new PostClientPhoneModel(){PhoneNumber = "+37529778899" };

                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.CheckMobilePhone.PostCheckMobilePhone(model, registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class PostCheckMobilePhoneEmpty : WalletApiBaseTest
        {
            [Test]
            [Description("Get valid test logic")]
            [Category("WalletApi")]
            public void PostCheckMobilePhoneEmptyTest()
            {
                var model = new PostClientPhoneModel() { PhoneNumber = "" };

                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.CheckMobilePhone.PostCheckMobilePhone(model, registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.Content, Does.Contain("Field should not be empty"));
            }
        }

        public class PostCheckMobilePhoneIvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("12345")]
            [TestCase("tetToken")]
            [TestCase("!@%^&(")]
            [Category("WalletApi")]
            public void PostCheckMobilePhoneIvalidTokenTest(string token)
            {
                var model = new PostClientPhoneModel() { PhoneNumber = "+37529778899" };

                var response = walletApi.CheckMobilePhone.PostCheckMobilePhone(model, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
