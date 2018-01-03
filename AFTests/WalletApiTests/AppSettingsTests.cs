using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.ApiModels.RegistrationModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace AFTests.WalletApiTests
{
    class AppSettingsTests
    {
        public class GetAppSettings : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetAppSettingsTest()
            {
                var user = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(user);

                var settings = walletApi.AppSettings.GetAppSettings(registeredClient.GetResponseObject().Result.Token);
                settings.Validate.StatusCode(System.Net.HttpStatusCode.OK);
                Assert.That(settings.GetResponseObject().Result.BaseAsset, Is.Not.Null, "BaseAsset is null");
            }
        }

        public class GetAppSettingsNegative : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("test")]
            [TestCase("1234")]
            [TestCase("!@#$%")]
            [Category("WalletApi")]
            public void GetAppSettingsNegativeTest(string token)
            {
                var settings = walletApi.AppSettings.GetAppSettings(token);
                settings.Validate.StatusCode(System.Net.HttpStatusCode.Unauthorized);
            }
        }
    }
}
