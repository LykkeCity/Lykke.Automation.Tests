using LykkeAutomationPrivate.DataGenerators;
using LykkeAutomationPrivate.Models.Registration.Models;
using NUnit.Framework;
using System.Net;
using System;
using System.Collections.Generic;
using System.Text;
using TestsCore.TestsData;

namespace LykkeAutomationPrivate.Tests.ClientAccount
{
    class DeleteClientAccount : BaseTest
    {
        string clientId;
        string clientEmail;

        [SetUp]
        public void CreateClient()
        {
            var postRegistration = lykkeApi.Registration.PostRegistration(new AccountRegistrationModel().GetTestModel());
            clientId = postRegistration.Account.Id;
            clientEmail = postRegistration.Account.Email;
        }

        [Test]
        [Description("Delete existing account.")]
        [Category("ClientAccountResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteClientAccountTest()
        {
            var deleteClient = lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(clientId);
            Assert.That(deleteClient.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            //delete again
            deleteClient = lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(clientId);
            Assert.That(deleteClient.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(deleteClient.GetResponseObject().ErrorMessage, Is.EqualTo("Client with id doesn't exist"));

            //check exist
            var getAccountExist = lykkeApi.ClientAccount.AccountExist.GetAccountExist(clientEmail);
            Assert.That(getAccountExist.GetResponseObject().IsClientAccountExisting, Is.False);
        }
    }

    class PutClientAccountEmail : BaseTest
    {
        ClientAccountInformationModel account;

        [SetUp]
        public void CreateClient()
        {
            account = lykkeApi.Registration.PostRegistration(new AccountRegistrationModel().GetTestModel()).Account;
        }

        [Test]
        [Description("Change client email.")]
        [Category("ClientAccountResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void PutClientAccountEmailTest()
        {
            string oldEmail = account.Email;
            string newEmail = TestData.GenerateEmail();
            //Check that no account with new email exist
            Assert.That(lykkeApi.ClientAccount.AccountExist.GetAccountExist(newEmail).GetResponseObject()
                .IsClientAccountExisting, Is.False, "Should not happens. Bad random - account exist.");
            //Change email
            var putClientEmail = lykkeApi.ClientAccount.ClientAccount.PutClientAccountEmail(account.Id, newEmail);
            Assert.That(putClientEmail.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Wrong responce code on email change");

            //Check that account with new email exist, with old - doesn't
            Assert.That(lykkeApi.ClientAccount.AccountExist.GetAccountExist(newEmail).GetResponseObject()
                .IsClientAccountExisting, Is.True, "Email has not been changed");
            Assert.That(lykkeApi.ClientAccount.AccountExist.GetAccountExist(oldEmail).GetResponseObject()
                .IsClientAccountExisting, Is.False, "Email has not been changed");

            //TODO: Check account by id
        }
    }

    class GetClientAccountTrusted : BaseTest
    {
        ClientAccountInformationModel account;

        [SetUp]
        public void CreateClient()
        {
            account = lykkeApi.Registration.PostRegistration(new AccountRegistrationModel().GetTestModel()).Account;
        }

        [Test] //TODO: Add check on trusted account. How to create one?
        [Description("Check whether account is trusted.")]
        [Category("ClientAccountResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetClientAccountTrustedTest()
        {
            var getClienyAccoutTrusted = lykkeApi.ClientAccount.ClientAccount.GetClientAccountTrusted(account.Id);
            Assert.That(getClienyAccoutTrusted.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getClienyAccoutTrusted.GetResponseObject(), Is.False, "Untrusted account is Trusted");
        }
    }

    class GetUsersCountByPartnerId : BaseTest
    {
        [Test]
        [Description("Get users count by partner id.")]
        [Category("ClientAccountResource"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetUsersCountByPartnerIdTest()
        {
            string partnerId = "NewTestPartner";
            var getUsersCountForAccount = lykkeApi.ClientAccount.ClientAccount
                .GetUsersCountByPartnerId(partnerId);
            Assert.That(getUsersCountForAccount.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getUsersCountForAccount.GetResponseObject(), Is.TypeOf<int>());

            int usersCount = getUsersCountForAccount.GetResponseObject();

            var accountReg = new AccountRegistrationModel().GetTestModel();
            accountReg.PartnerId = partnerId;
            var account = lykkeApi.Registration.PostRegistration(accountReg).Account;

            getUsersCountForAccount = lykkeApi.ClientAccount.ClientAccount
                .GetUsersCountByPartnerId(partnerId);
            Assert.That(getUsersCountForAccount.GetResponseObject(), Is.EqualTo(usersCount + 1));
        }
    }
}
