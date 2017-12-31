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
        ClientRegistrationModel existedClient;
        ClientRegistrationModel nonExistedClient;
        ClientAccountInformation client;

        [OneTimeSetUp]
        public void CreateClient()
        {
            existedClient = new ClientRegistrationModel().GetTestModel();
            nonExistedClient = new ClientRegistrationModel().GetTestModel();
            client = lykkeApi.ClientAccount.Clients.PostRegister(existedClient).GetResponseObject();
        }

        [OneTimeTearDown]
        public void RemoveClient()
        {
            lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(client.Id);
        }

        [Test]
        [Category("AccountExist"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetAccountForExistedClientTest()
        {
            var getAccountExist = lykkeApi.ClientAccount.AccountExist
                .GetAccountExist(existedClient.Email);
            getAccountExist.Validate.StatusCode(HttpStatusCode.OK);
            Assert.That(getAccountExist.GetResponseObject()
                .IsClientAccountExisting, Is.True);
        }

        [Test]
        [Category("AccountExist"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetAccountForNonExistedClientTest()
        {
            var getAccountExist = lykkeApi.ClientAccount.AccountExist
                .GetAccountExist(nonExistedClient.Email);
            getAccountExist.Validate.StatusCode(HttpStatusCode.OK);
            Assert.That(getAccountExist.GetResponseObject()
                .IsClientAccountExisting, Is.False);
        }
    }
}
