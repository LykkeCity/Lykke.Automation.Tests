using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class ClientTrading : ApiBase
    {
        string resource = "/ClientTrading";

        public IResponse<ResponseModelTermsOfUseModel> GetClientTradingTermsOfUse(string token)
        {
            return Request.Get(resource + "/termsOfUse").WithBearerToken(token).Build().Execute<ResponseModelTermsOfUseModel>();
        }

        public IResponse<ResponseModel> PostClientTradingTermsOfUseAgree(string token)
        {
            return Request.Post(resource + "/termsOfUse/margin/agree").WithBearerToken(token).Build().Execute<ResponseModel>();
        }
    }
}
