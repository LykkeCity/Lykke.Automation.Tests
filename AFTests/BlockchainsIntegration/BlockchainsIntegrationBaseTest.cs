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
            return Environment.GetEnvironmentVariable("BlockchainIntegration") ?? "Bitshares";//"Zcash"; //"Ripple";// "Dash"; "Litecoin";
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

        protected static string WALLET_SINGLE_USE = _currentSettings.Value.WalletSingleUse;
        protected static string KEY_WALLET_SINGLE_USE = _currentSettings.Value.WalletSingleUseKey;

        protected static string CLIENT_ID = _currentSettings.Value.ClientId;
        protected static string ASSET_ID = _currentSettings.Value.AssetId;

        protected static string EXTERNAL_WALLET = _currentSettings.Value.ExternalWalletAddress;
        protected static string EXTERNAL_WALLET_KEY = _currentSettings.Value.ExternalWalletKey;



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
            }
            catch (Exception) { /*do nothing*/}

            new Allure2Report().CreateEnvFile();
        }
    }  
}
