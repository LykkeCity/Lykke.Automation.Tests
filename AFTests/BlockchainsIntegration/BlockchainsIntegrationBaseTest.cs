using BlockchainsIntegration.BlockchainWallets;
using BlockchainsIntegration.Api;
using BlockchainsIntegration.Sign;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.Tests;
using Newtonsoft.Json;
using System.IO;
using AFTests.BlockchainsIntegration;
using XUnitTestCommon.TestsCore;
using System.Linq;
using BlockchainsIntegration.Models;
using Lykke.Client.AutorestClient.Models;
using System.Net;

namespace AFTests.BlockchainsIntegrationTests
{
    [NonParallelizable]
    class BlockchainsIntegrationBaseTest : BaseTest
    {
        private static object _lock = new object();

        private static Lazy<BlockchainSpecificModel> _currentSettings
        {
            get
            {
                return new Lazy<BlockchainSpecificModel>
                                    (BlockchainSpecificSettingsFactory.BlockchainSpecificSettings(SpecificBlockchain()));
            }
        }

       protected static string SpecificBlockchain()
       {
            return Environment.GetEnvironmentVariable("BlockchainIntegration") ?? "stellar-v2";// "stellar-v2";//"Zcash"; //"Ripple";// "Dash"; "Litecoin";
       }

        protected static string BlockchainApi { get { return _currentSettings.Value.BlockchainApi; } }
        protected BlockchainApi blockchainApi = new BlockchainApi(BlockchainApi);
        protected BlockchainSign blockchainSign = new BlockchainSign(_currentSettings.Value.BlockchainSign);
        protected BlockchainWallets blockchainWallets = new BlockchainWallets();

        protected static string HOT_WALLET
        {
            get
            {
                lock (_lock)
                {
                    if (string.IsNullOrEmpty(_currentSettings.Value.HotWallet))
                    {
                        var wallet = new BlockchainSign(_currentSettings.Value.BlockchainSign).PostWallet().GetResponseObject();
                        _currentSettings.Value.HotWallet = wallet.PublicAddress;
                        _currentSettings.Value.HotWalletKey = wallet.PrivateKey;
                    }
                }

                return _currentSettings.Value.HotWallet;
            }
        }
        protected static string HOT_WALLET_KEY = _currentSettings.Value.HotWalletKey;

        protected static string BlockChainName = _currentSettings.Value.BlockchainIntegration;

        protected static string WALLET_ADDRESS = _currentSettings.Value.DepositWalletAddress;
        protected static string PKey = _currentSettings.Value.DepositWalletKey;

        protected static string CLIENT_ID = _currentSettings.Value.ClientId;
        protected static string ASSET_ID = _currentSettings.Value.AssetId;

        protected static string EXTERNAL_WALLET = _currentSettings.Value.ExternalWalletAddress;
        protected static string EXTERNAL_WALLET_KEY = _currentSettings.Value.ExternalWalletKey;
        protected static string EXTERNAL_WALLET_ADDRESS_CONTEXT = _currentSettings.Value.ExternalWallerAddressContext;



        [OneTimeTearDown]
        public void SetProperty()
        {
            AllurePropertiesBuilder.Instance.AddPropertyPair("Date", DateTime.Now.ToString());
            try
            {
                var isAlive = blockchainApi.IsAlive.GetIsAlive().GetResponseObject();
                AllurePropertiesBuilder.Instance.AddPropertyPair("Env", isAlive.Env);
                AllurePropertiesBuilder.Instance.AddPropertyPair("Name", isAlive.Name);
                AllurePropertiesBuilder.Instance.AddPropertyPair("Version", isAlive.Version);
                AllurePropertiesBuilder.Instance.AddPropertyPair("ContractVersion", isAlive.ContractVersion);
            }
            catch (Exception) { /*do nothing*/}

            new Allure2Report().CreateEnvFile();
        }

        protected static void AddCyptoToBalanceFromExternal(string walletAddress)
        {
            var api = new BlockchainApi(BlockchainApi);
            var sign = new BlockchainSign(_currentSettings.Value.BlockchainSign);

            bool transferSupported = api.Capabilities.GetCapabilities().GetResponseObject().IsTestingTransfersSupported;
            if (transferSupported)
            {
                api.Balances.PostBalances(EXTERNAL_WALLET);
                api.Balances.PostBalances(walletAddress);
                TestingTransferRequest request = new TestingTransferRequest() { amount = "100001", assetId = ASSET_ID, fromAddress = EXTERNAL_WALLET, fromPrivateKey = EXTERNAL_WALLET_KEY, toAddress = walletAddress };
                var response = api.Testing.PostTestingTransfer(request);
            }
            else
            {
                api.Balances.PostBalances(walletAddress);
                var model = new BuildSingleTransactionRequest()
                {
                    Amount = "20000001",//"100001", this done for stellar. may got some problems with other
                    AssetId = ASSET_ID,
                    FromAddress = EXTERNAL_WALLET,
                    IncludeFee = false,
                    OperationId = Guid.NewGuid(),
                    ToAddress = walletAddress,
                    FromAddressContext = EXTERNAL_WALLET_ADDRESS_CONTEXT
                };

                var responseTransaction = api.Operations.PostTransactions(model).GetResponseObject();
                string operationId = model.OperationId.ToString();// should be identical as in response. do not change

                var signResponse = sign.PostSign(new SignRequest() { PrivateKeys = new List<string>() { EXTERNAL_WALLET_KEY }, TransactionContext = responseTransaction.TransactionContext }).GetResponseObject();

                var response = api.Operations.PostTransactionsBroadcast(new BroadcastTransactionRequest() { OperationId = model.OperationId, SignedTransaction = signResponse.SignedTransaction });

                var getResponse = api.Operations.GetOperationId(operationId);
               // response.Validate.StatusCode(HttpStatusCode.OK, "Could not update Balance from external wallet");
                //Assert.That(getResponse.GetResponseObject().OperationId, Is.EqualTo(model.OperationId));
            }
        }
    }  
}
