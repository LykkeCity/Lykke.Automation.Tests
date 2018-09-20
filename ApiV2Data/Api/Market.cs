using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Market : ApiBase
    {

        public IResponse<ConvertionResponse> PostMarketConvertor(ConvertionRequest model)
        {
            return Request.Post("/market/converter").AddJsonBody(model).Build().Execute<ConvertionResponse>();
        }
    }
}
