using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.DataGenerators;

namespace LykkeAutomationPrivate.Tests.ClientAccount
{
    class ClientSettingsTests : BaseTest
    {
        private ClientAccountInformation ClientAccount;

        [OneTimeSetUp]
        public void CreateClient()
        {
            ClientRegistrationModel clientRegistration = new ClientRegistrationModel().GetTestModel();
            ClientAccount = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration).GetResponseObject();
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetAppUsageTest()
        {
            lykkeApi.ClientAccount.ClientSettings.GetAppUsage(ClientAccount.Id);
        }

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostAppUsageTest()
        {
            lykkeApi.ClientAccount.ClientSettings.PostAppUsage(new AppUsageModel
            {
                LastUsedGraphPeriod = "1D",
                ClientId = ClientAccount.Id
            });
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteAppUsageTest()
        {
            lykkeApi.ClientAccount.ClientSettings.DeleteAppUsage(ClientAccount.Id);
        }
    }
}
