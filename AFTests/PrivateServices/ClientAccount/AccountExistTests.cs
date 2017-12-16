using LykkeAutomationPrivate.DataGenerators;
using LykkeAutomationPrivate.Models.Registration.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using LykkeAutomationPrivate.Models.ClientAccount.Models;

namespace LykkeAutomationPrivate.Tests.ClientAccount
{
    class AccountExistResourseTests : BaseTest
    {
        AccountRegistrationModel existedClient = new AccountRegistrationModel().GetTestModel();
        AccountRegistrationModel nonExistedClient = new AccountRegistrationModel().GetTestModel();

        [OneTimeSetUp]
        public void CreateClient()
        {
            //create new client
            lykkeApi.Registration.PostRegistration(existedClient);
        }

        [Test]
        [Category("AccountExist"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetAccountForExistedClient()
        {
            var accountExistResponceModel = lykkeApi.ClientAccount.AccountExist
                .GetAccountExist(existedClient.Email).GetResponseObject();

            Assert.That(accountExistResponceModel.IsClientAccountExisting, Is.True);
        }

        [Test]
        [Category("AccountExist"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetAccountForNonExistedClient()
        {
            var accountExistResponceModel = lykkeApi.ClientAccount.AccountExist
                .GetAccountExist(nonExistedClient.Email).GetResponseObject();

            Assert.That(accountExistResponceModel.IsClientAccountExisting, Is.False);
        }
    }
}
