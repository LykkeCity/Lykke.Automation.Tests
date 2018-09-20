﻿using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Markets : ApiBase
    {
        public IResponse<MarketSlice> GetMarkets()
        {
            return Request.Get("/markets").Build().Execute<MarketSlice>();
        }

        public IResponse<MarketSlice> GetMarketsAssetPairId(string assetId)
        {
            return Request.Get($"/markets/{assetId}").Build().Execute<MarketSlice>();
        }
    }
}
