using AlgoStoreData.DTOs;
using AlgoStoreData.DTOs.InstanceData;
using AlgoStoreData.DTOs.InstanceData.Builders;
using AlgoStoreData.Fixtures;
using ApiV2Data.DTOs;
using NUnit.Framework;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using XUnitTestData.Enums;

namespace AFTests.AlgoStore
{
    [Category("FullRegression")]
    [Category("AlgoStore")]
    public partial class AlgoStoreTestsInstanceNotRequired : AlgoStoreTestDataFixture
    {
        [Test]
        [Category("AlgoStore")]
        [TestCase(AlgoInstanceType.Live)]
        [TestCase(AlgoInstanceType.Demo)]
        [TestCase(AlgoInstanceType.Test)]
        public async Task PostInvalidInstanceAssetPair(AlgoInstanceType algoInstanceType)
        {
            WalletDTO walletDTO = null;
            if (algoInstanceType == AlgoInstanceType.Live)
            {
                walletDTO = await GetExistingWallet();
            }

            // Create algo
            var algoData = await CreateAlgo();

            // Build days offset
            DaysOffsetDTO daysOffsetDTO = BuildDaysOffsetByInstanceType(algoInstanceType);
            // Build InstanceParameters
            InstanceParameters instanceParameters = new InstanceParameters()
            {
                AssetPair = "BTCBTC",
                TradedAsset = "USD",
                InstanceTradeVolume = 2,
                InstanceCandleInterval = CandleTimeInterval.Minute,
                FunctionCandleInterval = CandleTimeInterval.Day,
                FunctionCandleOperationMode = CandleOperationMode.CLOSE,
                FunctionCapacity = 4,
                InstanceFunctions = new List<FunctionType>() { FunctionType.SMA_Short, FunctionType.SMA_Long }
            };

            // Build instance request payload
            var instanceForAlgo = InstanceDataBuilder.BuildInstanceData(algoData, walletDTO, algoInstanceType, instanceParameters, daysOffsetDTO);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            AlgoErrorDTO postInstanceDataResponseDTO = JsonUtils.DeserializeJson<AlgoErrorDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.InternalServerError), "responce should equals internal server erorr");

            Assert.That(postInstanceDataResponseDTO.ErrorMessage, Does.Contain("NotFound from asset service calling AssetPairGetWithHttpMessagesAsync"), "we should receive erorr for not found asset pair");

 
        }

        [Test]
        [Category("AlgoStore")]
        [TestCase(AlgoInstanceType.Live)]
        [TestCase(AlgoInstanceType.Demo)]
        [TestCase(AlgoInstanceType.Test)]
        public async Task PostInvalidInstanceTradedAsset(AlgoInstanceType algoInstanceType)
        {
            WalletDTO walletDTO = null;
            if (algoInstanceType == AlgoInstanceType.Live)
            {
                walletDTO = await GetExistingWallet();
            }

            // Create algo
            var algoData = await CreateAlgo();

            // Build days offset
            DaysOffsetDTO daysOffsetDTO = BuildDaysOffsetByInstanceType(algoInstanceType);
            // Build InstanceParameters
            InstanceParameters instanceParameters = new InstanceParameters()
            {
                AssetPair = "BTCUSD",
                TradedAsset = "EUR",
                InstanceTradeVolume = 2,
                InstanceCandleInterval = CandleTimeInterval.Minute,
                FunctionCandleInterval = CandleTimeInterval.Day,
                FunctionCandleOperationMode = CandleOperationMode.CLOSE,
                FunctionCapacity = 4,
                InstanceFunctions = new List<FunctionType>() { FunctionType.SMA_Short, FunctionType.SMA_Long }
            };

            // Build instance request payload
            var instanceForAlgo = InstanceDataBuilder.BuildInstanceData(algoData, walletDTO, algoInstanceType, instanceParameters, daysOffsetDTO);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            AlgoErrorDTO postInstanceDataResponseDTO = JsonUtils.DeserializeJson<AlgoErrorDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.BadRequest), "should be bad response erorr code");

            Assert.That(postInstanceDataResponseDTO.ErrorMessage, Does.Contain("ValidationError Message:Asset <USD> is not valid for asset pair <BTCEUR>"), "we should receive erorr for the invalid traded asset");
        }

        [Test]
        [Category("AlgoStore")]
        [TestCase(AlgoInstanceType.Live)]
        [TestCase(AlgoInstanceType.Demo)]
        [TestCase(AlgoInstanceType.Test)]
        public async Task PostInvalidAlgoId(AlgoInstanceType algoInstanceType)
        {
            WalletDTO walletDTO = null;
            if (algoInstanceType == AlgoInstanceType.Live)
            {
                walletDTO = await GetExistingWallet();
            }

            // Create algo
            var algoData = await CreateAlgo();

            // Build days offset
            DaysOffsetDTO daysOffsetDTO = BuildDaysOffsetByInstanceType(algoInstanceType);
            // Build InstanceParameters
            InstanceParameters instanceParameters = new InstanceParameters()
            {
                AssetPair = "BTCUSD",
                TradedAsset = "EUR",
                InstanceTradeVolume = 2,
                InstanceCandleInterval = CandleTimeInterval.Minute,
                FunctionCandleInterval = CandleTimeInterval.Day,
                FunctionCandleOperationMode = CandleOperationMode.CLOSE,
                FunctionCapacity = 4,
                InstanceFunctions = new List<FunctionType>() { FunctionType.SMA_Short, FunctionType.SMA_Long }
            };

            // Build instance request payload
            var instanceForAlgo = InstanceDataBuilder.BuildInstanceData(algoData, walletDTO, algoInstanceType, instanceParameters, daysOffsetDTO, true);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            Assert.That(postInstanceDataResponse.Status , Is.EqualTo(HttpStatusCode.NotFound), "we should receive not found response code");
        }

        [Test]
        [Category("AlgoStore")]
        [TestCase(AlgoInstanceType.Live)]
        [TestCase(AlgoInstanceType.Demo)]
        [TestCase(AlgoInstanceType.Test)]
        public async Task PostInvalidVolume(AlgoInstanceType algoInstanceType)
        {
            WalletDTO walletDTO = null;
            if (algoInstanceType == AlgoInstanceType.Live)
            {
                walletDTO = await GetExistingWallet();
            }

            // Create algo
            var algoData = await CreateAlgo();

            // Build days offset
            DaysOffsetDTO daysOffsetDTO = BuildDaysOffsetByInstanceType(algoInstanceType);
            // Build InstanceParameters
            InstanceParameters instanceParameters = new InstanceParameters()
            {
                AssetPair = "BTCUSD",
                TradedAsset = "EUR",
                InstanceTradeVolume = -2,
                InstanceCandleInterval = CandleTimeInterval.Minute,
                FunctionCandleInterval = CandleTimeInterval.Day,
                FunctionCandleOperationMode = CandleOperationMode.CLOSE,
                FunctionCapacity = 4,
                InstanceFunctions = new List<FunctionType>() { FunctionType.SMA_Short, FunctionType.SMA_Long }
            };

            // Build instance request payload
            var instanceForAlgo = InstanceDataBuilder.BuildInstanceData(algoData, walletDTO, algoInstanceType, instanceParameters, daysOffsetDTO);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            AlgoErrorDTO postInstanceDataResponseDTO = JsonUtils.DeserializeJson<AlgoErrorDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.BadRequest), "we should receive bad request response code");

            Assert.That(postInstanceDataResponseDTO.ErrorMessage, Does.Contain("Code:1000-ValidationError Message"), "we should receive validation erorr for invalid volume");
        }

        [Test]
        [Category("AlgoStore")]
        [TestCase(AlgoInstanceType.Live)]
        [TestCase(AlgoInstanceType.Demo)]
        [TestCase(AlgoInstanceType.Test)]
        public async Task PostInstanceDataOnlyWithZeroVolume(AlgoInstanceType algoInstanceType)
        {
            WalletDTO walletDTO = null;
            if (algoInstanceType == AlgoInstanceType.Live)
            {
                walletDTO = await GetExistingWallet();
            }

            // Create algo
            var algoData = await CreateAlgo();

            // Build days offset
            DaysOffsetDTO daysOffsetDTO = BuildDaysOffsetByInstanceType(algoInstanceType);
            // Build InstanceParameters
            InstanceParameters instanceParameters = new InstanceParameters()
            {
                AssetPair = "BTCUSD",
                TradedAsset = "EUR",
                InstanceTradeVolume = 0,
                InstanceCandleInterval = CandleTimeInterval.Minute,
                FunctionCandleInterval = CandleTimeInterval.Day,
                FunctionCandleOperationMode = CandleOperationMode.CLOSE,
                FunctionCapacity = 4,
                InstanceFunctions = new List<FunctionType>() { FunctionType.SMA_Short, FunctionType.SMA_Long }
            };

            // Build instance request payload
            var instanceForAlgo = InstanceDataBuilder.BuildInstanceData(algoData, walletDTO, algoInstanceType, instanceParameters, daysOffsetDTO);

            string url = ApiPaths.ALGO_STORE_ALGO_INSTANCE_DATA;

            var postInstanceDataResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            AlgoErrorDTO postInstanceDataResponseDTO = JsonUtils.DeserializeJson<AlgoErrorDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.BadRequest), "we should receive bad request response code");

            Assert.That(postInstanceDataResponseDTO.ErrorMessage, Does.Contain("Code:1000-ValidationError Message"), "we should receive validation erorr for invalid volume");
        }

    }
}