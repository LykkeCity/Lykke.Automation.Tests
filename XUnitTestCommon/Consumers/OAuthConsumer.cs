using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using XUnitTestCommon.DTOs;
using XUnitTestCommon.Utils;
using XUnitTestData.Domains.Authentication;
using User = XUnitTestData.Domains.Authentication.User;

namespace XUnitTestCommon.Consumers
{
    public class OAuthConsumer : IAuthentication
    {
        public string BaseAuthUrl { get; set; }
        public string AuthPath { get; set; }
        public int AuthTokenTimeout { get; set; }
        public User AuthUser { get; set; }
        public string AuthToken { get; private set; }

        private DateTime? _tokenUpdateTime;

        public async Task<bool> Authenticate()
        {
            return await UpdateToken();
        }

        public async Task<bool> UpdateToken()
        {
            try
            {
                //Only update token if it is expired
                if (!_tokenUpdateTime.HasValue || DateTime.UtcNow.Subtract(_tokenUpdateTime.Value).TotalMilliseconds >= AuthTokenTimeout)
                {
                    AuthToken = await GetToken();
                    _tokenUpdateTime = DateTime.UtcNow;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private async Task<string> GetToken()
        {
            var localClient = new RestClient(BaseAuthUrl);
            var localRequest = new RestRequest(AuthPath, Method.POST);

            var body = JsonUtils.SerializeObject(AuthUser);

            localRequest.AddParameter("application/json", body, ParameterType.RequestBody);

            var authResponse = await localClient.ExecuteAsync(localRequest);

            var token = JsonConvert.DeserializeObject<TokenDTO>(authResponse.Content);

            if (authResponse.StatusCode != HttpStatusCode.OK)
            {
                throw new ArgumentException("Could not get token with the provided credentials", new ArgumentException(token.message));
            }

            return token.AccessToken;
        }
    }
}