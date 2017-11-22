using System.Net;
using System.Threading.Tasks;
using BlueApiData.Fixtures;
using NUnit.Framework;
using RestSharp;
using XUnitTestCommon;
using XUnitTestCommon.Tests;

namespace AFTests.BlueApi
{
    [Category("FullRegression")]
    [Category("BlueApiService")]
    public partial class BlueApiTests: BaseTest
    {
        private readonly BlueApiTestDataFixture _fixture;

        public BlueApiTests()
        {
            this._fixture = new BlueApiTestDataFixture();
        }

        #region IsAlive
        [Test]
        [Category("Smoke")]
        [Category("IsAlive")]
        [Category("IsAliveGet")]
        public async Task IsAlive()
        {
            string url = ApiPaths.ISALIVE_BASE_PATH;
            var response = await _fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            Assert.True(response.ResponseJson.Contains("\"Env\":"));
            Assert.True(response.ResponseJson.Contains("\"Version\":"));
        }
        #endregion
    }
}
