using System.Net;
using BlueApiData.Fixtures;
using RestSharp;
using Xunit;
using XUnitTestCommon;

namespace AFTests.BlueApi
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "BlueApiService")]
    public partial class BlueApiTests : IClassFixture<BlueApiTestDataFixture>
    {
        private readonly BlueApiTestDataFixture _fixture;

        public BlueApiTests(BlueApiTestDataFixture fixture)
        {
            _fixture = fixture;
        }

        #region IsAlive
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "IsAlive")]
        [Trait("Category", "IsAliveGet")]
        public async void IsAlive()
        {
            string url = _fixture.ApiEndpointNames["IsAlive"];
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            Assert.True(response.ResponseJson.Contains("\"Env\":"));
            Assert.True(response.ResponseJson.Contains("\"Version\":"));
        }
        #endregion
    }
}
