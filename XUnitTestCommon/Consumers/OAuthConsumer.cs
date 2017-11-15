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
        public string baseAuthUrl { get; set; }
        public string authPath { get; set; }
        public int authTokenTimeout { get; set; }
        public User authentication { get; set; }
        public string authToken { get; private set; }

        private DateTime? tokenUpdateTime;

        public async Task<bool> Authenticate()
        {
            return await UpdateToken();
        }

        public async Task<bool> UpdateToken()
        {
            try
            {
                //Only update token if it is expired
                if (!tokenUpdateTime.HasValue || DateTime.UtcNow.Subtract(tokenUpdateTime.Value).TotalMilliseconds >= authTokenTimeout)
                {
                    this.authToken = await GetToken();
                    this.tokenUpdateTime = DateTime.UtcNow;
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
            var localClient = new RestClient(baseAuthUrl);
            var localRequest = new RestRequest(authPath, Method.POST);

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
    }
}