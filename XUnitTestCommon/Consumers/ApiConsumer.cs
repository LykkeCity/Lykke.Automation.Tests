using XUnitTestCommon.DTOs;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XUnitTestCommon.Utils;
using System.Net;

namespace XUnitTestCommon.Consumers
{
    public enum ApiConsumerType
    {
        Default,
        Create,
        Update,
        Delete
    }

    public class ApiConsumer
    {
        private string urlPrefix;
        private string baseUrl;
        private string baseAuthUrl;
        private bool isSecure;
        private string authPath;
        private int authTokenTimeout;
        private DateTime tokenUpdateTime;

        private User authentication;
        private string authToken;

        private RestClient client;
        private RestRequest request;

        private ConfigBuilder _configBuilder;

        public ApiConsumer(ConfigBuilder configBuilder)
        {
            this._configBuilder = configBuilder;
            this.urlPrefix = _configBuilder.Config["UrlPefix"];
            this.baseUrl = _configBuilder.Config["BaseUrl"];
            this.isSecure = Boolean.Parse(_configBuilder.Config["IsHttps"]);
            this.authentication = null;
        }

        public async Task<Response> ExecuteRequest(string path, Dictionary<string, string> queryParams, string body, Method method)
        {
            var uri = BuildUri(urlPrefix, baseUrl, path);
            client = new RestClient(uri);
            request = new RestRequest(method);

            AddQueryParams(queryParams);

            if (body != null)
            {
                request.AddParameter("application/json", body, ParameterType.RequestBody);
            }
            if (authentication != null)
            {
                if (DateTime.UtcNow.Subtract(tokenUpdateTime).TotalMilliseconds >= authTokenTimeout)
                {
                    if (!await UpdateToken())
                    {
                        throw new Exception("couldn't update token");
                    }
                }
                request.AddParameter("Authorization", "Bearer " + this.authToken, ParameterType.HttpHeader);
            }


            var response = await client.ExecuteAsync(request);

            return new Response(response.StatusCode, response.Content);
        }

        private async Task<string> GetToken()
        {
            RestClient localClient = new RestClient(baseAuthUrl);
            RestRequest localRequest = new RestRequest(authPath, Method.POST);

            var body = JsonUtils.SerializeObject(authentication);

            localRequest.AddParameter("application/json", body, ParameterType.RequestBody);

            var authResponse = await localClient.ExecuteAsync(localRequest);

            var token = JsonConvert.DeserializeObject<TokenDTO>(authResponse.Content);

            if (authResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ArgumentException("Could not get token with the provided credentials", new ArgumentException(token.message));
            }

            return token.AccessToken;
        }

        public bool Authenticate(ApiConsumerType apiConsumerType = ApiConsumerType.Default)
        {
            this.baseAuthUrl = _configBuilder.Config["BaseUrlAuth"];
            this.authPath = _configBuilder.Config["AuthPath"];
            this.authTokenTimeout = Int32.Parse(_configBuilder.Config["AuthTokenTimeout"]);

            var authEmail = _configBuilder.Config["AuthEmail"];

            if (apiConsumerType == ApiConsumerType.Create)
                authEmail = _configBuilder.Config["PledgeCreateAuthEmail"];
            else if (apiConsumerType == ApiConsumerType.Update)
                authEmail = _configBuilder.Config["PledgeUpdateAuthEmail"];
            else if (apiConsumerType == ApiConsumerType.Delete)
                authEmail = _configBuilder.Config["PledgeDeleteAuthEmail"];


            this.authentication = new User(
                authEmail,
                _configBuilder.Config["AuthPassword"],
                _configBuilder.Config["AuthClientInfo"],
                _configBuilder.Config["AuthPartnerId"]);

            return Task.Run(async () => { return await UpdateToken(); }).Result;
        }

        private async Task<bool> UpdateToken()
        {
            try
            {
                this.authToken = await GetToken();
                this.tokenUpdateTime = DateTime.UtcNow;
                return true;
            }
            catch
            {
                return false;
            }
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
            string protocol = "http";
            if (this.isSecure)
                protocol = "https";
            UriBuilder uriBuilder = new UriBuilder($"{protocol}://{urlPreffix}.{baseUrl}");
            uriBuilder.Path = path;
            if (port != null)
            {
                uriBuilder.Port = port.Value;
            }

            return uriBuilder.Uri;
        }
    }
}
