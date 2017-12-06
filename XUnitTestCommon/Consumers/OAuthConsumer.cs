using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using XUnitTestCommon.DTOs;
using XUnitTestCommon.Utils;
using XUnitTestData.Domains.Authentication;
using XUnitTestData.Repositories;
using XUnitTestData.Entities.ApiV2;
using XUnitTestData.Domains.ApiV2;
using AzureStorage.Tables;

namespace XUnitTestCommon.Consumers
{
    public class OAuthConsumer : IAuthentication
    {
        public string BaseAuthUrl { get; set; }
        public string BaseRegisterUrl { get; set; }
        public string AuthPath { get; set; }
        public string RegisterPath { get; set; }
        public int AuthTokenTimeout { get; set; }
        public User AuthUser { get; set; }
        public ClientRegisterResponseDTO ClientInfo { get; set; }
        public string AuthToken { get; private set; }

        private DateTime? _tokenUpdateTime;

        public OAuthConsumer() { }

        public OAuthConsumer(ConfigBuilder config)
        {
            AuthTokenTimeout = Int32.Parse(config.Config["AuthTokenTimeout"]);
            AuthPath = config.Config["AuthPath"];
            RegisterPath = config.Config["RegisterPath"];
            BaseAuthUrl = config.Config["BaseUrlAuth"];
            BaseRegisterUrl = config.Config["BaseUrlRegister"];
            AuthUser = new User
            {
                Email = config.Config["AuthEmail"],
                Password = config.Config["AuthPassword"]
            };


        }

        public async Task<bool> Authenticate()
        {
            Task<bool> tokenTask = UpdateToken(true);
            return await tokenTask;
        }

        public async Task<bool> UpdateToken(bool forceUpdate = false)
        {
            try
            {
                //Only update token if it is expired
                if (!_tokenUpdateTime.HasValue || DateTime.UtcNow.Subtract(_tokenUpdateTime.Value).TotalMilliseconds >= AuthTokenTimeout || forceUpdate)
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

        /// <summary>
        /// Registers new user. If another user was inputted via one of the constructors it will be overwritten.
        /// </summary>
        /// <param name="registrationInfo"></param>
        /// <returns></returns>
        public async Task<ClientRegisterResponseDTO> RegisterNewUser(ClientRegisterDTO registrationInfo = null)
        {
            var regClient = new RestClient(BaseRegisterUrl);
            var regRequest = new RestRequest(RegisterPath, Method.POST);

            ClientRegisterDTO registerDTO = registrationInfo ?? new ClientRegisterDTO()
            {
                Email = Helpers.RandomString(8) + GlobalConstants.AutoTestEmail,
                FullName = Helpers.RandomString(5) + " " + Helpers.RandomString(8),
                ContactPhone = Helpers.Random.Next(1000000, 9999999).ToString(),
                Password = Helpers.RandomString(10),
                Hint = Helpers.RandomString(3)
            };

            string registerParam = JsonUtils.SerializeObject(registerDTO);
            regRequest.AddParameter("application/json", registerParam, ParameterType.RequestBody);

            var regResponse = await regClient.ExecuteAsync(regRequest);
            if (regResponse.StatusCode == HttpStatusCode.OK)
            {
                ClientRegisterResponseDTO parsedRegResponse = JsonUtils.DeserializeJson<ClientRegisterResponseDTO>(regResponse.Content);
                this.AuthUser = new User()
                {
                    Email = registerDTO.Email,
                    Password = registerDTO.Password
                };

                this.ClientInfo = parsedRegResponse;

                await this.Authenticate();

                return parsedRegResponse;
            }

            return null;
        }
    }
}
