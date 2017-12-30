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
    class ClientSettingsBaseTest : PrivateApiBaseTest
    {
        protected ClientAccountInformation ClientAccount;
        protected ClientSettings api;

        [OneTimeSetUp]
        public void CreateClientAndApi()
        {
            ClientRegistrationModel clientRegistration = new ClientRegistrationModel().GetTestModel();
            ClientAccount = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration).GetResponseObject();

            api = lykkeApi.ClientAccount.ClientSettings;
        }

        [OneTimeTearDown]
        public void DeleteClient()
        {
            lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(ClientAccount.Id);
        }
    }


    class AppUsageTests : ClientSettingsBaseTest
    {
        private readonly string graphPeriod = "1D";
        private readonly string defaultGraphPeriod = "1H";

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostAppUsageTest()
        {
            var postAppUsage = api.PostAppUsage(new AppUsageModel
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
            var getAppUsage = api.GetAppUsage(ClientAccount.Id);
            Assert.That(getAppUsage.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getAppUsage.GetResponseObject().LastUsedGraphPeriod,
                Is.EqualTo(graphPeriod));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteAppUsageTest()
        {
            var deleteGraphPeriod = api.DeleteAppUsage(ClientAccount.Id);
            Assert.That(deleteGraphPeriod.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetAppUsage(ClientAccount.Id)
                .GetResponseObject().LastUsedGraphPeriod, Is.EqualTo(defaultGraphPeriod));
        }
    }

    class AssetPairsInvertedTests : ClientSettingsBaseTest
    {
        string asset1 = "BTC", asset2 = "USD";

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostAssetPairsInvertedTest()
        {
            var postAssetPairsInverted = api.PostAssetPairsInverted(
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
            var getAssetPairsInverted = api.GetAssetPairsInverted(ClientAccount.Id);
            Assert.That(getAssetPairsInverted.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getAssetPairsInverted.GetResponseObject().InvertedAssetIds,
                Has.Count.EqualTo(2).And.Contain(asset1).And.Contain(asset2));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteAssetPairsInvertedTest()
        {
            var deleteAssetPairsInverted = api.DeleteAssetPairsInverted(ClientAccount.Id);
            Assert.That(deleteAssetPairsInverted.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetAssetPairsInverted(ClientAccount.Id)
                .GetResponseObject().InvertedAssetIds, Is.Empty);
        }
    }

    class LastBaseAssetsIosTests : ClientSettingsBaseTest
    {
        string asset1 = "BTC", asset2 = "USD";

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostLastBaseAssetsIosTest()
        {
            var postLastBaseAssetsIos = api.PostLastBaseAssetsIos(
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
            var getLastBaseAssetsIos = api.GetLastBaseAssetsIos(ClientAccount.Id);
            Assert.That(getLastBaseAssetsIos.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getLastBaseAssetsIos.GetResponseObject().BaseAssets,
                Has.Count.EqualTo(2).And.Contain(asset1).And.Contain(asset2));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteLastBaseAssetsIosTest()
        {
            var deleteLastBaseAssetsIos = api.DeleteLastBaseAssetsIos(ClientAccount.Id);
            Assert.That(deleteLastBaseAssetsIos.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetLastBaseAssetsIos(ClientAccount.Id)
                .GetResponseObject().BaseAssets, Is.Empty);
        }
    }

    class LastBaseAssetsOtherTests : ClientSettingsBaseTest
    {
        string asset1 = "USD", asset2 = "BTC";

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostLastBaseAssetsOtherTest()
        {
            var postLastBaseAssetsOther = api.PostLastBaseAssetsOther(
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
            var getLastBaseAssetsOther = api.GetLastBaseAssetsOther(ClientAccount.Id);
            Assert.That(getLastBaseAssetsOther.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getLastBaseAssetsOther.GetResponseObject().BaseAssets,
                Has.Count.EqualTo(2).And.Contain(asset1).And.Contain(asset2));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteLastBaseAssetsOtherTest()
        {
            var deleteLastBaseAssetsOther = api.DeleteLastBaseAssetsOther(ClientAccount.Id);
            Assert.That(deleteLastBaseAssetsOther.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetLastBaseAssetsOther(ClientAccount.Id)
                .GetResponseObject().BaseAssets, Is.Empty);
        }
    }

    class RefundAddressTests : ClientSettingsBaseTest
    {
        string address = Guid.NewGuid().ToString();
        int validDays = 15;
        bool sendAutomatically = true;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostRefundAddressTest()
        {
            var postRefundAddress = api.PostRefundAddress(
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
            var getRefundAddress = api.GetRefundAddress(ClientAccount.Id);
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
            var deleteRefundAddress = api.DeleteRefundAddress(ClientAccount.Id);
            Assert.That(deleteRefundAddress.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetRefundAddress(ClientAccount.Id)
                .GetResponseObject().Address, Is.Empty);
        }
    }

    class PushNotificationTests : ClientSettingsBaseTest
    {
        bool enabled = false;
        bool defaultEnabled = true;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostPushNotificationTest()
        {
            var postPushNotification = api.PostPushNotification(
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
            var getPushNotification = api.GetPushNotification(ClientAccount.Id);
            Assert.That(getPushNotification.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPushNotification.GetResponseObject().Enabled, Is.EqualTo(enabled));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeletePushNotification()
        {
            var deletePushNotification = api.DeletePushNotification(ClientAccount.Id);
            Assert.That(deletePushNotification.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetPushNotification(ClientAccount.Id)
                .GetResponseObject().Enabled, Is.EqualTo(defaultEnabled));
        }
    }

    class MyLykkeTest : ClientSettingsBaseTest
    {
        bool myLykkeEnabled = true;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostMyLykkeTest()
        {
            var postMyLykke = api.PostMyLykke(new MyLykkeModel()
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
            var getMyLykke = api.GetMyLykke(ClientAccount.Id);
            Assert.That(getMyLykke.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getMyLykke.GetResponseObject().MyLykkeEnabled, Is.EqualTo(myLykkeEnabled));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteMyLykkeTest()
        {
            var deleteMyLykke = api.DeleteMyLykke(ClientAccount.Id);
            Assert.That(deleteMyLykke.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetMyLykke(ClientAccount.Id)
                .GetResponseObject().MyLykkeEnabled, Is.Null);
        }
    }

    class BackupTest: ClientSettingsBaseTest
    {
        bool backupDone = true;
        bool defaultBackupDone = false;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostBackupTest()
        {
            var postBackup = api.PostBackup(new BackupModel()
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
            var getBackup = api.GetBackup(ClientAccount.Id);
            Assert.That(getBackup.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getBackup.GetResponseObject().BackupDone, Is.EqualTo(backupDone));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteBackupTest()
        {
            var deleteBackup = api.DeleteBackup(ClientAccount.Id);
            Assert.That(deleteBackup.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetBackup(ClientAccount.Id)
                .GetResponseObject().BackupDone, Is.EqualTo(defaultBackupDone));
        }
    }

    class SmsTest : ClientSettingsBaseTest
    {
        bool useAlternativeProvider = true;
        bool defaultUeAlternativeProvider = false;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostSmsTest()
        {
            var postSms = api.PostSms(new SmsModel()
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
            var getSms = api.GetSms(ClientAccount.Id);
            Assert.That(getSms.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getSms.GetResponseObject().UseAlternativeProvider, Is.EqualTo(useAlternativeProvider));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteSmsTest()
        {
            var deleteSms = api.DeleteSms(ClientAccount.Id);
            Assert.That(deleteSms.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetSms(ClientAccount.Id)
                .GetResponseObject().UseAlternativeProvider, Is.EqualTo(defaultUeAlternativeProvider));
        }
    }

    class HashedPwd : ClientSettingsBaseTest
    {
        bool isPwdHashed = true;
        bool defaultIsPwdHashed = false;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostHashedPwdTest()
        {
            var postHashedPwd = api.PostHashedPwd(new HashedPwdModel()
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
            var getHashedPwd = api.GetHashedPwd(ClientAccount.Id);
            Assert.That(getHashedPwd.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getHashedPwd.GetResponseObject().IsPwdHashed, Is.EqualTo(isPwdHashed));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteHashedPwdTest()
        {
            var deleteHashedPwd = api.DeleteHashedPwd(ClientAccount.Id);
            Assert.That(deleteHashedPwd.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetHashedPwd(ClientAccount.Id)
                .GetResponseObject().IsPwdHashed, Is.EqualTo(defaultIsPwdHashed));
        }
    }

    class CashOutBlockTest : ClientSettingsBaseTest
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
            var postCashOutBlock = api.PostCashOutBlock(new CashOutBlockModel()
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
            var getCashOutBlock = api.GetCashOutBlock(ClientAccount.Id);
            Assert.That(getCashOutBlock.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getCashOutBlock.GetResponseObject().CashOutBlocked, Is.EqualTo(cashOutBlocked));
            Assert.That(getCashOutBlock.GetResponseObject().TradesBlocked, Is.EqualTo(tradesBlocked));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteCashOutBlockTest()
        {
            var deleteCashOutBLock = api.DeleteCashOutBlock(ClientAccount.Id);
            Assert.That(deleteCashOutBLock.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var getCashOutBlock = api.GetCashOutBlock(ClientAccount.Id);
            Assert.That(getCashOutBlock.GetResponseObject().CashOutBlocked, Is.EqualTo(defaultCashOutBlocked));
            Assert.That(getCashOutBlock.GetResponseObject().TradesBlocked, Is.EqualTo(defaultTradesBlocked));
        }
    }

    class MarginEnabledTest : ClientSettingsBaseTest
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
            var postMarginEnabled = api.PostMarginEnabled(new MarginEnabledModel()
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
            var getMarginEnabled = api.GetMarginEnabled(ClientAccount.Id);
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
            var deleteMarginEnabled = api.DeleteMarginEnabled(ClientAccount.Id);
            Assert.That(deleteMarginEnabled.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var marginEnabled = api.GetMarginEnabled(ClientAccount.Id).GetResponseObject();
            Assert.That(marginEnabled.Enabled, Is.EqualTo(defaultEnabled));
            Assert.That(marginEnabled.EnabledLive, Is.EqualTo(defaultEnadledLive));
            Assert.That(marginEnabled.TermsOfUseAgreed, Is.EqualTo(defaultTermsOfUseAgreed));
        }
    }

    class IsUsaUserTest : ClientSettingsBaseTest
    {
        bool isUsaUser = true;
        bool defaultIsUsaUser = false;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostIsUsaUserTest()
        {
            var postIsUsaUser = api.PostIsUsaUser(new IsUsaUserModel()
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
            var getIsUsaUser = api.GetIsUsaUser(ClientAccount.Id);
            Assert.That(getIsUsaUser.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getIsUsaUser.GetResponseObject().IsUSA, Is.EqualTo(isUsaUser));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteIsUsaUserTest()
        {
            var deleteIsUsaUser = api.DeleteIsUsaUser(ClientAccount.Id);
            Assert.That(deleteIsUsaUser.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetIsUsaUser(ClientAccount.Id)
                .GetResponseObject().IsUSA, Is.EqualTo(defaultIsUsaUser));
        }
    }

    class IsOffchainUserTest : ClientSettingsBaseTest
    {
        bool isOffchain = false;
        bool defaultIsOffchain = true;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostIsOffchainUserTest()
        {
            var postIsOffchainUser = api.PostIsOffchainUser(new IsOffchainUserModel()
            {
                IsOffchain = isOffchain,
                ClientId = ClientAccount.Id
            });
            Assert.That(postIsOffchainUser.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetIsOffchainUserTest()
        {
            var getIsOffchain = api.GetIsOffchainUser(ClientAccount.Id);
            Assert.That(getIsOffchain.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getIsOffchain.GetResponseObject().IsOffchain, Is.EqualTo(isOffchain));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteIsOffchainUserTest()
        {
            var deleteIsOffchainUser = api.DeleteIsOffchainUser(ClientAccount.Id);
            Assert.That(deleteIsOffchainUser.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetIsOffchainUser(ClientAccount.Id)
                .GetResponseObject().IsOffchain, Is.EqualTo(defaultIsOffchain));
        }
    }

    class NeedReinitTest : ClientSettingsBaseTest
    {
        bool needReinit = true;
        bool defaultNeedReinit = false;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostNeedReinitTest()
        {
            var postNeedReinit = api.PostNeedReinit(new NeedReinitModel()
            {
                NeedReinit = needReinit,
                ClientId = ClientAccount.Id
            });
            Assert.That(postNeedReinit.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetNeedReinitTest()
        {
            var getNeedReinit = api.GetNeedReinit(ClientAccount.Id);
            Assert.That(getNeedReinit.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getNeedReinit.GetResponseObject().NeedReinit, Is.EqualTo(needReinit));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteNeedReinitTest()
        {
            var deleteNeedReinit = api.DeleteNeedReinit(ClientAccount.Id);
            Assert.That(deleteNeedReinit.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetNeedReinit(ClientAccount.Id)
                .GetResponseObject().NeedReinit, Is.EqualTo(defaultNeedReinit));
        }
    }

    class IsLimitOrdersAvailableTest : ClientSettingsBaseTest
    {
        bool available = true;
        bool defaultAvailable = false;

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostIsLimitOrdersAvailableTest()
        {
            var postIsLimitOrderAvailable = api.PostIsLimitOrdersAvailable(new IsLimitOrdersAvailableModel()
            {
                Available = available,
                ClientId = ClientAccount.Id
            });
            Assert.That(postIsLimitOrderAvailable.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetIsLimitOrderAvailableTest()
        {
            var getIsLimitOrderAvailable = api.GetIsLimitOrdersAvailable(ClientAccount.Id);
            Assert.That(getIsLimitOrderAvailable.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getIsLimitOrderAvailable.GetResponseObject().Available, Is.EqualTo(available));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteIsLimitOrderAvailableTest()
        {
            var deleteIsLimitOrderAvailable = api.DeleteIsLimitOrdersAvailable(ClientAccount.Id);
            Assert.That(deleteIsLimitOrderAvailable.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetIsLimitOrdersAvailable(ClientAccount.Id)
                .GetResponseObject().Available, Is.EqualTo(defaultAvailable));
        }
    }

    class BaseAsset : ClientSettingsBaseTest
    {
        string baseAsset = "BTC";

        [Test]
        [Order(1)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostBaseAssetTest()
        {
            var postBaseAsset = api.PostBaseAsset(new BaseAssetModel()
            {
                BaseAssetId = baseAsset,
                ClientId = ClientAccount.Id
            });
            Assert.That(postBaseAsset.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetBaseAssetTest()
        {
            var getBaseAsset = api.GetBaseAsset(ClientAccount.Id);
            Assert.That(getBaseAsset.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getBaseAsset.GetResponseObject().BaseAssetId, Is.EqualTo(baseAsset));
        }

        [Test]
        [Order(3)]
        [Category("ClientSettings"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteBaseAssetTest()
        {
            var deleteBaseAsset = api.DeleteBaseAsset(ClientAccount.Id);
            Assert.That(deleteBaseAsset.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetBaseAsset(ClientAccount.Id)
                .GetResponseObject().BaseAssetId, Is.Null);
        }
    }
}
