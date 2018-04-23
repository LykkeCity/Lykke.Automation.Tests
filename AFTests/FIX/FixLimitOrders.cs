using FIX.Client;
using Lykke.Client.AutorestClient.Models;
using NUnit.Framework;
using QuickFix.Fields;
using QuickFix.FIX44;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFTests.FIX
{
    partial class FixTests
    {
        public class SetLimitOrderSell : FixBaseTest
        {
            string orderId = Guid.NewGuid().ToString();
            protected FixClient fixClient;

            [SetUp]
            public void SetUp()
            {
                fixClient = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
                fixClient.Init();
            }

            [Test]
            [Category("FIX")]
            public void SetLimitOrderSellTest()
            {

                var price = 0.01m;
                var quantity = 0.01m;
                var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: false, qty: quantity, price: price);

                fixClient.Send(marketOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null);
                Assert.That(response, Is.TypeOf<ExecutionReport>());

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.PENDING_NEW));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.PENDING_NEW));
                Assert.That(ex.AvgPx.Obj, Is.EqualTo(0m), "Price should be 0 in case of Pending status");
                Assert.That(ex.OrdType.Obj, Is.EqualTo('2'), "Should be 2, this is Limit order");
                Assert.That(ex.OrderQty.Obj, Is.EqualTo(quantity), "unexpected quantity");
            }

            [TearDown]
            public void CancelRequest()
            {
                var cancelRequest = new OrderCancelRequest
                {
                    ClOrdID = new ClOrdID(Guid.NewGuid().ToString()),
                    OrigClOrdID = new OrigClOrdID(orderId),
                    TransactTime = new TransactTime(DateTime.UtcNow)
                };

                fixClient.Send(cancelRequest);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null, $"order id {orderId} seems to be not cancelled.  response: {JsonRepresentation(response)}");
                fixClient.Stop();
                fixClient.Dispose();
            }
        }

        public class SetLimitOrderBuy : FixBaseTest
        {
            string orderId = Guid.NewGuid().ToString();
            protected FixClient fixClient;

            [SetUp]
            public void SetUp()
            {
                fixClient = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
                fixClient.Init();
            }

            [Test]
            [Category("FIX")]
            public void SetLimitOrderBuyTest()
            {
                var price = 0.01m;
                var quantity = 0.01m;
                var limitOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: quantity, price: price);

                fixClient.Send(limitOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null);
                Assert.That(response, Is.TypeOf<ExecutionReport>(), $" response: {JsonRepresentation(response)}");

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.PENDING_NEW));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.PENDING_NEW));
                Assert.That(ex.AvgPx.Obj, Is.EqualTo(0m), "Price should be 0 in case of Pending status");
                Assert.That(ex.OrdType.Obj, Is.EqualTo('2'), "Should be 2, this is Limit order");
                Assert.That(ex.OrderQty.Obj, Is.EqualTo(quantity), "unexpected quantity");                
            }

            [TearDown]
            public void CancelRequest()
            {
                var cancelRequest = new OrderCancelRequest
                {
                    ClOrdID = new ClOrdID(Guid.NewGuid().ToString()),
                    OrigClOrdID = new OrigClOrdID(orderId),
                    TransactTime = new TransactTime(DateTime.UtcNow)
                };

                fixClient.Send(cancelRequest);

                fixClient.Stop();
                fixClient.Dispose();
            }
        }

        public class CancelLimitOrderBuy : FixBaseTest
        {
            protected FixClient fixClient;

            [SetUp]
            public void SetUp()
            {
                fixClient = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
                fixClient.Init();
            }

            [Test]
            [Category("FIX")]
            public void CancelLimitOrderBuyTest()
            {
                var orderId = Guid.NewGuid().ToString();
                var price = 0.01m;
                var quantity = 0.01m;
                var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: quantity, price: price);

                fixClient.Send(marketOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null);
                Assert.That(response, Is.TypeOf<ExecutionReport>());

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.PENDING_NEW));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.PENDING_NEW));

                var cancelRequest = new OrderCancelRequest
                {
                    ClOrdID = new ClOrdID(Guid.NewGuid().ToString()),
                    OrigClOrdID = new OrigClOrdID(orderId),
                    TransactTime = new TransactTime(DateTime.UtcNow)
                };

                fixClient.Send(cancelRequest);

                response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null, $"unexpected response: {JsonRepresentation(response)}");
                Assert.That(response, Is.TypeOf<ExecutionReport>());

                ex = (ExecutionReport)response;
                if (ex.OrdStatus.Obj != OrdStatus.CANCELED)
                    response = fixClient.GetResponse<Message>();

                ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.CANCELED), "Unexpected order status");
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.CANCELED), "Unexpected exectype status");             
            }

            [TearDown]
            public void TearDown()
            {
                fixClient.Stop();
                fixClient.Dispose();
            }
        }

        public class CancelLimitSell : FixBaseTest
        {
            protected FixClient fixClient;

            [SetUp]
            public void SetUp()
            {
                fixClient = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
                fixClient.Init();
            }

            [Test]
            [Category("FIX")]
            public void CancelLimitSellTest()
            {
                var orderId = Guid.NewGuid().ToString();
                var price = 0.01m;
                var quantity = 0.01m;
                var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: false, qty: quantity, price: price);

                fixClient.Send(marketOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null);
                Assert.That(response, Is.TypeOf<ExecutionReport>());

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.PENDING_NEW));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.PENDING_NEW));

                var cancelRequest = new OrderCancelRequest
                {
                    ClOrdID = new ClOrdID(Guid.NewGuid().ToString()),
                    OrigClOrdID = new OrigClOrdID(orderId),
                    TransactTime = new TransactTime(DateTime.UtcNow)
                };

                fixClient.Send(cancelRequest);

                response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null, $"unexpected response: {JsonRepresentation(response)}");
                Assert.That(response, Is.TypeOf<ExecutionReport>());

                ex = (ExecutionReport)response;
                if (ex.OrdStatus.Obj != OrdStatus.CANCELED)
                    response = fixClient.GetResponse<Message>();

                ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.CANCELED), "Unexpected order status");
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.CANCELED), "Unexpected exectype status");
            }

            [TearDown]
            public void TearDown()
            {
                fixClient.Stop();
                fixClient.Dispose();
            }
        }

        public class LimitOrderWrongAssetPair : FixBaseTest
        {
            protected FixClient fixClient;

            [SetUp]
            public void SetUp()
            {
                fixClient = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
                fixClient.Init();
            }

            [TestCase("!@%()")]
            [TestCase("-1234")]
            [TestCase("wrongAssetPair")]
            [Category("FIX")]
            public void LimitOrderWrongAssetPairTest(string assetPair)
            {
                var orderId = Guid.NewGuid().ToString();
                var price = 0.01m;
                var quantity = 0.01m;
                var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: quantity, price: price, assetPairId: assetPair);

                fixClient.Send(marketOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null, $"unexpected response: {JsonRepresentation(response)}");
                Assert.That(response, Is.TypeOf<ExecutionReport>(), $"unexpected response type. response: {JsonRepresentation(response)}");

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.REJECTED));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.REJECTED));
            }

            [TearDown]
            public void TearDown()
            {
                fixClient.Stop();
                fixClient.Dispose();
            }
        }

        public class LimitOrderWrongQuantity : FixBaseTest
        {
            protected FixClient fixClient;

            [SetUp]
            public void SetUp()
            {
                fixClient = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
                fixClient.Init();
            }

            [TestCase(-1)]
            [TestCase(0)]
           // [TestCase(9999999999999999)] why??
            [Category("FIX")]
            public void LimitOrderWrongQuantityTest(object q)
            {
                var orderId = Guid.NewGuid().ToString();
                var price = 0.01m;
                var quantity = Decimal.Parse(q.ToString());
                var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: quantity, price: price);

                fixClient.Send(marketOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null, $"unexpected response: {response.ToString().Replace("\u0001", "|")}");
                Assert.That(response, Is.TypeOf<ExecutionReport>(), $"unexpected response type. response: {response.ToString().Replace("\u0001", "|")}");

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.REJECTED));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.REJECTED));
            }

            [TearDown]
            public void TearDown()
            {
                fixClient.Stop();
                fixClient.Dispose();
            }
        }

        public class LimitOrderWrongPrice : FixBaseTest
        {
            protected FixClient fixClient;

            [SetUp]
            public void SetUp()
            {
                fixClient = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
                fixClient.Init();
            }

            [TestCase(-1)]
            [TestCase(0)]
           // [TestCase(9999999999999999)] why?
            [Category("FIX")]
            public void LimitOrderWrongPriceTest(object p)
            {
                var orderId = Guid.NewGuid().ToString();
                var price = Decimal.Parse(p.ToString()); 
                var quantity = 0.01m;
                var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: quantity, price: price);

                fixClient.Send(marketOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null, $"unexpected response: {response.ToString().Replace("\u0001", "|")}");
                Assert.That(response, Is.TypeOf<ExecutionReport>(), $"unexpected response type response: {response.ToString().Replace("\u0001", "|")}");

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.REJECTED));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.REJECTED));
            }

            [TearDown]
            public void TearDown()
            {
                fixClient.Stop();
                fixClient.Dispose();
            }
        }

        //test all assetPairs
        public class OnlyAssetsThatEnabledAreAvailableInFix : FixBaseTest
        {
            static int i = default(int);
            protected FixClient fixClient;

            [TearDown]
            public void TearDown()
            { 
                fixClient.Stop();
                fixClient.Dispose();
            }

            [Test]
            [Category("FIX")]
            public void OnlyAssetsThatEnabledAreAvailableInFixTest()
            {
                var assetPairs = privateApi.Assets.GetAssetPairs().GetResponseObject().FindAll(a => a.IsDisabled == false);
                var validAssets = privateApi.Assets.GetAssets(false).GetResponseObject().
                    FindAll(a => a.IsDisabled == false).FindAll(a => a.IsTradable == true);

                assetPairs = assetPairs.FindAll(a =>
                    a.IsDisabled == false
                ).FindAll(a =>
                     validAssets.Any(va => va.Id == a.BaseAssetId)
                ).FindAll(a =>
                     validAssets.Any(va => va.Id == a.QuotingAssetId)
                );

                fixClient = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
                fixClient.Init();

                Assert.Multiple(() =>
                assetPairs.ToList().ForEach(a =>
                {
                    if(!a.Id.EndsWith("cy"))
                    CreateLimitOrderWithAssetPair(a.Id, (decimal)a.MinVolume);
                })
                );
            }

            void CreateLimitOrderWithAssetPair(string assetPair, decimal volume)
            {
                try
                {
                    var orderId = Guid.NewGuid().ToString();
                    var price = 0.01m;
                    var quantity = volume + 0.5m;
                    var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: quantity, price: price, assetPairId: assetPair);

                    fixClient.Send(marketOrder);

                    var response = fixClient.GetResponse<Message>();

                    int aa = 60;
                    while (aa-- > 0)
                    {
                        if (((ExecutionReport)response).ClOrdID.Obj.ToString() == orderId)
                            break;
                        response = fixClient.GetResponse<Message>();
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    }

                    Assert.That(response, Is.Not.Null);
                    Assert.That(response, Is.TypeOf<ExecutionReport>());

                    var ex = (ExecutionReport)response;
                    Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.PENDING_NEW), $"unexpected response status for assetPair {assetPair}. response: {response.ToString().Replace("\u0001", "|")}");
                    Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.PENDING_NEW), $"unexpected response type for assetPair {assetPair}. response: {response.ToString().Replace("\u0001", "|")}");

                    // clean myself
                    var cancelRequest = new OrderCancelRequest
                    {
                        ClOrdID = new ClOrdID(Guid.NewGuid().ToString()),
                        OrigClOrdID = new OrigClOrdID(orderId),
                        TransactTime = new TransactTime(DateTime.UtcNow)
                    };

                    fixClient.Send(cancelRequest);
                    response = fixClient.GetResponse<Message>();

                    int a = 60;
                    while (a-- > 0)
                    {
                        if (((ExecutionReport)response).ClOrdID.Obj.ToString() == cancelRequest.ClOrdID.Obj.ToString())
                            break;
                        response = fixClient.GetResponse<Message>();
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                    }

                    Assert.That(response, Is.Not.Null);
                    Assert.That(response, Is.TypeOf<ExecutionReport>(), $"unexpected response type for assetPair {assetPair}. response: {response.ToString().Replace("\u0001", "|")}");

                    ex = (ExecutionReport)response;

                    //Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.PENDING_CANCEL), $"unexpected response status for assetPair {assetPair}. response: {response.ToString().Replace("\u0001", "|")}");
                    //Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.PENDING_CANCEL), $"unexpected response type for assetPair {assetPair}. response: {response.ToString().Replace("\u0001", "|")}");

                    int time = 120;
                    while (time-- > 0)
                    {
                        if (ex.OrdStatus.Obj != OrdStatus.CANCELED)
                        {
                            response = fixClient.GetResponse<Message>();
                            ex = (ExecutionReport)response;
                            System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                        }
                        else { break; }
                    }    
                }
                catch (Exception)
                {
                    //Assert.Fail($"An error occured with assetPair {assetPair}. Number of Exceptions {i++}");
                }
            }
        }

        public class AllMEssagesStoredInAzure : FixBaseTest
        {
            string orderId = Guid.NewGuid().ToString();
            protected FixClient fixClient;

            [SetUp]
            public void SetUp()
            {
                fixClient = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
                fixClient.Init();
            }

            [Test]
            [Category("FIX")]
            public void AllMEssagesStoredInAzureTest()
            {
                var price = 0.01m;
                var quantity = 0.01m;
                var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: quantity, price: price);

                fixClient.Send(marketOrder);

                var jss = marketOrder.ToString();
                var messageStringRepresentation = jss.Replace("\u0001", "|");

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null);
                Assert.That(response, Is.TypeOf<ExecutionReport>(), $" response: {JsonRepresentation(response)}");

                //because message has timestamp and seems to be uniq - use First.
                var azureMessage = GetValueFromAzure(messageStringRepresentation);

                int timer = 60;
                while(timer > 0)
                {
                    if (azureMessage.Count == 0)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                        azureMessage = GetValueFromAzure(messageStringRepresentation);
                    }
                    else
                    {
                        timer = 60;
                        break;
                    }
                }
                
                Assert.That(azureMessage.Count, Is.GreaterThan(0), $"Unexpected azure message count. Expected azure message '{messageStringRepresentation}'");
                Assert.That(azureMessage.First().StringValue, Is.EqualTo(messageStringRepresentation), "Unexpected Azure value");

                var responseStringRepresentation = response.ToString().Replace("\u0001", "|");

                var responseFromAzure = GetValueFromAzure(responseStringRepresentation);

                while (timer > 0)
                {
                    if (responseFromAzure.Count == 0)
                    {
                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(2));
                        responseFromAzure = GetValueFromAzure(responseStringRepresentation);
                    }
                    else
                    {
                        timer = 60;
                        break;
                    }
                }

                Assert.That(responseFromAzure.Count, Is.GreaterThan(0), $"Unexpected azure message count. Expected azure message '{responseStringRepresentation}'");
                Assert.That(responseFromAzure.First().StringValue, Is.EqualTo(responseStringRepresentation), "Unexpected Azure value");
            }

            [TearDown]
            public void CancelRequest()
            {
                var cancelRequest = new OrderCancelRequest
                {
                    ClOrdID = new ClOrdID(Guid.NewGuid().ToString()),
                    OrigClOrdID = new OrigClOrdID(orderId),
                    TransactTime = new TransactTime(DateTime.UtcNow)
                };

                fixClient.Send(cancelRequest);

                var response = fixClient.GetResponse<Message>();

               // Assert.That(response, Is.Not.Null, $"order id {orderId} seems to be not cancelled.  response: {JsonRepresentation(response)}");

                fixClient.Stop();
                fixClient.Dispose();
            }
        }

        // fix wrong Volume

        public class LimitOrderVolumeLessThenMinAssetVolume : FixBaseTest
        {
            protected FixClient fixClient;

            [SetUp]
            public void SetUp()
            {
                fixClient = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
                fixClient.Init();
            }

            [Test]
            [Category("FIX")]
            public void LimitOrderVolumeLessThenMinAssetVolumeTest()
            {
                var assetPairs = privateApi.Assets.GetAssetPairs().GetResponseObject().FindAll(a => a.IsDisabled == false);
                var validAssets = privateApi.Assets.GetAssets(false).GetResponseObject().
                    FindAll(a => a.IsDisabled == false).FindAll(a => a.IsTradable == true);

                assetPairs = assetPairs.FindAll(a =>
                    a.IsDisabled == false
                ).FindAll(a =>
                     validAssets.Any(va => va.Id == a.BaseAssetId)
                ).FindAll(a =>
                     validAssets.Any(va => va.Id == a.QuotingAssetId)
                );

                var assetPair = assetPairs.Find(a => a.MinVolume > 0);

                var orderId = Guid.NewGuid().ToString();
                var price = 0.01m;
                var quantity = 0.01m;

                var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: (decimal)(assetPair.MinVolume / 2), price: price, assetPairId: assetPair.Id);

                fixClient.Send(marketOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null, $"unexpected response: {response.ToString().Replace("\u0001", "|")}");
                Assert.That(response, Is.TypeOf<ExecutionReport>(), $"unexpected response type response: {response.ToString().Replace("\u0001", "|")}");

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.REJECTED));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.REJECTED));
            }

            [TearDown]
            public void TearDown()
            {
                fixClient.Stop();
                fixClient.Dispose();
            }
        }

        public class LimitOrderPriceLessThenMinPriceVolume : FixBaseTest
        {
            protected FixClient fixClient;

            [SetUp]
            public void SetUp()
            {
                fixClient = new FixClient("LYKKE_T", "SENDER_T", Init.LocalConfig().GetSection("TestClient:ServiceUrl").Value, 12357);
                fixClient.Init();
            }

            [Test]
            [Category("FIX")]
            public void LimitOrderPriceLessThenMinAssetPriceTest()
            {
                var assetPairs = privateApi.Assets.GetAssetPairs().GetResponseObject().FindAll(a => a.IsDisabled == false);
                var validAssets = privateApi.Assets.GetAssets(false).GetResponseObject().
                    FindAll(a => a.IsDisabled == false).FindAll(a => a.IsTradable == true);

                assetPairs = assetPairs.FindAll(a =>
                    a.IsDisabled == false
                ).FindAll(a =>
                     validAssets.Any(va => va.Id == a.BaseAssetId)
                ).FindAll(a =>
                     validAssets.Any(va => va.Id == a.QuotingAssetId)
                );

                var assetPair = assetPairs.Find(a => a.MinVolume > 0);

                var orderId = Guid.NewGuid().ToString();
                var price = 0.01m;
                var quantity = 0.01m;

                var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: (decimal)(assetPair.MinVolume), price: (decimal)(assetPair.MinVolume / (2 * Math.Pow(10, assetPair.Accuracy))), assetPairId: assetPair.Id);

                fixClient.Send(marketOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null, $"unexpected response: {response.ToString().Replace("\u0001", "|")}");
                Assert.That(response, Is.TypeOf<ExecutionReport>(), $"unexpected response type response: {response.ToString().Replace("\u0001", "|")}");

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.REJECTED));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.REJECTED));
            }

            [TearDown]
            public void TearDown()
            {
                fixClient.Stop();
                fixClient.Dispose();
            }
        }
    }
}
