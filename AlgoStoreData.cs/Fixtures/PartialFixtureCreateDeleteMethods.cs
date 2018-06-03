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


namespace AlgoStoreData.Fixtures
{
    public partial class AlgoStoreTestDataFixture : BaseTest
    {
        private static List<BuilInitialDataObjectDTO> initialDTOObjectsList = new List<BuilInitialDataObjectDTO>();

        public async Task<List<BuilInitialDataObjectDTO>> UploadSomeBaseMetaData(int nuberDto)
        {
            string url = ApiPaths.ALGO_STORE_CREATE_ALGO;
            string message;
            List <MetaDataDTO> metadataList = new List<MetaDataDTO>();
            List<MetaDataResponseDTO> responceMetadataList = new List<MetaDataResponseDTO>();

            for (int i = 0; i < nuberDto; i++)
            {
                MetaDataDTO metadata = new MetaDataDTO()
                {
                    Name = $"{ GlobalConstants.AutoTest }_AlgoMetaDataName_{Helpers.GetFullUtcTimestamp()}",
                    Description = $"{ GlobalConstants.AutoTest }_AlgoMetaDataName_{Helpers.GetFullUtcTimestamp()} - Description",
                    Content = Base64Helpers.EncodeToBase64(CSharpAlgoString)
                };
                metadataList.Add(metadata);
            }

            foreach (var metadata in metadataList)
            {
                var response = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
                message = $"POST {url} returned status: {response.Status} and response: {response.ResponseJson}. Expected: {HttpStatusCode.OK}";
                Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK), message);

                MetaDataResponseDTO responceMetaData = JsonUtils.DeserializeJson<MetaDataResponseDTO>(response.ResponseJson);
                responceMetadataList.Add(responceMetaData);
            }

            for (int i = 0; i < nuberDto; i++)
            {
                WalletDTO walletDTO = await GetExistingWallet();
                instanceForAlgo = GetPopulatedInstanceDataDTO.returnInstanceDataDTO(responceMetadataList[i].Id, walletDTO, "Demo");
                url = ApiPaths.ALGO_STORE_SAVE_ALGO_INSTANCE;

                string requestBody = JsonUtils.SerializeObject(instanceForAlgo);
                var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, requestBody, Method.POST);
                message = $"POST {url} returned status: {postInstanceDataResponse.Status} and response: {postInstanceDataResponse.ResponseJson}. Expected: {HttpStatusCode.OK}";
                Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.OK), message);

                postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

                url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

                DeployBinaryDTO deploy = new DeployBinaryDTO()
                {
                    AlgoId = responceMetadataList[i].Id,
                    InstanceId = postInstanceData.InstanceId,
                };

                var deployBynaryResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(deploy), Method.POST);
                message = $"POST {url} returned status: {deployBynaryResponse.Status} and response: {deployBynaryResponse.ResponseJson}. Expected: {HttpStatusCode.OK}";
                Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.OK));

                BuilInitialDataObjectDTO tempDataDTO = new BuilInitialDataObjectDTO()
                {
                    AlgoId = responceMetadataList[i].Id,
                    InstanceId = postInstanceData.InstanceId,
                    Name = responceMetadataList[i].Name,
                    Description = responceMetadataList[i].Description
                };

                initialDTOObjectsList.Add(tempDataDTO);
            }

            return initialDTOObjectsList;
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
                } else
                {
                    Assert.That(responceCascadeDelete.Status, Is.EqualTo(HttpStatusCode.NotFound));
                }

                responces.Add(responceCascadeDelete);               
            }

            return responces;
        }

        public async Task<WalletDTO> GetWallet()
        {
            return await GetExistingWallet();
        }
    }
}
