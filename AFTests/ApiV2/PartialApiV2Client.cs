using ApiV2Data.Fixtures;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using XUnitTestData.Domains.ApiV2;
using XUnitTestData.Entities.ApiV2;

namespace AFTests.ApiV2
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "ApiV2Service")]
    public partial class ApiV2Tests : IClassFixture<ApiV2TestDataFixture>
    {
        [Fact(Skip="ApiV2 currently has some issues")]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Client")]
        [Trait("Category", "ClientPost")]
        public async void RegisterClient()
        {
            string url = ApiPaths.CLIENT_REGISTER_PATH;

            ClientRegisterDTO registerDTO = new ClientRegisterDTO()
            {
                Email = Helpers.RandomString(8) + GlobalConstants.AutoTestEmail,
                FullName = Helpers.RandomString(5) + " " + Helpers.RandomString(8),
                ContactPhone = Helpers.Random.Next(1000000, 9999999).ToString(),
                Password = Helpers.RandomString(10),
                Hint = Helpers.RandomString(3),
                ClientInfo = Helpers.RandomString(5),
                PartnerId = Guid.NewGuid().ToString()
            };

            string registerParam = JsonUtils.SerializeObject(registerDTO);
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, registerParam, Method.POST);

            Assert.True(response.Status == System.Net.HttpStatusCode.OK);

            ClientDTO parsedResponse = JsonUtils.DeserializeJson<ClientDTO>(response.ResponseJson);

            PersonalDataEntity entity = await _fixture.PersonalDataRepository.TryGetAsync(
                p => p.PartitionKey == PersonalDataEntity.GeneratePartitionKey() && p.Email == registerDTO.Email) as PersonalDataEntity;
        }
    }
}
