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
            Assert.True(response.Status == HttpStatusCode.BadRequest);
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
            Assert.True(responseMetaDataAfterEdit.Status == HttpStatusCode.BadRequest);
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
            Assert.True(responceCascadeDelete.Status == HttpStatusCode.BadRequest);
        }
        [Test]
        [Category("BadRequest")]
        [Category("UploadBinaryAlgoBadRequest")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task UploadBinaryAlgoBadRequest(string badID)
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", badID }
            };

            string pathFile = Path.Combine(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar, "AlgoStore" + Path.DirectorySeparatorChar, "TestData" + Path.DirectorySeparatorChar, "myalgo-1.0-SNAPSHOT-jar-with-dependencies-fil-01.jar");

            var responceAllClientMetadata = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.True(responceAllClientMetadata.Status == HttpStatusCode.BadRequest);
        }
        [Test]
        [Category("BadRequest")]
        [Category("DeployBianryAlgoBadRequest")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task DeployBinaryAlgoBadRequest(string badID)
        {
            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = badID
            };

            string url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.True(uploadBinaryresponce.Status == HttpStatusCode.BadRequest);
        }
        [Test]
        [Category("BadRequest")]
        [Category("StartBinaryBadRequest")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task StartBinaryBadRequest(string badID)
        {
            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = badID
            };

            string url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.True(startBinaryresponce.Status == HttpStatusCode.BadRequest);
        }
        [Test]
        [Category("BadRequest")]
        [Category("StopBinaryBadRequest")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task StopBinaryBadRequest(string badId)
        {
            string url = ApiPaths.ALGO_STORE_ALGO_STOP;

            StopBinaryDTO stopAlgo = new StopBinaryDTO
            {
                AlgoId = badId
            };

            var stopBinaryResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);
            Assert.True(stopBinaryResponse.Status == HttpStatusCode.BadRequest);
        }
        [Test]
        [Category("BadRequest")]
        [Category("GetLogBadRequest")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task GetLogBadRequest(string badId)
        {
            var url = ApiPaths.ALGO_STORE_ALGO_LOG;

            Dictionary<string, string> algoIDLog = new Dictionary<string, string>()
            {
                { "AlgoId", badId }
            };

            var algoIDLogResponse = await this.Consumer.ExecuteRequest(url, algoIDLog, null, Method.GET);
            Assert.True(algoIDLogResponse.Status == HttpStatusCode.BadRequest);
        }
        [Test]
        [Category("BadRequest")]
        [Category("GetTailLogBadRequest")]
        [TestCase("", "")]
        [TestCase("     ", "   ")]
        [TestCase(null, null)]
        public async Task GetTailLogBadRequest(string AlgoID, string tail)
        {
            var url = ApiPaths.ALGO_STORE_ALGO_TAIL_LOG;

            Dictionary<string, string> algoIDTailLog = new Dictionary<string, string>()
            {
                { "AlgoId", AlgoID },
                {"Tail" , tail }
            };

            var algoIDTailLogResponse = await this.Consumer.ExecuteRequest(url, algoIDTailLog, null, Method.GET);
            Assert.True(algoIDTailLogResponse.Status == HttpStatusCode.BadRequest);
        }
        [Test]
        [Category("BadRequest")]
        [Category("UploadStringBadRequest")]
        [TestCase("", "")]
        [TestCase("     ", "   ")]
        [TestCase(null, null)]
        public async Task UploadStringBadRequest(string badID, string AlgoString)
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", badID },
                {"Data" , AlgoString }
            };
            var responceUploadString = await this.Consumer.ExecuteRequest(url, quaryParam, null, Method.POST);
            Assert.True(responceUploadString.Status == HttpStatusCode.BadRequest);
        }
        [Test]
        [Category("BadRequest")]
        [Category("GetStringBadRequest")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task GetUploadedStringBadRequest(string badId)
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            Dictionary<string, string> quaryParamGetString = new Dictionary<string, string>()
            {
                {"AlgoId", badId }
            };

            var responceGetUploadString = await this.Consumer.ExecuteRequest(url, quaryParamGetString, null, Method.GET);
            Assert.True(responceGetUploadString.Status == HttpStatusCode.BadRequest);
        }
    }
}
