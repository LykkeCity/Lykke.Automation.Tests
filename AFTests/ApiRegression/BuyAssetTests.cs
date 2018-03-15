using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AFTests.ApiRegression.Steps;
using Lykke.Client.AutorestClient.Models;
using LykkeAutomationPrivate.DataGenerators;
using NBitcoin;
using NUnit.Framework;

namespace AFTests.ApiRegression
{
    class BuyAssetTests : ApiRegressionBaseTest
    {
        public enum BuyOrSell { Buy, Sell }
        public enum Order { Limit, Market }

        private string email = Config.BuyAssetEmail;
        private string password = Config.BuyAssetPassword;
        private string pin = Config.BuyAssetPin;

        private MobileSteps steps;
        private string token;
        private Key key;

        [OneTimeSetUp]
        public void Login()
        {
            steps = new MobileSteps(walletApi);

            Step($"Login as {email} user", () =>
            {
                var loginStep = steps.Login(email, password, pin);
                token = loginStep.token;
                key = loginStep.privateKey;
            });
        }

        [TestCase("BTC", "EUR", BuyOrSell.Buy, Order.Limit, 0.001, Category = "ApiRegression")]
        [TestCase("BTC", "USD", BuyOrSell.Sell, Order.Limit, 0.001, Category = "ApiRegression")]
        [TestCase("BTC", "EUR", BuyOrSell.Buy, Order.Market, 0.001, Category = "ApiRegression")]
        [TestCase("BTC", "USD", BuyOrSell.Sell, Order.Market, 0.001, Category = "ApiRegression")]
        public void OrderTest(string asset1, string asset2, BuyOrSell buyOrSell, Order order, double volume)
        {
            

            string assetPair = asset1 + asset2;
            double asset1Balance = 0;
            double asset2Balance = 0;
            double assetPairPrice = 0;
            string orderId = null;


            if (order == Order.Limit)
            {
                Step("Cancel any limit orders user already has", () =>
                {
                    steps.CancelAnyLimitOrder(token);
                });
            }

            Step("Get current wallets and balances", () =>
            {
                asset1Balance = steps.GetAssetBalance(asset1, token);
                asset2Balance = steps.GetAssetBalance(asset2, token);
            });

            if (order == Order.Limit)
            {
                Step("Find prices", () =>
                {
                    var assetPairRates = walletApi.AssetPairRates.GetById(assetPair, token)
                        .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                        .GetResponseObject().Result;
                    Assert.That(assetPairRates?.Rate?.Ask, Is.Not.Null, "No Ask rate found");
                    Assert.That(assetPairRates?.Rate?.Bid, Is.Not.Null, "No Bid rate found");
                    assetPairPrice = buyOrSell == BuyOrSell.Buy 
                        ? assetPairRates.Rate.Ask.Value //Buying for the highest price
                        : assetPairRates.Rate.Bid.Value; //Selling for the lowest price
                });
            }

            Step($"{buyOrSell} {volume} {asset1} for {asset2}", () =>
            {
                string accessToken = steps.GetAccessToken(email, token, key);
                double assetVolume = buyOrSell == BuyOrSell.Buy ? volume : -volume;
                //TODO: Add assertion to check volume limits

                if (order == Order.Limit)
                {
                    Console.WriteLine($"Placing Limit order for pair {asset1 + asset2} with price {assetPairPrice}");
                    orderId = walletApi.HotWallet
                        .PostLimitOrder(new HotWalletLimitOperation
                        {
                            AssetId = asset1,
                            AssetPair = asset1 + asset2,
                            Price = assetPairPrice,
                            Volume = assetVolume
                        }, accessToken, token)
                        .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                        .GetResponseObject().Result.Order?.Id;
                }
                else
                {
                    var marketOrder = walletApi.HotWallet
                        .PostMarketOrder(new HotWalletOperation
                        {
                            AssetId = asset1,
                            AssetPair = assetPair,
                            Volume = assetVolume
                        }, accessToken, token)
                        .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                        .GetResponseObject().Result;
                    orderId = marketOrder.Order?.Id;

                    Assert.That(marketOrder.Order?.Price, Is.Not.Null, "No order price found");
                    assetPairPrice = marketOrder.Order.Price.Value;
                    Console.WriteLine($"Market order has been placed for pair {asset1 + asset2} with price {assetPairPrice}");
                }
                
                Assert.That(orderId, Is.Not.Null.Or.Not.Empty, "Order Id is null or empty");
            });

            if (order == Order.Limit)
            {
                Step("Waiting for 1 minute until asset has been sold", () =>
                {
                    Assert.That(() => walletApi.LimitOrders.GetOffchainLimitList(token, assetPair)
                            .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                            .GetResponseObject().Result.Orders,
                        Is.Empty.After(60).Seconds.PollEvery(1).Seconds, 
                        "Limit order has not been sold!");
                });
            }

            Step("Assert that balance has been changed", () =>
            {
                double expectedAsset1Balance = buyOrSell == BuyOrSell.Buy
                    ? asset1Balance + volume
                    : asset1Balance - volume;

                double expectedAsset2Balance = buyOrSell == BuyOrSell.Buy
                    ? asset2Balance - volume * assetPairPrice
                    : asset2Balance + volume * assetPairPrice;

                //TODO: Add more acurate assertion
                //TODO: Remove after and polling?
                Assert.That(() => steps.GetAssetBalance(asset1, token),
                    Is.EqualTo(expectedAsset1Balance).Within(expectedAsset1Balance * 0.01)
                        .After(60).Seconds.PollEvery(2).Seconds,
                    $"{asset1} is not equal to expected");

                Assert.That(() => steps.GetAssetBalance(asset2, token),
                    Is.EqualTo(expectedAsset2Balance).Within(expectedAsset2Balance * 0.02)
                        .After(60).Seconds.PollEvery(2).Seconds,
                    $"{asset2} is not equal to expected");
            });

            Step("Asserting history", () =>
            {
                if (walletApi.ApiUrl.Contains("test"))
                {
                    Console.WriteLine("BUG: Wrong order id in history, skipping step");
                    return;
                }
                Assert.That(() => walletApi.History.GetByAssetId("", token)
                                 .Validate.StatusCode(HttpStatusCode.OK).Validate.NoApiError()
                                 .GetResponseObject().Result
                                 .Any(record => record.Trade?.OrderId == orderId && record.Trade?.Asset == asset1),
                    Is.True.After(60).Seconds.PollEvery(5).Seconds, 
                    "No history found for the last order");
            });
        }
    }
}
