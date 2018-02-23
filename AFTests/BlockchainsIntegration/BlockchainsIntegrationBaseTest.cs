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

namespace AFTests.BlockchainsIntegrationTests
{
    class BlockchainsIntegrationBaseTest : BaseTest
    {
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

        protected static string HOT_WALLET = new BlockchainSign(_currentSettings.Value.BlockchainSign).PostWallet().GetResponseObject().PublicAddress;
        protected static string WALLET_ADDRESS = _currentSettings.Value.WalletAddress;
        protected static string PKey = _currentSettings.Value.WalletKey;

        protected static string WALLET_SINGLE_USE = _currentSettings.Value.WalletSingleUse;
        protected static string KEY_WALLET_SINGLE_USE = _currentSettings.Value.WalletSingleUseKey;

        protected static string CLIENT_ID = _currentSettings.Value.ClientId;
        protected static string ASSET_ID = _currentSettings.Value.AssetId;
        //fill here http://faucet.thonguyen.net/ltc
    }  
}
