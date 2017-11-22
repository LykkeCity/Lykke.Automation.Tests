using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XUnitTestData.Domains.Authentication;

namespace XUnitTestCommon.Consumers
{
    public class ApiConsumer
    {
        private readonly string _urlPrefix;
        private readonly string _baseUrl;
        private readonly bool _isSecure;

        private RestClient _client;
        private RestRequest _request;

        private readonly ConfigBuilder _configBuilder;

        private readonly OAuthConsumer _oAuthConsumer;

        public UserExtended ClientInfo => _oAuthConsumer.ClientInfo;

        public ApiConsumer(ConfigBuilder configBuilder, OAuthConsumer oAuthConsumer)
        {
            _configBuilder = configBuilder;

            _urlPrefix = _configBuilder.Config["UrlPefix"];
            _baseUrl = _configBuilder.Config["BaseUrl"];
            _isSecure = Boolean.Parse(_configBuilder.Config["IsHttps"]);

            _oAuthConsumer = oAuthConsumer;
            _oAuthConsumer.Authenticate().Wait();
        }

        public async Task<Response> ExecuteRequest(string path, Dictionary<string, string> queryParams, string body, Method method)
        {
            var uri = BuildUri(_urlPrefix, _baseUrl, path);
            _client = new RestClient(uri);
            _request = new RestRequest(method);

            AddQueryParams(queryParams);

            if (body != null)
            {
                _request.AddParameter("application/json", body, ParameterType.RequestBody);
            }

            await _oAuthConsumer.UpdateToken();
            _request.AddParameter("Authorization", "Bearer " + _oAuthConsumer.AuthToken, ParameterType.HttpHeader);

            var response = await _client.ExecuteAsync(_request);

            return new Response(response.StatusCode, response.Content);
        }

        private void AddQueryParams(Dictionary<string, string> queryParams)
        {
            foreach (var param in queryParams)
            {
                _request.AddQueryParameter(param.Key, param.Value);
            }
        }

        private Uri BuildUri(string urlPreffix, string baseUrl, string path, int? port = null)
        {
            string protocol = "http";
            if (_isSecure)
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
