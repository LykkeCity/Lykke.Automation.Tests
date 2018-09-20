using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class CandlesHistory : ApiBase
    {

        public IResponse<CandleSticksResponseModel> GetCandlesHistory(MarketType Type, string AssetPairId, CandlePriceType PriceType, CandleTimeInterval TimeInterval, DateTime FromMoment, DateTime ToMoment)
        {
            return Request.Get($"/candlesHistory").
                AddQueryParameter("Type", Type.ToSerializedValue()).
                AddQueryParameter("PriceType", PriceType.ToSerializedValue()).
                AddQueryParameter("TimeInterval", TimeInterval.ToSerializedValue()).
                AddQueryParameter("FromMoment", FromMoment.ToString()).
                AddQueryParameter("ToMoment", ToMoment.ToString()).Build().Execute<CandleSticksResponseModel>();
        }
    }
}
