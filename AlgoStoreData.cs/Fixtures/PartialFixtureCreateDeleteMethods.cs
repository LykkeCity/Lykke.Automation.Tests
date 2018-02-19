using AlgoStoreData.DTOs;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Tests;
using XUnitTestCommon.Utils;


namespace AlgoStoreData.Fixtures
{
    public partial class AlgoStoreTestDataFixture : BaseTest
    {
        public async Task<List<BuilInitialDataObjectDTO>> UploadSomeBaseMetaData(int nuberDto)
        {
            string url = ApiPaths.ALGO_STORE_METADATA;
            List<BuilInitialDataObjectDTO> initialDTOObjectsList = new List<BuilInitialDataObjectDTO>();
            List <MetaDataDTO> metadataList = new List<MetaDataDTO>();
            List<MetaDataResponseDTO> responceMetadataList = new List<MetaDataResponseDTO>();

            for (int i = 0; i < nuberDto; i++)
            {
                MetaDataDTO metadata = new MetaDataDTO()
                {
                    Name = Helpers.RandomString(13),
                    Description = Helpers.RandomString(13)
                };
                metadataList.Add(metadata);
            }

            foreach (var metadata in metadataList)
            {
                var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
                MetaDataResponseDTO responceMetaData = JsonUtils.DeserializeJson<MetaDataResponseDTO>(response.ResponseJson);
                responceMetadataList.Add(responceMetaData);
            }

            for (int i = 0; i < nuberDto; i++)
            {
                url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

                UploadStringDTO stringDTO = new UploadStringDTO()
                {
                    AlgoId = responceMetadataList[i].Id,
                    Data = "TEST FOR NOW NOT WORKING ALGO"                    
                };

                var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stringDTO), Method.POST);
                Assert.True(response.Status == System.Net.HttpStatusCode.NoContent);

                InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
                {
                    AlgoId = stringDTO.AlgoId,
                    HftApiKey = "key",
                    AssetPair = "BTCUSD",
                    TradedAsset = "USD",
                    Margin = "1",
                    Volume = "1"
                };

                url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

                var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
                Assert.That(postInstanceDataResponse.Status == System.Net.HttpStatusCode.OK);

                InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

                url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

                DeployBinaryDTO deploy = new DeployBinaryDTO()
                {
                    AlgoId = stringDTO.AlgoId,
                    InstanceId = postInstanceData.InstanceId,
                };

                var deployBynaryResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(deploy), Method.POST);
                Assert.That(postInstanceDataResponse.Status == System.Net.HttpStatusCode.NoContent);

                BuilInitialDataObjectDTO tempDataDTO = new BuilInitialDataObjectDTO()
                {
                    AlgoId = stringDTO.AlgoId,
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

            foreach (var deleteMetadata in listDtoToBeDeleted)
            {
                CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
                {
                    AlgoId = deleteMetadata.AlgoId,
                    InstanceId = deleteMetadata.InstanceId
                };
                var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);

                responces.Add(responceCascadeDelete);               
            }

            return responces;
        }

    }
}
