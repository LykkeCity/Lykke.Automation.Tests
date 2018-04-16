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
        public async Task PostInvalidInstanceAssetPair()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();
            string algoID = metadataForUploadedBinary.AlgoId;

            instanceForAlgo = GetPopulatedInstanceDataDTO.returnInstanceDataDTOInvalidAssetPair(algoID);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            AlgoErrorDTO postInstanceDataResponseDTO = JsonUtils.DeserializeJson<AlgoErrorDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.InternalServerError), "responce should equals internal server erorr");

            Assert.That(postInstanceDataResponseDTO.ErrorMessage, Does.Contain("NotFound from asset service calling AssetPairGetWithHttpMessagesAsync"), "we should receive erorr for not found asset pair");

 
        }
        [Test]
        [Category("AlgoStore")]
        public async Task PostInvalidInstanceTradedAsset()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();
            string algoID = metadataForUploadedBinary.AlgoId;

            instanceForAlgo = GetPopulatedInstanceDataDTO.returnInstanceDataDTOInvalidTradedAsset(algoID);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            AlgoErrorDTO postInstanceDataResponseDTO = JsonUtils.DeserializeJson<AlgoErrorDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.BadRequest), "should be bad response erorr code");

            Assert.That(postInstanceDataResponseDTO.ErrorMessage, Does.Contain("ValidationError Message:Asset <USD> is not valid for asset pair <BTCEUR>"), "we should receive erorr for the invalid traded asset");

        }
        [Test]
        [Category("AlgoStore")]
        public async Task PostInvalidAlgoId()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();
            string algoID = metadataForUploadedBinary.AlgoId;

            instanceForAlgo = GetPopulatedInstanceDataDTO.returnInstanceDataDTO("123 invalid algo id");

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            Assert.That(postInstanceDataResponse.Status , Is.EqualTo(HttpStatusCode.NotFound), "we should receive not found response code");

        }
        [Test]
        [Category("AlgoStore")]
        public async Task PostInvalidVolume()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();
            string algoID = metadataForUploadedBinary.AlgoId;

            instanceForAlgo = GetPopulatedInstanceDataDTO.returnInstanceDataDTONegativeVolume(algoID);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            AlgoErrorDTO postInstanceDataResponseDTO = JsonUtils.DeserializeJson<AlgoErrorDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.BadRequest), "we should receive bad request response code");

            Assert.That(postInstanceDataResponseDTO.ErrorMessage, Does.Contain("Code:1000-ValidationError Message"), "we should receive validation erorr for invalid volume");
        }

        [Test]
        [Category("AlgoStore")]
        public async Task PostInstanceDataOnlyWithMetadata()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();

            string algoID = metadataForUploadedBinary.AlgoId;

            instanceForAlgo = GetPopulatedInstanceDataDTO.returnInstanceDataDTO(algoID);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.OK), "we shoudl recieve ok responce" );
        }
        [Test]
        [Category("AlgoStore")]
        public async Task PostInstanceDataOnlyWithZeroVolume()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();
            string algoID = metadataForUploadedBinary.AlgoId;

            instanceForAlgo = GetPopulatedInstanceDataDTO.returnInstanceDataDTOInvalidVolume(algoID);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            AlgoErrorDTO postInstanceDataResponseDTO = JsonUtils.DeserializeJson<AlgoErrorDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.BadRequest), "we should receive bad request response code");

            Assert.That(postInstanceDataResponseDTO.ErrorMessage, Does.Contain("Code:1000-ValidationError Message"), "we should receive validation erorr for invalid volume");
        }

    }
}