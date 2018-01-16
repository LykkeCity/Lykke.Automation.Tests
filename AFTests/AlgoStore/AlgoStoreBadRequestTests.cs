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

namespace AFTests.AlgoStore
{
    [Category("FullRegression")]
    [Category("AlgoStore")]
    public partial class AlgoStoreTests : AlgoStoreTestDataFixture
    { 
        [Test]
        [Category("BadRequest")]
        [Category("UploadMetadataBadRequest")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task UploadMetadataBadRequest(string badName)
        {

            string url = ApiPaths.ALGO_STORE_METADATA;

            MetaDataDTO metadata = new MetaDataDTO()
            {
                Name = badName,
                Description = badName
            };

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            Assert.True(response.Status == HttpStatusCode.InternalServerError);
        }
        [Test]
        [Category("BadRequest")]
        [Category("EditMetadataBadRequest")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task EditMetadataBadRequest(string badName)
        {
            string url = ApiPaths.ALGO_STORE_METADATA;

            MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForEdit();
            MetaDataEditDTO editMetaData = new MetaDataEditDTO()
            {
                Id = temporaryResponseDTO.Id,
                Name = badName,
                Description = badName
            };

            var responseMetaDataAfterEdit = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
            Assert.True(responseMetaDataAfterEdit.Status == HttpStatusCode.InternalServerError);
        }
        [Test]
        [Category("BadRequest")]
        [Category("DeleteMethadataBadRequest")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task DeleteMetadataBadRequest(string badID)
        {
            MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForEdit();
            CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
            {
                Id = badID,
                Name = temporaryResponseDTO.Name
            };

            string url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
            var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
            Assert.True(responceCascadeDelete.Status == HttpStatusCode.InternalServerError);
        }

        //[Test]
        //[Category("BadRequest")]
        //[Category("UploadBinaryAlgoBadRequest")]
        //public async Task UploadBinaryAlgoBadRequest()
        //{
        //    string url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

        //    string AlgoId = DataManager.getMetaDataForBinaryUpload().Id;

        //    Dictionary<string, string> quaryParam = new Dictionary<string, string>()
        //    {
        //        {"AlgoId", AlgoId }
        //    };

        //    string pathFile = Path.Combine(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar, "AlgoStore" + Path.DirectorySeparatorChar, "TestData" + Path.DirectorySeparatorChar, "myalgo-1.0-SNAPSHOT-jar-with-dependencies-fil-01.jar");

        //    var responceAllClientMetadata = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
        //    Assert.True(responceAllClientMetadata.Status == HttpStatusCode.NoContent);
        //    bool blobExists = await this.BlobRepository.CheckIfBlobExists(AlgoId);
        //    Assert.True(blobExists);
        //}

        //[Test]
        //[Category("BadRequest")]
        //[Category("DeployBianryAlgoBadRequest")]
        //public async Task DeployBinaryAlgoBadRequest()
        //{
        //    MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

        //    string AlgoID = metadataForUploadedBinary.Id;

        //    DeployBinaryDTO algo = new DeployBinaryDTO()
        //    {
        //        AlgoId = AlgoID
        //    };

        //    string url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

        //    var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
        //    Assert.True(uploadBinaryresponce.Status == HttpStatusCode.OK);

        //    RuntimeDataEntity runtimeDataEntity = await RuntimeDataRepository.TryGetAsync(t => t.Id == metadataForUploadedBinary.Id) as RuntimeDataEntity;
        //    Assert.NotNull(runtimeDataEntity);
        //}

        //[Test]
        //[Category("BadRequest")]
        //[Category("StartBinaryBadRequest")]
        //public async Task StartBinaryBadRequest()
        //{
        //    MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

        //    string AlgoID = metadataForUploadedBinary.Id;

        //    DeployBinaryDTO algo = new DeployBinaryDTO()
        //    {
        //        AlgoId = AlgoID
        //    };

        //    string url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

        //    var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
        //    Assert.True(uploadBinaryresponce.Status == HttpStatusCode.OK);

        //    StartBinaryDTO startAlgo = new StartBinaryDTO
        //    {
        //        AlgoId = algo.AlgoId
        //    };

        //    url = ApiPaths.ALGO_STORE_ALGO_START;

        //    var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
        //    Assert.True(startBinaryresponce.Status == HttpStatusCode.OK);

        //    StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
        //    Assert.True(startResponse.Status.Equals("STARTED"));
        //}

        //[Test]
        //[Category("BadRequest")]
        //[Category("StopBinaryBadRequest")]
        //public async Task StopBinaryBadRequest()
        //{
        //    MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

        //    string AlgoID = metadataForUploadedBinary.Id;

        //    DeployBinaryDTO algo = new DeployBinaryDTO()
        //    {
        //        AlgoId = AlgoID
        //    };

        //    string url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

        //    var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
        //    Assert.True(uploadBinaryresponce.Status == HttpStatusCode.OK);

        //    StartBinaryDTO startAlgo = new StartBinaryDTO
        //    {
        //        AlgoId = algo.AlgoId
        //    };

        //    url = ApiPaths.ALGO_STORE_ALGO_START;

        //    var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
        //    Assert.True(startBinaryresponce.Status == HttpStatusCode.OK);

        //    StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
        //    Assert.True(startResponse.Status.Equals("STARTED"));

        //    StopBinaryDTO stopAlgo = new StopBinaryDTO
        //    {
        //        AlgoId = AlgoID
        //    };

        //    url = ApiPaths.ALGO_STORE_ALGO_STOP;

        //    var stopBinaryResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);
        //    Assert.True(stopBinaryResponse.Status == HttpStatusCode.OK);

        //    StartBinaryResponseDTO stopResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(stopBinaryResponse.ResponseJson);
        //    Assert.True(stopResponse.Status.Equals("STOPPED"));
        //}

        //[Test]
        //[Category("BadRequest")]
        //[Category("GetLogBadRequest")]
        //public async Task GetLogBadRequest(string AlgoID)
        //{
        //    var url = ApiPaths.ALGO_STORE_ALGO_LOG;

        //    Dictionary<string, string> algoIDLog = new Dictionary<string, string>()
        //    {
        //        { "AlgoId", AlgoID }
        //    };

        //    var algoIDLogResponse = await this.Consumer.ExecuteRequest(url, algoIDLog, null, Method.GET);
        //    Assert.True(algoIDLogResponse.Status == HttpStatusCode.OK);

        //    LogResponseDTO LogObject = JsonUtils.DeserializeJson<LogResponseDTO>(algoIDLogResponse.ResponseJson);

        //    Assert.NotNull(LogObject);
        //}

        //[Test]
        //[Category("BadRequest")]
        //[Category("GetTailLogBadRequest")]
        //public async Task GetTailLogBadRequest(string AlgoID)
        //{
        //    var url = ApiPaths.ALGO_STORE_ALGO_TAIL_LOG;

        //    Dictionary<string, string> algoIDTailLog = new Dictionary<string, string>()
        //    {
        //        { "AlgoId", AlgoID },
        //        {"Tail" , "60" }
        //    };

        //    var algoIDTailLogResponse = await this.Consumer.ExecuteRequest(url, algoIDTailLog, null, Method.GET);
        //    Assert.True(algoIDTailLogResponse.Status == HttpStatusCode.OK);

        //}

        //[Test]
        //[Category("BadRequest")]
        //[Category("UploadStringBadRequest")]
        //public async Task UploadStringBadRequest()
        //{
        //    string url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

        //    MetaDataResponseDTO metadataWithUploadedString = DataManager.getMetaDataForBinaryUpload();

        //    string AlgoId = metadataWithUploadedString.Id;

        //    Dictionary<string, string> quaryParam = new Dictionary<string, string>()
        //    {
        //        {"AlgoId", AlgoId },
        //        {"Data" , Helpers.RandomString(300) }
        //    };

        //    var responceUploadString = await this.Consumer.ExecuteRequest(url, quaryParam, null, Method.POST);
        //    Assert.True(responceUploadString.Status == HttpStatusCode.NoContent);

        //    bool blobExists = await this.BlobRepository.CheckIfBlobExists(AlgoId);
        //    Assert.True(blobExists);
        //}

        //[Test]
        //[Category("BadRequest")]
        //[Category("GetStringBadRequest")]
        //public async Task GetUploadedStringBadRequest(string AlgoId)
        //{
        //    string url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

        //    Dictionary<string, string> quaryParamGetString = new Dictionary<string, string>()
        //    {
        //        {"AlgoId", AlgoId }
        //    };

        //    var responceGetUploadString = await this.Consumer.ExecuteRequest(url, quaryParamGetString, null, Method.GET);
        //    Assert.True(responceGetUploadString.Status == HttpStatusCode.OK);
        //}
    }
}
