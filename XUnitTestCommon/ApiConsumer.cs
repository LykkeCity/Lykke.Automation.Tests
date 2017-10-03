using XUnitTestCommon.DTOs;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XUnitTestCommon
{
    public class ApiConsumer
    {
        private ConfigBuilder _configBuilder;

        private string baseUrl;
        private string baseAuthUrl;

        private RestClient client;
        private RestRequest request;

        public ApiConsumer(ConfigBuilder configBuilder)
        {
            this._configBuilder = configBuilder;
            this.baseUrl = _configBuilder.Config["BaseUrl"];
            this.baseAuthUrl = _configBuilder.Config["BaseUrlAuth"];
        }

        public async Task<Response> ExecuteRequest(string token, string path, Dictionary<string, string> queryParams, string body, Method method, string urlPreffix)
        {
            var uri = BuildUri(urlPreffix, baseUrl, path);
            client = new RestClient(uri);
            request = new RestRequest(method);

            AddQueryParams(queryParams);

            if (body != null)
            {
                request.AddParameter("application/json", body, ParameterType.RequestBody);
            }
            if (token != null)
            {
                request.AddParameter("Authorization", "Bearer " + token, ParameterType.RequestBody);
            }


            var response = await client.ExecuteAsync(request);

            return new Response(response.StatusCode, response.Content);
        }

        public async Task<TokenDTO> GetToken(User user)
        {
            RestClient localClient = new RestClient(baseAuthUrl);
            RestRequest localRequest = new RestRequest("/api/Auth", Method.POST);

            var body = "{\"Email\": \"" + user.Email + "\", \"Password\": \"" + user.Password + "\" }";

            localRequest.AddParameter("application/json", body, ParameterType.RequestBody);

            var authResponse = await localClient.ExecuteAsync(localRequest);
            var token = JsonConvert.DeserializeObject<TokenDTO>(authResponse.Content);

            return token;
        }

        private void AddQueryParams(Dictionary<string, string> queryParams)
        {
            foreach (var param in queryParams)
            {
                request.AddQueryParameter(param.Key, param.Value);
            }
        }

        private Uri BuildUri(string urlPreffix, string baseUrl, string path, int? port = null)
        {
            UriBuilder uriBuilder = new UriBuilder($"http://{urlPreffix}.{baseUrl}");
            uriBuilder.Path = path;
            if (port != null)
            {
                uriBuilder.Port = port.Value;
            }

            return uriBuilder.Uri;
        }
    }
}
