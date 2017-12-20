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
using TestsCore.RestRequests.Interfaces;
using TestsCore.TestsCore;

namespace LykkeAutomation.Api.RegistrationResource
{
    public class Registration : LykkeExternalApi
    {

        private const string resource = "/Registration";

        public IResponse GetRegistrationResponse(string token)
        {
            return Request.Get(resource).WithBearerToken(token).Build().Execute();
        }

        public IResponse<ResultRegistrationResponseModel> PostRegistrationResponse(AccountRegistrationModel user)
        {
            return Request.Post(resource).AddJsonBody(user).Build().Execute<ResultRegistrationResponseModel>();
        }
    }
}
