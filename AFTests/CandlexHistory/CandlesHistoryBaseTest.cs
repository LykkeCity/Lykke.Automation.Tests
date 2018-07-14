using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AFTests.PrivateApiTests;
using Lykke.Client.AutorestClient.Models;
using MoreLinq;
using NUnit.Framework;

namespace AFTests.CandlexHistory
{
    [NonParallelizable]
    class CandlesHistoryBaseTest : PrivateApiBaseTest
    {
        public string ApiKey = "92ca97e5-93ff-4847-ae6e-aee488c3ca35";
        public string SecondWalletApiKey = "1606b4dd-fe22-4425-92ea-dccd5fffcce8";
        public string AssetPairId = "chfDEB";
        public string FirstAssetId = "CHF";
        public string SecondAssetId = "DEBt";
        public DateTime fromMoment;
        public double tradingVolume = 0.3;

        [SetUp]
        public void SetUp()
        {
            //to start from the minute
            if (DateTime.Now.Second > 10)
                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(60 - DateTime.Now.Second));

            var orderBooks = hft.OrderBooks.GetOrderBooks(AssetPairId).GetResponseObject();

            var minSellPrice = Double.MaxValue;
            var maxBuyPrice = Double.MinValue;

                  orderBooks.FindAll(o => o.IsBuy == true)?.ForEach(o =>
                {
                    o.Prices.ToList()?.ForEach(p =>
                    {
                        if (p.Price > maxBuyPrice)
                            maxBuyPrice = p.Price;
                    });
                });

                orderBooks.FindAll(o => o.IsBuy == false).ForEach(o =>
                {
                    o.Prices.ToList()?.ForEach(p =>
                    {
                        if (p.Price < minSellPrice)
                            minSellPrice = p.Price;
                    });
                });
            if (maxBuyPrice == double.MinValue && minSellPrice != double.MaxValue)
                maxBuyPrice = 0.9 * minSellPrice;

            if (minSellPrice == double.MaxValue && maxBuyPrice != double.MinValue)
                minSellPrice = 1.1 * maxBuyPrice;

            if (minSellPrice == double.MaxValue && maxBuyPrice == double.MinValue)
            {
                maxBuyPrice = 1.0;
                minSellPrice = 1.3;
            }

            // accuracy = 5

            maxBuyPrice = Make5numberAfterDot(maxBuyPrice);
            minSellPrice = Make5numberAfterDot(minSellPrice);

            fromMoment = DateTime.Now.ToUniversalTime();

            var limitOrderRequestBuy = new LimitOrderRequest() { Price = maxBuyPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Buy, Volume = tradingVolume };

            var response = hft.Orders.PostOrdersLimitOrder(limitOrderRequestBuy, ApiKey);
            response.Validate.StatusCode(HttpStatusCode.OK);

            var limitOrderRequestSell = new LimitOrderRequest() { Price = minSellPrice, AssetPairId = AssetPairId, OrderAction = OrderAction.Sell, Volume = tradingVolume };

            var response1 = hft.Orders.PostOrdersLimitOrder(limitOrderRequestSell, ApiKey);
            response1.Validate.StatusCode(HttpStatusCode.OK);

            //wait to appear in orderbook
            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(7));
        }

        [TearDown]
        public void TearDown()
        {
            var take = "100";
            var skip = "0";

            var response = hft.Orders.GetOrders(OrderStatus.InOrderBook, skip, take, ApiKey);
            response.Validate.StatusCode(HttpStatusCode.OK);

            response.GetResponseObject().ForEach(o => hft.Orders.PostOrdersCancelOrder(o.Id.ToString(), ApiKey));
        }

        public (double min, double max) newMinMaxPrices()
        {
            var orderBooks = hft.OrderBooks.GetOrderBooks(AssetPairId).GetResponseObject();

            var minSellPrice = double.MaxValue;
            var maxBuyPrice = double.MinValue;

            orderBooks.FindAll(o => o.IsBuy == true).ForEach(o =>
            {
                o.Prices.ToList().ForEach(p =>
                {
                    if (p.Price > maxBuyPrice)
                        maxBuyPrice = p.Price;
                });
            });

            orderBooks.FindAll(o => o.IsBuy == false).ForEach(o =>
            {
                o.Prices.ToList().ForEach(p =>
                {
                    if (p.Price < minSellPrice)
                        minSellPrice = p.Price;
                });
            });

            if (2 >= (minSellPrice - maxBuyPrice) * Math.Pow(10, 5))
                return (Make5numberAfterDot(minSellPrice), Make5numberAfterDot(maxBuyPrice));

            var middle = (maxBuyPrice + minSellPrice) / 2;
            var newMinSellPrice = minSellPrice - (minSellPrice - middle) / 2;
            var newMaxBuyPrice = maxBuyPrice + (middle - maxBuyPrice) / 2;
            return (Make5numberAfterDot(newMinSellPrice), Make5numberAfterDot(newMaxBuyPrice));
        }

        public (double minSell, double maxBuy) currentMinMaxPrices()
        {
            var orderBooks = hft.OrderBooks.GetOrderBooks(AssetPairId).GetResponseObject();

            var minSellPrice = double.MaxValue;
            var maxBuyPrice = double.MinValue;

            orderBooks.FindAll(o => o.IsBuy == true).ForEach(o =>
            {
                o.Prices.ToList().ForEach(p =>
                {
                    if (p.Price > maxBuyPrice)
                        maxBuyPrice = p.Price;
                });
            });

            orderBooks.FindAll(o => o.IsBuy == false).ForEach(o =>
            {
                o.Prices.ToList().ForEach(p =>
                {
                    if (p.Price < minSellPrice)
                        minSellPrice = p.Price;
                });
            });

            return (Make5numberAfterDot(minSellPrice), Make5numberAfterDot(maxBuyPrice));
        }

        protected static double Make5numberAfterDot(double input)
        {
            var s = input.ToString(System.Globalization.CultureInfo.InvariantCulture);
            if (s.Contains("."))
            {
                if ((s.Length - s.IndexOf(".")) > 5)
                    return double.Parse(s.Substring(0, s.IndexOf('.') + 6));

                return double.Parse(s);
            }
            else
                return double.Parse(s);
        }

        public static double SUM(IEnumerable<double> collection)
        {
            double result = 0;
            collection.ForEach(c => result += c);
            return result;
        }

        public static decimal Decimal(double d) => decimal.Parse(d.ToString(), System.Globalization.NumberStyles.Float);    }
}
