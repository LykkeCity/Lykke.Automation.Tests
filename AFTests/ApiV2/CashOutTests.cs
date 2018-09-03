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
        public class CannotCashoutLessThanMin : ApiV2BaseTest
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
                        try
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
                        catch (Exception) { };
                    }
                });
            }
        }

        public class CannotCashOutMaxWalletBalance : ApiV2BaseTest
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
                        try
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

                                Assert.That(() => apiV2.Operations.GetOperationById(cashOut.Content.Replace("\"", ""), token).GetResponseObject().Status, Is.EqualTo(OperationStatus.Failed).After(60).Seconds.PollEvery(2).Seconds);
                            });
                        }
                        catch (Exception) { }
                    });
                });
            }
        }

        public class MakeCashOutIntoExternalWallet : ApiV2BaseTest
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
                            try
                            {
                                var (assetId, address, addressExtension, minCashOut) = GetBlockchainCashoutData(blockchain);

                                Step($"Add 2 asset '{assetId}' to wallet", () =>
                            {
                                FillWalletWithAsset(wallet.WalletAddress, assetId);
                            });

                                Step($"Make valid cashout of asset {assetId}", () =>
                            {
                                MakeCashOut(assetId, address, addressExtension, 1, token);
                            });
                            }
                            catch (AssertionException) { }
                        });
                });
            } 
        }

        // cashout to lukke wallet

        public class MakeCashOutIntoInternalWalletEnabled : ApiV2BaseTest
        {
            [Test]
            public void MakeCashOutIntoInternalWalletEnabledTest()
            {
             
            }
        }
        
        //cashout to lykke wallet  - enabled

    }
}
