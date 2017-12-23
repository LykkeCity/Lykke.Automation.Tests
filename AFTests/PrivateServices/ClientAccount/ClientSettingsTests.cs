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
    class WithNewUser : PrivateApiBaseTest
    {
        protected ClientAccountInformation ClientAccount;
        protected ClientSettings clientSettings;

        [OneTimeSetUp]
        public void CreateClientAndApi()
        {
            ClientRegistrationModel clientRegistration = new ClientRegistrationModel().GetTestModel();
            ClientAccount = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration).GetResponseObject();

            clientSettings = lykkeApi.ClientAccount.ClientSettings;
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
            var postAppUsage = clientSettings.PostAppUsage(new AppUsageModel
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
            var getAppUsage = clientSettings.GetAppUsage(ClientAccount.Id);
            Assert.That(getAppUsage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getAppUsage.GetResponseObject().LastUsedGraphPeriod,
                Is.EqualTo(graphPeriod));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteAppUsageTest()
        {
            var deleteGraphPeriod = clientSettings.DeleteAppUsage(ClientAccount.Id);
            Assert.That(deleteGraphPeriod.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clientSettings.GetAppUsage(ClientAccount.Id)
                .GetResponseObject().LastUsedGraphPeriod, Is.EqualTo(defaultGraphPeriod));
        }
    }

    class AssetPairsInvertedTests : WithNewUser
    {
        string asset1 = "BTC", asset2 = "USD";

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostAssetPairsInvertedTest()
        {
            var postAssetPairsInverted = clientSettings.PostAssetPairsInverted(
                new AssetPairsInvertedModel
                {
                    InvertedAssetIds = new List<string>() { asset1, asset2 },
                    ClientId = ClientAccount.Id
                });
            Assert.That(postAssetPairsInverted.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetAssetPairsInvertedTest()
        {
            var getAssetPairsInverted = clientSettings.GetAssetPairsInverted(ClientAccount.Id);
            Assert.That(getAssetPairsInverted.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getAssetPairsInverted.GetResponseObject().InvertedAssetIds,
                Has.Count.EqualTo(2).And.Contain(asset1).And.Contain(asset2));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteAssetPairsInvertedTest()
        {
            var deleteAssetPairsInverted = clientSettings.DeleteAssetPairsInverted(ClientAccount.Id);
            Assert.That(deleteAssetPairsInverted.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clientSettings.GetAssetPairsInverted(ClientAccount.Id)
                .GetResponseObject().InvertedAssetIds, Is.Empty);
        }
    }

    class LastBaseAssetsIosTests : WithNewUser
    {
        string asset1 = "BTC", asset2 = "USD";

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostLastBaseAssetsIosTest()
        {
            var postLastBaseAssetsIos = clientSettings.PostLastBaseAssetsIos(
                new LastBaseAssetsIosModel()
                {
                    BaseAssets = new List<string>() { asset1, asset2},
                    ClientId = ClientAccount.Id
                });
            Assert.That(postLastBaseAssetsIos.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetLastBaseAssetsIosTest()
        {
            var getLastBaseAssetsIos = clientSettings.GetLastBaseAssetsIos(ClientAccount.Id);
            Assert.That(getLastBaseAssetsIos.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getLastBaseAssetsIos.GetResponseObject().BaseAssets,
                Has.Count.EqualTo(2).And.Contain(asset1).And.Contain(asset2));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteLastBaseAssetsIosTest()
        {
            var deleteLastBaseAssetsIos = clientSettings.DeleteLastBaseAssetsIos(ClientAccount.Id);
            Assert.That(deleteLastBaseAssetsIos.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clientSettings.GetLastBaseAssetsIos(ClientAccount.Id)
                .GetResponseObject().BaseAssets, Is.Empty);
        }
    }

    class LastBaseAssetsOtherTests : WithNewUser
    {
        string asset1 = "USD", asset2 = "BTC";

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostLastBaseAssetsOtherTest()
        {
            var postLastBaseAssetsOther = clientSettings.PostLastBaseAssetsOther(
                new LastBaseAssetsOtherModel()
                {
                    BaseAssets = new List<string>() { asset1, asset2 },
                    ClientId = ClientAccount.Id
                });
            Assert.That(postLastBaseAssetsOther.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetLastBaseAssetsOtherTest()
        {
            var getLastBaseAssetsOther = clientSettings.GetLastBaseAssetsOther(ClientAccount.Id);
            Assert.That(getLastBaseAssetsOther.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getLastBaseAssetsOther.GetResponseObject().BaseAssets,
                Has.Count.EqualTo(2).And.Contain(asset1).And.Contain(asset2));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteLastBaseAssetsOtherTest()
        {
            var deleteLastBaseAssetsOther = clientSettings.DeleteLastBaseAssetsOther(ClientAccount.Id);
            Assert.That(deleteLastBaseAssetsOther.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clientSettings.GetLastBaseAssetsOther(ClientAccount.Id)
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
            var postRefundAddress = clientSettings.PostRefundAddress(
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
            var getRefundAddress = clientSettings.GetRefundAddress(ClientAccount.Id);
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
            var deleteRefundAddress = clientSettings.DeleteRefundAddress(ClientAccount.Id);
            Assert.That(deleteRefundAddress.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clientSettings.GetRefundAddress(ClientAccount.Id)
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
            var postPushNotification = clientSettings.PostPushNotification(
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
            var getPushNotification = clientSettings.GetPushNotification(ClientAccount.Id);
            Assert.That(getPushNotification.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPushNotification.GetResponseObject().Enabled, Is.EqualTo(enabled));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeletePushNotification()
        {
            var deletePushNotification = clientSettings.DeletePushNotification(ClientAccount.Id);
            Assert.That(deletePushNotification.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clientSettings.GetPushNotification(ClientAccount.Id)
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
            var postMyLykke = clientSettings.PostMyLykke(new MyLykkeModel()
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
            var getMyLykke = clientSettings.GetMyLykke(ClientAccount.Id);
            Assert.That(getMyLykke.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getMyLykke.GetResponseObject().MyLykkeEnabled, Is.EqualTo(myLykkeEnabled));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteMyLykkeTest()
        {
            var deleteMyLykke = clientSettings.DeleteMyLykke(ClientAccount.Id);
            Assert.That(deleteMyLykke.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clientSettings.GetMyLykke(ClientAccount.Id)
                .GetResponseObject().MyLykkeEnabled, Is.Null);
        }
    }

    class BackupTest: WithNewUser
    {
        bool backupDone = true;
        bool defaultBackupDone = false;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostBackupTest()
        {
            var postBackup = clientSettings.PostBackup(new BackupModel()
            {
                BackupDone = backupDone,
                ClientId = ClientAccount.Id
            });
            Assert.That(postBackup.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetBackupTest()
        {
            var getBackup = clientSettings.GetBackup(ClientAccount.Id);
            Assert.That(getBackup.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getBackup.GetResponseObject().BackupDone, Is.EqualTo(backupDone));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteBackupTest()
        {
            var deleteBackup = clientSettings.DeleteBackup(ClientAccount.Id);
            Assert.That(deleteBackup.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clientSettings.GetBackup(ClientAccount.Id)
                .GetResponseObject().BackupDone, Is.EqualTo(defaultBackupDone));
        }
    }

    class SmsTest : WithNewUser
    {
        bool useAlternativeProvider = true;
        bool defaultUeAlternativeProvider = false;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostSmsTest()
        {
            var postSms = clientSettings.PostSms(new SmsModel()
            {
                UseAlternativeProvider = useAlternativeProvider,
                ClientId = ClientAccount.Id
            });
            Assert.That(postSms.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetSmsTest()
        {
            var getSms = clientSettings.GetSms(ClientAccount.Id);
            Assert.That(getSms.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getSms.GetResponseObject().UseAlternativeProvider, Is.EqualTo(useAlternativeProvider));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteSmsTest()
        {
            var deleteSms = clientSettings.DeleteSms(ClientAccount.Id);
            Assert.That(deleteSms.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clientSettings.GetSms(ClientAccount.Id)
                .GetResponseObject().UseAlternativeProvider, Is.EqualTo(defaultUeAlternativeProvider));
        }
    }

    class HashedPwd : WithNewUser
    {
        bool isPwdHashed = true;
        bool defaultIsPwdHashed = false;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostHashedPwdTest()
        {
            var postHashedPwd = clientSettings.PostHashedPwd(new HashedPwdModel()
            {
                IsPwdHashed = isPwdHashed,
                ClientId = ClientAccount.Id
            });
            Assert.That(postHashedPwd.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetHashedPwdTest()
        {
            var getHashedPwd = clientSettings.GetHashedPwd(ClientAccount.Id);
            Assert.That(getHashedPwd.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getHashedPwd.GetResponseObject().IsPwdHashed, Is.EqualTo(isPwdHashed));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteHashedPwdTest()
        {
            var deleteHashedPwd = clientSettings.DeleteHashedPwd(ClientAccount.Id);
            Assert.That(deleteHashedPwd.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clientSettings.GetHashedPwd(ClientAccount.Id)
                .GetResponseObject().IsPwdHashed, Is.EqualTo(defaultIsPwdHashed));
        }
    }

    class CashOutBlockTest : WithNewUser
    {
        bool cashOutBlocked = true;
        bool defaultCashOutBlocked = false;
        bool tradesBlocked = true;
        bool defaultTradesBlocked = false;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostCashOutBlockTest()
        {
            var postCashOutBlock = clientSettings.PostCashOutBlock(new CashOutBlockModel()
            {
                CashOutBlocked = cashOutBlocked,
                TradesBlocked = tradesBlocked,
                ClientId = ClientAccount.Id
            });
            Assert.That(postCashOutBlock.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetCashOutBlockTest()
        {
            var getCashOutBlock = clientSettings.GetCashOutBlock(ClientAccount.Id);
            Assert.That(getCashOutBlock.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getCashOutBlock.GetResponseObject().CashOutBlocked, Is.EqualTo(cashOutBlocked));
            Assert.That(getCashOutBlock.GetResponseObject().TradesBlocked, Is.EqualTo(tradesBlocked));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteCashOutBlockTest()
        {
            var deleteCashOutBLock = clientSettings.DeleteCashOutBlock(ClientAccount.Id);
            Assert.That(deleteCashOutBLock.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var getCashOutBlock = clientSettings.GetCashOutBlock(ClientAccount.Id);
            Assert.That(getCashOutBlock.GetResponseObject().CashOutBlocked, Is.EqualTo(defaultCashOutBlocked));
            Assert.That(getCashOutBlock.GetResponseObject().TradesBlocked, Is.EqualTo(defaultTradesBlocked));
        }
    }

    class MarginEnabledTest : WithNewUser
    {
        bool enabled = false;
        bool defaultEnabled = true;
        bool enabledLive = true;
        bool defaultEnadledLive = false;
        bool termsOfUseAgreed = true;
        bool defaultTermsOfUseAgreed = false;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostMarginEnabledTest()
        {
            var postMarginEnabled = clientSettings.PostMarginEnabled(new MarginEnabledModel()
            {
                Enabled = enabled,
                EnabledLive = enabledLive,
                TermsOfUseAgreed = termsOfUseAgreed,
                ClientId = ClientAccount.Id
            });
            Assert.That(postMarginEnabled.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetMarginEnabledTest()
        {
            var getMarginEnabled = clientSettings.GetMarginEnabled(ClientAccount.Id);
            Assert.That(getMarginEnabled.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var marginEnabled = getMarginEnabled.GetResponseObject();
            Assert.That(marginEnabled.Enabled, Is.EqualTo(enabled));
            Assert.That(marginEnabled.EnabledLive, Is.EqualTo(enabledLive));
            Assert.That(marginEnabled.TermsOfUseAgreed, Is.EqualTo(termsOfUseAgreed));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteMarginEnabledTest()
        {
            var deleteMarginEnabled = clientSettings.DeleteMarginEnabled(ClientAccount.Id);
            Assert.That(deleteMarginEnabled.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var marginEnabled = clientSettings.GetMarginEnabled(ClientAccount.Id).GetResponseObject();
            Assert.That(marginEnabled.Enabled, Is.EqualTo(defaultEnabled));
            Assert.That(marginEnabled.EnabledLive, Is.EqualTo(defaultEnadledLive));
            Assert.That(marginEnabled.TermsOfUseAgreed, Is.EqualTo(defaultTermsOfUseAgreed));
        }
    }

    class IsUsaUserTest : WithNewUser
    {
        bool isUsaUser = true;
        bool defaultIsUsaUser = false;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostIsUsaUserTest()
        {
            var postIsUsaUser = clientSettings.PostIsUsaUser(new IsUsaUserModel()
            {
                IsUSA = isUsaUser,
                ClientId = ClientAccount.Id
            });
            Assert.That(postIsUsaUser.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetIsUsaUserTest()
        {
            var getIsUsaUser = clientSettings.GetIsUsaUser(ClientAccount.Id);
            Assert.That(getIsUsaUser.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getIsUsaUser.GetResponseObject().IsUSA, Is.EqualTo(isUsaUser));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteIsUsaUserTest()
        {
            var deleteIsUsaUser = clientSettings.DeleteIsUsaUser(ClientAccount.Id);
            Assert.That(deleteIsUsaUser.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(clientSettings.GetIsUsaUser(ClientAccount.Id)
                .GetResponseObject().IsUSA, Is.EqualTo(defaultIsUsaUser));
        }
    }
}
