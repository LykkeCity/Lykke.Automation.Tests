using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.ApiV2.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace ApiV2Data.Api
{
    public class Affiliate : ApiBase
    {
        public IResponse<AffiliateLinkResponse> GetAffiliateLink(string token)
        {
            return Request.Get("/Affiliate/link").WithBearerToken(token).Build().Execute<AffiliateLinkResponse>();
        }

        public IResponse<AffiliateLinkResponse> PostAffiliateCreate(string token)
        {
            return Request.Post("/Affiliate/create").WithBearerToken(token).Build().Execute<AffiliateLinkResponse>();
        }

        public IResponse<ClientBalanceResponseModel> GetAffiliateStats(string walletId, string assetId, string token)
        {
            return Request.Get("/Affiliate/stats").WithBearerToken(token).AddQueryParameter("walletId", walletId).AddQueryParameter("assetId", assetId).Build().Execute<ClientBalanceResponseModel>();
        }
    }
}
