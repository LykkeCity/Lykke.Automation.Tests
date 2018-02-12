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
        public BaseAssets BaseAssets => new BaseAssets();
        public BcnTransaction BcnTransaction => new BcnTransaction();
        public BcnTransactionByCashOperation BcnTransactionByCashOperation => new BcnTransactionByCashOperation();
        public BcnTransactionByExchange BcnTransactionByExchange => new BcnTransactionByExchange();
        public BcnTransactionByTransfer BcnTransactionByTransfer => new BcnTransactionByTransfer();
        public BitcoinCash BitcoinCash => new BitcoinCash();
        public BlockchainTransaction BlockchainTransaction => new BlockchainTransaction();
        public BroadcastTransaction BroadcastTransaction => new BroadcastTransaction();
        public CashOut CashOut => new CashOut();
        public CashOutSwiftRequest CashOutSwiftRequest => new CashOutSwiftRequest();
        public ClientState ClientState => new ClientState();
        public ChangePinAndPassword ChangePinAndPassword => new ChangePinAndPassword();
        public CheckDocumentsToUpload CheckDocumentsToUpload => new CheckDocumentsToUpload();
        public CheckMobilePhone CheckMobilePhone => new CheckMobilePhone();
        public Client Client => new Client();
        public ClientFirstNameLastName ClientFirstNameLastName => new ClientFirstNameLastName();
        public ClientFullName ClientFullName => new ClientFullName();
        public ClientKeys ClientKeys => new ClientKeys();
        public ClientLog ClientLog => new ClientLog();
        public ClientPhone ClientPhone => new ClientPhone();
        public ClientTrading ClientTrading => new ClientTrading();
        public CountryPhoneCodes CountryPhoneCodes => new CountryPhoneCodes();
        public Dialogs Dialogs => new Dialogs();
        public Dictionary Dictionary => new Dictionary();
        public EmailVerification EmailVerification => new EmailVerification();
        public PinSecurity PinSecurity => new PinSecurity();
        public MyLykkeSettings MyLykkeSettings => new MyLykkeSettings();
        public Dicts Dicts => new Dicts();
        public Issuers Issuers => new Issuers();
        public LimitOrders LimitOrders => new LimitOrders();
        public Wallets Wallets => new Wallets();
        public WatchLists WatchLists => new WatchLists();
        public SignatureVerificationToken SignatureVerificationToken => new SignatureVerificationToken();
    }
}
