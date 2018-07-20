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
    public partial class AlgoStoreTestsInstanceRequired : CreateAlgoWithInstanceFixture
    {

        [Category("AlgoStore")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task UploadMetadataBadRequest(string badName)
        {

            string url = ApiPaths.ALGO_STORE_METADATA;

            CreateAlgoDTO metadata = new CreateAlgoDTO()
            {
                Name = badName,
                Description = badName
            };

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            Assert.That(response.Status , Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Category("AlgoStore")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task EditMetadataBadRequest(string badName)
        {

            string url = ApiPaths.ALGO_STORE_METADATA;

            CreateAlgoDTO metadata = new CreateAlgoDTO()
            {
                Name = Helpers.RandomString(8),
                Description = Helpers.RandomString(8)
            };

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
            AlgoDataDTO responseMetaData = JsonUtils.DeserializeJson<AlgoDataDTO>(response.ResponseJson);

            url = ApiPaths.ALGO_STORE_METADATA;

            EditAlgoDTO editMetaData = new EditAlgoDTO()
            {
                Id = responseMetaData.Id,
                Name = badName,
                Description = badName
            };

            var responseMetaDataAfterEdit = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
            Assert.That(responseMetaDataAfterEdit.Status , Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Category("AlgoStore")]
        [TestCase("")]
        [TestCase("     ")]
        [TestCase(null)]
        public async Task DeleteMetadataBadRequest(string badID)
        {
            string url = ApiPaths.ALGO_STORE_METADATA;

            CreateAlgoDTO metadata = new CreateAlgoDTO()
            {
                Name = Helpers.RandomString(8),
                Description = Helpers.RandomString(8)
            };

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
            AlgoDataDTO responseMetaData = JsonUtils.DeserializeJson<AlgoDataDTO>(response.ResponseJson);

            CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
            {
                AlgoId = badID,
                InstanceId = responseMetaData.Name
            };

            url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
            var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
            Assert.That(responceCascadeDelete.Status , Is.EqualTo(HttpStatusCode.BadRequest));
        }   

        [Category("AlgoStore")]
        [TestCase("", "")]
        [TestCase("     ", "   ")]
        [TestCase(null, null)]
        public async Task GetTailLogBadRequest(string AlgoID, string tail)
        {
            var url = ApiPaths.ALGO_STORE_LOGGING_API_TAIL_LOG;

            Dictionary<string, string> algoIDTailLog = new Dictionary<string, string>()
            {
                { "AlgoId", AlgoID },
                {"Tail" , tail }
            };

            var algoIDTailLogResponse = await this.Consumer.ExecuteRequest(url, algoIDTailLog, null, Method.GET);
            Assert.That(algoIDTailLogResponse.Status , Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Category("AlgoStore")]
        [TestCase("", "")]
        [TestCase("     ", "   ")]
        [TestCase(null, null)]
        public async Task UploadStringBadRequest(string badID, string AlgoString)
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            string Algoid = badID;

            PostUploadStringAlgoDTO uploadedStringDTO = new PostUploadStringAlgoDTO()
            {
                AlgoId = Algoid,
                Data = AlgoString
            };

            var responceUploadString = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(uploadedStringDTO), Method.POST);
            Assert.That(responceUploadString.Status , Is.EqualTo(HttpStatusCode.BadRequest));
        }

        [Category("AlgoStore")]
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
            Assert.That(responceGetUploadString.Status , Is.EqualTo(HttpStatusCode.BadRequest));
        }
    }
}
