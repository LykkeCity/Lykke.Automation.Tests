using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XUnitTestCommon.Consumers
{
    public class ApiConsumer
    {
        private string urlPrefix;
        private string baseUrl;
        private bool isSecure;

        private RestClient client;
        private RestRequest request;

        private ConfigBuilder _configBuilder;

        private OAuthConsumer _oAuthConsumer;

        public ApiConsumer(ConfigBuilder configBuilder, OAuthConsumer oAuthConsumer)
        {
            this._configBuilder = configBuilder;
            this.urlPrefix = _configBuilder.Config["UrlPefix"];
            this.baseUrl = _configBuilder.Config["BaseUrl"];
            this.isSecure = Boolean.Parse(_configBuilder.Config["IsHttps"]);

            _oAuthConsumer = oAuthConsumer;
            _oAuthConsumer.Authenticate().Wait();
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

            await _oAuthConsumer.UpdateToken();
            request.AddParameter("Authorization", "Bearer " + _oAuthConsumer.authToken, ParameterType.HttpHeader);

            var response = await client.ExecuteAsync(request);

            return new Response(response.StatusCode, response.Content);
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
