using LykkeAutomationPrivate.DataGenerators;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.Resources.ClientAccountResource;
using NUnit.Framework;
using System.Net;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.PrivateApiTests
{
    class ClientAccountBaseTest : PrivateApiBaseTest
    {
        protected ClientAccount api;
        protected ClientRegistrationModel clientRegistration;
        protected ClientAccountInformation account;
        protected Partner partner;
        protected string pin = "1111";

        [OneTimeSetUp]
        public void CreatePartnerAndClientAndApi()
        {
            api = lykkeApi.ClientAccount.ClientAccount;
            partner = new Partner().GetTestModel();
            lykkeApi.ClientAccount.Partners.PostPartners(partner);

            clientRegistration = new ClientRegistrationModel().GetTestModel(partner.PublicId);
            account = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration)
                .GetResponseObject();
            lykkeApi.ClientAccount.ClientAccountInformation.PostSetPIN(account.Id, pin);
            account.Pin = pin;
        }

        [OneTimeTearDown]
        public void RemovePartnerAndClient()
        {
            lykkeApi.ClientAccount.Partners.DeleteRemovePartner(partner.InternalId);
            lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(account.Id);
        }
    }

    class DeleteClientAccount : ClientAccountBaseTest
    {
        [Test]
        [Order(1)]
        [Description("Delete existing account.")]
        [Category("ClientAccountResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteClientAccountTest()
        {
            var deleteClient = api.DeleteClientAccount(account.Id);
            Assert.That(deleteClient.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.AccountExist.GetAccountExist(account.Email, partner.PublicId)
                            .GetResponseObject().IsClientAccountExisting, Is.False);
        }

        [Test]
        [Order(2)]
        [Description("Delete existing account again.")]
        [Category("ClientAccountResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteClientAccountAgainTest()
        {
            var deleteClient = api.DeleteClientAccount(account.Id);
            Assert.That(deleteClient.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(deleteClient.GetResponseObject().ErrorMessage, Is.EqualTo("Client with id doesn't exist"));
            Assert.That(api.AccountExist.GetAccountExist(account.Email, partner.PublicId)
                            .GetResponseObject().IsClientAccountExisting, Is.False);
        }
    }

    class PutClientAccountEmail : ClientAccountBaseTest
    {
        [Test]
        [Description("Change client email.")]
        [Category("ClientAccountResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void PutClientAccountEmailTest()
        {
            string oldEmail = account.Email;
            string newEmail = TestData.GenerateEmail();
            //Check that no account with new email exist
            Assert.That(lykkeApi.ClientAccount.AccountExist.GetAccountExist(newEmail, partner.PublicId).GetResponseObject()
                .IsClientAccountExisting, Is.False, "Should not happens. Bad random - account exist.");
            //Change email
            var putClientEmail = api.PutClientAccountEmail(account.Id, newEmail);
            Assert.That(putClientEmail.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Wrong responce code on email change");
            //Check that account with new email exist, with old - doesn't
            Assert.That(lykkeApi.ClientAccount.AccountExist.GetAccountExist(newEmail, partner.PublicId).GetResponseObject()
                .IsClientAccountExisting, Is.True, "Email has not been changed");
            Assert.That(lykkeApi.ClientAccount.AccountExist.GetAccountExist(oldEmail, partner.PublicId).GetResponseObject()
                .IsClientAccountExisting, Is.False, "Email has not been changed");
            Assert.That(lykkeApi.ClientAccount.ClientAccountInformation.GetClientById(account.Id)
                .GetResponseObject().Email, Is.EqualTo(newEmail));
        }
    }

    class GetClientAccountTrusted : ClientAccountBaseTest
    {
        [Test] //TODO: Add check on trusted account. How to create one?
        [Description("Check whether account is trusted.")]
        [Category("ClientAccountResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetClientAccountTrustedTest()
        {
            var getClienyAccoutTrusted = api.GetClientAccountTrusted(account.Id);
            Assert.That(getClienyAccoutTrusted.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getClienyAccoutTrusted.GetResponseObject(), Is.False, "Untrusted account is Trusted");
        }
    }

    class GetUsersCountByPartnerId : ClientAccountBaseTest
    {
        ClientRegistrationModel newAccountReg;
        ClientAccountInformation newAccount;

        [SetUp]
        public void CreateOneMoreClient() => 
            newAccountReg = new ClientRegistrationModel().GetTestModel(partner.PublicId);

        [TearDown]
        public void RemoveClient() => 
            lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(newAccount?.Id);

        [Test]
        [Description("Get users count by partner id.")]
        [Category("ClientAccountResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetUsersCountByPartnerIdTest()
        {
            var getUsersCountForAccount = api.GetUsersCountByPartnerId(partner.PublicId);
            Assert.That(getUsersCountForAccount.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getUsersCountForAccount.GetResponseObject(), Is.EqualTo(1));
                        
            newAccount = lykkeApi.ClientAccount.Clients.PostRegister(newAccountReg).GetResponseObject();

            getUsersCountForAccount = api.GetUsersCountByPartnerId(partner.PublicId);
            Assert.That(getUsersCountForAccount.GetResponseObject(), Is.EqualTo(2));
        }
    }
}
