using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Net;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.DataGenerators;
using LykkeAutomationPrivate.Resources.ClientAccountResource;
using XUnitTestCommon.TestsData;

namespace AFTests.PrivateApiTests
{
    class VerifiedEmailsBaseTest : PrivateApiBaseTest
    {
        protected ClientAccountInformation ClientAccount;
        protected VerifiedEmails api;

        [OneTimeSetUp]
        public void CreateClientAndApi()
        {
            ClientRegistrationModel clientRegistration = new ClientRegistrationModel().GetTestModel();
            ClientAccount = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration).GetResponseObject();

            api = lykkeApi.ClientAccount.VerifiedEmails;
        }

        [OneTimeTearDown]
        public void DeleteClient()
        {
            lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(ClientAccount.Id);
        }
    }

    class PostVerifiedEmailsTests : VerifiedEmailsBaseTest
    {
        [Test]
        [Category("VerifiedEmails"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostVerifiedEmailTest()
        {
            var verifiedEmail = new VerifiedEmailModel
            {
                Email = ClientAccount.Email
            };

            var postVerifiedEmail = api.PostVerifiedEmails(verifiedEmail);
            Assert.That(postVerifiedEmail.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.IsEmailVerified.PostIsEmailVerified(verifiedEmail)
                .GetResponseObject(), Is.True);
        }
    }

    class DeleteVerifiedEmailsTests : VerifiedEmailsBaseTest
    {
        [Test]
        [Category("VerifiedEmails"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteVerifiedEmailTest()
        {
            var verifiedEmail = new VerifiedEmailModel
            {
                Email = ClientAccount.Email
            };

            var postVerifiedEmail = api.PostVerifiedEmails(verifiedEmail);
            Assert.That(api.IsEmailVerified.PostIsEmailVerified(verifiedEmail)
                .GetResponseObject(), Is.True);

            var deleteVerifiedEmail = api.DeleteVerifiedEmails(verifiedEmail);
            Assert.That(deleteVerifiedEmail.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.IsEmailVerified.PostIsEmailVerified(verifiedEmail)
                .GetResponseObject(), Is.False);
        }
    }

    class PutVerifiedEmailTests : VerifiedEmailsBaseTest
    {
        [Test]
        [Category("VerifiedEmails"), Category("ClientAccount"), Category("ServiceAll")]
        public void PutVerifiedEmailTest()
        {
            var newEmail = TestData.GenerateEmail();
            var verifiedEmail = new VerifiedEmailModel
            {
                Email = ClientAccount.Email
            };
            api.PostVerifiedEmails(verifiedEmail);

            var putVerifiedEmail = api.PutVerifiedEmails(newEmail, verifiedEmail);
            Assert.That(putVerifiedEmail.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            verifiedEmail.Email = newEmail;
            Assert.That(api.IsEmailVerified.PostIsEmailVerified(verifiedEmail)
                .GetResponseObject(), Is.True);
        }
    }
}
