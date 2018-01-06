using Lykke.Client.AutorestClient.Models;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon.RestRequests.Interfaces;

namespace WalletApi.Api
{
    public class BackupCompleted : WalletApi
    {
        string resource = "/BackupCompleted";

        public IResponse<ResponseModel> PostBackupCompleted(string token)
        {
            return Request.Post(resource).WithBearerToken(token).Build().Execute<ResponseModel>();
        }
    }
}
