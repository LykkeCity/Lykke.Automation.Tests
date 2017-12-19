using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.DataGenerators;

namespace LykkeAutomationPrivate.Tests.ClientAccount
{
    class WithNewUser : BaseTest
    {
        protected ClientAccountInformation ClientAccount;

        [OneTimeSetUp]
        public void CreateClient()
        {
            ClientRegistrationModel clientRegistration = new ClientRegistrationModel().GetTestModel();
            ClientAccount = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration).GetResponseObject();
        }

        [OneTimeTearDown]
        public void DeleteClient()
        {
            lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(ClientAccount.Id);
        }
    }


    class AppUsageTests : WithNewUser
    {
        private readonly string graphPeriod = "1D";
        private readonly string defaultGraphPeriod = "1H";

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostAppUsageTest()
        {
            var postAppUsage = lykkeApi.ClientAccount.ClientSettings.PostAppUsage(new AppUsageModel
            {
                LastUsedGraphPeriod = graphPeriod,
                ClientId = ClientAccount.Id
            });
            Assert.That(postAppUsage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetAppUsageTest()
        {
            var getAppUsage = lykkeApi.ClientAccount.ClientSettings.GetAppUsage(ClientAccount.Id);
            Assert.That(getAppUsage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getAppUsage.GetResponseObject().LastUsedGraphPeriod,
                Is.EqualTo(graphPeriod));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteAppUsageTest()
        {
            var deleteGraphPeriod = lykkeApi.ClientAccount.ClientSettings.DeleteAppUsage(ClientAccount.Id);
            Assert.That(deleteGraphPeriod.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(lykkeApi.ClientAccount.ClientSettings.GetAppUsage(ClientAccount.Id)
                .GetResponseObject().LastUsedGraphPeriod, Is.EqualTo(defaultGraphPeriod));
        }
    }

    class AssetPairsInvertedTests : WithNewUser
    {
        string assetPair1 = "BTCUSD", assetPair2 = "BTCEUR";

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostAssetPairsInvertedTest()
        {
            var postAssetPairsInverted = lykkeApi.ClientAccount.ClientSettings.PostAssetPairsInverted(
                new AssetPairsInvertedModel
                {
                    InvertedAssetIds = new List<string>() { assetPair1, assetPair2 },
                    ClientId = ClientAccount.Id
                });
            Assert.That(postAssetPairsInverted.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetAssetPairsInvertedTest()
        {
            var getAssetPairsInverted = lykkeApi.ClientAccount.ClientSettings.GetAssetPairsInverted(ClientAccount.Id);
            Assert.That(getAssetPairsInverted.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getAssetPairsInverted.GetResponseObject().InvertedAssetIds,
                Has.Count.EqualTo(2).And.Contain(assetPair1).And.Contain(assetPair2));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteAssetPairsInvertedTest()
        {
            var deleteAssetPairsInverted = lykkeApi.ClientAccount.ClientSettings.DeleteAssetPairsInverted(ClientAccount.Id);
            Assert.That(deleteAssetPairsInverted.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(lykkeApi.ClientAccount.ClientSettings.GetAssetPairsInverted(ClientAccount.Id)
                .GetResponseObject().InvertedAssetIds, Is.Empty);
        }
    }

    class LastBaseAssetsIosTests : WithNewUser
    {
        string assetPair1 = "BTCUSD", assetPair2 = "BTCEUR";

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostLastBaseAssetsIosTest()
        {
            var postLastBaseAssetsIos = lykkeApi.ClientAccount.ClientSettings.PostLastBaseAssetsIos(
                new LastBaseAssetsIosModel()
                {
                    BaseAssets = new List<string>() { assetPair1, assetPair2},
                    ClientId = ClientAccount.Id
                });
            Assert.That(postLastBaseAssetsIos.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetLastBaseAssetsIosTest()
        {
            var getLastBaseAssetsIos = lykkeApi.ClientAccount.ClientSettings.GetLastBaseAssetsIos(ClientAccount.Id);
            Assert.That(getLastBaseAssetsIos.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getLastBaseAssetsIos.GetResponseObject().BaseAssets,
                Has.Count.EqualTo(2).And.Contain(assetPair1).And.Contain(assetPair2));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteLastBaseAssetsIosTest()
        {
            var deleteLastBaseAssetsIos = lykkeApi.ClientAccount.ClientSettings.DeleteLastBaseAssetsIos(ClientAccount.Id);
            Assert.That(deleteLastBaseAssetsIos.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(lykkeApi.ClientAccount.ClientSettings.GetLastBaseAssetsIos(ClientAccount.Id)
                .GetResponseObject().BaseAssets, Is.Empty);
        }
    }

    class LastBaseAssetsOtherTests : WithNewUser
    {
        string assetPair1 = "BTCUSD", assetPair2 = "BTCEUR";

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostLastBaseAssetsOtherTest()
        {
            var postLastBaseAssetsOther = lykkeApi.ClientAccount.ClientSettings.PostLastBaseAssetsOther(
                new LastBaseAssetsOtherModel()
                {
                    BaseAssets = new List<string>() { assetPair1, assetPair2 },
                    ClientId = ClientAccount.Id
                });
            Assert.That(postLastBaseAssetsOther.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetLastBaseAssetsOtherTest()
        {
            var getLastBaseAssetsOther = lykkeApi.ClientAccount.ClientSettings.GetLastBaseAssetsOther(ClientAccount.Id);
            Assert.That(getLastBaseAssetsOther.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getLastBaseAssetsOther.GetResponseObject().BaseAssets,
                Has.Count.EqualTo(2).And.Contain(assetPair1).And.Contain(assetPair2));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteLastBaseAssetsOtherTest()
        {
            var deleteLastBaseAssetsOther = lykkeApi.ClientAccount.ClientSettings.DeleteLastBaseAssetsOther(ClientAccount.Id);
            Assert.That(deleteLastBaseAssetsOther.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(lykkeApi.ClientAccount.ClientSettings.GetLastBaseAssetsOther(ClientAccount.Id)
                .GetResponseObject().BaseAssets, Is.Empty);
        }
    }

    class RefundAddressTests : WithNewUser
    {
        string address = Guid.NewGuid().ToString();
        int validDays = 15;
        bool sendAutomatically = true;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostRefundAddressTest()
        {
            var postRefundAddress = lykkeApi.ClientAccount.ClientSettings.PostRefundAddress(
                new RefundAddressModel()
                {
                    Address = address,
                    ValidDays = validDays,
                    SendAutomatically = sendAutomatically,
                    ClientId = ClientAccount.Id
                });
            Assert.That(postRefundAddress.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetRefundAddressTest()
        {
            var getRefundAddress = lykkeApi.ClientAccount.ClientSettings.GetRefundAddress(ClientAccount.Id);
            Assert.That(getRefundAddress.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var refundAddressSettings = getRefundAddress.GetResponseObject();
            Assert.Multiple(() =>
            {
                Assert.That(refundAddressSettings.Address, Is.EqualTo(address));
                Assert.That(refundAddressSettings.ValidDays, Is.EqualTo(validDays));
                Assert.That(refundAddressSettings.SendAutomatically, Is.EqualTo(sendAutomatically));
            });
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteRefundAddressTest()
        {
            var deleteRefundAddress = lykkeApi.ClientAccount.ClientSettings.DeleteRefundAddress(ClientAccount.Id);
            Assert.That(deleteRefundAddress.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(lykkeApi.ClientAccount.ClientSettings.GetRefundAddress(ClientAccount.Id)
                .GetResponseObject().Address, Is.Empty);
        }
    }

    class PushNotificationTests : WithNewUser
    {
        bool enabled = false;
        bool defaultEnabled = true;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostPushNotificationTest()
        {
            var postPushNotification = lykkeApi.ClientAccount.ClientSettings.PostPushNotification(
                new PushNotificationModel()
                {
                    Enabled = enabled,
                    ClientId = ClientAccount.Id
                });
            Assert.That(postPushNotification.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetPushNotification()
        {
            var getPushNotification = lykkeApi.ClientAccount.ClientSettings.GetPushNotification(ClientAccount.Id);
            Assert.That(getPushNotification.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPushNotification.GetResponseObject().Enabled, Is.EqualTo(enabled));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeletePushNotification()
        {
            var deletePushNotification = lykkeApi.ClientAccount.ClientSettings.DeletePushNotification(ClientAccount.Id);
            Assert.That(deletePushNotification.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(lykkeApi.ClientAccount.ClientSettings.GetPushNotification(ClientAccount.Id)
                .GetResponseObject().Enabled, Is.EqualTo(defaultEnabled));
        }
    }

    class MyLykkeTest : WithNewUser
    {
        bool myLykkeEnabled = true;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostMyLykkeTest()
        {
            var postMyLykke = lykkeApi.ClientAccount.ClientSettings.PostMyLykke(new MyLykkeModel()
            {
                MyLykkeEnabled = myLykkeEnabled,
                ClientId = ClientAccount.Id
            });
            Assert.That(postMyLykke.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetMyLykkeTest()
        {
            var getMyLykke = lykkeApi.ClientAccount.ClientSettings.GetMyLykke(ClientAccount.Id);
            Assert.That(getMyLykke.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getMyLykke.GetResponseObject().MyLykkeEnabled, Is.EqualTo(myLykkeEnabled));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteMyLykkeTest()
        {
            var deleteMyLykke = lykkeApi.ClientAccount.ClientSettings.DeleteMyLykke(ClientAccount.Id);
            Assert.That(deleteMyLykke.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(lykkeApi.ClientAccount.ClientSettings.GetMyLykke(ClientAccount.Id)
                .GetResponseObject().MyLykkeEnabled, Is.Null);
        }
    }
}
