using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.Api.ApiModels.AuthModels;
using LykkeAutomation.TestsCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using TestsCore.ApiRestClient;
using TestsCore.TestsCore;
using static LykkeAutomation.Api.ApiModels.AuthModels.AuthModels;

namespace LykkeAutomation.Api.AuthResource
{
    public class Auth : ExternalRestApi
    {
        private const string resource = "/Auth";
        private const string resourceLogOut = "/Auth/LogOut";

        public IRestResponse PostAuthResponse(AuthenticateModel auth)
        {
            var request = new RestRequest(resource, Method.POST);
            request.AddJsonBody(auth);
            var response = client.Execute(request);
            return response;
        }

        public AuthModelResponse PostAuthResponseModel(AuthenticateModel auth)
        {
            return JsonConvert.DeserializeObject<AuthModelResponse>(PostAuthResponse(auth)?.Content);
        }

        public IRestResponse PostAuthLogOutResponse(AuthenticateModel auth)
        {
            var request = new RestRequest(resourceLogOut, Method.POST);
            request.AddJsonBody(auth);
            var response = client.Execute(request);
            return response;
        }

        public override void SetAllureProperties()
        {
            var isAlive = GetIsAlive();
            AllurePropertiesBuilder.Instance.AddPropertyPair("Service", client.BaseUrl.AbsoluteUri + "/api" + resource);
            AllurePropertiesBuilder.Instance.AddPropertyPair("Environment", isAlive.Env);
            AllurePropertiesBuilder.Instance.AddPropertyPair("Version", isAlive.Version);
        }

        public IsAliveResponse GetIsAlive()
        {
            var request = new RestRequest("/IsAlive", Method.GET);
            var response = client.Execute(request);
            var isAlive = JsonConvert.DeserializeObject<IsAliveResponse>(response.Content);
            return isAlive;
        }
    }
}
