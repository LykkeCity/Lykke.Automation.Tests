using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class AppSettings : WalletApi
    {
        string resource = "/AppSettings";

        public IResponse<ResponseModelApiAppSettingsModel> GetAppSettings(string authorization)
        {
            return Request.Get(resource).WithBearerToken(authorization).Build().Execute<ResponseModelApiAppSettingsModel>();
        }
    }
}
