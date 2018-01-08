using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.ApiModels;
using LykkeAutomation.ApiModels.RegistrationModels;
using LykkeAutomation.TestsCore;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon.RestRequests.Interfaces;
using XUnitTestCommon.TestsCore;

namespace WalletApi.Api.RegistrationResource
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
