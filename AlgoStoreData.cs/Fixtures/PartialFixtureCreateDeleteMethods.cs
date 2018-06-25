using AlgoStoreData.DTOs;
using AlgoStoreData.HelpersAlgoStore;
using ApiV2Data.DTOs;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Tests;
using XUnitTestCommon.Utils;
using XUnitTestData.Entities.AlgoStore;
using XUnitTestData.Enums;

namespace AlgoStoreData.Fixtures
{
    public partial class AlgoStoreTestDataFixture : BaseTest
    {
        private string message;
        public AlgoDataDTO algoData;
        public AlgoInstanceType instanceType = AlgoInstanceType.Live;

        public async Task<AlgoDataDTO> CreateAlgo()
        {
            CreateAlgoDTO metadata = new CreateAlgoDTO()
            {
                Name = $"{GlobalConstants.AutoTest}_AlgoMetaDataName_{Helpers.GetFullUtcTimestamp()}",
                Description = $"{ GlobalConstants.AutoTest }_AlgoMetaDataName_{Helpers.GetFullUtcTimestamp()} - Description",
                Content = Base64Helpers.EncodeToBase64(CSharpAlgoString)
            };

            var createAlgoResponse = await Consumer.ExecuteRequest(ApiPaths.ALGO_STORE_CREATE_ALGO, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            message = $"POST {ApiPaths.ALGO_STORE_CREATE_ALGO} returned status: {createAlgoResponse.Status} and response: {createAlgoResponse.ResponseJson}. Expected: {HttpStatusCode.OK}";
            Assert.That(createAlgoResponse.Status, Is.EqualTo(HttpStatusCode.OK), message);

            return JsonUtils.DeserializeJson<AlgoDataDTO>(createAlgoResponse.ResponseJson);
        }

        public async Task<InstanceDataDTO> SaveInstance(AlgoDataDTO algoData, AlgoInstanceType instanceType, bool useExistingWallet = true)
        {
            WalletDTO walletDTO = useExistingWallet == true ? await GetExistingWallet() : await CreateTestWallet();

            instanceForAlgo = GetPopulatedInstanceDataDTO.ReturnInstanceDataDTO(algoData.Id, walletDTO, instanceType);

            var url = instanceType == AlgoInstanceType.Live ? ApiPaths.ALGO_STORE_SAVE_ALGO_INSTANCE : ApiPaths.ALGO_STORE_FAKE_TRADING_INSTANCE_DATA;

            string requestBody = JsonUtils.SerializeObject(instanceForAlgo);
            var saveInstanceResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, requestBody, Method.POST);
            message = $"POST {url} returned status: {saveInstanceResponse.Status} and response: {saveInstanceResponse.ResponseJson}. Expected: {HttpStatusCode.OK}";
            Assert.That(saveInstanceResponse.Status, Is.EqualTo(HttpStatusCode.OK), message);

            return JsonUtils.DeserializeJson<InstanceDataDTO>(saveInstanceResponse.ResponseJson);
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

        public async Task<List<BuilInitialDataObjectDTO>> CreateAlgoAndStartInstance(int numberOfInstances)
        {
            List<BuilInitialDataObjectDTO> instancesList = new List<BuilInitialDataObjectDTO>();

            for (int i = 0; i < numberOfInstances; i++)
            {
                algoData = await CreateAlgo();
                InstanceDataDTO instanceData = await SaveInstance(algoData, instanceType);
                postInstanceData = instanceData;
                await DeployInstance(instanceData);

                BuilInitialDataObjectDTO tempDataDTO = new BuilInitialDataObjectDTO()
                {
                    AlgoId = algoData.Id,
                    InstanceId = instanceData.InstanceId,
                    Name = algoData.Name,
                    Description = algoData.Description
                };

                instancesList.Add(tempDataDTO);
            }

            return instancesList;
        }

        public async Task<List<Response>> ClearAllCascadeDelete(List<BuilInitialDataObjectDTO> listDtoToBeDeleted)
        {
            List<Response> responces = new List<Response>();
            string url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
            int retryCounter = 0;

            foreach (var deleteMetadata in listDtoToBeDeleted)
            {
                CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
                {
                    AlgoId = deleteMetadata.AlgoId,
                    InstanceId = deleteMetadata.InstanceId
                };
                var responceCascadeDelete = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);

                // Currently we can not send cascade delete to kubernatees if he has not build the algo before that thorus not found and we leave data
                bool isPodMissing = !responceCascadeDelete.ResponseJson.Contains($"Code:504-PodNotFound Message:Pod is not found for {deleteMetadata.InstanceId}");
                while (responceCascadeDelete.Status.Equals(HttpStatusCode.NotFound) && isPodMissing && retryCounter <= 30)
                {
                    System.Threading.Thread.Sleep(10000);
                    responceCascadeDelete = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
                    isPodMissing = !responceCascadeDelete.ResponseJson.Contains($"Code:504-PodNotFound Message:Pod is not found for {deleteMetadata.InstanceId}");
                    retryCounter++;
                }

                if (isPodMissing)
                {
                    Assert.That(responceCascadeDelete.Status, Is.EqualTo(HttpStatusCode.NoContent));
                }
                else
                {
                    Assert.That(responceCascadeDelete.Status, Is.EqualTo(HttpStatusCode.NotFound));
                }

                responces.Add(responceCascadeDelete);
            }

            return responces;
        }

        public async Task<Response> DeleteAlgo(InstanceDataDTO instanceData, bool forceDelete = false)
        {
            DeleteAlgoDTO deleteAlgoDTO = new DeleteAlgoDTO()
            {
                AlgoId = instanceData.AlgoId,
                AlgoClientId = instanceData.AlgoClientId,
                ForceDelete = forceDelete
            };

            // Delete the algo
            return await Consumer.ExecuteRequest(ApiPaths.ALGO_STORE_DELETE_ALGO, Helpers.EmptyDictionary, JsonUtils.SerializeObject(deleteAlgoDTO), Method.DELETE);
        }

        public async Task AssertAlgoDeleted(Response deleteAlgoRequest, InstanceDataDTO instanceData)
        {
            AlgoErrorDTO deleteAlgoResponse = JsonUtils.DeserializeJson<AlgoErrorDTO>(deleteAlgoRequest.ResponseJson);

            Assert.That(deleteAlgoRequest.Status, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(deleteAlgoResponse, Is.Null);
            // Assert algo is deleted from DB
            Assert.That(await AlgoExists(instanceData), Is.False);
        }

        public async Task AssertAlgoNotDeleted(Response deleteAlgoRequest, InstanceDataDTO instanceData, string errorMessage)
        {
            AlgoErrorDTO deleteAlgoResponse = JsonUtils.DeserializeJson<AlgoErrorDTO>(deleteAlgoRequest.ResponseJson);

            Assert.That(deleteAlgoRequest.Status, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(deleteAlgoResponse.DisplayMessage, Is.EqualTo(errorMessage));
            // Assert algo is not deleted from DB
            Assert.That(await AlgoExists(instanceData), Is.True);
        }

        public async Task<bool> AlgoExists(InstanceDataDTO instanceDataDTO)
        {
            List<AlgoEntity> allUserAlgos = await AlgoRepository.GetAllAsync(t => t.ClientId == instanceDataDTO.AlgoClientId) as List<AlgoEntity>;
            return allUserAlgos.Exists(x => x.AlgoId == instanceDataDTO.AlgoId);
        }
    }
}
