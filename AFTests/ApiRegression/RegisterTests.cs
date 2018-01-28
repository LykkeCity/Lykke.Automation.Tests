using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using XUnitTestCommon.TestsData;

namespace AFTests.ApiRegression
{
    public class RegisterTests : ApiRegressionBaseTest
    {
        [Test]
        [Category("ApiRegression")]
        public void RegisterTest()
        {
            var email = TestData.GenerateEmail();
            string code = "0000"; //TODO: What is it?
            string clientInfo = "iPhone; Model:6 Plus; Os:9.3.5; Screen:414x736";
            string hint = "qwe";
            string password = Guid.NewGuid().ToString("N");

            //STEP 1
            var getClientState = walletApi.ClientState.GetClientState(email, null);
            Assert.Multiple(() =>
            {
                getClientState.Validate.StatusCode(HttpStatusCode.OK);
                var getClientStateData = getClientState.GetResponseObject();
                Assert.That(getClientStateData.Error, Is.Null);
                Assert.That(getClientStateData.Result.IsRegistered, Is.False);
            });
            //STEP 2
            var postEmailVerification = walletApi.EmailVerification
                .PostEmailVerification(new PostEmailModel()
                {
                    Email = email
                }).Validate.StatusCode(HttpStatusCode.OK);
            Assert.That(postEmailVerification.GetResponseObject().Error, Is.Null);
            //STEP 3
            var getEmailVerification = walletApi.EmailVerification
                .GetEmailVerification(email, code, null)
                .Validate.StatusCode(HttpStatusCode.OK);
            Assert.That(getEmailVerification.GetResponseObject().Result.Passed, Is.True);
            //STEP 4
            var postRegistration = walletApi.Registration.PostRegistrationResponse(new AccountRegistrationModel()
            {
                ClientInfo = clientInfo,
                Email = email,
                Hint = hint,
                Password = password
            }).Validate.StatusCode(HttpStatusCode.OK);
            Assert.Multiple(() =>
            {
                var account = postRegistration.GetResponseObject().Result;
                Assert.That(postRegistration.GetResponseObject().Error, Is.Null);
                //Assert.That(account.Token, Is.Not.Null);
                //Assert.That(account.NotificationsId, Is.Not.Null);
                Assert.That(account.PersonalData?.Email, Is.EqualTo(email));
            });
        }
    }
}
