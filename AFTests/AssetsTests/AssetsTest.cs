using AssetsData.Fixtures;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using NUnit.Framework;
using XUnitTestCommon;

namespace AFTests.AssetsTests
{

    [Category("FullRegression")]
    [Category("AssetsService")]
    public partial class AssetsTest
    {
        private AssetsTestDataFixture fixture;

        public AssetsTest()
        {
            this.fixture = new AssetsTestDataFixture();
        }

        #region IsAlive
        [Test]
        [Category("Smoke")]
        [Category("IsAlive")]
        [Category("IsAliveGet")]
        public async Task IsAlive()
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
