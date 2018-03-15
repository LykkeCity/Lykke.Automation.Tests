using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
     public class CheckMobilePhone : ApiBase
    {
        string resource = "/CheckMobilePhone";

        public IResponse<ResponseModelCheckMobilePhoneResultModel> GetCheckMobilePhone(string phoneNumber, string code, string token)
        {
            return Request.Get(resource).AddQueryParameterIfNotNull("phoneNumber", phoneNumber).AddQueryParameterIfNotNull("code", code)
                .WithBearerToken(token).Build().Execute<ResponseModelCheckMobilePhoneResultModel>();
        }

        public IResponse<ResponseModel> PostCheckMobilePhone(PostClientPhoneModel model, string token)
        {
            return Request.Post(resource).AddJsonBody(model).WithBearerToken(token).Build().Execute<ResponseModel>();
        }
    }
}
