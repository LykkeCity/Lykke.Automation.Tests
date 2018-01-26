using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.ChangePinAndPassword
{
    class ChangePinAndPasswordTests
    {

        public class PostChangePinAndPassword : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostChangePinAndPasswordTest()
            {
                Assert.Ignore("How get positive case?");
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();
                Assert.That(registrationresponse.Error, Is.Null, $"Error message not empty {registrationresponse.Error?.Message}");

                var model = new PostChangePinAndPasswordModel()
                {
                    Email = newUser.Email,//invalid value
                    EncodedPrivateKey = TestData.GenerateString(),
                    NewHint = newUser.Hint,
                    NewPassword = TestData.GenerateString(8),
                    NewPin = TestData.GenerateNumbers(8),// only dig
                    PartnerId = TestData.GenerateLetterString(6),
                    SignedOwnershipMsg = TestData.GenerateString(8),
                    SmsCode = TestData.GenerateNumbers(8)
                };
                var response = walletApi.ChangePinAndPassword.PostChangePinAndPassword(model);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class PostChangePinAndPasswordInvalidEmail : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostChangePinAndPasswordInvalidEmailTest()
            {
                var model = new PostChangePinAndPasswordModel()
                {
                    Email = TestData.GenerateNumbers(5),
                    EncodedPrivateKey = TestData.GenerateString(),
                    NewHint = TestData.GenerateString(),
                    NewPassword = TestData.GenerateString(8),
                    NewPin = TestData.GenerateNumbers(8),// only dig
                    PartnerId = TestData.GenerateLetterString(6),
                    SignedOwnershipMsg = TestData.GenerateString(8),
                    SmsCode = TestData.GenerateNumbers(8)
                };
                var response = walletApi.ChangePinAndPassword.PostChangePinAndPassword(model);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.Content, Does.Contain("Invalid email format"));
            }
        }

        public class PostChangePinAndPasswordInvalidPin : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostChangePinAndPasswordInvalidPinTest()
            {
                var model = new PostChangePinAndPasswordModel()
                {
                    Email = TestData.GenerateEmail(),
                    EncodedPrivateKey = TestData.GenerateString(),
                    NewHint = TestData.GenerateString(),
                    NewPassword = TestData.GenerateString(8),
                    NewPin = "test pin",// only dig
                    PartnerId = TestData.GenerateLetterString(6),
                    SignedOwnershipMsg = TestData.GenerateString(8),
                    SmsCode = TestData.GenerateNumbers(8)
                };
                var response = walletApi.ChangePinAndPassword.PostChangePinAndPassword(model);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.Content, Does.Contain("Pin should contain digits only"));
            }
        }

        public class PostChangePinAndPasswordInvalidModelSOM : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostChangePinAndPasswordInvalidModelSOMTest()
            {
                var model = new PostChangePinAndPasswordModel()
                {
                    Email = TestData.GenerateEmail()
                };
                var response = walletApi.ChangePinAndPassword.PostChangePinAndPassword(model);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.Content, Does.Contain("SignedOwnershipMsg"));
                Assert.That(response.Content, Does.Contain("Field should not be empty"));
            }
        }

        public class PostChangePinAndPasswordInvalidModel : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostChangePinAndPasswordInvalidModelTest()
            {
                var model = new PostChangePinAndPasswordModel()
                {
                    Email = TestData.GenerateEmail(),
                    SignedOwnershipMsg = TestData.GenerateString()
                };
                var response = walletApi.ChangePinAndPassword.PostChangePinAndPassword(model);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.Content, Does.Contain("NewHint"));
                Assert.That(response.Content, Does.Contain("Field should not be empty"));
            }
        }

        public class PostChangePinAndPasswordInvalidModelPassword : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostChangePinAndPasswordInvalidModelPasswordTest()
            {
                var model = new PostChangePinAndPasswordModel()
                {
                    Email = TestData.GenerateEmail(),
                    SignedOwnershipMsg = TestData.GenerateString(),
                    NewHint = TestData.GenerateString(6),
                    NewPassword = TestData.GenerateString(10)
                };
                var response = walletApi.ChangePinAndPassword.PostChangePinAndPassword(model);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.Content, Does.Contain("NewPin"));
                Assert.That(response.Content, Does.Contain("Field should not be empty"));
            }
        }

        public class PostChangePinAndPasswordInvalidModelPin : WalletApiBaseTest
        {
            [Test]
            [Description("Find the correct behavior in this case")]
            [Category("WalletApi")]
            public void PostChangePinAndPasswordInvalidModelPinTest()
            {
                var model = new PostChangePinAndPasswordModel()
                {
                    Email = TestData.GenerateEmail(),
                    SignedOwnershipMsg = TestData.GenerateString(),
                    NewHint = TestData.GenerateString(6),
                    NewPassword = TestData.GenerateString(10),
                    NewPin = TestData.GenerateNumbers(4)
                };
                var response = walletApi.ChangePinAndPassword.PostChangePinAndPassword(model);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.Content, Does.Contain("NewPin"));
                Assert.That(response.Content, Does.Contain("Field should not be empty"));
            }
        }
    }
}
