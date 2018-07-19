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
    public partial class AlgoStoreTestsInstanceNotRequired : AlgoStoreTestDataFixture
    {
        [Test, Description("AL-524")]
        [Category("AlgoStore")]
        [Category("AlgoStoreLoggingApi")]
        public async Task CheckIsAlive()
        {
            var loggingServiceIsAliveUrl = $"{BaseUrl.AlgoStoreLoggingApiBaseUrl}{ApiPaths.ALGO_STORE_LOGGING_API_TAIL_LOG}";

            var getIsLoggingServiceAlive = await Consumer.ExecuteRequestCustomEndpoint(loggingServiceIsAliveUrl, Helpers.EmptyDictionary, null, Method.GET);
            Assert.That(getIsLoggingServiceAlive.Status, Is.EqualTo(HttpStatusCode.OK));

            AssertIsAlive(getIsLoggingServiceAlive, "Lykke.AlgoStore.Service.Logging");
        }

        private readonly string isAliveUrl = ApiPaths.ALGO_STORE_STOPPING_JOB_API_IS_ALIVE;

        [Test]
        [Category("AlgoStore")]
        [Category("AlgoStoreStoppingJobApi")]
        public async Task CheckStoppingJobApiIsAlive()
        {
            var stoppingServiceIsAliveUrl = $"{BaseUrl.AlgoStoreStoppingJobApiBaseUrl}{isAliveUrl}";

            // Get Algo Store Stopping Job IsAlive status
            var getIsStoppingServiceAlive = await Consumer.ExecuteRequestCustomEndpoint(stoppingServiceIsAliveUrl, Helpers.EmptyDictionary, null, Method.GET);
            Assert.That(getIsStoppingServiceAlive.Status, Is.EqualTo(HttpStatusCode.OK));

            AssertIsAlive(getIsStoppingServiceAlive, "Lykke.AlgoStore.Job.Stopping");
        }

        private void AssertIsAlive(Response requestReponse, string serviceName)
        {
            var isAliveDTO = JsonUtils.DeserializeJson<IsAliveDTO>(requestReponse.ResponseJson);
            Assert.That(isAliveDTO.Name, Is.EqualTo(serviceName));
            Assert.That(isAliveDTO.Version, Does.Match(GlobalConstants.ApiVersionRegexPattern).IgnoreCase);
        }
    }
}
