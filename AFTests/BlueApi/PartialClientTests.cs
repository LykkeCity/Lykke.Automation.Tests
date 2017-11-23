using System.Net;
using System.Threading.Tasks;
using BlueApiData.DTOs;
using NUnit.Framework;
using RestSharp;
using XUnitTestCommon;
using XUnitTestCommon.Utils;

namespace AFTests.BlueApi
{
    [Category("FullRegression")]
    [Category("BlueApiService")]
    public partial class BlueApiTests
    {
        [Test]
        [Category("Smoke")]
        [Category("Client")]
        [Category("GetUsersCountByPartner")]
        public async Task GetUsersCountByPartner()
        {
            var url = $"{ApiPaths.CLIENT_BASE_PATH}/getUsersCountByPartner";

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);

            var parsedResponse = JsonUtils.DeserializeJson<GetUsersCountByPartnerDto>(response.ResponseJson);

            Assert.IsNotNull(parsedResponse);
        }
    }
}