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
           return Environment.GetEnvironmentVariable("BlockchainIntegration") ?? "Ripple"; //"Ripple";// "Dash"; "Litecoin";
       }

        protected static string CurrentAssetId()
        {
            return _currentSettings.Value.AssetId;
        }

        protected static string BlockchainApi { get { return _currentSettings.Value.BlockchainApi; } }
        protected BlockchainApi blockchainApi = new BlockchainApi(_currentSettings.Value.BlockchainApi);
        protected BlockchainSign blockchainSign = new BlockchainSign(_currentSettings.Value.BlockchainSign);
        protected BlockchainWallets blockchainWallets = new BlockchainWallets();

        protected static string HOT_WALLET
        {
            get
            {
                lock (_lock)
                {
                    if (string.IsNullOrEmpty(_currentSettings.Value.HotWallet))
                        _currentSettings.Value.HotWallet = new BlockchainSign(_currentSettings.Value.BlockchainSign).PostWallet().GetResponseObject().PublicAddress;
                }

                return _currentSettings.Value.HotWallet;
            }
        }

        protected static string WALLET_ADDRESS = _currentSettings.Value.WalletAddress;
        protected static string PKey = _currentSettings.Value.WalletKey;

        protected static string WALLET_SINGLE_USE = _currentSettings.Value.WalletSingleUse;
        protected static string KEY_WALLET_SINGLE_USE = _currentSettings.Value.WalletSingleUseKey;

        protected static string CLIENT_ID = _currentSettings.Value.ClientId;
        protected static string ASSET_ID = _currentSettings.Value.AssetId;


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
