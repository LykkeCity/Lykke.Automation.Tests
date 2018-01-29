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
        public static string pathFile = Path.Combine(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar, "AlgoStore" + Path.DirectorySeparatorChar, "TestData" + Path.DirectorySeparatorChar, "myalgo-1.0-SNAPSHOT-jar-with-dependencies-fil-01.jar");

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
        public async Task CheckIfServiceIsAlive()
        {

            string url = ApiPaths.ALGO_STORE_IS_ALIVE;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.That(response.Status , Is.EqualTo(HttpStatusCode.OK));
            var baseDate = JsonUtils.DeserializeJson<IsAliveDTO>(response.ResponseJson).Name;
            Assert.That(baseDate, Is.EqualTo("Lykke.AlgoStore.Api"));
        }

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
        public async Task UploadMetadata()
        {

            string url = ApiPaths.ALGO_STORE_METADATA;

            MetaDataDTO metadata = new MetaDataDTO()
            {
                Name = Helpers.RandomString(8),
                Description = Helpers.RandomString(8)
            };
            
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            Assert.That(response.Status , Is.EqualTo(HttpStatusCode.OK));
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
        [Category("AlgoStore")]
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
            Assert.That(responseMetaDataAfterEdit.Status , Is.EqualTo(HttpStatusCode.OK));
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
        [Category("AlgoStore")]
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
            Assert.That(responceCascadeDelete.Status , Is.EqualTo(HttpStatusCode.NoContent));
            MetaDataEntity metaDataEntityDeleted = await MetaDataRepository.TryGetAsync(t => t.Id == editMetaData.Id) as MetaDataEntity;
            Assert.Null(metaDataEntityDeleted);
        }

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
        public async Task GetAllMetadataForClient()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;
            var responceAllClientMetadata = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.That(responceAllClientMetadata.Status , Is.EqualTo(HttpStatusCode.OK));
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
            Assert.That(keysPreBuildData.Count , Is.EqualTo(mathcedKeysCounter));
        }

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
        public async Task UploadBinaryAlgo()
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_BINARY;

            string AlgoId = DataManager.getMetaDataForBinaryUpload().Id;

            Dictionary<string, string> quaryParam = new Dictionary<string, string>()
            {
                {"AlgoId", AlgoId }
            };

            var responceAllClientMetadata = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.That(responceAllClientMetadata.Status , Is.EqualTo(HttpStatusCode.NoContent));
            bool blobExists = await this.BlobRepository.CheckIfBlobExists(AlgoId, BinaryAlgoFileType.JAR);
            Assert.That(blobExists , Is.EqualTo(true));
        }

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
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
            Assert.That(uploadBinaryresponce.Status , Is.EqualTo(HttpStatusCode.OK));

            RuntimeDataEntity runtimeDataEntity = await RuntimeDataRepository.TryGetAsync(t => t.Id == metadataForUploadedBinary.Id) as RuntimeDataEntity;
            Assert.NotNull(runtimeDataEntity);
        }

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
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
            Assert.That(uploadBinaryresponce.Status , Is.EqualTo(HttpStatusCode.OK));

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.That(startBinaryresponce.Status , Is.EqualTo(HttpStatusCode.OK));

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.That(startResponse.Status, Is.EqualTo("STARTED"));
        }

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
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
            Assert.That(uploadBinaryresponce.Status , Is.EqualTo(HttpStatusCode.OK));

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.That(startBinaryresponce.Status , Is.EqualTo(HttpStatusCode.OK));

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.That(startResponse.Status, Is.EqualTo("STARTED"));

            StopBinaryDTO stopAlgo = new StopBinaryDTO
            {
                AlgoId = AlgoID
            };

            url = ApiPaths.ALGO_STORE_ALGO_STOP;

            var stopBinaryResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stopAlgo), Method.POST);
            Assert.That(stopBinaryResponse.Status == HttpStatusCode.OK);

            StartBinaryResponseDTO stopResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(stopBinaryResponse.ResponseJson);
            Assert.That(stopResponse.Status, Is.EqualTo("STOPPED"));
        }

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
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
            Assert.That(uploadBinaryresponce.Status , Is.EqualTo(HttpStatusCode.OK));

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.That(startBinaryresponce.Status , Is.EqualTo(HttpStatusCode.OK));

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.That(startResponse.Status, Is.EqualTo("STARTED"));

            url = ApiPaths.ALGO_STORE_ALGO_LOG;

            Dictionary<string, string> algoIDLog = new Dictionary<string, string>()
            {
                { "AlgoId", AlgoID }
            };

            var algoIDLogResponse = await this.Consumer.ExecuteRequest(url, algoIDLog, null, Method.GET);
            Assert.That(algoIDLogResponse.Status , Is.EqualTo(HttpStatusCode.OK));

            LogResponseDTO LogObject = JsonUtils.DeserializeJson<LogResponseDTO>(algoIDLogResponse.ResponseJson);

            Assert.NotNull(LogObject);
        }

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
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
            Assert.That(uploadBinaryresponce.Status , Is.EqualTo(HttpStatusCode.OK));

            StartBinaryDTO startAlgo = new StartBinaryDTO
            {
                AlgoId = algo.AlgoId
            };

            url = ApiPaths.ALGO_STORE_ALGO_START;

            var startBinaryresponce = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(startAlgo), Method.POST);
            Assert.That(startBinaryresponce.Status , Is.EqualTo(HttpStatusCode.OK));

            StartBinaryResponseDTO startResponse = JsonUtils.DeserializeJson<StartBinaryResponseDTO>(startBinaryresponce.ResponseJson);
            Assert.That(startResponse.Status, Is.EqualTo("STARTED"));

            url = ApiPaths.ALGO_STORE_ALGO_TAIL_LOG;

            Dictionary<string, string> algoIDTailLog = new Dictionary<string, string>()
            {
                { "AlgoId", AlgoID },
                {"Tail" , "60" }
            };

            var algoIDTailLogResponse = await this.Consumer.ExecuteRequest(url, algoIDTailLog, null, Method.GET);
            Assert.That(algoIDTailLogResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            LogResponseDTO LogObject = JsonUtils.DeserializeJson<LogResponseDTO>(algoIDTailLogResponse.ResponseJson);

            Assert.NotNull(LogObject);
        }

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
        public async Task UploadString()
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            MetaDataResponseDTO metadataWithUploadedString = DataManager.getMetaDataForBinaryUpload();

            string Algoid = metadataWithUploadedString.Id;

            PostUploadStringAlgoDTO uploadedStringDTO = new PostUploadStringAlgoDTO()
            {
                AlgoId = Algoid,
                Data = Helpers.RandomString(300) 
            };

            var responceUploadString = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(uploadedStringDTO), Method.POST);
            Assert.That(responceUploadString.Status , Is.EqualTo(HttpStatusCode.NoContent));

            bool blobExists = await this.BlobRepository.CheckIfBlobExists(Algoid, BinaryAlgoFileType.STRING);
            Assert.That(blobExists , Is.EqualTo(true));
        }

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
        public async Task GetUploadedString()
        {
            string url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            MetaDataResponseDTO metadataWithUploadedString = DataManager.getMetaDataForBinaryUpload();

            string Algoid = metadataWithUploadedString.Id;

            PostUploadStringAlgoDTO uploadedStringDTO = new PostUploadStringAlgoDTO()
            {
                AlgoId = Algoid,
                Data = Helpers.RandomString(300)
            };

            var responceUploadString = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(uploadedStringDTO), Method.POST);
            Assert.That(responceUploadString.Status , Is.EqualTo(HttpStatusCode.NoContent));

            Dictionary<string, string> quaryParamGetString = new Dictionary<string, string>()
            {
                {"AlgoId", Algoid }
            };

            var responceGetUploadString = await this.Consumer.ExecuteRequest(url, quaryParamGetString, null, Method.GET);
            Assert.That(responceGetUploadString.Status , Is.EqualTo(HttpStatusCode.OK));

            UploadStringDTO uploadedStringContent = JsonUtils.DeserializeJson<UploadStringDTO>(responceGetUploadString.ResponseJson);

            Assert.That(uploadedStringDTO.Data, Is.EqualTo(uploadedStringContent.Data));
        }

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
        public async Task GetAllClientInstanceData()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string algoID = metadataForUploadedBinary.Id;

            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = algoID,
                HftApiKey = "key",
                AssetPair = "BTCUSD",
                TradedAsset = "USD",
                Margin = "1",
                Volume = "1"
            };

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.OK);

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceData.AssetPair, Is.EqualTo(instanceForAlgo.AssetPair));
            Assert.That(postInstanceData.HftApiKey, Is.EqualTo(instanceForAlgo.HftApiKey));
            Assert.That(postInstanceData.TradedAsset, Is.EqualTo(instanceForAlgo.TradedAsset));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Volume), (int)Convert.ToDouble(instanceForAlgo.Volume));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Margin), (int)Convert.ToDouble(instanceForAlgo.Margin));
            Assert.NotNull(postInstanceData.InstanceId);

            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);

            url = ApiPaths.ALGO_STORE_ALGO_GET_ALL_INSTANCE_DATA;

            Dictionary<string, string> queryParmas = new Dictionary<string, string>()
            {
                { "algoId" , algoID }
            };

            var responceAllClientInstance = await this.Consumer.ExecuteRequest(url, queryParmas, null, Method.GET);
            Assert.That(responceAllClientInstance.Status , Is.EqualTo(HttpStatusCode.OK));
            Object responceClientGetAll = JsonUtils.DeserializeJson(responceAllClientInstance.ResponseJson);
            List<InstanceDataDTO> listAllClinetObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<InstanceDataDTO>>(responceClientGetAll.ToString());
            int mathcedKeysCounter = 0;
            foreach (InstanceDataDTO currentData in listAllClinetObjects)
            {
                bool Exists = currentData.InstanceId.Equals(postInstanceData.InstanceId);
                if (Exists)
                {
                    mathcedKeysCounter++;
                }
            }
            Assert.That(mathcedKeysCounter , Is.EqualTo(1));
        }

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
        public async Task PostInstanceDataForAlgo()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string algoID = metadataForUploadedBinary.Id;

            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = algoID,
                HftApiKey = "key",
                AssetPair = "BTCUSD",
                TradedAsset = "USD",
                Margin ="1",
                Volume = "1"
            };

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.OK);

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoId, Is.EqualTo((instanceForAlgo.AlgoId)));
            Assert.That(postInstanceData.AssetPair, Is.EqualTo((instanceForAlgo.AssetPair)));
            Assert.That(postInstanceData.HftApiKey, Is.EqualTo((instanceForAlgo.HftApiKey)));
            Assert.That(postInstanceData.TradedAsset, Is.EqualTo((instanceForAlgo.TradedAsset)));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Volume) , (int)Convert.ToDouble(instanceForAlgo.Volume));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Margin) , (int)Convert.ToDouble(instanceForAlgo.Margin));
            Assert.NotNull(postInstanceData.InstanceId);

            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);
        }

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
        public async Task EditInstanceDataForAlgo()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();
            string algoID = metadataForUploadedBinary.Id;

            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = algoID,
                HftApiKey = "key",
                AssetPair = "BTCUSD",
                TradedAsset = "USD",
                Margin = "1",
                Volume = "1"
            };

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status , Is.EqualTo(HttpStatusCode.OK));

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoId, Is.EqualTo((instanceForAlgo.AlgoId)));
            Assert.That(postInstanceData.AssetPair, Is.EqualTo((instanceForAlgo.AssetPair)));
            Assert.That(postInstanceData.HftApiKey, Is.EqualTo((instanceForAlgo.HftApiKey)));
            Assert.That(postInstanceData.TradedAsset, Is.EqualTo((instanceForAlgo.TradedAsset)));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Volume), (int)Convert.ToDouble(instanceForAlgo.Volume));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Margin), (int)Convert.ToDouble(instanceForAlgo.Margin));
            Assert.NotNull(postInstanceData.InstanceId);

            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);
            InstanceDataDTO instanceForAlgoEdit = new InstanceDataDTO()
            {
                InstanceId = postInstanceData.InstanceId,
                AlgoId = algoID,
                HftApiKey = "key",
                AssetPair = "BTCEUR",
                TradedAsset = "EUR",
                Margin = "2",
                Volume = "2"
            };
            var postInstanceDataResponseEdit = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgoEdit), Method.POST);

            Assert.That(postInstanceDataResponseEdit.Status , Is.EqualTo(HttpStatusCode.OK));
            InstanceDataDTO postInstanceDataEdit = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponseEdit.ResponseJson);
            Assert.That(postInstanceDataEdit.AlgoId, Is.EqualTo((instanceForAlgoEdit.AlgoId)));
            Assert.That(postInstanceDataEdit.AssetPair, Is.EqualTo((instanceForAlgoEdit.AssetPair)));
            Assert.That(postInstanceDataEdit.HftApiKey, Is.EqualTo((instanceForAlgoEdit.HftApiKey)));
            Assert.That(postInstanceDataEdit.TradedAsset, Is.EqualTo((instanceForAlgoEdit.TradedAsset)));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceDataEdit.Volume), (int)Convert.ToDouble(instanceForAlgoEdit.Volume));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceDataEdit.Margin), (int)Convert.ToDouble(instanceForAlgoEdit.Margin));
            Assert.That(postInstanceDataEdit.InstanceId, Is.EqualTo((postInstanceData.InstanceId)));

            ClientInstanceEntity instanceDataEntityExistsEdit = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceDataEdit.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExistsEdit);
        }

        [Test]
        [Category("Smoke")]
        [Category("AlgoStore")]
        public async Task GetInstanceData()
        {
            MetaDataResponseDTO metadataForUploadedBinary = await UploadBinaryAlgoAndGetResponceDTO();

            string algoID = metadataForUploadedBinary.Id;

            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = algoID,
                HftApiKey = "key",
                AssetPair = "BTCUSD",
                TradedAsset = "USD",
                Margin = "1",
                Volume = "1"
            };

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status , Is.EqualTo(HttpStatusCode.OK));

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoId, Is.EqualTo((instanceForAlgo.AlgoId)));
            Assert.That(postInstanceData.AssetPair, Is.EqualTo((instanceForAlgo.AssetPair)));
            Assert.That(postInstanceData.HftApiKey, Is.EqualTo((instanceForAlgo.HftApiKey)));
            Assert.That(postInstanceData.TradedAsset, Is.EqualTo((instanceForAlgo.TradedAsset)));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Volume), (int)Convert.ToDouble(instanceForAlgo.Volume));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Margin), (int)Convert.ToDouble(instanceForAlgo.Margin));
            Assert.NotNull(postInstanceData.InstanceId);

            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);

            Dictionary<string, string> queryParmas = new Dictionary<string, string>()
            {
                { "algoId" , algoID },
                { "instanceId", postInstanceData.InstanceId}
            };

            var instanceDataResponse = await this.Consumer.ExecuteRequest(url, queryParmas, null, Method.GET);
            Assert.That(instanceDataResponse.Status , Is.EqualTo(HttpStatusCode.OK));

            InstanceDataDTO returnedClinetInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(instanceDataResponse.ResponseJson);

            Assert.That(returnedClinetInstanceData.AlgoId, Is.EqualTo((postInstanceData.AlgoId)));
            Assert.That(returnedClinetInstanceData.AssetPair, Is.EqualTo((postInstanceData.AssetPair)));
            Assert.That(returnedClinetInstanceData.HftApiKey, Is.EqualTo((postInstanceData.HftApiKey)));
            Assert.That(returnedClinetInstanceData.TradedAsset, Is.EqualTo((postInstanceData.TradedAsset)));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Volume), (int)Convert.ToDouble(postInstanceData.Volume));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Margin), (int)Convert.ToDouble(postInstanceData.Margin));
            Assert.That(returnedClinetInstanceData.InstanceId, Is.EqualTo((postInstanceData.InstanceId)));
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

            var responceUploadBinary = await this.Consumer.ExecuteRequestFileUpload(url, quaryParam, null, Method.POST, pathFile);
            Assert.That(responceUploadBinary.Status, Is.EqualTo(HttpStatusCode.NoContent));

            return metadataWithUploadedBinary;
        }
    }
}
