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
            string token = null;

            //STEP 1
            var getClientState = walletApi.ClientState
                .GetClientState(email, null)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            Assert.That(getClientState.GetResponseObject().Result
                    .IsRegistered, Is.False);

            //STEP 2
            var postEmailVerification = walletApi.EmailVerification
                .PostEmailVerification(new PostEmailModel()
                {
                    Email = email
                })
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            Assert.That(postEmailVerification.GetResponseObject().Error, Is.Null);

            //STEP 3
            var getEmailVerification = walletApi.EmailVerification
                .GetEmailVerification(email, code, null)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            Assert.That(getEmailVerification.GetResponseObject().Result.Passed, Is.True);

            //STEP 4
            var postRegistration = walletApi.Registration.PostRegistrationResponse(new AccountRegistrationModel()
            {
                ClientInfo = clientInfo,
                Email = email,
                Hint = hint,
                Password = password
            })
            .Validate.StatusCode(HttpStatusCode.OK)
            .Validate.NoApiError();
            Assert.Multiple(() =>
            {
                var postRegistrationData = postRegistration.GetResponseObject();
                Assert.That(postRegistrationData.Result.PersonalData?.Email, Is.EqualTo(email));
                Assert.That(postRegistrationData.Result.Token, Is.Not.Null.And.Not.Empty);
            });
            token = postRegistration.GetResponseObject().Result.Token;

            //STEP 5
            getClientState = walletApi.ClientState
                .GetClientState(email, null)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            Assert.Multiple(() =>
            {
                var getClientStateData = getClientState.GetResponseObject();
                Assert.That(getClientStateData.Result.IsRegistered, Is.True);
                Assert.That(getClientStateData.Result.IsPwdHashed, Is.True);
            });

            //STEP 6
            var getPersonalData = walletApi.PersonalData
                .GetPersonalDataResponse(token)
                .Validate.StatusCode(HttpStatusCode.OK)
                .Validate.NoApiError();
            Assert.That(getPersonalData.GetResponseObject().Result
                    .Email, Is.EqualTo(email));

            //STEP 7
            //var postClientFullName = walletApi.ClientFullName.PostClientFullName
        }
    }
}
