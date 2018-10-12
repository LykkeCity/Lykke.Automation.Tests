namespace AFTests.HftTests
{
    using HFT;
    using Lykke.Client.AutorestClient.Models;
    using NUnit.Framework;
    using System;
    using System.Net;
    using XUnitTestCommon.RestRequests.Interfaces;
    using XUnitTestCommon.Tests;
    using XUnitTestCommon.TestsCore;

    [TestFixture]
    class HftBaseTest : BaseTest
    {
        protected Hft hft = new Hft();
        protected string ApiKey = HFTSettings.GetHFTSettings().ApiKey;
        protected string SecondWalletApiKey = HFTSettings.GetHFTSettings().SecondApiKey;
        protected string FirstAssetId = HFTSettings.GetHFTSettings().FirstAssetId;
        protected string SecondAssetId = HFTSettings.GetHFTSettings().SecondAssetId;
        protected string AssetPair = HFTSettings.GetHFTSettings().AssetPair;

        // Market order
        protected IResponse<MarketOrderResponseModel> CreateAndValidateMarketOrder(
            string assetId, 
            string assetPairId, 
            OrderAction orderAction, 
            double volume, 
            string apiKey,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var request = new PlaceMarketOrderModel()
            {
                Asset = assetId,
                AssetPairId = assetPairId,
                OrderAction = orderAction,
                Volume = volume
            };

            return hft.Orders.PostOrdersMarket(request, apiKey)
                .Validate
                .StatusCode(statusCode);
        }

        // Limit order
        protected IResponse<LimitOrderResponseModel> CreateAndValidateLimitOrder(
            double price,
            string assetPairId,
            OrderAction orderAction,
            double volume,
            string apiKey,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var request = new PlaceLimitOrderModel()
            {
                Price = price,
                AssetPairId = assetPairId,
                OrderAction = orderAction,
                Volume = volume
            };

            return hft.Orders.PostOrdersLimitOrder(request, apiKey)
                .Validate
                .StatusCode(statusCode);
        }

        // Stop limit order    
        protected IResponse<LimitOrderResponseModel> CreateAndValidateStopLimitOrder(
            string assetPairId,
            OrderAction orderAction,
            double volume,
            double lowerLimitPrice,
            double lowerPrice,
            double upperLimitPrice,
            double upperPrice,
            string apiKey,
            HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var request = new PlaceStopLimitOrderModel()
            {
                AssetPairId = AssetPair,
                OrderAction = orderAction,
                Volume = volume,
                LowerLimitPrice = lowerLimitPrice,
                LowerPrice = lowerPrice,
                UpperLimitPrice = upperLimitPrice,
                UpperPrice = upperPrice
            };

            return hft.Orders.PostOrdersStopLimitOrder(request, apiKey)
                .Validate
                .StatusCode(statusCode);
        }
    }

    [SetUpFixture]
    public class HFTAttributeClass
    {
        protected Hft hft = new Hft();
        protected string ApiKey = HFTSettings.GetHFTSettings().ApiKey;
        protected string SecondWalletApiKey = HFTSettings.GetHFTSettings().SecondApiKey;

        public void CancelAllOrders(string apiKey)
        {
            var cancelOrders = hft.Orders.DeleteOrders(apiKey);
            cancelOrders.Validate.StatusCode(HttpStatusCode.OK);
        }

        [OneTimeSetUp, OneTimeTearDown]
        public void CancelAllOrders()
        {
            CancelAllOrders(ApiKey);
            CancelAllOrders(SecondWalletApiKey);
        }

        [OneTimeTearDown]
        public void SetProperty()
        {
            AllurePropertiesBuilder.Instance.AddPropertyPair("Date", DateTime.Now.ToString());
            try
            {
                var isAlive = hft.IsAlive.GetIsAlive().GetResponseObject();
                AllurePropertiesBuilder.Instance.AddPropertyPair("Env", isAlive.Env);
                AllurePropertiesBuilder.Instance.AddPropertyPair("Version", isAlive.Version);
            }
            catch (Exception) { /*do nothing*/}

            new Allure2Report().CreateEnvFile();
        }
    }
}
