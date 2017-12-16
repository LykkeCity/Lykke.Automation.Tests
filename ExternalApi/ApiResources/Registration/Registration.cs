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
using TestsCore.ApiRestClient;
using TestsCore.TestsCore;

namespace LykkeAutomation.Api.RegistrationResource
{
    public class Registration : ExternalRestApi
    {

        private const string resource = "/Registration";

        public IRestResponse GetRegistrationResponse(string token)
        {
            var request = new RestRequest(resource, Method.GET);
            request.AddHeader("Authorization", $"Bearer {token}");
            var response = client.Execute(request);
            return response;
        }

        public ResultRegistrationResponseModel PostRegistrationResponse(AccountRegistrationModel user)
        {
            var request = new RestRequest(resource, Method.POST);
            request.AddJsonBody(user);
            var response = client.Execute(request);
            return JsonConvert.DeserializeObject<ResultRegistrationResponseModel>(response?.Content);
        }

        public override void SetAllureProperties()
        {
            try
            {
                var isAlive = GetIsAlive();
                AllurePropertiesBuilder.Instance.AddPropertyPair("Service", client.BaseUrl.AbsoluteUri + "/api" + resource);
                AllurePropertiesBuilder.Instance.AddPropertyPair("Environment", isAlive.Env);
                AllurePropertiesBuilder.Instance.AddPropertyPair("Version", isAlive.Version);
            }catch (Exception)
            { }
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
