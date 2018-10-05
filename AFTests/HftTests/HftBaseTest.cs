using HFT;
using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Net;
using XUnitTestCommon.Tests;
using XUnitTestCommon.TestsCore;

namespace AFTests.HftTests
{
    [TestFixture]
    class HftBaseTest : BaseTest
    {
        protected Hft hft = new Hft();
        protected string ApiKey = HFTSettings.GetHFTSettings().ApiKey;
        protected string SecondWalletApiKey = HFTSettings.GetHFTSettings().SecondApiKey;
        protected string FirstAssetId = HFTSettings.GetHFTSettings().FirstAssetId;
        protected string SecondAssetId = HFTSettings.GetHFTSettings().SecondAssetId;
        protected string AssetPair = HFTSettings.GetHFTSettings().AssetPair;
    }

    [SetUpFixture]
    public class HFTAttributeClass 
    {
        protected Hft hft = new Hft();
        protected string ApiKey = HFTSettings.GetHFTSettings().ApiKey;

        [OneTimeTearDown]
        public void CancelAllOrders()
        {
            var take = "100";
            var skip = "0";

            var response = hft.Orders.GetOrders(OrderStatusQuery.InOrderBook, skip, take, ApiKey);
            response.Validate.StatusCode(HttpStatusCode.OK);
            hft.Orders.DeleteOrders(ApiKey);
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
