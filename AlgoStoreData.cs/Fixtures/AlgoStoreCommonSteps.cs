using AlgoStoreData.DTOs;
using AlgoStoreData.DTOs.InstanceData;
using AlgoStoreData.DTOs.InstanceData.Builders;
using AlgoStoreData.HelpersAlgoStore;
using ApiV2Data.DTOs;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Tests;
using XUnitTestCommon.Utils;
using XUnitTestData.Entities.AlgoStore;
using XUnitTestData.Enums;

namespace AlgoStoreData.Fixtures
{
    [TestFixture]
    public partial class AlgoStoreTestDataFixture : BaseTest
    {
        private string stopAlgoPath = ApiPaths.ALGO_STORE_ALGO_STOP;
        private string statisticsPath = ApiPaths.ALGO_STORE_STATISTICS;

        protected WalletDTO walletDTO = null;
        protected InstanceParameters instanceParameters = null;

        private string message = string.Empty;
        public static string CSharpAlgoStringFile = Path.Combine(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar, "AlgoStore" + Path.DirectorySeparatorChar, "TestData" + Path.DirectorySeparatorChar, "DummyAlgo.txt");
        public string CSharpAlgoString = File.ReadAllText(CSharpAlgoStringFile);

        public async Task<AlgoDataDTO> CreateAlgo()
        {
            var algoCreationTimestamp = Helpers.GetFullUtcTimestamp();
            CreateAlgoDTO metadata = new CreateAlgoDTO()
            {
                Name = $"{GlobalConstants.AutoTest}_AlgoMetaDataName_{algoCreationTimestamp}",
                Description = $"{ GlobalConstants.AutoTest }_AlgoMetaDataName_{algoCreationTimestamp} - Description",
                Content = Base64Helpers.EncodeToBase64(CSharpAlgoString)
            };

            var createAlgoResponse = await Consumer.ExecuteRequest(ApiPaths.ALGO_STORE_CREATE_ALGO, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            message = $"POST {ApiPaths.ALGO_STORE_CREATE_ALGO} returned status: {createAlgoResponse.Status} and response: {createAlgoResponse.ResponseJson}. Expected: {HttpStatusCode.OK}";
            Assert.That(createAlgoResponse.Status, Is.EqualTo(HttpStatusCode.OK), message);

            var algoData = JsonUtils.DeserializeJson<AlgoDataDTO>(createAlgoResponse.ResponseJson);
            // Add Algo to the list so that it can be deleted in the the TearDown
            algosList.Add(algoData);

            return algoData;
        }

        public async Task<InstanceDataDTO> SaveInstance(AlgoDataDTO algoData, AlgoInstanceType instanceType, bool useExistingWallet = true)
        {
            if (instanceType == AlgoInstanceType.Live)
            {
                walletDTO = useExistingWallet == true ? await GetExistingWallet() : await CreateTestWallet();
            }

            // Build days offset
            DaysOffsetDTO daysOffsetDTO = BuildDaysOffsetByInstanceType(instanceType);
            // Build InstanceParameters
            instanceParameters = new InstanceParameters()
            {
                AssetPair = "BTCUSD",
                TradedAsset = "USD",
                InstanceTradeVolume = 4,
                InstanceCandleInterval = CandleTimeInterval.Minute,
                FunctionCandleInterval = CandleTimeInterval.Day,
                FunctionCandleOperationMode = CandleOperationMode.CLOSE,
                FunctionCapacity = 4,
                InstanceFunctions = new List<FunctionType>() { FunctionType.SMA_Short, FunctionType.SMA_Long }
            };

            // Build instance request payload
            var instanceForAlgo = InstanceDataBuilder.BuildInstanceData(algoData, walletDTO, instanceType, instanceParameters, daysOffsetDTO);

            // Build save instance url
            var url = instanceType == AlgoInstanceType.Live ? ApiPaths.ALGO_STORE_SAVE_ALGO_INSTANCE : ApiPaths.ALGO_STORE_FAKE_TRADING_INSTANCE_DATA;

            string requestBody = JsonUtils.SerializeObject(instanceForAlgo);
            var saveInstanceResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, requestBody, Method.POST);
            message = $"POST {url} returned status: {saveInstanceResponse.Status} and response: {saveInstanceResponse.ResponseJson}. Expected: {HttpStatusCode.OK}";
            Assert.That(saveInstanceResponse.Status, Is.EqualTo(HttpStatusCode.OK), message);

            var instanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(saveInstanceResponse.ResponseJson);
            // Add Instance to the list so that it can be deleted in the the TearDown
            instancesList.Add(instanceData);

            return instanceData;
        }

        public async Task DeployInstance(InstanceDataDTO instanceData)
        {
            DeployBinaryDTO deploy = new DeployBinaryDTO()
            {
                AlgoId = instanceData.AlgoId,
                InstanceId = instanceData.InstanceId,
            };

            var deployBynaryResponse = await Consumer.ExecuteRequest(ApiPaths.ALGO_STORE_DEPLOY_BINARY, Helpers.EmptyDictionary, JsonUtils.SerializeObject(deploy), Method.POST);
            message = $"POST {ApiPaths.ALGO_STORE_DEPLOY_BINARY} returned status: {deployBynaryResponse.Status} and response: {deployBynaryResponse.ResponseJson}. Expected: {HttpStatusCode.OK}";
            Assert.That(deployBynaryResponse.Status, Is.EqualTo(HttpStatusCode.OK));
        }

        public async Task WaitAlgoInstanceToStart(string instanceId)
        {
            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == instanceId && t.PartitionKey != "StoppingEntity") as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);

            // Wait up to 3 minutes for the algo to be started
            int count = 45;
            while (instanceDataEntityExists.AlgoInstanceStatus != AlgoInstanceStatus.Started && count > 1) // TODO: Update when a health check endpoint is created
            {
                Wait.ForPredefinedTime(5000); // Wait for five secodns before getting the algo instance data again
                instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == instanceId) as ClientInstanceEntity;
                count--;
            }

            Wait.ForPredefinedTime(30000); // Wait for half a minute more so that the deploy can finish successfully
        }

        public async Task StopAlgoInstance(InstanceDataDTO instanceData)
        {
            StopBinaryDTO stopAlgo = new StopBinaryDTO()
            {
                AlgoId = instanceData.AlgoId,
                InstanceId = instanceData.InstanceId
            };
            var stopAlgoRequest = await Consumer.ExecuteRequest(stopAlgoPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);
            StopBinaryResponseDTO stopAlgoResponce = JsonUtils.DeserializeJson<StopBinaryResponseDTO>(stopAlgoRequest.ResponseJson);

            int retryCounter = 1;
            while (stopAlgoResponce != null && stopAlgoResponce.Status != null && !stopAlgoResponce.Status.Equals("Stopped") && retryCounter <= 30)
            {
                System.Threading.Thread.Sleep(10000);
                stopAlgoRequest = await Consumer.ExecuteRequest(stopAlgoPath, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);

                stopAlgoResponce = JsonUtils.DeserializeJson<StopBinaryResponseDTO>(stopAlgoRequest.ResponseJson);

                retryCounter++;
            }
        }

        public async Task<StatisticsDTO> GetStatisticsResponseAsync(InstanceDataDTO instanceData, int waitTime = 10000)
        {
            // Build statistics endpoint query param dictionary
            Dictionary<string, string> statisticsQueryParams = new Dictionary<string, string>()
            {
                { "instanceId", instanceData.InstanceId}
            };

            Wait.ForPredefinedTime(waitTime); // Wait for some trades to be done
            Response statisticsResponse = await Consumer.ExecuteRequest(statisticsPath, statisticsQueryParams, null, Method.GET);
            Assert.That(statisticsResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            return JsonUtils.DeserializeJson<StatisticsDTO>(statisticsResponse.ResponseJson);
        }

        public async Task DeleteAlgoInstance(InstanceDataDTO instanceData)
        {
            CascadeDeleteDTO deleteInstanceDTO = new CascadeDeleteDTO()
            {
                AlgoId = instanceData.AlgoId,
                AlgoClientId = instanceData.AlgoClientId,
                InstanceId = instanceData.InstanceId
            };
            var deleteInstanceRequest = await Consumer.ExecuteRequest(ApiPaths.ALGO_STORE_DELETE_INSTANCE, Helpers.EmptyDictionary, JsonUtils.SerializeObject(deleteInstanceDTO), Method.DELETE);
            Assert.That(deleteInstanceRequest.Status, Is.EqualTo(HttpStatusCode.NoContent));
        }

        public async Task MakeAlgoPublic(AlgoDataDTO algoData)
        {
            AddToPublicDTO addAlgo = new AddToPublicDTO()
            {
                AlgoId = algoData.Id,
                ClientId = algoData.ClientId
            };
            var makeAlgoPublicResponse = await Consumer.ExecuteRequest(ApiPaths.ALGO_STORE_ADD_TO_PUBLIC, Helpers.EmptyDictionary, JsonUtils.SerializeObject(addAlgo), Method.POST);
            Assert.That(makeAlgoPublicResponse.Status, Is.EqualTo(HttpStatusCode.OK));
        }

        public async Task MakeAlgoPrivate(AlgoDataDTO algoData)
        {
            AddToPublicDTO addAlgo = new AddToPublicDTO()
            {
                AlgoId = algoData.Id,
                ClientId = algoData.ClientId
            };
            var makeAlgoPrivateResponse = await Consumer.ExecuteRequest(ApiPaths.ALGO_STORE_REMOVE_FROM_PUBLIC, Helpers.EmptyDictionary, JsonUtils.SerializeObject(addAlgo), Method.POST);
            Assert.That(makeAlgoPrivateResponse.Status, Is.EqualTo(HttpStatusCode.OK));
        }

        public async Task<Response> DeleteAlgo(AlgoDataDTO algoData, bool forceDelete = false)
        {
            DeleteAlgoDTO deleteAlgoDTO = new DeleteAlgoDTO()
            {
                AlgoId = algoData.Id,
                AlgoClientId = algoData.ClientId,
                ForceDelete = forceDelete
            };

            // Delete the algo
            return await Consumer.ExecuteRequest(ApiPaths.ALGO_STORE_DELETE_ALGO, Helpers.EmptyDictionary, JsonUtils.SerializeObject(deleteAlgoDTO), Method.DELETE);
        }

        public async Task AssertAlgoDeleted(Response deleteAlgoRequest, AlgoDataDTO algoData)
        {
            AlgoErrorDTO deleteAlgoResponse = JsonUtils.DeserializeJson<AlgoErrorDTO>(deleteAlgoRequest.ResponseJson);

            Assert.That(deleteAlgoRequest.Status, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(deleteAlgoResponse, Is.Null);
            // Assert algo is deleted from DB
            Assert.That(await AlgoExists(algoData), Is.False);
        }

        public async Task AssertAlgoNotDeleted(Response deleteAlgoRequest, AlgoDataDTO algoData, string errorMessage)
        {
            AlgoErrorDTO deleteAlgoResponse = JsonUtils.DeserializeJson<AlgoErrorDTO>(deleteAlgoRequest.ResponseJson);

            Assert.That(deleteAlgoRequest.Status, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(deleteAlgoResponse.DisplayMessage, Is.EqualTo(errorMessage));
            // Assert algo is not deleted from DB
            Assert.That(await AlgoExists(algoData), Is.True);
        }

        public async Task<bool> AlgoExists(AlgoDataDTO algoDataDTO)
        {
            List<AlgoEntity> allUserAlgos = await AlgoRepository.GetAllAsync(t => t.ClientId == algoDataDTO.ClientId) as List<AlgoEntity>;
            return allUserAlgos.Exists(x => x.AlgoId == algoDataDTO.Id);
        }

        private async Task ClearTestData()
        {
            // Stop all created instances
            foreach (var instanceData in instancesList)
            {
                await StopAlgoInstance(instanceData);
            }

            // Delete all created instances
            await DeleteCreatedInstances();

            // Wait for a few seconds before deleting the algos
            Wait.ForPredefinedTime(3000);

            // Delete all created algos
            await DeleteCreatedAlgos();
        }

        public async Task DeleteCreatedInstances()
        {
            foreach (var instanceToDelete in instancesList)
            {
                // Get instance Data
                ClientInstanceEntity instanceDataFromDB = await ClientInstanceRepository.TryGetAsync(t => t.Id == instanceToDelete.InstanceId) as ClientInstanceEntity;
                var authToken = instanceDataFromDB.AuthToken;

                // Delete instance
                await DeleteAlgoInstance(instanceToDelete);

                // Delete instance pod if not already deleted
                var podExists = await InstancePodExists(instanceToDelete.InstanceId, authToken);
                if (podExists)
                {
                    await DeleteInstancePod(instanceToDelete.InstanceId, authToken);
                    await DeleteAlgoInstance(instanceToDelete);
                }
            }
        }

        public async Task DeleteCreatedAlgos()
        {
            foreach (var algoToDelete in algosList)
            {
                await DeleteAlgo(algoToDelete);
            }
        }

        public DaysOffsetDTO BuildDaysOffsetByInstanceType(AlgoInstanceType algoInstanceType)
        {
            if (algoInstanceType != AlgoInstanceType.Test)
            {
                return new DaysOffsetDTO()
                {
                    AlgoStartOffset = -1,
                    AlgoEndOffset = 60,
                    SmaShortStartOffset = -30,
                    SmaShortEndOffset = -3,
                    SmaLongStartOffset = -30,
                    SmaLongEndOffset = -3,
                    AdxStartOffset = -30,
                    AdxEndOffset = -3
                };
            }
            else
            {
                return new DaysOffsetDTO()
                {
                    AlgoStartOffset = -60,
                    AlgoEndOffset = -2,
                    SmaShortStartOffset = -59,
                    SmaShortEndOffset = -3,
                    SmaLongStartOffset = -59,
                    SmaLongEndOffset = -3,
                    AdxStartOffset = -59,
                    AdxEndOffset = -3
                };
            }
        }
    }
}
