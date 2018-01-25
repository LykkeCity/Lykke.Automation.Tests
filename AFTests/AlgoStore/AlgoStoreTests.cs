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
        [Category("Smoke")]
        [Category("IsAlive")]
        public async Task CheckIfServiceIsAlive()
        {

            string url = ApiPaths.ALGO_STORE_IS_ALIVE;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            var baseDate = JsonUtils.DeserializeJson<IsAliveDTO>(response.ResponseJson).Name;
            Assert.True(baseDate.Equals("Lykke.AlgoStore.Api"));
        }

        [Test]
        [Category("Smoke")]
        [Category("UploadMetadata")]
        public async Task UploadMetadata()
        {

            string url = ApiPaths.ALGO_STORE_METADATA;

            MetaDataDTO metadata = new MetaDataDTO()
            {
                Name = Helpers.RandomString(8),
                Description = Helpers.RandomString(8)
            };
            
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            Assert.True(response.Status == HttpStatusCode.OK);
            MetaDataResponseDTO responceMetaData = JsonUtils.DeserializeJson<MetaDataResponseDTO>(response.ResponseJson);

            DataManager.addSingleMetadata(responceMetaData);

            Assert.AreEqual(metadata.Name, responceMetaData.Name);
            Assert.AreEqual(metadata.Description, responceMetaData.Description);
            Assert.NotNull(responceMetaData.Date);
            Assert.NotNull(responceMetaData.Id);
            Assert.Null(responceMetaData.Status);

            MetaDataEntity metaDataEntity = await MetaDataRepository.TryGetAsync(t => t.Id == responceMetaData.Id) as MetaDataEntity;

            Assert.NotNull(metaDataEntity);
            Assert.AreEqual(metaDataEntity.Id, responceMetaData.Id);
            Assert.AreEqual(metaDataEntity.Name, responceMetaData.Name);
            Assert.AreEqual(metaDataEntity.Description, responceMetaData.Description);
        }

        [Test]
        [Category("Smoke")]
        [Category("EditMetadata")]
        public async Task EditMetadata()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;

            MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForEdit();
            MetaDataEditDTO editMetaData = new MetaDataEditDTO()
            {
                Id = temporaryResponseDTO.Id,
                Name = Helpers.RandomString(9),
                Description = Helpers.RandomString(9)
            };

            temporaryResponseDTO.Name = editMetaData.Name;
            temporaryResponseDTO.Description = editMetaData.Description;

            var responseMetaDataAfterEdit = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
            Assert.True(responseMetaDataAfterEdit.Status == HttpStatusCode.OK);
            MetaDataResponseDTO responceMetaDataAfterEdit = JsonUtils.DeserializeJson<MetaDataResponseDTO>(responseMetaDataAfterEdit.ResponseJson);

            Assert.AreEqual(responceMetaDataAfterEdit.Name, editMetaData.Name);
            Assert.AreEqual(responceMetaDataAfterEdit.Description, editMetaData.Description);
            Assert.NotNull(responceMetaDataAfterEdit.Date);
            Assert.NotNull(responceMetaDataAfterEdit.Id);
            Assert.Null(responceMetaDataAfterEdit.Status);


            MetaDataEntity metaDataEntity = await MetaDataRepository.TryGetAsync(t => t.Id == responceMetaDataAfterEdit.Id) as MetaDataEntity;

            Assert.NotNull(metaDataEntity);
            Assert.AreEqual(metaDataEntity.Id, responceMetaDataAfterEdit.Id);
            Assert.AreEqual(metaDataEntity.Name, responceMetaDataAfterEdit.Name);
            Assert.AreEqual(metaDataEntity.Description, responceMetaDataAfterEdit.Description);
        }


        [Test]
        [Category("Smoke")]
        [Category("DeleteMethadata")]
        public async Task DeleteMetadata()
        {
            MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForDelete();
            CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
            {
                Id = temporaryResponseDTO.Id,
                Name = temporaryResponseDTO.Name
            };

            string url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
            var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
            Assert.True(responceCascadeDelete.Status == HttpStatusCode.NoContent);
            MetaDataEntity metaDataEntityDeleted = await MetaDataRepository.TryGetAsync(t => t.Id == editMetaData.Id) as MetaDataEntity;
            Assert.Null(metaDataEntityDeleted);
        }

        [Test]
        [Category("Smoke")]
        [Category("GetAllMetadataForClient")]
        public async Task GetAllMetadataForClient()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;
            var responceAllClientMetadata = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(responceAllClientMetadata.Status == HttpStatusCode.OK);
            Object responceClientGetAll = JsonUtils.DeserializeJson(responceAllClientMetadata.ResponseJson);
            List<MetaDataResponseDTO> listAllClinetObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MetaDataResponseDTO>>(responceClientGetAll.ToString());
            List<string> keysPreBuildData = new List<string>();
            DataManager.getAllMetaData().ForEach(e => keysPreBuildData.Add(e.Id));
            int mathcedKeysCounter = 0;
            foreach (MetaDataResponseDTO currentData in listAllClinetObjects)
            {
                bool Exists = keysPreBuildData.Contains(currentData.Id);
                if (Exists)
                {
                    mathcedKeysCounter++;
                }
            }
            Assert.True(keysPreBuildData.Count == mathcedKeysCounter);
        }

        [Test]
        [Category("Smoke")]
        [Category("UploadBinaryAlgo")]
        public async Task UploadBinaryAlgo()
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

            string AlgoId = DataManager.getMetaDataForBinaryUpload().Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId }
            };

            string pathFile = Path.Combine(Directory.GetCurrentDirectory()+ Path.DirectorySeparatorChar,"AlgoStore"+ Path.DirectorySeparatorChar, "TestData"+Path.DirectorySeparatorChar, "myalgo-1.0-SNAPSHOT-jar-with-dependencies-fil-01.jar");

            var responceAllClientMetadata = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.True(responceAllClientMetadata.Status == HttpStatusCode.NoContent);
            bool blobExists = await this.BlobRepository.CheckIfBlobExists(AlgoId, BinaryAlgoFileType.JAR);
            Assert.True(blobExists);
        }

        [Test]
        [Category("Smoke")]
        [Category("DeployBianryAlgo")]
        public async Task DeployBinaryAlgo()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string AlgoID = metadataForUploadedBinary.Id;

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoID
            };

            string url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.True(uploadBinaryresponce.Status == HttpStatusCode.OK);

            RuntimeDataEntity runtimeDataEntity = await RuntimeDataRepository.TryGetAsync(t => t.Id == metadataForUploadedBinary.Id) as RuntimeDataEntity;
            Assert.NotNull(runtimeDataEntity);
        }

        [Test]
        [Category("Smoke")]
        [Category("StartBinary")]
        public async Task StartBinary()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string AlgoID = metadataForUploadedBinary.Id;

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoID
            };

            string url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.True(uploadBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.True(startBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.True(startResponse.Status.Equals("STARTED"));
        }

        [Test]
        [Category("Smoke")]
        [Category("StopBinary")]
        public async Task StopBinary()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string AlgoID = metadataForUploadedBinary.Id;

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoID
            };

            string url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.True(uploadBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.True(startBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.True(startResponse.Status.Equals("STARTED"));

            StopBinaryDTO stopAlgo = new StopBinaryDTO
            {
                AlgoId = AlgoID
            };

            url = ApiPaths.ALGO_STORE_ALGO_STOP;

            var stopBinaryResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);
            Assert.True(stopBinaryResponse.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO stopResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(stopBinaryResponse.ResponseJson);
            Assert.True(stopResponse.Status.Equals("STOPPED"));
        }

        [Test]
        [Category("Smoke")]
        [Category("GetLog")]
        public async Task GetLog()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string AlgoID = metadataForUploadedBinary.Id;

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoID
            };

            string url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.True(uploadBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.True(startBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.True(startResponse.Status.Equals("STARTED"));

            url = ApiPaths.ALGO_STORE_ALGO_LOG;

            Dictionary<string, string> algoIDLog = new Dictionary<string, string>()
            {
                { "AlgoId", AlgoID }
            };

            var algoIDLogResponse = await this.Consumer.ExecuteRequest(url, algoIDLog, null, Method.GET);
            Assert.True(algoIDLogResponse.Status == HttpStatusCode.OK);

            LogResponseDTO LogObject = JsonUtils.DeserializeJson<LogResponseDTO>(algoIDLogResponse.ResponseJson);

            Assert.NotNull(LogObject);
        }

        [Test]
        [Category("Smoke")]
        [Category("GetTailLog")]
        public async Task GetTailLog()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string AlgoID = metadataForUploadedBinary.Id;

            DeployBinaryDTO algo = new DeployBinaryDTO()
            {
                AlgoId = AlgoID
            };

            string url = ApiPaths.ALGO_STORE_DEPLOY_BINARY;

            var uploadBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algo), Method.POST);
            Assert.True(uploadBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.True(startBinaryresponce.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.True(startResponse.Status.Equals("STARTED"));

            url = ApiPaths.ALGO_STORE_ALGO_TAIL_LOG;

            Dictionary<string, string> algoIDTailLog = new Dictionary<string, string>()
            {
                { "AlgoId", AlgoID },
                {"Tail" , "60" }
            };

            var algoIDTailLogResponse = await this.Consumer.ExecuteRequest(url, algoIDTailLog, null, Method.GET);
            Assert.True(algoIDTailLogResponse.Status == HttpStatusCode.OK);

            LogResponseDTO LogObject = JsonUtils.DeserializeJson<LogResponseDTO>(algoIDTailLogResponse.ResponseJson);

            Assert.NotNull(LogObject);
        }

        [Test]
        [Category("Smoke")]
        [Category("UploadString")]
        public async Task UploadString()
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            MetaDataResponseDTO metadataWithUploadedString = DataManager.getMetaDataForBinaryUpload();

            string AlgoId = metadataWithUploadedString.Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId },
                {"Data" , Helpers.RandomString(300) }
            };

            var responceUploadString = await this.Consumer.ExecuteRequest(url, quaryParam, null, Method.POST);
            Assert.True(responceUploadString.Status == HttpStatusCode.NoContent);

            bool blobExists = await this.BlobRepository.CheckIfBlobExists(AlgoId, BinaryAlgoFileType.STRING);
            Assert.True(blobExists);
        }

        [Test]
        [Category("Smoke")]
        [Category("GetString")]
        public async Task GetUploadedString()
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            MetaDataResponseDTO metadataWithUploadedString = DataManager.getMetaDataForBinaryUpload();

            string AlgoId = metadataWithUploadedString.Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId },
                {"Data" , Helpers.RandomString(300) }
            };

            var responceUploadString = await this.Consumer.ExecuteRequest(url, quaryParam, null, Method.POST);
            Assert.True(responceUploadString.Status == HttpStatusCode.NoContent);

            Dictionary<string, string> quaryParamGetString = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId }
            };

            var responceGetUploadString = await this.Consumer.ExecuteRequest(url, quaryParamGetString, null, Method.GET);
            Assert.True(responceGetUploadString.Status == HttpStatusCode.OK);

            UploadStringDTO uploadedStringContent = JsonUtils.DeserializeJson<UploadStringDTO>(responceGetUploadString.ResponseJson);

            Assert.True(quaryParam["Data"].Equals(uploadedStringContent.Data));
        }

        [Test]
        [Category("Smoke")]
        [Category("GetAllClientInstanceData")]
        public async Task GetAllClientInstanceData()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string AlgoID = metadataForUploadedBinary.Id;



        }

        [Test]
        [Category("Smoke")]
        [Category("PostInstanceDataForAlgo")]
        public async Task PostInstanceDataForAlgo()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string AlgoID = metadataForUploadedBinary.Id;



        }

        [Test]
        [Category("Smoke")]
        [Category("EditInstanceDataForAlgo")]
        public async Task EditInstanceDataForAlgo()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string AlgoID = metadataForUploadedBinary.Id;

        }

        [Test]
        [Category("Smoke")]
        [Category("GetInstanceData")]
        public async Task GetInstanceData()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string AlgoID = metadataForUploadedBinary.Id;

        }


        private async Task<MetaDataResponseDTO> UploadBinaryAlgoAndGetResponceDTO()
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

            MetaDataResponseDTO metadataWithUploadedBinary = DataManager.getMetaDataForBinaryUpload();

            string AlgoId = metadataWithUploadedBinary.Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId }
            };

            string pathFile = Path.Combine(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar, "AlgoStore" + Path.DirectorySeparatorChar, "TestData" + Path.DirectorySeparatorChar, "myalgo-1.0-SNAPSHOT-jar-with-dependencies-fil-01.jar");

            var responceUploadBinary = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.True(responceUploadBinary.Status == HttpStatusCode.NoContent);

            return metadataWithUploadedBinary;
        }
    }
}
