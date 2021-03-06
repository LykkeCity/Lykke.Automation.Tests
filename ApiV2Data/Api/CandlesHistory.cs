﻿using System;
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
                AddQueryParameterIfNotNull("Type", Type.ToSerializedValue()).
                AddQueryParameterIfNotNull("AssetPairId", AssetPairId).
                AddQueryParameterIfNotNull("PriceType", PriceType.ToSerializedValue()).
                AddQueryParameterIfNotNull("TimeInterval", TimeInterval.ToSerializedValue()).
                AddQueryParameterIfNotNull("FromMoment", FromMoment.ToString("s")).
                AddQueryParameterIfNotNull("ToMoment", ToMoment.ToString("s")).Build().Execute<CandleSticksResponseModel>();
        }
    }
}
