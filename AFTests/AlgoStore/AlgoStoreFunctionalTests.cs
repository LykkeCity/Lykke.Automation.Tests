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
        //[Test]
        //[Category("Functional")]
        //[Category("UploadMetadataWithEmptyDescription")]
        //public async Task UploadMetadataWithEmptyDescription()
        //{

        //    string url = ApiPaths.ALGO_STORE_METADATA;

        //    MetaDataDTO metadata = new MetaDataDTO()
        //    {
        //        Name = Helpers.RandomString(8),
        //        Description = Helpers.RandomString(8)
        //    };

        //    var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(metadata), Method.POST);
        //    Assert.True(response.Status == HttpStatusCode.OK);
        //    MetaDataResponseDTO responceMetaData = JsonUtils.DeserializeJson<MetaDataResponseDTO>(response.ResponseJson);

        //    DataManager.addSingleMetadata(responceMetaData);

        //    Assert.AreEqual(metadata.Name, responceMetaData.Name);
        //    Assert.AreEqual(metadata.Description, responceMetaData.Description);
        //    Assert.NotNull(responceMetaData.Date);
        //    Assert.NotNull(responceMetaData.Id);
        //    Assert.Null(responceMetaData.Status);

        //    MetaDataEntity metaDataEntity = await MetaDataRepository.TryGetAsync(t => t.Id == responceMetaData.Id) as MetaDataEntity;

        //    Assert.NotNull(metaDataEntity);
        //    Assert.AreEqual(metaDataEntity.Id, responceMetaData.Id);
        //    Assert.AreEqual(metaDataEntity.Name, responceMetaData.Name);
        //    Assert.AreEqual(metaDataEntity.Description, responceMetaData.Description);
        //}

        //[Test]
        //[Category("Functional")]
        //[Category("DeleteMetadataOnlyWithIdParam")]
        //public async Task DeleteMetadataOnlyWithIdParam()
        //{
        //    MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForDelete();
        //    CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
        //    {
        //        Id = temporaryResponseDTO.Id,
        //        Name = temporaryResponseDTO.Name
        //    };

        //    string url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
        //    var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
        //    Assert.True(responceCascadeDelete.Status == HttpStatusCode.NoContent);
        //    MetaDataEntity metaDataEntityDeleted = await MetaDataRepository.TryGetAsync(t => t.Id == editMetaData.Id) as MetaDataEntity;
        //    Assert.Null(metaDataEntityDeleted);
        //}

        //[Test]
        //[Category("Functional")]
        //[Category("DeleteMetadataWithUpoadAlgoString")]
        //public async Task DeleteMetadataWithUpoadAlgoString()
        //{
        //    MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForDelete();
        //    CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
        //    {
        //        Id = temporaryResponseDTO.Id,
        //        Name = temporaryResponseDTO.Name
        //    };

        //    string url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
        //    var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
        //    Assert.True(responceCascadeDelete.Status == HttpStatusCode.NoContent);
        //    MetaDataEntity metaDataEntityDeleted = await MetaDataRepository.TryGetAsync(t => t.Id == editMetaData.Id) as MetaDataEntity;
        //    Assert.Null(metaDataEntityDeleted);
        //}

        //[Test]
        //[Category("Functional")]
        //[Category("DeleteMetadataWithUpoadAlgoBinary")]
        //public async Task DeleteMetadataWithUpoadAlgoBinary()
        //{
        //    MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForDelete();
        //    CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
        //    {
        //        Id = temporaryResponseDTO.Id,
        //        Name = temporaryResponseDTO.Name
        //    };

        //    string url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
        //    var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
        //    Assert.True(responceCascadeDelete.Status == HttpStatusCode.NoContent);
        //    MetaDataEntity metaDataEntityDeleted = await MetaDataRepository.TryGetAsync(t => t.Id == editMetaData.Id) as MetaDataEntity;
        //    Assert.Null(metaDataEntityDeleted);
        //}

        //[Test]
        //[Category("Functional")]
        //[Category("DeleteMetadataWithDeployedAlgo")]
        //public async Task DeleteMetadataWithDeployedAlgo()
        //{
        //    MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForDelete();
        //    CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
        //    {
        //        Id = temporaryResponseDTO.Id,
        //        Name = temporaryResponseDTO.Name
        //    };

        //    string url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
        //    var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
        //    Assert.True(responceCascadeDelete.Status == HttpStatusCode.NoContent);
        //    MetaDataEntity metaDataEntityDeleted = await MetaDataRepository.TryGetAsync(t => t.Id == editMetaData.Id) as MetaDataEntity;
        //    Assert.Null(metaDataEntityDeleted);
        //}

        //[Test]
        //[Category("Functional")]
        //[Category("DeleteMetadataWithStoppedAlgo")]
        //public async Task DeleteMetadataWithStoppedAlgo()
        //{
        //    MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForDelete();
        //    CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
        //    {
        //        Id = temporaryResponseDTO.Id,
        //        Name = temporaryResponseDTO.Name
        //    };

        //    string url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
        //    var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
        //    Assert.True(responceCascadeDelete.Status == HttpStatusCode.NoContent);
        //    MetaDataEntity metaDataEntityDeleted = await MetaDataRepository.TryGetAsync(t => t.Id == editMetaData.Id) as MetaDataEntity;
        //    Assert.Null(metaDataEntityDeleted);
        //}

        //[Test]
        //[Category("Functional")]
        //[Category("DeleteMetadataWithStartedAlgo")]
        //public async Task DeleteMetadataWithStartedAlgo()
        //{
        //    MetaDataResponseDTO temporaryResponseDTO = DataManager.getMetadataForDelete();
        //    CascadeDeleteDTO editMetaData = new CascadeDeleteDTO()
        //    {
        //        Id = temporaryResponseDTO.Id,
        //        Name = temporaryResponseDTO.Name
        //    };

        //    string url = ApiPaths.ALGO_STORE_CASCADE_DELETE;
        //    var responceCascadeDelete = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(editMetaData), Method.POST);
        //    Assert.True(responceCascadeDelete.Status == HttpStatusCode.NoContent);
        //    MetaDataEntity metaDataEntityDeleted = await MetaDataRepository.TryGetAsync(t => t.Id == editMetaData.Id) as MetaDataEntity;
        //    Assert.Null(metaDataEntityDeleted);
        //}

        //[Test]
        //[Category("Functional")]
        //[Category("UploadBinaryAlgoWithIdThatHasAlreadyUploadedAlgo")]
        //public async Task UploadBinaryAlgoWithIdThatHasAlreadyUploadedAlgo()
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
        //[Category("Functional")]
        //[Category("UploadBinaryAlgoWithWrongId")]
        //public async Task UploadBinaryAlgoWithWrongId()
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
        //[Category("Functional")]
        //[Category("DeployBianryAlgoWithWrongId")]
        //public async Task DeployBianryAlgoWithWrongId()
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
        //[Category("Functional")]
        //[Category("RestartBinary")]
        //public async Task RestartBinary()
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
        //[Category("Functional")]
        //[Category("RestartStringAlgo")]
        //public async Task RestartStringAlgo()
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
        //[Category("Functional")]
        //[Category("DoubleStopBinary")]
        //public async Task DoubleStopBinary()
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
        //[Category("Functional")]
        //[Category("DoubleStartBinary")]
        //public async Task DoubleStartBinary()
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
        //[Category("Functional")]
        //[Category("GetLogOnRunningAlgo")]
        //public async Task GetLogOnRunningAlgo()
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

        //    url = ApiPaths.ALGO_STORE_ALGO_LOG;

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
        //[Category("Functional")]
        //[Category("GetLogOnStoppedAlgo")]
        //public async Task GetLogOnStoppedAlgo()
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

        //    url = ApiPaths.ALGO_STORE_ALGO_LOG;

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
        //[Category("Functional")]
        //[Category("GetLogOnDeployedOnlyAlgo")]
        //public async Task GetLogOnDeployedOnlyAlgo()
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

        //    url = ApiPaths.ALGO_STORE_ALGO_LOG;

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
        //[Category("Functional")]
        //[Category("GetTailLogOnStoppedAlgo")]
        //public async Task GetTailLogOnStoppedAlgo()
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

        //    url = ApiPaths.ALGO_STORE_ALGO_TAIL_LOG;

        //    Dictionary<string, string> algoIDTailLog = new Dictionary<string, string>()
        //    {
        //        { "AlgoId", AlgoID },
        //        {"Tail" , "60" }
        //    };

        //    var algoIDTailLogResponse = await this.Consumer.ExecuteRequest(url, algoIDTailLog, null, Method.GET);
        //    Assert.True(algoIDTailLogResponse.Status == HttpStatusCode.OK);

        //    LogResponseDTO LogObject = JsonUtils.DeserializeJson<LogResponseDTO>(algoIDTailLogResponse.ResponseJson);

        //    Assert.NotNull(LogObject);
        //}

        //[Test]
        //[Category("Functional")]
        //[Category("GetTailLogOnStartedAlgo")]
        //public async Task GetTailLogOnStartedAlgo()
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

        //    url = ApiPaths.ALGO_STORE_ALGO_TAIL_LOG;

        //    Dictionary<string, string> algoIDTailLog = new Dictionary<string, string>()
        //    {
        //        { "AlgoId", AlgoID },
        //        {"Tail" , "60" }
        //    };

        //    var algoIDTailLogResponse = await this.Consumer.ExecuteRequest(url, algoIDTailLog, null, Method.GET);
        //    Assert.True(algoIDTailLogResponse.Status == HttpStatusCode.OK);

        //    LogResponseDTO LogObject = JsonUtils.DeserializeJson<LogResponseDTO>(algoIDTailLogResponse.ResponseJson);

        //    Assert.NotNull(LogObject);
        //}

        //[Test]
        //[Category("Functional")]
        //[Category("GetTailLogOnDeployedOnlyAlgo")]
        //public async Task GetTailLogOnDeployedOnlyAlgo()
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

        //    url = ApiPaths.ALGO_STORE_ALGO_TAIL_LOG;

        //    Dictionary<string, string> algoIDTailLog = new Dictionary<string, string>()
        //    {
        //        { "AlgoId", AlgoID },
        //        {"Tail" , "60" }
        //    };

        //    var algoIDTailLogResponse = await this.Consumer.ExecuteRequest(url, algoIDTailLog, null, Method.GET);
        //    Assert.True(algoIDTailLogResponse.Status == HttpStatusCode.OK);

        //    LogResponseDTO LogObject = JsonUtils.DeserializeJson<LogResponseDTO>(algoIDTailLogResponse.ResponseJson);

        //    Assert.NotNull(LogObject);
        //}

        //[Test]
        //[Category("Functional")]
        //[Category("UploadStringLarge")]
        //public async Task UploadStringLarge()
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
        //[Category("Functional")]
        //[Category("UploadStringEmptyFile")]
        //public async Task UploadStringEmptyFile()
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
        //[Category("Functional")]
        //[Category("GetStringWrongId")]
        //public async Task GetStringWrongId()
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

        //    Dictionary<string, string> quaryParamGetString = new Dictionary<string, string>()
        //    {
        //        {"AlgoId", AlgoId }
        //    };

        //    var responceGetUploadString = await this.Consumer.ExecuteRequest(url, quaryParamGetString, null, Method.GET);
        //    Assert.True(responceGetUploadString.Status == HttpStatusCode.OK);

        //    UploadStringDTO uploadedStringContent = JsonUtils.DeserializeJson<UploadStringDTO>(responceGetUploadString.ResponseJson);

        //    Assert.True(quaryParam["Data"].Equals(uploadedStringContent.Data));
        //}
    }
}
