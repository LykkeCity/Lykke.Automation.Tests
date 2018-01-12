using AlgoStoreData.DTOs;
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
        public async Task<List<MetaDataResponseDTO>> UploadSomeBaseMetaData(int nuberDto)
        {
            string url = ApiPaths.ALGO_STORE_METADATA;
            List<MetaDataDTO> metadataList = new List<MetaDataDTO>();
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

            return responceMetadataList;
        }

        public async Task<List<Response>> ClearAllCascadeDelete(List<MetaDataResponseDTO> listDtoToBeDeleted)
        {
            List<Response> responces = new List<Response>();
            string url = ApiPaths.ALGO_STORE_CASCADE_DELETE;

            foreach (var deleteMetadata in listDtoToBeDeleted)
            {
                CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
                {
                    Id = deleteMetadata.Id,
                    Name = deleteMetadata.Name
                };
                var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);

                responces.Add(responceCascadeDelete);               
            }

            return responces;
        }

    }
}
