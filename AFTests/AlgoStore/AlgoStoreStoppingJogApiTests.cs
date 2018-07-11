using AlgoStoreData.DTOs;
using AlgoStoreData.Fixtures;
using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Utils;

namespace AFTests.AlgoStore
{
    [Category("FullRegression")]
    [Category("AlgoStore")]
    [Category("AlgoStoreStoppingJobApi")]
    class AlgoStoreStoppingJogApiTests : AlgoStoreTestDataFixture
    {
        private readonly string isAliveUrl = ApiPaths.ALGO_STORE_STOPPING_JOB_API_IS_ALIVE;

        [Test, Description("AL-483")]
        [Category("AlgoStore")]
        [Category("AlgoStoreStoppingJobApi")]
        public async Task CheckStoppingJobApiIsAlive()
        {
            var url = $"{BaseUrl.AlgoStoreStoppingJobApiBaseUrl}{isAliveUrl}";

            // Get Algo Store Stopping Job IsAlive status
            var isAliveRequest = await Consumer.ExecuteRequestCustomEndpoint(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.That(isAliveRequest.Status, Is.EqualTo(HttpStatusCode.OK));

            var isAliveDTO = JsonUtils.DeserializeJson<IsAliveDTO>(isAliveRequest.ResponseJson);
            Assert.That(isAliveDTO.Name, Is.EqualTo("Lykke.AlgoStore.Job.Stopping"));
            Assert.That(isAliveDTO.Version, Does.Match(GlobalConstants.ApiVersionRegexPattern).IgnoreCase);
        }
    }
}
