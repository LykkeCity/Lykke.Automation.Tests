using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using BlockchainsIntegration.Sign;
using Lykke.Client.ApiV2.Models;
using Lykke.Client.AutorestClient.Models;
using Newtonsoft.Json;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    class CashOutTests
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

        #region test data

        #endregion

        public class CannotCashoutLessThanMin : ApiV2BaseTest
        {
            string token = "";

            [SetUp]
            public void CashOutSetUp()
            {
                token = apiV2.Client.PostClientAuth(new AuthRequestModel
                {
                    Email = wallet.WalletAddress,
                    Password = wallet.WalletKey
                }).GetResponseObject().AccessToken;
            }

            [Test]
            [Category("ApiV2")]
            [Category("ApiV2Operations")]
            public void CannotCashoutLessThanMinTest()
            {
                var blockchains = JsonConvert.DeserializeObject<List<BlockchainSettings>>(cfg["BlockchainsIntegration"]["Blockchains"].ToString());

                var blockchainsCount = blockchains.Count;
                Assert.Multiple(() =>
                {
                    for (var i = 0; i < blockchainsCount; i++)
                    {
                        var (assetId, address, addressExtension, minCashOut) = GetBlockchainCashoutData(blockchains[i]);
                        if (minCashOut == 0)
                        {
                            Step($"Skipped asset {assetId}, because minCashout {minCashOut}", () => { });
                            continue;
                        }

                        Step($"Make CashOut for asset: {assetId}, min cashOut {minCashOut}, external wallet {address}, address extension {addressExtension}", () =>
                        {
                            var id = Guid.NewGuid().ToString();

                            var currentOperation = apiV2.Operations.PostOperationCashOut(new CreateCashoutRequest
                            {
                                AssetId = assetId,
                                DestinationAddress = address,
                                DestinationAddressExtension = addressExtension,
                                Volume = (minCashOut / 2)
                            }, id, token);

                            Assert.That(() => apiV2.Operations.GetOperationById(currentOperation.Content.Replace("\"", ""), token).GetResponseObject().Status, Is.EqualTo(OperationStatus.Failed).After(60).Seconds.PollEvery(2).Seconds);
                        });
                    }
                });
            }

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
                catch (Exception)
                {
                    return ($"error with {blockchain.Type}", $"error with {blockchain.Type}", $"error with {blockchain.Type}", 0);
                }
            }
        }


        public class CannotCashOutMaxWalletBalance : ApiV2BaseTest
        {
            string token = "";

            [SetUp]
            public void CashOutSetUp()
            {
                token = apiV2.Client.PostClientAuth(new AuthRequestModel
                {
                    Email = wallet.WalletAddress,
                    Password = wallet.WalletKey
                }).GetResponseObject().AccessToken;
            }

            [Test]
            public void CannotCashOutMaxWalletBalanceTest()
            {
                var blockchains = JsonConvert.DeserializeObject<List<BlockchainSettings>>(cfg["BlockchainsIntegration"]["Blockchains"].ToString());

                var blockchainsCount = blockchains.Count;

                var operationId = Guid.NewGuid().ToString();

                var walletId = apiV2.wallets.GetWallets(token);

                Assert.Multiple(() =>
                {
                    blockchains.ForEach(blockchain =>
                    {
                        var (assetId, address, addressExtension) = GetBlockchainCashoutData(blockchain);

                        Step($"Check cashOut in case of asset's {assetId} volume is greater than asset's balance on the wallet", () =>
                        {
                            var maxBalance = apiV2.wallets.GetWalletsBalanceAssetId(walletId.GetResponseObject()[0].Id, "", token);

                            var balances = maxBalance.GetResponseObject();

                            var assetIdBalance = balances.FirstOrDefault(b => b.AssetId == assetId);
                            var balance = 1d;

                            if(assetIdBalance != null)
                            {
                                balance = assetIdBalance.Balance;
                            }

                            var cashOut = apiV2.Operations.PostOperationCashOut(new CreateCashoutRequest
                            {
                                AssetId = assetId,
                                DestinationAddress = address,
                                DestinationAddressExtension = addressExtension,
                                Volume = balance * 2
                            }, operationId, token);

                            Assert.That(() => apiV2.Operations.GetOperationById(cashOut.Content.Replace("\"", ""), token).GetResponseObject().Status, Is.EqualTo(OperationStatus.Failed).After(60).Seconds.PollEvery(2).Seconds);
                        });
                    });
                });
            }

            public (string assetId, string address, string addressExtension) GetBlockchainCashoutData(BlockchainSettings blockchain)
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

                    blockchain.SignServiceUrl = blockchain.SignServiceUrl.TrimEnd('/');
                    blockchainSign = new BlockchainSign(blockchain.SignServiceUrl + "/api");
                    WalletCreationResponse wallet = blockchainSign.PostWallet().GetResponseObject();

                    return (currentAssetId, wallet.PublicAddress, wallet.AddressContext);
                }
                catch (Exception)
                {
                    return ($"error with {blockchain.Type}", $"error with {blockchain.Type}", $"error with {blockchain.Type}");
                }
            }
        }
    }
}
