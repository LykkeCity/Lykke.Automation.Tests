using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.HftTests
{
    class OrderBooksTests
    {

        public class GetOrderBooks : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetOrderBooksTest()
            {
                var response = hft.OrderBooks.GetOrderBooks();
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class GetOrderBooksByAssetPairId : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetOrderBooksByAssetPairIdTest()
            {
                var assets = hft.AssetPairs.GetAssetPairs();
                assets.Validate.StatusCode(HttpStatusCode.OK);

                var response = hft.OrderBooks.GetOrderBooks(assets.GetResponseObject().First().Id);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject()[0].AssetPair, Is.EqualTo(assets.GetResponseObject().First().Id));
            }
        }

        [NonParallelizable]
        public class GetOrderBooksGetNewOrder : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetOrderBooksGetNewOrderTest()
            {
                var ordersBefore = hft.OrderBooks.GetOrderBooks(AssetPair);
                ordersBefore.Validate.StatusCode(HttpStatusCode.OK);

                double price = default(double);
                var volume = 1000;
                int i = 0;
                HttpStatusCode code = HttpStatusCode.BadRequest;
                do
                {
                    price = double.Parse(TestData.GenerateNumbers(3)) / Math.Pow(10, 2);
                    var request = new PlaceLimitOrderModel() { Price = price, AssetPairId = AssetPair, OrderAction = OrderAction.Buy, Volume = volume };

                    var response = hft.Orders.PostOrdersLimitOrder(request, ApiKey);
                    code = response.StatusCode;
                    i++;
                }
                while (i<5 && code!= HttpStatusCode.OK);              

                Stopwatch sw = new Stopwatch();
                sw.Start();
                while (sw.Elapsed < TimeSpan.FromMinutes(5))
                {
                    if (ordersBefore.GetResponseObject().FindAll(r => r.IsBuy == true).First().Timestamp != hft.OrderBooks.GetOrderBooks(AssetPair).GetResponseObject().FindAll(r => r.IsBuy == true).First().Timestamp)
                        break;
                    System.Threading.Thread.Sleep(2);
                }
                sw.Stop();

                // check that it appear into ordersbook
                Assert.That(() =>
                {
                    var order = hft.OrderBooks.GetOrderBooks(AssetPair).GetResponseObject().FindAll(r => r.IsBuy == true).First();
                    return order.Prices.Any(o => o.Price == price) && order.Prices.Any(o =>o.Volume == volume);
                }, Is.True.After(5*60*1000, 2*1000), "Order does not appear in orderbook");
            }
        }

        public class GetOrderBooksByInvalidAssetPairId : HftBaseTest
        {
            [TestCase("invalidId")]
            [TestCase("1234567")]
            [TestCase("!@^&*(")]
            [TestCase("-1155.5")]
            [Category("HFT")]
            public void GetOrderBooksByInvalidAssetPairIdTest(string assertPair)
            {
                var response = hft.OrderBooks.GetOrderBooks(assertPair);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }
    }
}
