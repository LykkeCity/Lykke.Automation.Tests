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
        public async Task CheckIfServiceIsAlive()
        {

            string url = ApiPaths.ALGO_STORE_IS_ALIVE;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.That(response.Status , Is.EqualTo(HttpStatusCode.OK));
            var baseDate = JsonUtils.DeserializeJson<IsAliveDTO>(response.ResponseJson).Name;
            Assert.That(baseDate, Is.EqualTo("Lykke.AlgoStore.Api"));
        }

        [Test]
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
        [Category("AlgoStore")]
        public async Task EditMetadata()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;

            MetaDataDTO metadata = new MetaDataDTO()
            {
                Name = Helpers.RandomString(8),
                Description = Helpers.RandomString(8)
            };

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
            MetaDataResponseDTO responceMetaData = JsonUtils.DeserializeJson<MetaDataResponseDTO>(response.ResponseJson);

            url = ApiPaths.ALGO_STORE_METADATA;

            MetaDataEditDTO editMetaData = new MetaDataEditDTO()
            {
                Id = responceMetaData.Id,
                Name = Helpers.RandomString(9),
                Description = Helpers.RandomString(9)
            };

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
        [Category("AlgoStore")]
        public async Task GetAllMetadataForClient()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;
            var responceAllClientMetadata = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.That(responceAllClientMetadata.Status , Is.EqualTo(HttpStatusCode.OK));
            Object responceClientGetAll = JsonUtils.DeserializeJson(responceAllClientMetadata.ResponseJson);
            List<MetaDataResponseDTO> listAllClinetObjects = Newtonsoft.Json.JsonConvert.DeserializeObject<List<MetaDataResponseDTO>>(responceClientGetAll.ToString());
            List<string> keysPreBuildData = new List<string>();
            DataManager.getAllMetaData().ForEach(e => keysPreBuildData.Add(e.AlgoId));
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
        [Category("AlgoStore")]
        public async Task GetTailLog()
        {
            List<BuilInitialDataObjectDTO> metadataForUploadedBinaryList = await UploadSomeBaseMetaData(1);

            BuilInitialDataObjectDTO metadataForUploadedBinary = metadataForUploadedBinaryList[metadataForUploadedBinaryList.Count - 1];

            string AlgoID = metadataForUploadedBinary.AlgoId;


            string url = ApiPaths.ALGO_STORE_ALGO_TAIL_LOG;

            Dictionary<string, string> algoIDTailLog = new Dictionary<string, string>()
            {
                { "AlgoId", AlgoID },
                { "InstanceId" , metadataForUploadedBinary.InstanceId },
                {"AlgoClientId" , "e658abfc-1779-427c-8316-041a2deb1db8"  },
                {"Tail" , "60" }
            };
            int retryCounter = 0;

            var algoIDTailLogResponse = await this.Consumer.ExecuteRequest(url, algoIDTailLog, null, Method.GET);

            while (algoIDTailLogResponse.Status.Equals(System.Net.HttpStatusCode.NotFound) && retryCounter <= 30)
            {
                System.Threading.Thread.Sleep(10000);
                algoIDTailLogResponse = await this.Consumer.ExecuteRequest(url, algoIDTailLog, null, Method.GET);
                retryCounter++;
            }

            Assert.That(algoIDTailLogResponse.Status, Is.EqualTo(HttpStatusCode.OK));

            LogResponseDTO LogObject = JsonUtils.DeserializeJson<LogResponseDTO>(algoIDTailLogResponse.ResponseJson);

            Assert.NotNull(LogObject);
        }

        [Test]
        [Category("AlgoStore")]
        public async Task UploadString()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;

            MetaDataDTO metadata = new MetaDataDTO()
            {
                Name = Helpers.RandomString(13),
                Description = Helpers.RandomString(13)
            };

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            MetaDataResponseDTO responceMetaData = JsonUtils.DeserializeJson<MetaDataResponseDTO>(response.ResponseJson);


            url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            UploadStringDTO stringDTO = new UploadStringDTO()
            {
                AlgoId = responceMetaData.Id,
                Data = "TEST FOR NOW NOT WORKING ALGO"
            };

            var responsetemp = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stringDTO), Method.POST);
            Assert.True(response.Status == System.Net.HttpStatusCode.OK);

            bool blobExists = await this.BlobRepository.CheckIfBlobExists(stringDTO.AlgoId, BinaryAlgoFileType.STRING);
            Assert.That(blobExists , Is.EqualTo(true));
        }

        [Test]
        [Category("AlgoStore")]
        public async Task GetUploadedString()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;

            MetaDataDTO metadata = new MetaDataDTO()
            {
                Name = Helpers.RandomString(13),
                Description = Helpers.RandomString(13)
            };

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            MetaDataResponseDTO responceMetaData = JsonUtils.DeserializeJson<MetaDataResponseDTO>(response.ResponseJson);


            url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            UploadStringDTO stringDTO = new UploadStringDTO()
            {
                AlgoId = responceMetaData.Id,
                Data =  this.CSharpAlgoString
            };

            var responsetemp = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stringDTO), Method.POST);
            Assert.True(response.Status == System.Net.HttpStatusCode.OK);

            Dictionary<string, string> quaryParamGetString = new Dictionary<string, string>()
            {
                {"AlgoId", responceMetaData.Id }
            };

            var responceGetUploadString = await this.Consumer.ExecuteRequest(url, quaryParamGetString, null, Method.GET);
            Assert.That(responceGetUploadString.Status , Is.EqualTo(HttpStatusCode.OK));

            UploadStringDTO uploadedStringContent = JsonUtils.DeserializeJson<UploadStringDTO>(responceGetUploadString.ResponseJson);

            Assert.That(stringDTO.Data, Is.EqualTo(uploadedStringContent.Data));
        }

        [Test]
        [Category("AlgoStore")]
        public async Task GetAllClientInstanceData()
        {

            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();

            string algoID = metadataForUploadedBinary.AlgoId;

            GetPopulatedInstanceDataDTO getinstanceAlgo = new GetPopulatedInstanceDataDTO();

            InstanceDataDTO instanceForAlgo = getinstanceAlgo.returnInstanceDataDTO(algoID);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.OK);

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoClientId, Is.EqualTo(instanceForAlgo.AlgoClientId));
            Assert.That(postInstanceData.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceData.InstanceId, Is.EqualTo(instanceForAlgo.InstanceId));
            Assert.That(postInstanceData.InstanceName, Is.EqualTo(instanceForAlgo.InstanceName));

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
        [Category("AlgoStore")]
        public async Task PostInstanceDataForAlgo()
        {

            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();

            string algoID = metadataForUploadedBinary.AlgoId;

            GetPopulatedInstanceDataDTO getinstanceAlgo = new GetPopulatedInstanceDataDTO();

            InstanceDataDTO instanceForAlgo = getinstanceAlgo.returnInstanceDataDTO(algoID);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.OK);

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoId, Is.EqualTo((instanceForAlgo.AlgoId)));
            Assert.That(postInstanceData.AlgoClientId, Is.EqualTo(instanceForAlgo.AlgoClientId));
            Assert.That(postInstanceData.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceData.InstanceId, Is.EqualTo(instanceForAlgo.InstanceId));
            Assert.That(postInstanceData.InstanceName, Is.EqualTo(instanceForAlgo.InstanceName));


            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);
        }
        [Test]
        [Category("AlgoStore")]
        public async Task GetInstanceDataForAlgo()
        {

            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();

            string algoID = metadataForUploadedBinary.AlgoId;

            GetPopulatedInstanceDataDTO getinstanceAlgo = new GetPopulatedInstanceDataDTO();

            InstanceDataDTO instanceForAlgo = getinstanceAlgo.returnInstanceDataDTO(algoID);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.OK);

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            Dictionary<string, string> queryParmas = new Dictionary<string, string>()
            {
                { "algoId" , postInstanceData.AlgoId },
                { "instanceId", postInstanceData.InstanceId}
            };

            var getInstanceDataResponse = await this.Consumer.ExecuteRequest(url, queryParmas, null, Method.GET);
            Assert.That(getInstanceDataResponse.Status == HttpStatusCode.OK);
            InstanceDataDTO getInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(getInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoClientId, Is.EqualTo(instanceForAlgo.AlgoClientId));
            Assert.That(postInstanceData.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceData.InstanceId, Is.EqualTo(instanceForAlgo.InstanceId));
            Assert.That(postInstanceData.InstanceName, Is.EqualTo(instanceForAlgo.InstanceName));
        }

        [Test]
        [Category("AlgoStore")]
        public async Task EditInstanceDataForAlgo()
        {

            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();
            string algoID = metadataForUploadedBinary.AlgoId;

            GetPopulatedInstanceDataDTO getinstanceAlgo = new GetPopulatedInstanceDataDTO();

            InstanceDataDTO instanceForAlgo = getinstanceAlgo.returnInstanceDataDTO(algoID);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status , Is.EqualTo(HttpStatusCode.OK));

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoClientId, Is.EqualTo(instanceForAlgo.AlgoClientId));
            Assert.That(postInstanceData.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceData.InstanceId, Is.EqualTo(instanceForAlgo.InstanceId));
            Assert.That(postInstanceData.InstanceName, Is.EqualTo(instanceForAlgo.InstanceName));

            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);
            InstanceDataDTO instanceForAlgoEdit = postInstanceData;
            postInstanceData.InstanceName = "EditedTest";

            var postInstanceDataResponseEdit = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgoEdit), Method.POST);

            Assert.That(postInstanceDataResponseEdit.Status , Is.EqualTo(HttpStatusCode.OK));
            InstanceDataDTO postInstanceDataEdit = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponseEdit.ResponseJson);
            Assert.That(postInstanceData.AlgoClientId, Is.EqualTo(instanceForAlgo.AlgoClientId));
            Assert.That(postInstanceData.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceData.InstanceId, Is.EqualTo(instanceForAlgo.InstanceId));
            Assert.That(postInstanceData.InstanceName, Is.EqualTo("EditedTest"));

            ClientInstanceEntity instanceDataEntityExistsEdit = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceDataEdit.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExistsEdit);
        }

        [Test]
        [Category("AlgoStore")]
        public async Task GetInstanceData()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();

            string algoID = metadataForUploadedBinary.AlgoId;

            GetPopulatedInstanceDataDTO getinstanceAlgo = new GetPopulatedInstanceDataDTO();

            InstanceDataDTO instanceForAlgo = getinstanceAlgo.returnInstanceDataDTO(algoID);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status , Is.EqualTo(HttpStatusCode.OK));

            InstanceDataDTO postInstanceData = JsonUtils.DeserializeJson<InstanceDataDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceData.AlgoClientId, Is.EqualTo(instanceForAlgo.AlgoClientId));
            Assert.That(postInstanceData.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceData.InstanceId, Is.EqualTo(instanceForAlgo.InstanceId));
            Assert.That(postInstanceData.InstanceName, Is.EqualTo(instanceForAlgo.InstanceName));

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

            Assert.That(postInstanceData.AlgoClientId, Is.EqualTo(instanceForAlgo.AlgoClientId));
            Assert.That(postInstanceData.AlgoId, Is.EqualTo(instanceForAlgo.AlgoId));
            Assert.That(postInstanceData.InstanceId, Is.EqualTo(instanceForAlgo.InstanceId));
            Assert.That(postInstanceData.InstanceName, Is.EqualTo(instanceForAlgo.InstanceName));
        }

        [Test]
        [Category("AlgoStore")]
        public async Task ClientDataGetAllAlgos()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();

            string algoID = metadataForUploadedBinary.AlgoId;

            string url = ApiPaths.ALGO_STORE_ADD_TO_PUBLIC;

            MetaDataEntity metaDataEntity = await MetaDataRepository.TryGetAsync(t => t.Id == algoID) as MetaDataEntity;
            Assert.NotNull(metaDataEntity);

            AddToPublicDTO addAlgo = new AddToPublicDTO()
            {
                AlgoId = algoID,
                ClientId = metaDataEntity.PartitionKey
            };

            var addAlgoToPublicEndpoint = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(addAlgo), Method.POST);
            Assert.That(addAlgoToPublicEndpoint.Status, Is.EqualTo(HttpStatusCode.OK));

            url = ApiPaths.ALGO_STORE_CLIENT_DATA_GET_ALL_ALGOS;

            var clientDataAllAlgos = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.That(clientDataAllAlgos.Status, Is.EqualTo(HttpStatusCode.OK));

            Object responceclientDataAllAlgos = JsonUtils.DeserializeJson(clientDataAllAlgos.ResponseJson);
            List<AlgoDTO> listAllAlgos = Newtonsoft.Json.JsonConvert.DeserializeObject<List<AlgoDTO>>(responceclientDataAllAlgos.ToString());
            AlgoDTO expectedAlgoDTO = listAllAlgos.FindLast(t => t.Id.Equals(algoID));
            string AlgoIdFromGettAllAlgos = expectedAlgoDTO.Id;
            Assert.That(algoID, Is.EqualTo(AlgoIdFromGettAllAlgos));
            foreach (AlgoDTO algo in listAllAlgos)
            {
                Assert.Zero(expectedAlgoDTO.Rating);
                Assert.NotZero(expectedAlgoDTO.UsersCount);
                Assert.NotNull(expectedAlgoDTO.Id);
                Assert.NotNull(expectedAlgoDTO.Name);
                Assert.NotNull(expectedAlgoDTO.Description);
                Assert.NotNull(expectedAlgoDTO.Date);
                Assert.NotNull(expectedAlgoDTO.Author);
            }
        }
       
        [Category("AlgoStore")]
        [TestCase("")]
        [TestCase("getFromData")]
        public async Task GetAlgoMetaData(string clientIdTemp)
        {
            List<BuilInitialDataObjectDTO> metadataForUploadedBinaryList = await UploadSomeBaseMetaData(1);

            BuilInitialDataObjectDTO metadataForUploadedBinary = metadataForUploadedBinaryList[metadataForUploadedBinaryList.Count - 1];

            MetaDataEntity metaDataEntity = await MetaDataRepository.TryGetAsync(t => t.Id == metadataForUploadedBinary.AlgoId) as MetaDataEntity;

            if (clientIdTemp.Equals("getFromData"))
            {
                clientIdTemp = metaDataEntity.PartitionKey;
            }

            string url = ApiPaths.ALGO_STORE_GET_ALGO_METADATA;

            Dictionary<string, string> quaryParamAlgoData = new Dictionary<string, string>()
            {
                {"AlgoId",  metadataForUploadedBinary.AlgoId },
                {"clientId", clientIdTemp}
            };

            var responceAlgoMetadata = await this.Consumer.ExecuteRequest(url, quaryParamAlgoData, null, Method.GET);
            Assert.That(responceAlgoMetadata.Status , Is.EqualTo(HttpStatusCode.OK));

            GetAlgoMetaDataDTO postInstanceData = JsonUtils.DeserializeJson<GetAlgoMetaDataDTO>(responceAlgoMetadata.ResponseJson);

            Assert.That(postInstanceData.AlgoId, Is.EqualTo(metadataForUploadedBinary.AlgoId));
            Assert.That(postInstanceData.Name, Is.EqualTo(metadataForUploadedBinary.Name));
            Assert.That(postInstanceData.Description, Is.EqualTo(metadataForUploadedBinary.Description));
            Assert.That(postInstanceData.Date, Is.Not.Null);
            Assert.That(postInstanceData.Author, Is.Not.Null);
            Assert.That(postInstanceData.Rating, Is.Zero);
            Assert.That(postInstanceData.UsersCount, Is.Not.Zero);
            Assert.That(postInstanceData.AlgoMetaDataInformation, Is.Null);
        }
        [Test]
        [Category("AlgoStore")]
        public async Task DeployStringAlgo()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;

            MetaDataDTO metadata = new MetaDataDTO()
             {
                 Name = Helpers.RandomString(13),
                 Description = Helpers.RandomString(13)
             };
        
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            MetaDataResponseDTO responceMetaData = JsonUtils.DeserializeJson<MetaDataResponseDTO>(response.ResponseJson);

            url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            UploadStringDTO stringDTO = new UploadStringDTO()
                {
                    AlgoId = responceMetaData.Id,
                    Data = this.CSharpAlgoString
                };

            var responsetemp = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stringDTO), Method.POST);
            Assert.True(responsetemp.Status == System.Net.HttpStatusCode.NoContent);

            GetPopulatedInstanceDataDTO getinstanceAlgo = new GetPopulatedInstanceDataDTO();

            InstanceDataDTO instanceForAlgo = getinstanceAlgo.returnInstanceDataDTO(stringDTO.AlgoId);

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
            Assert.That(postInstanceDataResponse.Status == System.Net.HttpStatusCode.OK);

            url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
            int retryCounter = 0;

            CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
                {
                    AlgoId = postInstanceData.AlgoId,
                    InstanceId = postInstanceData.InstanceId
                };
            var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);

           // Currently we can not send cascade delete to kubernatees if he has not build the algo before that thorus not found and we leave data

           while (responceCascadeDelete.Status.Equals(System.Net.HttpStatusCode.NotFound) && retryCounter <= 30)
                {
                    System.Threading.Thread.Sleep(10000);
                    responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
                    retryCounter++;
                }

           Assert.That(responceCascadeDelete.Status == System.Net.HttpStatusCode.NoContent);

            }

        private async Task<UploadStringDTO> UploadStringAlgo()
        {
            string url = ApiPaths.ALGO_STORE_METADATA;

            MetaDataDTO metadata = new MetaDataDTO()
            {
                Name = Helpers.RandomString(13),
                Description = Helpers.RandomString(13)
            };

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
            MetaDataResponseDTO responceMetaData = JsonUtils.DeserializeJson<MetaDataResponseDTO>(response.ResponseJson);


            url = ApiPaths.ALGO_STORE_UPLOAD_STRING;

            UploadStringDTO stringDTO = new UploadStringDTO()
            {
                AlgoId = responceMetaData.Id,
                Data = this.CSharpAlgoString
            };

            var responsetemp = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(stringDTO), Method.POST);
            Assert.True(response.Status == System.Net.HttpStatusCode.OK);
            return stringDTO;
        }
    }
}
