using QuickFix.FIX44;
using QuickFix.Fields;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIX.Client
{
    public class FixHelpers
    {

        public static NewOrderSingle CreateNewOrder(string clientOrderId, bool isMarket = true, bool isBuy = true, string assetPairId = "BTCUSD", decimal qty = 0.1m, decimal? price = null)
        {
            var nos = new NewOrderSingle
            {
                Account = new Account(Const.ClientId),
                ClOrdID = new ClOrdID(clientOrderId),
                Symbol = new Symbol(assetPairId),
                Side = isBuy ? new Side(Side.BUY) : new Side(Side.SELL),
                OrderQty = new OrderQty(qty),
                OrdType = isMarket ? new OrdType(OrdType.MARKET) : new OrdType(OrdType.LIMIT),
                Price = new Price(price ?? 0M),
                TimeInForce = isMarket ? new TimeInForce(TimeInForce.FILL_OR_KILL) : new TimeInForce(TimeInForce.GOOD_TILL_CANCEL),
                TransactTime = new TransactTime(DateTime.UtcNow)
            };
            return nos;
        }
    }
}
