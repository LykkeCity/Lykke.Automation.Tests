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
        public class CashOutBaseTest : ApiV2BaseTest
        {
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
                    Password = wallet.WalletKey,
                    PartnerId = "lykke"
                }).GetResponseObject().AccessToken;
            }
            #endregion
        }

        public class CannotCashoutLessThanMin : CashOutBaseTest
        {
            [Test]
            [Category("ApiV2")]
            [Category("ApiV2Operations")]
            public void CannotCashoutLessThanMinTest()
            {
                var blockchains = JsonConvert.DeserializeObject<List<BlockchainSettings>>(cfg["BlockchainsIntegration"]["Blockchains"].ToString());

                var blockchainsCount = blockchains.Count;
                Step("Validate that we can't do cashout of amount, that is less than minCashout Value",() =>
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

                                Assert.That(currentOperation.StatusCode, Is.Not.EqualTo(HttpStatusCode.BadRequest).And.Not.EqualTo(HttpStatusCode.NotFound), "Cashout Failed");

                                Assert.That(() => apiV2.Operations.GetOperationById(currentOperation.Content.Replace("\"", ""), token).GetResponseObject().Status, Is.Not.EqualTo(OperationStatus.Confirmed).After(10).Seconds.PollEvery(2).Seconds, "Cashout confirmed instead of Failed");

                                Assert.That(() => apiV2.Operations.GetOperationById(currentOperation.Content.Replace("\"", ""), token).GetResponseObject().Status, Is.EqualTo(OperationStatus.Failed).After(50).Seconds.PollEvery(2).Seconds);
                            }, false);
                    }
                });
            }
        }

        public class CannotCashOutMaxWalletBalance : CashOutBaseTest
        {
            [Test]
            [Category("ApiV2")]
            [Category("ApiV2Operations")]
            public void CannotCashOutMaxWalletBalanceTest()
            {
                var blockchains = JsonConvert.DeserializeObject<List<BlockchainSettings>>(cfg["BlockchainsIntegration"]["Blockchains"].ToString());

                var blockchainsCount = blockchains.Count;  

                var walletId = apiV2.wallets.GetWallets(token);

                Step("Check cannot do cashout with volume greater than wallet's volume", () =>
                {
                    blockchains.ForEach(blockchain =>
                    {
                            var (assetId, address, addressExtension, minCashOut) = GetBlockchainCashoutData(blockchain);

                            Step($"Check cashOut in case of asset's {assetId} volume is greater than asset's balance on the wallet", () =>
                            {
                                var operationId = Guid.NewGuid().ToString();

                                var maxBalance = apiV2.wallets.GetWalletsBalanceAssetId(walletId.GetResponseObject()[0].Id, "", token);

                                var balances = maxBalance.GetResponseObject();

                                var assetIdBalance = balances.FirstOrDefault(b => b.AssetId == assetId);
                                var balance = 1d;

                                if (assetIdBalance != null)
                                {
                                    balance = assetIdBalance.Balance;
                                }

                                Step($"asset {assetId} balance is {balance}", () => { });

                                var cashOut = apiV2.Operations.PostOperationCashOut(new CreateCashoutRequest
                                {
                                    AssetId = assetId,
                                    DestinationAddress = address,
                                    DestinationAddressExtension = addressExtension,
                                    Volume = balance * 2
                                }, operationId, token);

                                Assert.That(cashOut.StatusCode, Is.Not.EqualTo(HttpStatusCode.BadRequest).And.Not.EqualTo(HttpStatusCode.NotFound), "Cashout Failed");

                                OperationStatus status() { return apiV2.Operations.GetOperationById(cashOut.Content.Replace("\"", ""), token).GetResponseObject().Status; }

                                Assert.That(() => status(), Is.Not.EqualTo(OperationStatus.Confirmed).After(10).Seconds.PollEvery(1).Seconds);
                                Assert.That(() => status(), Is.EqualTo(OperationStatus.Failed).After(50).Seconds.PollEvery(2).Seconds);
                            }, false);
                    });
                });
            }
        }

        public class MakeCashOutIntoExternalWallet : CashOutBaseTest
        {
            [Test]
            [Category("ApiV2")]
            [Category("ApiV2Operations")]
            public void MakeCashOutIntoExternalWalletTest()
            {
                var blockchains = JsonConvert.DeserializeObject<List<BlockchainSettings>>(cfg["BlockchainsIntegration"]["Blockchains"].ToString());

                var blockchainsCount = blockchains.Count;

                Step($"Validate CashOut to External wallets for {blockchainsCount} blockchains", () =>
                {
                    blockchains.ForEach(blockchain =>
                    {

                        var (assetId, address, addressExtension, minCashOut) = GetBlockchainCashoutData(blockchain);

                        Step($"Add 2 asset '{assetId}' to wallet", () =>
                    {
                        FillWalletWithAsset(wallet.WalletAddress, assetId);
                    }, false);

                        Step($"Make valid cashout of asset {assetId}", () =>
                    {
                        MakeCashOut(assetId, address, addressExtension, 1, token);
                    }, false);

                    });
                });
            } 
        }

        // cashout to lukke wallet

        public class MakeCashOutIntoInternalWalletEnabled : CashOutBaseTest
        {
            [Test]
            public void MakeCashOutIntoInternalWalletEnabledTest()
            {
             
            }
        }
        
        //cashout to lykke wallet  - enabled

    }
}
