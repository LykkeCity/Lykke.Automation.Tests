using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon.RestRequests.Interfaces;
using Lykke.Client.AutorestClient.Models;

namespace WalletApi.Api
{
    public class Auth : ApiBase
    {
        public IResponse<ResponseModelAuthenticateResponseModel> PostAuthResponse(AuthenticateModel auth)
        {
            return Request.Post("Auth").AddJsonBody(auth).Build().Execute<ResponseModelAuthenticateResponseModel>();
        }

        public IResponse PostAuthLogOutResponse(AuthenticateModel auth)
        {
            return Request.Post("Auth/LogOut").AddJsonBody(auth).Build().Execute();

        }
    }
}
