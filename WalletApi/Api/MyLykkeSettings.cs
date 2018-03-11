using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Client.AutorestClient.Models;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class MyLykkeSettings : ApiBase
    {
        public IResponse<ResponseModelMyLykkeSettingsModel> GetMyLykkeSettings(string token)
        {
            return Request.Get("/MyLykkeSettings").WithBearerToken(token)
                .Build().Execute<ResponseModelMyLykkeSettingsModel>();
        }
    }
}
