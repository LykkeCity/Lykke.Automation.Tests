using AlgoStoreData.DTOs;
using AlgoStoreData.DTOs.InstanceData;
using AlgoStoreData.DTOs.InstanceData.Builders;
using AlgoStoreData.Fixtures;
using ApiV2Data.DTOs;
using NUnit.Framework;
using RestSharp;
using System.Net;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using XUnitTestData.Entities.AlgoStore;
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
            InstanceParameters instanceParameters = InstanceConfig.InvalidInstanceAssetPair;

            // Build instance request payload
            var instanceForAlgo = InstanceDataBuilder.BuildInstanceData(algoData, walletDTO, algoInstanceType, instanceParameters, daysOffsetDTO);

            var url = algoInstanceType == AlgoInstanceType.Live ? ApiPaths.ALGO_STORE_SAVE_ALGO_INSTANCE : ApiPaths.ALGO_STORE_FAKE_TRADING_INSTANCE_DATA;

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
            InstanceParameters instanceParameters = InstanceConfig.InvalidInstanceTradedAsset;

            // Build instance request payload
            var instanceForAlgo = InstanceDataBuilder.BuildInstanceData(algoData, walletDTO, algoInstanceType, instanceParameters, daysOffsetDTO);

            var url = algoInstanceType == AlgoInstanceType.Live ? ApiPaths.ALGO_STORE_SAVE_ALGO_INSTANCE : ApiPaths.ALGO_STORE_FAKE_TRADING_INSTANCE_DATA;

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
            InstanceParameters instanceParameters = InstanceConfig.UseInvalidAlgoId;

            // Build instance request payload
            var instanceForAlgo = InstanceDataBuilder.BuildInstanceData(algoData, walletDTO, algoInstanceType, instanceParameters, daysOffsetDTO);

            var url = algoInstanceType == AlgoInstanceType.Live ? ApiPaths.ALGO_STORE_SAVE_ALGO_INSTANCE : ApiPaths.ALGO_STORE_FAKE_TRADING_INSTANCE_DATA;

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
            InstanceParameters instanceParameters = InstanceConfig.NegativeTradeVolume;

            // Build instance request payload
            var instanceForAlgo = InstanceDataBuilder.BuildInstanceData(algoData, walletDTO, algoInstanceType, instanceParameters, daysOffsetDTO);

            var url = algoInstanceType == AlgoInstanceType.Live ? ApiPaths.ALGO_STORE_SAVE_ALGO_INSTANCE : ApiPaths.ALGO_STORE_FAKE_TRADING_INSTANCE_DATA;

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
            InstanceParameters instanceParameters = InstanceConfig.ZeroTradedVolume;

            // Build instance request payload
            var instanceForAlgo = InstanceDataBuilder.BuildInstanceData(algoData, walletDTO, algoInstanceType, instanceParameters, daysOffsetDTO);

            // Build save instance url
            var url = algoInstanceType == AlgoInstanceType.Live ? ApiPaths.ALGO_STORE_SAVE_ALGO_INSTANCE : ApiPaths.ALGO_STORE_FAKE_TRADING_INSTANCE_DATA;

            var postInstanceDataResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            AlgoErrorDTO postInstanceDataResponseDTO = JsonUtils.DeserializeJson<AlgoErrorDTO>(postInstanceDataResponse.ResponseJson);

            Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.BadRequest), "we should receive bad request response code");

            Assert.That(postInstanceDataResponseDTO.ErrorMessage, Does.Contain("Code:1000-ValidationError Message"), "we should receive validation erorr for invalid volume");
        }

        // List instance is not included since there is no problem for it
        [Test, Description("AL-694")]
        [Category("AlgoStore")]
        [TestCase(AlgoInstanceType.Demo)]
        [TestCase(AlgoInstanceType.Test)]
        public async Task BackTestWithLongFunctionPeriod(AlgoInstanceType algoInstanceType)
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

            // Set Instance start date to be three years ago and end date to be 10 days ago
            int start = 365 * -3;
            int end = -10;
            daysOffsetDTO.AlgoStartOffset = start;
            daysOffsetDTO.AlgoEndOffset = end;
            daysOffsetDTO.SmaShortStartOffset = start;
            daysOffsetDTO.SmaShortEndOffset = end;
            daysOffsetDTO.SmaShortStartOffset = start;
            daysOffsetDTO.SmaLongEndOffset = end;

            // Build InstanceParameters
            InstanceParameters instanceParameters = InstanceConfig.ValidMetaData;

            // Build instance request payload
            var instanceForAlgo = InstanceDataBuilder.BuildInstanceData(algoData, walletDTO, algoInstanceType, instanceParameters, daysOffsetDTO);

            // Build save instance url
            var url = algoInstanceType == AlgoInstanceType.Live ? ApiPaths.ALGO_STORE_SAVE_ALGO_INSTANCE : ApiPaths.ALGO_STORE_FAKE_TRADING_INSTANCE_DATA;

            var postInstanceDataResponse = await Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, JsonUtils.SerializeObject(instanceForAlgo), Method.POST);

            AlgoErrorDTO postInstanceDataResponseDTO = JsonUtils.DeserializeJson<AlgoErrorDTO>(postInstanceDataResponse.ResponseJson);

            // Get instance data from DB
            ClientInstanceEntity instanceDataFromDB = await ClientInstanceRepository.TryGetAsync(t => t.AlgoId == algoData.Id) as ClientInstanceEntity;

            Assert.Multiple(() => 
            {
                Assert.That(postInstanceDataResponse.Status, Is.EqualTo(HttpStatusCode.InternalServerError));
                Assert.That(postInstanceDataResponseDTO.ErrorMessage, Does.StartWith("Code:511-InitialWalletBalanceNotCalculated Message:Initial wallet balance could not be calculated. Could not get history price for"));
                // Verify intance is not created in DB
                Assert.That(instanceDataFromDB, Is.Null);
            }) ;
        }
    }
}