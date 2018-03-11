using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class ApplicationInfo : ApiBase
    {
        private const string resource = "/ApplicationInfo";

        public IResponse<ResponseModelApplicationInfoResponseModel> GetApplicationInfo()
        {
            return Request.Get(resource).Build().Execute<ResponseModelApplicationInfoResponseModel>();
        }
    }
}
