using LykkeAutomationPrivate.DataGenerators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using System.Net;

namespace AFTests.PrivateApiTests
{
    class AccountExistResourseTests : PrivateApiBaseTest
    {
        ClientRegistrationModel nonExistedClient;
        ClientAccountInformation existedClient;
        ClientRegistrationModel nonExistedClientWithPartnerId;
        ClientAccountInformation existedClientWithPatnerId;
        Partner partner;

        [OneTimeSetUp]
        public void CreateClients()
        {
            nonExistedClient = new ClientRegistrationModel().GetTestModel();
            existedClient = lykkeApi.ClientAccount.Clients
                .PostRegister(new ClientRegistrationModel().GetTestModel()).GetResponseObject();

            partner = new Partner().GetTestModel();
            nonExistedClientWithPartnerId = new ClientRegistrationModel().GetTestModel(partner.PublicId);
            existedClientWithPatnerId = lykkeApi.ClientAccount.Clients
                .PostRegister(new ClientRegistrationModel().GetTestModel(partner.PublicId)).GetResponseObject();
        }

        [OneTimeTearDown]
        public void RemoveClients()
        {
            lykkeApi.ClientAccount.Partners.DeleteRemovePartner(partner.InternalId);
            lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(existedClient.Id);
            lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(existedClientWithPatnerId.Id);
        }

        [Test]
        [Category("AccountExist"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetAccountForExistedClientTest()
        {
            var getAccountExist = lykkeApi.ClientAccount.AccountExist
                .GetAccountExist(existedClient.Email, null);
            getAccountExist.Validate.StatusCode(HttpStatusCode.OK);
            Assert.That(getAccountExist.GetResponseObject()
                .IsClientAccountExisting, Is.True);
        }

        [Test]
        [Category("AccountExist"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetAccountForNonExistedClientTest()
        {
            var getAccountExist = lykkeApi.ClientAccount.AccountExist
                .GetAccountExist(nonExistedClient.Email, null);
            getAccountExist.Validate.StatusCode(HttpStatusCode.OK);
            Assert.That(getAccountExist.GetResponseObject()
                .IsClientAccountExisting, Is.False);
        }

        [Test]
        [Category("AccountExist"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetAccountForExistedWithPartnerClientTest()
        {
            var getAccountExist = lykkeApi.ClientAccount.AccountExist
                .GetAccountExist(existedClientWithPatnerId.Email, partner.PublicId);
            getAccountExist.Validate.StatusCode(HttpStatusCode.OK);
            Assert.That(getAccountExist.GetResponseObject()
                .IsClientAccountExisting, Is.True);
        }

        [Test]
        [Category("AccountExist"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetAccountForNonExistedClientWithPartnerTest()
        {
            var getAccountExist = lykkeApi.ClientAccount.AccountExist
                .GetAccountExist(nonExistedClientWithPartnerId.Email, partner.PublicId);
            getAccountExist.Validate.StatusCode(HttpStatusCode.OK);
            Assert.That(getAccountExist.GetResponseObject()
                .IsClientAccountExisting, Is.False);
        }
    }
}
