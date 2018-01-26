using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.DataGenerators;
using LykkeAutomationPrivate.Resources.ClientAccountResource;

namespace AFTests.PrivateApiTests
{
    class IsEmailVerifiedBaseTest : PrivateApiBaseTest
    {
        private Partner partner;
        protected ClientAccountInformation clientWithPartner;
        protected ClientAccountInformation clientWithOutPartner;

        [OneTimeSetUp]
        public void RegisterClientAndPartner()
        {
            partner = new Partner().GetTestModel();
            lykkeApi.ClientAccount.Partners.PostPartners(partner);

            clientWithPartner = lykkeApi.ClientAccount.Clients
                .PostRegister(new ClientRegistrationModel().GetTestModel(partner.PublicId)).GetResponseObject();
            clientWithOutPartner = lykkeApi.ClientAccount.Clients
                .PostRegister(new ClientRegistrationModel().GetTestModel()).GetResponseObject();
        }

        [OneTimeTearDown]
        public void RemoveClientsAndPartner()
        {
            lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(clientWithPartner.Id);
            lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(clientWithOutPartner.Id);

            lykkeApi.ClientAccount.Partners.DeleteRemovePartner(partner.InternalId);
        }
    }

    class IsNewEmailVerifiedTests : IsEmailVerifiedBaseTest
    {
        [Test]
        [Category("IsEmailVerified"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetIsNewEmailVerifiedTest()
        {
            var getIsEmailVerified = lykkeApi.ClientAccount.IsEmailVerified
                .PostIsEmailVerified(new VerifiedEmailModel()
                {
                    Email = clientWithPartner.Email,
                    PartnerId = clientWithPartner.PartnerId
                });
            Assert.That(getIsEmailVerified.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getIsEmailVerified.GetResponseObject(), Is.False, 
                "New registered client email is verified");
        }

        [Test]
        [Category("IsEmailVerified"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetIsNewEmailVerifiedWithOutPartnerTest()
        {
            var getIsEmailVerified = lykkeApi.ClientAccount.IsEmailVerified
                .PostIsEmailVerified(new VerifiedEmailModel()
                {
                    Email = clientWithOutPartner.Email
                });
            Assert.That(getIsEmailVerified.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getIsEmailVerified.GetResponseObject(), Is.False,
                "New registered client email is verified");
        }
    }

    class IsEmailVerifiedTests : IsEmailVerifiedBaseTest
    {
        [Test]
        [Category("IsEmailVerified"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetIsEmailVerifiedTest()
        {
            lykkeApi.ClientAccount.VerifiedEmails.PostVerifiedEmails(new VerifiedEmailModel
            {
                Email = clientWithPartner.Email,
                PartnerId = clientWithPartner.PartnerId
            });

            var getIsEmailVerified = lykkeApi.ClientAccount.IsEmailVerified
                .PostIsEmailVerified(new VerifiedEmailModel()
                {
                    Email = clientWithPartner.Email,
                    PartnerId = clientWithPartner.PartnerId
                });
            Assert.That(getIsEmailVerified.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getIsEmailVerified.GetResponseObject(), Is.True,
                "Client email has not been verified");
        }

        [Test]
        [Category("IsEmailVerified"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetIsEmailVerifiedWithOutPartnerTest()
        {
            lykkeApi.ClientAccount.VerifiedEmails.PostVerifiedEmails(new VerifiedEmailModel
            {
                Email = clientWithOutPartner.Email
            });

            var getIsEmailVerified = lykkeApi.ClientAccount.IsEmailVerified
                .PostIsEmailVerified(new VerifiedEmailModel()
                {
                    Email = clientWithOutPartner.Email
                });
            Assert.That(getIsEmailVerified.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getIsEmailVerified.GetResponseObject(), Is.True,
                "Client email has not been verified");
        }
    }
}
