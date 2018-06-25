using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AlgoStoreData.Fixtures;
using NUnit.Framework;
using RestSharp;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using AlgoStoreData.DTOs;
using XUnitTestData.Entities.AlgoStore;
using System.IO;
using AlgoStoreData.HelpersAlgoStore;

namespace AFTests.AlgoStore
{
    [Category("FullRegression")]
    [Category("AlgoStore")]
    public partial class AlgoStoreTests : AlgoStoreTestDataFixture
    {
        [Test]
        [Category("AlgoStore")]
        public async Task UploadMetadataWithEmptyDescription()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;

            CreateAlgoDTO metadata = new CreateAlgoDTO()
            {
                Name = Helpers.RandomString(8),
                Description = ""
            };

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            Assert.That(response.Status , Is.EqualTo(HttpStatusCode.OK));
            AlgoDataDTO responseMetaData = JsonUtils.DeserializeJson<AlgoDataDTO>(response.ResponseJson);

            Assert.AreEqual(metadata.Name, responseMetaData.Name);
            Assert.AreEqual(metadata.Description, responseMetaData.Description);
            Assert.NotNull(responseMetaData.DateCreated);
            Assert.NotNull(responseMetaData.DateModified);
            Assert.NotNull(responseMetaData.Id);
            Assert.Null(responseMetaData.AlgoVisibility);

            MetaDataEntity metaDataEntity = await MetaDataRepository.TryGetAsync(t => t.Id == responseMetaData.Id) as MetaDataEntity;

            Assert.NotNull(metaDataEntity);
            Assert.AreEqual(metaDataEntity.Id, responseMetaData.Id);
            Assert.AreEqual(metaDataEntity.Name, responseMetaData.Name);
            Assert.AreEqual(metaDataEntity.Description, responseMetaData.Description);
        }

        [Test]
        [Category("AlgoStore")]
        public async Task GetStringWrongId()
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            Dictionary<string, string> quaryParamGetString = new Dictionary<string, string>()
            {
                {"AlgoId", "non-existing-id-234-555-666" }
            };

            var responceGetUploadString = await this.Consumer.ExecuteRequest(url, quaryParamGetString, null, Method.GET);
            Assert.That(responceGetUploadString.Status , Is.EqualTo(HttpStatusCode.NotFound));
        }
        [Test]
        [Category("AlgoStore")]
        public async Task UpdateUploadedStringAlgo()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;

            CreateAlgoDTO metadata = new CreateAlgoDTO()
            {
                Name = Helpers.RandomString(13),
                Description = Helpers.RandomString(13)
            };

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            AlgoDataDTO responseMetaData = JsonUtils.DeserializeJson<AlgoDataDTO>(response.ResponseJson);


            url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            UploadStringDTO stringDTO = new UploadStringDTO()
            {
                AlgoId = responseMetaData.Id,
                Data = this.CSharpAlgoString
            };

            var responsetemp = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stringDTO), Method.POST);
            Assert.True(responsetemp.Status == System.Net.HttpStatusCode.NoContent);

            Dictionary<string, string> quaryParamGetString = new Dictionary<string, string>()
            {
                {"AlgoId", responseMetaData.Id }
            };

            var responceGetUploadString = await this.Consumer.ExecuteRequest(url, quaryParamGetString, null, Method.GET);
            Assert.That(responceGetUploadString.Status, Is.EqualTo(HttpStatusCode.OK));

            UploadStringDTO uploadedStringContent = JsonUtils.DeserializeJson<UploadStringDTO>(responceGetUploadString.ResponseJson);

            Assert.That(stringDTO.Data, Is.EqualTo(uploadedStringContent.Data));

            UploadStringDTO UpdatedstringDTO = new UploadStringDTO()
            {
                AlgoId = responseMetaData.Id,
                Data = "Updated Text"
            };

            var responsetempUpdated = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(UpdatedstringDTO), Method.POST);
            Assert.True(responsetempUpdated.Status == System.Net.HttpStatusCode.NoContent);

            var responceGetUploadStringUpdated = await this.Consumer.ExecuteRequest(url, quaryParamGetString, null, Method.GET);
            Assert.That(responceGetUploadStringUpdated.Status, Is.EqualTo(HttpStatusCode.OK));

            UploadStringDTO uploadedStringContentUpdated = JsonUtils.DeserializeJson<UploadStringDTO>(responceGetUploadStringUpdated.ResponseJson);

            Assert.That(UpdatedstringDTO.Data, Is.EqualTo(uploadedStringContentUpdated.Data));
        }
        [Test]
        [Category("AlgoStore")]
        public async Task UploadStringWrongAlgoId()
        {
           string  url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            UploadStringDTO stringDTO = new UploadStringDTO()
            {
                AlgoId = "Invalid Id",
                Data = "TEST FOR NOW NOT WORKING ALGO"
            };

            var responsetemp = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stringDTO), Method.POST);
            Assert.True(responsetemp.Status == System.Net.HttpStatusCode.NotFound);
        }

        //TODO
        //[Test]
        //[Category("AlgoStore")]
        //public async Task EditInstanceDataWhileRunningAlgo()
        //{
        //    List<BuilInitialDataObjectDTO> metadataForUploadedBinaryList = await UploadSomeBaseMetaData(1);

        //    BuilInitialDataObjectDTO metadataForUploadedBinary = metadataForUploadedBinaryList[metadataForUploadedBinaryList.Count - 1];
        //}

        //TODO
        //[Test]
        //[Category("AlgoStore")]
        //public async Task EditMethadataWhileRunningAlgo()
        //{
        //    List<BuilInitialDataObjectDTO> metadataForUploadedBinaryList = await UploadSomeBaseMetaData(1);

        //    BuilInitialDataObjectDTO metadataForUploadedBinary = metadataForUploadedBinaryList[metadataForUploadedBinaryList.Count - 1];
        //}

        //TODO
        //[Test]
        //[Category("AlgoStore")]
        //public async Task UpdateAlgoWhileDeployed()
        //{
        //    List<BuilInitialDataObjectDTO> metadataForUploadedBinaryList = await UploadSomeBaseMetaData(1);

        //    BuilInitialDataObjectDTO metadataForUploadedBinary = metadataForUploadedBinaryList[metadataForUploadedBinaryList.Count - 1];
        //}
    }
}
