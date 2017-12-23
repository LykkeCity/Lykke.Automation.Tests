using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon.TestsCore;
using XUnitTestCommon.DTOs;

namespace XUnitTestCommon.Consumers
{
    public class ApiConsumer
    {
        private readonly string _urlPrefix;
        private readonly string _baseUrl;
        private readonly bool _isSecure;

        private RestClient _client;
        private RestRequest _request;

        private readonly OAuthConsumer _oAuthConsumer;

        public ClientRegisterResponseDTO ClientInfo {
            get
            {
                if (_oAuthConsumer != null)
                    return _oAuthConsumer.ClientInfo;
                return null;
            }
            private set { }
        }

        public ApiConsumer(string urlPrefix, string baseUrl, bool isHttps, OAuthConsumer oAuthConsumer = null)
        {
            _urlPrefix = urlPrefix;
            _baseUrl = baseUrl;
            _isSecure = isHttps;


            _oAuthConsumer = oAuthConsumer;
            if (_oAuthConsumer != null)
                _oAuthConsumer.Authenticate().Wait();
        }

        public ApiConsumer(ConfigBuilder configBuilder, OAuthConsumer oAuthConsumer)
            : this(configBuilder.Config["UrlPefix"], configBuilder.Config["BaseUrl"], Boolean.Parse(configBuilder.Config["IsHttps"]), oAuthConsumer)
        { }

        public ApiConsumer(ConfigBuilder configBuilder) : this(configBuilder, new OAuthConsumer(configBuilder))
        {

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

            if (_oAuthConsumer != null)
            {
                await _oAuthConsumer.UpdateToken();
                _request.AddParameter("Authorization", "Bearer " + _oAuthConsumer.AuthToken, ParameterType.HttpHeader);
            }

            var response = await _client.ExecuteAsync(_request);

            Log(response);

            return new Response(response.StatusCode, response.Content);
        }

        public async Task<ClientRegisterResponseDTO> RegisterNewUser(ClientRegisterDTO registerInfo = null)
        {
            if (this._oAuthConsumer != null)
            {
                return await this._oAuthConsumer.RegisterNewUser(registerInfo);
            }
            return null;
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

        private void Log(IRestResponse response)
        {
            var requestBody = response.Request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);

            string attachName = $"{response.Request.Method} {response.Request.Resource}";
            var attachContext = new StringBuilder();
            attachContext.AppendLine($"Executing {response.Request.Method} {response.ResponseUri}");
            if (requestBody != null)
            {
                attachContext.AppendLine($"Content-Type: {requestBody.ContentType}").AppendLine();
                attachContext.AppendLine(requestBody.Value.ToString());
            }
            attachContext.AppendLine().AppendLine();
            attachContext.AppendLine($"Response: {response.StatusCode}");
            if (response.ErrorMessage != null)
                attachContext.AppendLine(response.ErrorMessage);
            attachContext.AppendLine(response.Content);
            Allure2Helper.AttachJson(attachName, attachContext.ToString());
        }
    }
}
