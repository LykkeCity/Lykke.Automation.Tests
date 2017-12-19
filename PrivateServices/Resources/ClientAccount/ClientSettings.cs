using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using TestsCore.RestRequests.Interfaces;

namespace LykkeAutomationPrivate.Resources.ClientAccountResource
{
    public class ClientSettings : ClientAccountBase
    {
        #region AppUsage
        public IResponse<AppUsageSettings> GetAppUsage(string clientId)
        {
            return Request.Get("/api/ClientSettings/AppUsage").AddQueryParameter("clientId", clientId)
                .Build().Execute<AppUsageSettings>();
        }

        public IResponse PostAppUsage(AppUsageModel appUsage)
        {
            return Request.Post("/api/ClientSettings/AppUsage").AddJsonBody(appUsage)
                .Build().Execute();
        }

        public IResponse DeleteAppUsage(string clientId)
        {
            return Request.Delete("/api/ClientSettings/AppUsage").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region AssetPairsInverted
        public IResponse<AssetPairsInvertedSettings> GetAssetPairsInverted(string clientId)
        {
            return Request.Get("/api/ClientSettings/AssetPairsInverted").AddQueryParameter("clientId", clientId)
                .Build().Execute<AssetPairsInvertedSettings>();
        }

        public IResponse PostAssetPairsInverted(AssetPairsInvertedModel assetPairsInverted)
        {
            return Request.Post("/api/ClientSettings/AssetPairsInverted").AddJsonBody(assetPairsInverted)
                .Build().Execute();
        }

        public IResponse DeleteAssetPairsInverted(string clientId)
        {
            return Request.Delete("/api/ClientSettings/AssetPairsInverted").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region LastBaseAssetsIos
        public IResponse<LastBaseAssetsIos> GetLastBaseAssetsIos(string clientId)
        {
            return Request.Get("/api/ClientSettings/LastBaseAssetsIos").AddQueryParameter("clientId", clientId)
                .Build().Execute<LastBaseAssetsIos>();
        }

        public IResponse PostLastBaseAssetsIos(LastBaseAssetsIosModel lastBaseAssetsIos)
        {
            return Request.Post("/api/ClientSettings/LastBaseAssetsIos").AddJsonBody(lastBaseAssetsIos)
                .Build().Execute();
        }

        public IResponse DeleteLastBaseAssetsIos(string clientId)
        {
            return Request.Delete("/api/ClientSettings/LastBaseAssetsIos").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region LastBaseAssetsOther
        public IResponse<LastBaseAssetsOther> GetLastBaseAssetsOther(string clientId)
        {
            return Request.Get("/api/ClientSettings/LastBaseAssetsOther").AddQueryParameter("clientId", clientId)
                .Build().Execute<LastBaseAssetsOther>();
        }

        public IResponse PostLastBaseAssetsOther(LastBaseAssetsOtherModel lastBaseAssetsOther)
        {
            return Request.Post("/api/ClientSettings/LastBaseAssetsOther").AddJsonBody(lastBaseAssetsOther)
                .Build().Execute();
        }

        public IResponse DeleteLastBaseAssetsOther(string clientId)
        {
            return Request.Delete("/api/ClientSettings/LastBaseAssetsOther").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion

        #region RefundAddress
        public IResponse<RefundAddressSettings> GetRefundAddress(string clientId)
        {
            return Request.Get("/api/ClientSettings/RefundAddress").AddQueryParameter("clientId", clientId)
                .Build().Execute<RefundAddressSettings>();
        }

        public IResponse PostRefundAddress(RefundAddressModel refundAddress)
        {
            return Request.Post("/api/ClientSettings/RefundAddress").AddJsonBody(refundAddress)
                .Build().Execute();
        }

        public IResponse DeleteRefundAddress(string clientId)
        {
            return Request.Delete("/api/ClientSettings/RefundAddress").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region PushNotification
        public IResponse<PushNotificationsSettings> GetPushNotification(string clientId)
        {
            return Request.Get("/api/ClientSettings/PushNotification").AddQueryParameter("clientId", clientId)
                .Build().Execute<PushNotificationsSettings>();
        }

        public IResponse PostPushNotification(PushNotificationModel pushNotification)
        {
            return Request.Post("/api/ClientSettings/PushNotification").AddJsonBody(pushNotification)
                .Build().Execute();
        }

        public IResponse DeletePushNotification(string clientId)
        {
            return Request.Delete("/api/ClientSettings/PushNotification").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region MyLykke
        public IResponse<MyLykkeSettings> GetMyLykke(string clientId)
        {
            return Request.Get("/api/ClientSettings/MyLykke").AddQueryParameter("clientId", clientId)
                .Build().Execute<MyLykkeSettings>();
        }

        public IResponse PostMyLykke(MyLykkeModel myLykke)
        {
            return Request.Post("/api/ClientSettings/MyLykke").AddJsonBody(myLykke)
                .Build().Execute();
        }

        public IResponse DeleteMyLykke(string clientId)
        {
            return Request.Delete("/api/ClientSettings/MyLykke").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region Backup
        public IResponse<BackupSettings> GetBackup(string clientId)
        {
            return Request.Get("/api/ClientSettings/Backup").AddQueryParameter("clientId", clientId)
                .Build().Execute<BackupSettings>();
        }

        public IResponse PostBackup(BackupModel backup)
        {
            return Request.Post("/api/ClientSettings/Backup").AddJsonBody(backup)
                .Build().Execute();
        }

        public IResponse DeleteBackup(string clientId)
        {
            return Request.Delete("/api/ClientSettings/Backup").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region Sms
        public IResponse<SmsSettings> GetSms(string clientId)
        {
            return Request.Get("/api/ClientSettings/Sms").AddQueryParameter("clientId", clientId)
                .Build().Execute<SmsSettings>();
        }

        public IResponse PostSms(SmsModel sms)
        {
            return Request.Post("/api/ClientSettings/Sms").AddJsonBody(sms)
                .Build().Execute();
        }

        public IResponse DeleteSms(string clientId)
        {
            return Request.Delete("/api/ClientSettings/Sms").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region HashedPwd
        public IResponse<HashedPwdSettings> GetHashedPwd(string clientId)
        {
            return Request.Get("/api/ClientSettings/HashedPwd").AddQueryParameter("clientId", clientId)
                .Build().Execute<HashedPwdSettings>();
        }

        public IResponse PostHashedPwd(HashedPwdModel hashedPwd)
        {
            return Request.Post("/api/ClientSettings/HashedPwd").AddJsonBody(hashedPwd)
                .Build().Execute();
        }

        public IResponse DeleteHashedPwd(string clientId)
        {
            return Request.Delete("/api/ClientSettings/HashedPwd").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region CashOutBlock
        public IResponse<CashOutBlockSettings> GetCashOutBlock(string clientId)
        {
            return Request.Get("/api/ClientSettings/CashOutBlock").AddQueryParameter("clientId", clientId)
                .Build().Execute<CashOutBlockSettings>();
        }

        public IResponse PostCashOutBlock(CashOutBlockModel cashOutBlock)
        {
            return Request.Post("/api/ClientSettings/CashOutBlock").AddJsonBody(cashOutBlock)
                .Build().Execute();
        }

        public IResponse DeleteCashOutBlock(string clientId)
        {
            return Request.Delete("/api/ClientSettings/CashOutBlock").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region MarginEnabled
        public IResponse<MarginEnabledSettings> GetMarginEnabled(string clientId)
        {
            return Request.Get("/api/ClientSettings/MarginEnabled").AddQueryParameter("clientId", clientId)
                .Build().Execute<MarginEnabledSettings>();
        }

        public IResponse PostMarginEnabled(MarginEnabledModel marginEnabled)
        {
            return Request.Post("/api/ClientSettings/MarginEnabled").AddJsonBody(marginEnabled)
                .Build().Execute();
        }

        public IResponse DeleteMarginEnabled(string clientId)
        {
            return Request.Delete("/api/ClientSettings/MarginEnabled").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region IsUsaUser
        public IResponse<IsUsaUserSettings> GetIsUsaUser(string clientId)
        {
            return Request.Get("/api/ClientSettings/IsUsaUser").AddQueryParameter("clientId", clientId)
                .Build().Execute<IsUsaUserSettings>();
        }

        public IResponse PostIsUsaUser(IsUsaUserModel isUsaUser)
        {
            return Request.Post("/api/ClientSettings/IsUsaUser").AddJsonBody(isUsaUser)
                .Build().Execute();
        }

        public IResponse DeleteIsUsaUser(string clientId)
        {
            return Request.Delete("/api/ClientSettings/IsUsaUser").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region IsOffchainUser
        public IResponse<IsOffchainUserSettings> GetIsOffchainUser(string clientId)
        {
            return Request.Get("/api/ClientSettings/IsOffchainUser").AddQueryParameter("clientId", clientId)
                .Build().Execute<IsOffchainUserSettings>();
        }

        public IResponse PostIsOffchainUser(IsOffchainUserModel isOffchainUser)
        {
            return Request.Post("/api/ClientSettings/IsOffchainUser").AddJsonBody(isOffchainUser)
                .Build().Execute();
        }

        public IResponse DeleteIsOffchainUser(string clientId)
        {
            return Request.Delete("/api/ClientSettings/IsOffchainUser").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region NeedReinit
        public IResponse<NeedReinitSettings> GetNeedReinit(string clientId)
        {
            return Request.Get("/api/ClientSettings/NeedReinit").AddQueryParameter("clientId", clientId)
                .Build().Execute<NeedReinitSettings>();
        }

        public IResponse PostNeedReinit(NeedReinitModel needReinit)
        {
            return Request.Post("/api/ClientSettings/NeedReinit").AddJsonBody(needReinit)
                .Build().Execute();
        }

        public IResponse DeleteNeedReinit(string clientId)
        {
            return Request.Delete("/api/ClientSettings/NeedReinit").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region IsLimitOrdersAvailable
        public IResponse<IsLimitOrdersAvailable> GetIsLimitOrdersAvailable(string clientId)
        {
            return Request.Get("/api/ClientSettings/IsLimitOrdersAvailable").AddQueryParameter("clientId", clientId)
                .Build().Execute<IsLimitOrdersAvailable>();
        }

        public IResponse PostIsLimitOrdersAvailable(IsLimitOrdersAvailableModel isLimitOrdersAvailable)
        {
            return Request.Post("/api/ClientSettings/IsLimitOrdersAvailable").AddJsonBody(isLimitOrdersAvailable)
                .Build().Execute();
        }

        public IResponse DeleteIsLimitOrdersAvailable(string clientId)
        {
            return Request.Delete("/api/ClientSettings/IsLimitOrdersAvailable").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
        #region BaseAsset
        public IResponse<BaseAsset> GetBaseAsset(string clientId)
        {
            return Request.Get("/api/ClientSettings/BaseAsset").AddQueryParameter("clientId", clientId)
                .Build().Execute<BaseAsset>();
        }

        public IResponse PostBaseAsset(BaseAssetModel baseAsset)
        {
            return Request.Post("/api/ClientSettings/BaseAsset").AddJsonBody(baseAsset)
                .Build().Execute();
        }

        public IResponse DeleteBaseAsset(string clientId)
        {
            return Request.Delete("/api/ClientSettings/BaseAsset").AddQueryParameter("clientId", clientId)
                .Build().Execute();
        }
        #endregion
    }
}
