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
        public async Task PostInvalidInstanceTradedAsset()
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
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Volume), (int)Convert.ToDouble(instanceForAlgo.Volume));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Margin), (int)Convert.ToDouble(instanceForAlgo.Margin));
            Assert.NotNull(postInstanceData.InstanceId);

            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);
        }
        [Test]
        [Category("AlgoStore")]
        public async Task PostInvalidInstanceAssetPair()
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
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Volume), (int)Convert.ToDouble(instanceForAlgo.Volume));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Margin), (int)Convert.ToDouble(instanceForAlgo.Margin));
            Assert.NotNull(postInstanceData.InstanceId);

            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);
        }
        [Test]
        [Category("AlgoStore")]
        public async Task PostInvalidAlgoId()
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
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Volume), (int)Convert.ToDouble(instanceForAlgo.Volume));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Margin), (int)Convert.ToDouble(instanceForAlgo.Margin));
            Assert.NotNull(postInstanceData.InstanceId);

            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);
        }
        [Test]
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
                Margin = "1",
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
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Volume), (int)Convert.ToDouble(instanceForAlgo.Volume));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Margin), (int)Convert.ToDouble(instanceForAlgo.Margin));
            Assert.NotNull(postInstanceData.InstanceId);

            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);
        }
        [Test]
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
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Volume), (int)Convert.ToDouble(instanceForAlgo.Volume));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Margin), (int)Convert.ToDouble(instanceForAlgo.Margin));
            Assert.NotNull(postInstanceData.InstanceId);

            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);
        }

        [Test]
        [Category("AlgoStore")]
        public async Task PostInstanceDataOnlyWithMetadata()
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
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Volume), (int)Convert.ToDouble(instanceForAlgo.Volume));
            Assert.AreEqual((int)Convert.ToDouble(postInstanceData.Margin), (int)Convert.ToDouble(instanceForAlgo.Margin));
            Assert.NotNull(postInstanceData.InstanceId);

            ClientInstanceEntity instanceDataEntityExists = await ClientInstanceRepository.TryGetAsync(t => t.Id == postInstanceData.InstanceId) as ClientInstanceEntity;
            Assert.NotNull(instanceDataEntityExists);
        }

    }
}