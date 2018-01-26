using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.Dialogs
{
    class DialogsTests
    {

        public class GetDialogs : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetDialogsTest()
            {
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.Dialogs.GetDialogs(registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);

                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class GetDialogsInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%^&*(")]
            [Description("Authorization should have highest priority then other requests")]
            [Category("WalletApi")]
            public void GetDialogsInvalidTokenTest(string token)
            {
                var response = walletApi.Dialogs.GetDialogs(token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        public class PostDialogs : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostDialogsTest()
            {
                Assert.Ignore("Get valid test logic");
                AccountRegistrationModel newUser = new AccountRegistrationModel().GetTestModel();
                var registrationresponse = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.Dialogs.PostDialogs(
                    new ClientDialogSubmitModel()
                {
                    ButtonId = Guid.NewGuid(),
                    Id = Guid.NewGuid()
                },registrationresponse.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);

                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class PostDialogsInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%^&*(")]
            [Category("WalletApi")]
            public void PostDialogsInvalidTokenTest(string token)
            {
                var response = walletApi.Dialogs.PostDialogs(
                    new ClientDialogSubmitModel()
                    {
                        ButtonId = Guid.NewGuid(),
                        Id = Guid.NewGuid()
                    }, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
