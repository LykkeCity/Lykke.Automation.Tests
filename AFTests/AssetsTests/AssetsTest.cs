using AssetsData.Fixtures;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using XUnitTestCommon;

namespace AFTests.AssetsTests
{

    [Trait("Category", "FullRegression")]
    [Trait("Category", "AssetsService")]
    public partial class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        private AssetsTestDataFixture fixture;

        public AssetsTest(AssetsTestDataFixture fixture)
        {
            this.fixture = fixture;
        }

        #region IsAlive
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "IsAlive")]
        [Trait("Category", "IsAliveGet")]
        public async void IsAlive()
        {
            string url = fixture.ApiEndpointNames["assetIsAlive"];
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            Assert.True(response.ResponseJson.Contains("\"Env\":"));
            Assert.True(response.ResponseJson.Contains("\"Version\":"));
        }
        #endregion
    }
}
