using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon.RestRequests.Interfaces;
using Lykke.Client.AutorestClient.Models;

namespace WalletApi.Api
{
    public class Registration : ApiBase
    {

        private const string resource = "/Registration";

        public IResponse<ResponseModelAccountsRegistrationResponseModel> GetRegistrationResponse(string token)
        {
            return Request.Get(resource).WithBearerToken(token).Build().Execute<ResponseModelAccountsRegistrationResponseModel>();
        }

        public IResponse<ResponseModelAccountsRegistrationResponseModel> PostRegistrationResponse(AccountRegistrationModel user)
        {
            return Request.Post(resource).AddJsonBody(user).Build().Execute<ResponseModelAccountsRegistrationResponseModel>();
        }
    }
}
