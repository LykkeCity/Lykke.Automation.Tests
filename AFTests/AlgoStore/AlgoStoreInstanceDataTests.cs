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
        [Ignore("rewokr logic")]
        [Category("AlgoStore")]
        public async Task PostInvalidInstanceAssetPair()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();
            string algoID = metadataForUploadedBinary.AlgoId;

            GetPopulatedInstanceDataDTO getinstanceAlgo = new GetPopulatedInstanceDataDTO();

            InstanceDataDTO instanceForAlgo = getinstanceAlgo.returnInstanceDataDTO(algoID);

            // set custrom data here for the metadata asset pair

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.NotFound);
        }
        [Test]
        [Ignore("rewokr logic")]
        [Category("AlgoStore")]
        public async Task PostInvalidInstanceTradedAsset()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();
            string algoID = metadataForUploadedBinary.AlgoId;

            GetPopulatedInstanceDataDTO getinstanceAlgo = new GetPopulatedInstanceDataDTO();

            InstanceDataDTO instanceForAlgo = getinstanceAlgo.returnInstanceDataDTO(algoID);

            // set custrom data here for the metadata traded data

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.NotFound);

        }
        [Test]
        [Category("AlgoStore")]
        public async Task PostInvalidAlgoId()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();
            string algoID = metadataForUploadedBinary.AlgoId;

            GetPopulatedInstanceDataDTO getinstanceAlgo = new GetPopulatedInstanceDataDTO();

            InstanceDataDTO instanceForAlgo = getinstanceAlgo.returnInstanceDataDTO("123 invalid algo id");

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.NotFound);

        }
        [Test]
        [Ignore("Bug AL-274")]
        [Category("AlgoStore")]
        public async Task PostInvalidMargin()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();
            string algoID = metadataForUploadedBinary.AlgoId;

            GetPopulatedInstanceDataDTO getinstanceAlgo = new GetPopulatedInstanceDataDTO();

            InstanceDataDTO instanceForAlgo = getinstanceAlgo.returnInstanceDataDTO(algoID);

            // set custrom data here for the metadata margin data

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.NotFound);

        }
        [Test]
        [Ignore("rewokr logic")]
        [Category("AlgoStore")]
        public async Task PostInvalidVolume()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();
            string algoID = metadataForUploadedBinary.AlgoId;

            GetPopulatedInstanceDataDTO getinstanceAlgo = new GetPopulatedInstanceDataDTO();

            InstanceDataDTO instanceForAlgo = getinstanceAlgo.returnInstanceDataDTO(algoID);

            // set custrom data here for the metadata volume data

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.NotFound);
        }

        [Test]
        [Category("AlgoStore")]
        public async Task PostInstanceDataOnlyWithMetadata()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();

            string algoID = metadataForUploadedBinary.AlgoId;

            GetPopulatedInstanceDataDTO getinstanceAlgo = new GetPopulatedInstanceDataDTO();

            InstanceDataDTO instanceForAlgo = getinstanceAlgo.returnInstanceDataDTO(algoID);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.OK);
        }

        [Test]
        [Ignore("rewokr logic")]
        [Category("AlgoStore")]
        public async Task PostInstanceDataOnlyWithZeroMArginZeroVolume()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();
            string algoID = metadataForUploadedBinary.AlgoId;

            GetPopulatedInstanceDataDTO getinstanceAlgo = new GetPopulatedInstanceDataDTO();

            InstanceDataDTO instanceForAlgo = getinstanceAlgo.returnInstanceDataDTO(algoID);

            // set custrom data here for the metadata traded 0 margin 0 volume

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.NotFound);
        }

    }
}