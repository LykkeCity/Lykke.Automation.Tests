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
    class FixLimitOrders
    {
        public class SetLimitOrderSell : FixBaseTest
        {
            string orderId = Guid.NewGuid().ToString("N");

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

                Assert.That(response, Is.Not.Null, $"order id {orderId} seems to be not cancelled");
            }
        }

        public class SetLimitOrderBuy : FixBaseTest
        {
            string orderId = Guid.NewGuid().ToString("N");

            [Test]
            [Category("FIX")]
            public void SetLimitOrderBuyTest()
            {
                
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

                Assert.That(response, Is.Not.Null, $"order id {orderId} seems to be not cancelled");
            }
        }

        public class CancelLimitOrderBuy : FixBaseTest
        {
            [Test]
            [Category("FIX")]
            public void CancelLimitOrderBuyTest()
            {
                var orderId = Guid.NewGuid().ToString("N");
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

                Assert.That(response, Is.Not.Null);
                Assert.That(response, Is.TypeOf<ExecutionReport>());

                ex = (ExecutionReport)response;
                if (ex.OrdStatus.Obj != OrdStatus.CANCELED)
                    response = fixClient.GetResponse<Message>();

                ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.CANCELED), "Unexpected order status");
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.CANCELED), "Unexpected exectype status");             
            }
        }

        public class LimitOrderWrongAssetPair : FixBaseTest
        {
            [TestCase("")]
            [TestCase("!@%()")]
            [TestCase("-1234")]
            [TestCase("wrongAssetPair")]
            [Category("FIX")]
            public void LimitOrderWrongAssetPairTest(string assetPair)
            {
                var orderId = Guid.NewGuid().ToString("N");
                var price = 0.01m;
                var quantity = 0.01m;
                var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: quantity, price: price, assetPairId: assetPair);

                fixClient.Send(marketOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null, "unexpected response");
                Assert.That(response, Is.TypeOf<ExecutionReport>(), "unexpected response type");

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.REJECTED));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.REJECTED));
            }
        }

        public class LimitOrderWrongQuantity : FixBaseTest
        {
            [TestCase(-1)]
            [TestCase(0)]
            [TestCase(9999999999999999)]
            [Category("FIX")]
            public void LimitOrderWrongQuantityTest(object q)
            {
                var orderId = Guid.NewGuid().ToString("N");
                var price = 0.01m;
                var quantity = Decimal.Parse(q.ToString());
                var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: quantity, price: price);

                fixClient.Send(marketOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null, "unexpected response");
                Assert.That(response, Is.TypeOf<ExecutionReport>(), "unexpected response type");

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.REJECTED));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.REJECTED));
            }
        }

        public class LimitOrderWrongPrice : FixBaseTest
        {
            [TestCase(-1)]
            [TestCase(0)]
            [TestCase(9999999999999999)]
            [Category("FIX")]
            public void LimitOrderWrongPriceTest(object p)
            {
                var orderId = Guid.NewGuid().ToString("N");
                var price = Decimal.Parse(p.ToString()); 
                var quantity = 0.01m;
                var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: quantity, price: price);

                fixClient.Send(marketOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null, "unexpected response");
                Assert.That(response, Is.TypeOf<ExecutionReport>(), "unexpected response type");

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.REJECTED));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.REJECTED));
            }
        }

        //test all assetPairs
        public class OnlyAssetsThatEnabledAreAvailableInFix : FixBaseTest
        {
            [Test]
            [Category("FIX")]
            public void OnlyAssetsThatEnabledAreAvailableInFixTest()
            {
                var user = new AccountRegistrationModel().GetTestModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(user);
                var assetPairs = walletApi.AssetPairs.GetAssetPairs(registeredClient.GetResponseObject().Result.Token).GetResponseObject();

                Assert.Multiple(() =>
                assetPairs.Result.AssetPairs.ToList().ForEach(a =>
                {
                    CreateLimitOrderWithAssetPair(a.Id);
                })
                );
            }

            void CreateLimitOrderWithAssetPair(string assetPair)
            {
                try
                {
                    var orderId = Guid.NewGuid().ToString("N");
                    var price = 0.01m;
                    var quantity = 0.01m;
                    var marketOrder = FixHelpers.CreateNewOrder(orderId, isMarket: false, isBuy: true, qty: quantity, price: price, assetPairId: assetPair);

                    fixClient.Send(marketOrder);

                    var response = fixClient.GetResponse<Message>();

                    Assert.That(response, Is.Not.Null);
                    Assert.That(response, Is.TypeOf<ExecutionReport>());

                    var ex = (ExecutionReport)response;
                    Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.PENDING_NEW));
                    Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.PENDING_NEW));
                    // clean myself
                    var cancelRequest = new OrderCancelRequest
                    {
                        ClOrdID = new ClOrdID(Guid.NewGuid().ToString()),
                        OrigClOrdID = new OrigClOrdID(orderId),
                        TransactTime = new TransactTime(DateTime.UtcNow)
                    };

                    fixClient.Send(cancelRequest);
                    response = fixClient.GetResponse<Message>();

                    Assert.That(response, Is.Not.Null);
                    Assert.That(response, Is.TypeOf<ExecutionReport>());

                    ex = (ExecutionReport)response;
                    if (ex.OrdStatus.Obj != OrdStatus.CANCELED)
                        response = fixClient.GetResponse<Message>();
                }
                catch (Exception e)
                {
                    Assert.Fail($"An error occured with assetPair {assetPair}. Error stacktrace: {e.StackTrace}");
                }
            }
        }
    }
}
