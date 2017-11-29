using RestSharp;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using XUnitTestData.Entities.ApiV2;
using XUnitTestCommon.DTOs;
using XUnitTestData.Domains.Authentication;
using ApiV2Data.DTOs;

namespace AFTests.ApiV2
{
    [Category("FullRegression")]
    [Category("ApiV2Service")]
    public partial class ApiV2Tests
    {
        [Test]
        [Category("Smoke")]
        [Category("Client")]
        [Category("ClientPost")]
        public async Task RegisterAuthClient()
        {
            string url = ApiPaths.CLIENT_REGISTER_PATH;

            ClientRegisterDTO registerDTO = new ClientRegisterDTO()
            {
                Email = Helpers.RandomString(8) + GlobalConstants.AutoTestEmail,
                FullName = Helpers.RandomString(5) + " " + Helpers.RandomString(8),
                ContactPhone = Helpers.Random.Next(1000000, 9999999).ToString(),
                Password = Helpers.RandomString(10),
                Hint = Helpers.RandomString(3)
            };

            string registerParam = JsonUtils.SerializeObject(registerDTO);
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, registerParam, Method.POST);

            Assert.True(response.Status == System.Net.HttpStatusCode.OK);

            ClientDTO parsedResponse = JsonUtils.DeserializeJson<ClientDTO>(response.ResponseJson);

            PersonalDataEntity pdEntity = await this.PersonalDataRepository.TryGetAsync(
                p => p.PartitionKey == PersonalDataEntity.GeneratePartitionKey() && p.Email == registerDTO.Email.ToLower()) as PersonalDataEntity;

            Assert.NotNull(pdEntity);

            Assert.True(pdEntity.ContactPhone == registerDTO.ContactPhone);
            Assert.True(pdEntity.FullName == registerDTO.FullName);
            Assert.True(pdEntity.PasswordHint == registerDTO.Hint);

            TradersEntity traderEntity = await this.TradersRepository.TryGetAsync(
                t => t.PartitionKey == "Trader" && t.Id == pdEntity.RowKey
                ) as TradersEntity;

            Assert.NotNull(traderEntity);


            TradersEntity traderEmailEntity = await this.TradersRepository.TryGetAsync(
                t => t.PartitionKey == "IndexEmail" && t.Id == registerDTO.Email && t.PrimaryRowKey == pdEntity.RowKey
                ) as TradersEntity;

            Assert.NotNull(traderEmailEntity);

            //Authentication
            User newUser = new User()
            {
                Email = registerDTO.Email,
                Password = registerDTO.Password
            };
            string userParam = JsonUtils.SerializeObject(newUser);


            string authUrl = ApiPaths.CLIENT_BASE_PATH + "/auth";
            var authResponse = await this.Consumer.ExecuteRequest(authUrl, Helpers.EmptyDictionary, userParam, Method.POST);

            Assert.True(authResponse.Status == HttpStatusCode.OK);
        }

        [Test]
        [Category("Smoke")]
        [Category("Client")]
        [Category("ClientPost")]
        public async Task UserInfo()
        {
            string url = ApiPaths.CLIENT_INFO_PATH;
            var response = await this.ClientInfoConsumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            ClientInfoDTO parsedResponse = JsonUtils.DeserializeJson<ClientInfoDTO>(response.ResponseJson);
            Assert.True(parsedResponse.Email == this.ClientInfoConsumer.ClientInfo.Account.Email);
            //Assert.True(parsedResponse.FirstName == ClientInfoInstance.FullName);
            //Assert.True(parsedResponse.LastName == ClientInfoInstance.FullName);

        }


    }
}
