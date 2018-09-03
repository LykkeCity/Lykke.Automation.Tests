using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ApiV2Data.Api;
using BlockchainsIntegration.Sign;
using Lykke.Client.ApiV2.Models;
using Lykke.Client.AutorestClient.Models;
using LykkeAutomationPrivate.Api;
using NUnit.Framework;
using XUnitTestCommon.Tests;


namespace AFTests.ApiV2
{
    public class BlockchainSettings
    {
        public bool AreCashinsDisabled { get; set; }
        public string Type { get; set; }
        public string ApiUrl { get; set; }
        public string SignServiceUrl { get; set; }
        public string HotWalletAddress { get; set; }
        public Monitoring Monitoring { get; set; }
    }

    public class Monitoring
    {
        public string InProgressOperationAlarmPeriod { get; set; }
    }

    class ApiV2BaseTest : BaseTest
    {
        protected ApiV2Client apiV2 = new ApiV2Client();
        protected Wallet wallet = new Wallet();
        protected LykkeApi lykkePrivateApi = new LykkeApi();
        protected BlockchainSign blockchainSign;

        #region helpers
        public (string assetId, string address, string addressExtension, double minCashOut) GetBlockchainCashoutData(BlockchainSettings blockchain)
        {
            try
            {
                var currentAsset = lykkePrivateApi.Assets.GetAssets().GetResponseObject().FirstOrDefault(a =>
                {
                    if (a.BlockchainIntegrationLayerId != null)
                        return a.BlockchainIntegrationLayerId.ToString().ToLower() == blockchain.Type.ToLower();
                    else
                        return false;
                });
                var currentAssetId = currentAsset?.Id;
                var minCashout = currentAsset?.CashoutMinimalAmount;

                blockchain.SignServiceUrl = blockchain.SignServiceUrl.TrimEnd('/');
                blockchainSign = new BlockchainSign(blockchain.SignServiceUrl + "/api");
                WalletCreationResponse wallet = blockchainSign.PostWallet().GetResponseObject();

                return (currentAssetId, wallet.PublicAddress, wallet.AddressContext, minCashout.Value);
            }
            catch (Exception e)
            {
                return ($"error with {blockchain.Type}", $"error with {blockchain.Type}", $"error with {blockchain.Type}", 0);
            }
        }

        public void FillWalletWithAsset(string userEmail, string assetId)
        {
            var clientId = lykkePrivateApi.ClientAccount.ClientAccountInformation.
                GetClientsByEmail(userEmail).GetResponseObject().FirstOrDefault()?.Id;

            var manualCashIn = new ManualCashInRequestModel
            {
                Amount = 2, // should be enough
                AssetId = assetId,
                ClientId = clientId,
                Comment = "AutotestFund",
                UserId = "Autotest user"
            };
            var cryptoToWalletResponse = lykkePrivateApi.ExchangeOperation.PostManualCashIn(manualCashIn).GetResponseObject();
        }

        public void MakeCashOut(string assetId, string destinationAdress, string addressExtension, double volume, string token)
        {
            var id = Guid.NewGuid().ToString();

            var currentOperation = apiV2.Operations.PostOperationCashOut(new CreateCashoutRequest
            {
                AssetId = assetId,
                DestinationAddress = destinationAdress,
                DestinationAddressExtension = addressExtension,
                Volume = volume
            }, id, token);

            if (apiV2.Operations.GetOperationById(currentOperation.Content.Replace("\"", ""), token).GetResponseObject().Status == OperationStatus.Failed)
                Assert.Fail("Manual CashIn operation Failed");

            Assert.That(() => apiV2.Operations.GetOperationById(currentOperation.Content.Replace("\"", ""), token).GetResponseObject().Status, Is.EqualTo(OperationStatus.Confirmed).After(60).Seconds.PollEvery(2).Seconds);
        }
        #endregion

        #region setup teardown

        protected string token = "";

        [SetUp]
        public void CashOutSetUp()
        {
            token = apiV2.Client.PostClientAuth(new AuthRequestModel
            {
                Email = wallet.WalletAddress,
                Password = wallet.WalletKey
            }).GetResponseObject().AccessToken;
        }
        #endregion
    }

    public class Wallet
    {
        public string WalletAddress
        {
            get
            {
                return "lykke_autotest_021b074415@lykke.com";
            }
        }

        public string WalletKey
        {
            get
            {
                return "0fc1dbf03917f8eeb8d5e0722cf473141ba2fe048e1820b5743ba054d090f425";
            }
        }

        public string AuthorizationToken
        {
            get
            {
                return "";
            }
        }
    }

}
