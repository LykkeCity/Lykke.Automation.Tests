using HFT;
using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.Tests;
using XUnitTestCommon.TestsCore;

namespace AFTests.HftTests
{
    [TestFixture]
    class HftBaseTest : BaseTest
    {
        protected Hft hft = new Hft();
        protected const string ApiKey = "3982d2d7-ae7b-4995-8074-d563707b986e";
    }

    [SetUpFixture]
    public class HFTAttributeClass 
    {
        protected Hft hft = new Hft();
        protected const string ApiKey = "3982d2d7-ae7b-4995-8074-d563707b986e";

        [OneTimeTearDown]
        public void CancelAllOrders()
        {
            var take = "100";
            var skip = "0";

            var response = hft.Orders.GetOrders(OrderStatus.InOrderBook, skip, take, ApiKey);
            response.Validate.StatusCode(HttpStatusCode.OK);

            response.GetResponseObject().ForEach(o => hft.Orders.PostOrdersCancelOrder(o.Id.ToString(), ApiKey));
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
