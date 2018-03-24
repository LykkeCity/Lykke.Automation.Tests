using FIX.Client;
using NUnit.Framework;
using QuickFix.Fields;
using QuickFix.FIX44;
using System;
using System.Collections.Generic;
using System.Text;

namespace AFTests.FIX
{
    class FixLimitOrders
    {
        public class SetLimitOrderSell : FixBaseTest
        {
            [Test]
            [Category("FIX")]
            public void SetLimitOrderSellTest()
            {
                var orderId = Guid.NewGuid().ToString("N");
                var price = 0.01m;
                var quantity = 0.01m;
                var marketOrder = FixHelpers.CreateNewOrder(orderId,isMarket: false , isBuy: false, qty: quantity, price: price);

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
        }

        public class SetLimitOrderBuy : FixBaseTest
        {
            [Test]
            [Category("FIX")]
            public void SetLimitOrderBuyTest()
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
                Assert.That(ex.AvgPx.Obj, Is.EqualTo(0m), "Price should be 0 in case of Pending status");
                Assert.That(ex.OrdType.Obj, Is.EqualTo('2'), "Should be 2, this is Limit order");
                Assert.That(ex.OrderQty.Obj, Is.EqualTo(quantity), "unexpected quantity");
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
    }
}
