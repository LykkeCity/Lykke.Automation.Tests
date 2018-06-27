using AlgoStoreData.DTOs;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.Utils;
using XUnitTestData.Domains.AlgoStore;
using XUnitTestData.Entities.AlgoStore;
using XUnitTestData.Repositories;

namespace AFTests.AlgoStore
{
    public class AlgoStoreCommonSteps
    {
        private static string stopAlgoPath = ApiPaths.ALGO_STORE_ALGO_STOP;
        private static string statisticsPath = ApiPaths.ALGO_STORE_STATISTICS;

        public static async Task WaitAlgoToStart(GenericRepository<ClientInstanceEntity, IClientInstance> clientInstanceRepository, InstanceDataDTO postInstanceData)
        {
            ClientInstanceEntity instanceDataEntityExists = await clientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);

            // Wait up to 3 minutes for the algo to be started
            int count = 45;
            while (instanceDataEntityExists.AlgoInstanceStatusValue != "Started" && count > 1) // TODO: Update when a health check endpoint is created
            {
                Wait.ForPredefinedTime(5000); // Wait for five secodns before getting the algo instance data again
                instanceDataEntityExists = await clientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
                count--;
            }

            Wait.ForPredefinedTime(30000); // Wait for half a minute more so that the deploy can finish successfully
        }

        public static async Task StopAlgoInstance(ApiConsumer apiConsumer, InstanceDataDTO postInstanceData)
        {
            StopBinaryDTO stopAlgo = new StopBinaryDTO()
            {
                AlgoId = postInstanceData.AlgoId,
                InstanceId = postInstanceData.InstanceId
            };
            var stopAlgoRequest = await apiConsumer.ExecuteRequest(stopAlgoPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);
            StopBinaryResponseDTO stopAlgoResponce = JsonUtils.DeserializeJson<StopBinaryResponseDTO>(stopAlgoRequest.ResponseJson);

            int retryCounter = 1;
            while ((stopAlgoResponce.Status.Equals("Deploying") || stopAlgoResponce.Status.Equals("Started")) && retryCounter <= 30)
            {
                System.Threading.Thread.Sleep(10000);
                stopAlgoRequest = await apiConsumer.ExecuteRequest(stopAlgoPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);

                stopAlgoResponce = JsonUtils.DeserializeJson<StopBinaryResponseDTO>(stopAlgoRequest.ResponseJson);

                retryCounter++;
            }
        }

        public static async Task<StatisticsDTO> GetStatisticsResponseAsync(ApiConsumer apiConsumer, InstanceDataDTO postInstanceData, int waitTime = 10000)
        {
            // Build statistics endpoint query param dictionary
            Dictionary<string, string> statisticsQueryParams = new Dictionary<string, string>()
            {
                { "instanceId", postInstanceData.InstanceId}
            };

            Wait.ForPredefinedTime(waitTime); // Wait for some trades to be done
            Response statisticsResponse = await apiConsumer.ExecuteRequest(statisticsPath, statisticsQueryParams, null, Method.GET);
            Assert.That(statisticsResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            return JsonUtils.DeserializeJson<StatisticsDTO>(statisticsResponse.ResponseJson);
        }

        public static async Task DeleteAlgoInstance(ApiConsumer apiConsumer, InstanceDataDTO instanceData)
        {
            CascadeDeleteDTO deleteInstanceDTO = new CascadeDeleteDTO()
            {
                AlgoId = instanceData.AlgoId,
                AlgoClientId = instanceData.AlgoClientId,
                InstanceId = instanceData.InstanceId
            };
            var deleteInstanceRequest = await apiConsumer.ExecuteRequest(ApiPaths.ALGO_STORE_DELETE_INSTANCE, Helpers.EmptyDictionary, JsonUtils.SerializeObject(deleteInstanceDTO), Method.DELETE);
            Assert.That(deleteInstanceRequest.Status, Is.EqualTo(HttpStatusCode.NoContent));
        }

        public static async Task MakeAlgoPublic(ApiConsumer apiConsumer, InstanceDataDTO instanceData)
        {
            AddToPublicDTO addAlgo = new AddToPublicDTO()
            {
                AlgoId = instanceData.AlgoId,
                ClientId = instanceData.AlgoClientId
            };
            var makeAlgoPublicResponse = await apiConsumer.ExecuteRequest(ApiPaths.ALGO_STORE_ADD_TO_PUBLIC, Helpers.EmptyDictionary, JsonUtils.SerializeObject(addAlgo), Method.POST);
            Assert.That(makeAlgoPublicResponse.Status, Is.EqualTo(HttpStatusCode.OK));
        }

        public static async Task MakeAlgoPrivate(ApiConsumer apiConsumer, InstanceDataDTO instanceData)
        {
            AddToPublicDTO addAlgo = new AddToPublicDTO()
            {
                AlgoId = instanceData.AlgoId,
                ClientId = instanceData.AlgoClientId
            };
            var makeAlgoPrivateResponse = await apiConsumer.ExecuteRequest(ApiPaths.ALGO_STORE_REMOVE_FROM_PUBLIC, Helpers.EmptyDictionary, JsonUtils.SerializeObject(addAlgo), Method.POST);
            Assert.That(makeAlgoPrivateResponse.Status, Is.EqualTo(HttpStatusCode.OK));
        }

        public static async Task<ClientInstanceEntity> GetStoppingEntityForInstance(GenericRepository<ClientInstanceEntity, IClientInstance> clientInstanceRepository, InstanceDataDTO postInstanceData)
        {
            return await clientInstanceRepository.TryGetAsync(t => t.InstanceId == postInstanceData.InstanceId) as ClientInstanceEntity;
        }
    }
}
