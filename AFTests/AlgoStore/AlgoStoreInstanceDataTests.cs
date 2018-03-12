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
    [Ignore("rewokr logic")]
    [Category("AlgoStore")]
    public partial class AlgoStoreTests : AlgoStoreTestDataFixture
    {
        [Test]
        [Category("AlgoStore")]
        public async Task PostInvalidInstanceAssetPair()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();

            string algoID = metadataForUploadedBinary.AlgoId;


            //rework logic new params
            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = algoID,
                HftApiKey = "key",
                AssetPair = "BTCcoin",
                TradedAsset = "USD",
                Margin = "1",
                Volume = "1",
                WalletId = "2134"
            };

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

            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = algoID,
                HftApiKey = "key",
                AssetPair = "BTCUSD",
                TradedAsset = "BTC",
                Margin = "1",
                Volume = "1",
                WalletId = "2134"
            };

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.BadRequest);

        }
        [Test]
        [Ignore("rewokr logic")]
        [Category("AlgoStore")]
        public async Task PostInvalidAlgoId()
        {
            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = "Invalid Id",
                HftApiKey = "key",
                AssetPair = "BTCUSD",
                TradedAsset = "USD",
                Margin = "1",
                Volume = "1",
                WalletId = "2134"
            };

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

            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = algoID,
                HftApiKey = "key",
                AssetPair = "BTCUSD",
                TradedAsset = "USD",
                Margin = "-111",
                Volume = "1",
                WalletId = "2134"
            };

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.BadRequest);

        }
        [Test]
        [Ignore("rewokr logic")]
        [Category("AlgoStore")]
        public async Task PostInvalidVolume()
        {
            UploadStringDTO metadataForUploadedBinary = await UploadStringAlgo();

            string algoID = metadataForUploadedBinary.AlgoId;

            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = algoID,
                HftApiKey = "key",
                AssetPair = "BTCUSD",
                TradedAsset = "USD",
                Margin = "1",
                Volume = "-333",
                WalletId = "2134"
            };

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.BadRequest);
        }

        [Test]
        [Ignore("rewokr logic")]
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

            InstanceDataDTO instanceForAlgo = new InstanceDataDTO()
            {
                AlgoId = algoID,
                HftApiKey = "key",
                AssetPair = "BTCUSD",
                TradedAsset = "USD",
                Margin = "0",
                Volume = "0",
                WalletId = "2134"
            };

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);
            Assert.That(postInstanceDataResponse.Status == HttpStatusCode.BadRequest);
        }

    }
}