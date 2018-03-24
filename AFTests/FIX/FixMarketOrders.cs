using FIX.Client;
using NUnit.Framework;
using QuickFix.Fields;
using QuickFix.FIX44;
using System;
using System.Collections.Generic;
using System.Text;

namespace AFTests.FIX
{
    class FixMarketOrders
    {

        public class SetMarketBuyOrder : FixBaseTest
        {
            [Test]
            [Category("FIX")]
            public void SetMarketBuyOrderTest()
            {
                var orderId = Guid.NewGuid().ToString("N");
                var marketOrder = FixHelpers.CreateNewOrder(orderId);

                fixClient.Send(marketOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null);
                Assert.That(response, Is.TypeOf<ExecutionReport>());

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.PENDING_NEW));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.PENDING_NEW));


                response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null);
                Assert.That(response, Is.TypeOf<ExecutionReport>());

                ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.FILLED));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.TRADE));
                Assert.That(ex.LastQty.Obj, Is.EqualTo(marketOrder.OrderQty.Obj));
                Assert.That(ex.LastPx.Obj, Is.GreaterThan(0));
            }
        }

        public class SetMarketSellOrder : FixBaseTest
        {
            [Test]
            [Category("FIX")]
            public void SetMarketSellOrderTest()
            {
                var orderId = Guid.NewGuid().ToString("N");
                var marketOrder = FixHelpers.CreateNewOrder(orderId, isBuy: false);

                fixClient.Send(marketOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null);
                Assert.That(response, Is.TypeOf<ExecutionReport>());

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.PENDING_NEW));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.PENDING_NEW));


                response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null);
                Assert.That(response, Is.TypeOf<ExecutionReport>());

                ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.FILLED));
                Assert.That(ex.ExecType.Obj, Is.EqualTo(ExecType.TRADE));
                Assert.That(ex.LastQty.Obj, Is.EqualTo(marketOrder.OrderQty.Obj));
                Assert.That(ex.LastPx.Obj, Is.GreaterThan(0));
            }
        }

        public class SetMarketSellOrderWrongAssetPair : FixBaseTest
        {
            [Test]
            [Category("FIX")]
            public void SetMarketSellOrderWrongAssetPairTest()
            {
                var orderId = Guid.NewGuid().ToString("N");
                var marketOrder = FixHelpers.CreateNewOrder(orderId, isBuy: false, assetPairId:"FakeAssetId");

                fixClient.Send(marketOrder);

                var response = fixClient.GetResponse<Message>();

                Assert.That(response, Is.Not.Null);
                Assert.That(response, Is.TypeOf<ExecutionReport>());

                var ex = (ExecutionReport)response;
                Assert.That(ex.OrdStatus.Obj, Is.EqualTo(OrdStatus.REJECTED));
            }
        }
    }
}
