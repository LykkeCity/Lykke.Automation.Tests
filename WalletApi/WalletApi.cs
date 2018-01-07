using WalletApi.Api.AuthResource;
using WalletApi.Api.PersonalDataResource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WalletApi.Api.RegistrationResource;
using LykkeAutomation.Api.ApiModels.AccountExistModels;
using WalletApi.Api.ApiResources.AccountExist;
using XUnitTestCommon.RestRequests;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
   public class WalletApi
    {
        protected string URL = "https://api-test.lykkex.net/api";

        protected IRequestBuilder Request => Requests.For(URL);

        public PersonalData PersonalData => new PersonalData();
        public Registration Registration => new Registration();
        public Auth Auth => new Auth();
        public AccountExist AccountExist => new AccountExist();
        public ApplicationInfo.ApplicationInfo ApplicationInfo => new ApplicationInfo.ApplicationInfo();
        public AllAssetPairRates AllAssetPairRates => new AllAssetPairRates();
        public AllAssets AllAssets => new AllAssets();
        public AppSettings AppSettings => new AppSettings();
        public AssetsCategories AssetsCategories => new AssetsCategories();
        public AssetDescription AssetDescription => new AssetDescription();
        public AssetPair AssetPair => new AssetPair();
        public AssetPairDetailedRates AssetPairDetailedRates => new AssetPairDetailedRates();
        public AssetPairs AssetPairs => new AssetPairs();
        public Assets Assets => new Assets();
        public BackupCompleted BackupCompleted => new BackupCompleted();
        public BankCardPaymentUrl BankCardPaymentUrl => new BankCardPaymentUrl();
        public BankCardPaymentUrlFormValues BankCardPaymentUrlFormValues => new BankCardPaymentUrlFormValues();
        public BankTransferRequest BankTransferRequest => new BankTransferRequest();
        public BaseAsset BaseAsset => new BaseAsset();
    }
}
