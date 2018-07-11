using AlgoStoreData.DTOs;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Tests;
using XUnitTestCommon.Utils;

namespace AlgoStoreData.Fixtures
{
    public partial class AlgoStoreTestDataFixture : BaseTest
    {
        public async Task<bool> InstancePodExists(string instanceId, string authToken)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams.Add("instanceId", instanceId);

            var getInstancePodUrl = $"{BaseUrl.AlgoStoreStoppingJobApiBaseUrl}{ApiPaths.ALGO_STORE_STOPPING_JOB_API_GET_INSTANCE_PODS}";

            var getPodResponse = await Consumer.ExecuteRequestCustomEndpoint(getInstancePodUrl, queryParams, null, Method.GET, authToken);

            var instancePods = JsonUtils.DeserializeJson<List<InstancePodDTO>>(getPodResponse.ResponseJson);

            return instancePods != null;
        }

        public async Task DeleteInstancePod(string instanceId, string authToken)
        {
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams.Add("instanceId", instanceId);

            var deleteInstancePodUrl = $"{BaseUrl.AlgoStoreStoppingJobApiBaseUrl}{ApiPaths.ALGO_STORE_STOPPING_JOB_API_DELETE_POD_BY_INSTANCE_ID}";

            var deleteInstancePodResponse = await Consumer.ExecuteRequestCustomEndpoint(deleteInstancePodUrl, queryParams, null, Method.DELETE, authToken);
            message = $"POST {deleteInstancePodUrl} returned status: {deleteInstancePodResponse.Status} and response: {deleteInstancePodResponse.ResponseJson}. Expected: {HttpStatusCode.OK}";
            Assert.That(deleteInstancePodResponse.Status, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
