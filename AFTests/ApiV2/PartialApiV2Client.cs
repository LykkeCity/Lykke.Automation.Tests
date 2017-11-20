using RestSharp;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using XUnitTestData.Entities.ApiV2;
using XUnitTestCommon.DTOs;
using XUnitTestData.Domains.Authentication;

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
        public async Task RegisterClient()
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
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, registerParam, Method.POST);

            Assert.True(response.Status == System.Net.HttpStatusCode.OK);

            ClientDTO parsedResponse = JsonUtils.DeserializeJson<ClientDTO>(response.ResponseJson);


            //test auth
            User newUser = new User()
            {
                Email = registerDTO.Email,
                Password = registerDTO.Password
            };
            string userParam = JsonUtils.SerializeObject(newUser);


            string authUrl = ApiPaths.CLIENT_BASE_PATH + "/auth";
            var authResponse = await _fixture.Consumer.ExecuteRequest(authUrl, Helpers.EmptyDictionary, userParam, Method.POST);

            Assert.True(authResponse.Status == HttpStatusCode.OK);

            PersonalDataEntity entity = await _fixture.PersonalDataRepository.TryGetAsync(
                p => p.PartitionKey == PersonalDataEntity.GeneratePartitionKey() && p.Email == registerDTO.Email.ToLower()) as PersonalDataEntity;

            Assert.NotNull(entity);

            Assert.True(entity.ContactPhone == registerDTO.ContactPhone);
            Assert.True(entity.FullName == registerDTO.FullName);
            Assert.True(entity.PasswordHint == registerDTO.Hint);
        }
    }
}
