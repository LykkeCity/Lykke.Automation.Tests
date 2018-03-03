using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

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
                Assert.That(response.GetResponseObject().Count, Is.GreaterThan(10));
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

                var response = hft.OrderBooks.GetOrderBooks(assets.GetResponseObject()[0].Id);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject()[0].AssetPair, Is.EqualTo(assets.GetResponseObject()[0].Id));
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
