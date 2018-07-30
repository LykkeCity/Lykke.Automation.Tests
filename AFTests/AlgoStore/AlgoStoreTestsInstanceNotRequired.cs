using AlgoStoreData.DTOs;
using AlgoStoreData.DTOs.InstanceData.Builders;
using AlgoStoreData.Fixtures;
using AlgoStoreData.HelpersAlgoStore;
using NUnit.Framework;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        #region Path Variables

        private String isAlivePath = ApiPaths.ALGO_STORE_IS_ALIVE;

        #endregion

        [Test]
        [Category("AlgoStore")]
        public async Task CheckIfServiceIsAlive()
        {
            var response = await Consumer.ExecuteRequest(isAlivePath, Helpers.EmptyDictionary, null, Method.GET);

            Assert.That(response.Status, Is.EqualTo(HttpStatusCode.OK));
            var baseDate = JsonUtils.DeserializeJson<IsAliveDTO>(response.ResponseJson).Name;
            Assert.That(baseDate, Is.EqualTo("Lykke.AlgoStore.Api"));
        }

        [Test]
        [Category("AlgoStore")]
        [Category("AlgoStoreSmokeTest")]
        [TestCase(AlgoInstanceType.Live)]
        [TestCase(AlgoInstanceType.Demo)]
        //[TestCase(AlgoInstanceType.Test)] // Ignored for now due to issues when creating the function parameters
        public async Task CreateAlgoWithInstanceAndCheckTrades(AlgoInstanceType algoInstanceType)
        {
            // Create algo
            var algoData = await CreateAlgo();

            // Create Live instance
            var instanceData = await SaveInstance(algoData, algoInstanceType, useExistingWallet: false);
            // Wait up to 3 minutes for the instance to start
            await WaitAlgoInstanceToStart(instanceData.InstanceId);

            // Get actual values from statistics endpoint 6 times within 1 minute
            StatisticsDTO statistics = null;
            for (int i = 0; i < 6; i++)
            {
                statistics = await GetStatisticsResponseAsync(instanceData);
            }

            Assert.That(statistics.TotalNumberOfTrades, Is.GreaterThan(0));

            // Get trades fram algo store api
            Dictionary<string, string> algoStoreTradesParams = new Dictionary<string, string>() { { "instanceId", instanceData.InstanceId } };
            var instanceTradesAlgoStoreRequest = await Consumer.ExecuteRequest(ApiPaths.ALGO_STORE_GET_INSTANCE_TRADES, algoStoreTradesParams, null, Method.GET);
            Assert.That(instanceTradesAlgoStoreRequest.Status, Is.EqualTo(HttpStatusCode.OK));
            List<InstanceTradeDTO> instanceTradesAlgoStore = JsonUtils.DeserializeJson<List<InstanceTradeDTO>>(instanceTradesAlgoStoreRequest.ResponseJson);

            // Get trades from DB
            //List<AlgoInstanceTradesEntity> algoInstanceTrades = await AlgoInstanceTradesRepository.GetAllAsync(t => t.PartitionKey.StartsWith(instanceData.InstanceId)) as List<AlgoInstanceTradesEntity>;

            // Assert there is at least one trade for the instance
            Assert.That(instanceTradesAlgoStore.Count, Is.GreaterThan(0));
            Assert.That(instanceTradesAlgoStore.Count, Is.GreaterThanOrEqualTo(statistics.TotalNumberOfTrades));

            // Get trades from HFT api only if instance is Live
            if (algoInstanceType == AlgoInstanceType.Live)
            {
                var hftTradesUrl = $"{BaseUrl.HftApiBaseUrl}{ApiPaths.HFT_HISTORY_TRADES}";

                // Build HFT History trades query params
                Dictionary<string, string> hftHistoryTradeQueryParams = new Dictionary<string, string>
                {
                    { "assetId", instanceParameters.TradedAsset },
                    { "assetPairId", instanceParameters.AssetPair },
                    { "skip", "0" },
                    { "take", "100" }
                };

                var apiKey = (await GetAllWalletsOfUser()).Find(w => w.Id == walletDTO.Id).ApiKey;

                Dictionary<string, string> hftHistoryTradeHeaders = new Dictionary<string, string>
                {
                    { "accept",  "text/plain" },
                    { "api-key", apiKey }
                };

                var instanceTradesHftRequest = await Consumer.ExecuteRequestCustomEndpoint(hftTradesUrl, hftHistoryTradeQueryParams, null, Method.GET, headers: hftHistoryTradeHeaders);
                Assert.That(instanceTradesHftRequest.Status, Is.EqualTo(HttpStatusCode.OK));
                List<InstanceTradeDTO> instanceTradesHFT = JsonUtils.DeserializeJson<List<InstanceTradeDTO>>(instanceTradesAlgoStoreRequest.ResponseJson);

                Assert.That(instanceTradesHFT.Count, Is.GreaterThan(0));
                Assert.That(instanceTradesHFT.Count, Is.GreaterThanOrEqualTo(statistics.TotalNumberOfTrades));

                List<string> algoStoreTradeIds = instanceTradesAlgoStore.Select(x => x.OrderId).ToList();
                List<string> hftTradeIds = instanceTradesHFT.Select(x => x.OrderId).ToList();

                algoStoreTradeIds.Sort();
                hftTradeIds.Sort();

                Assert.That(algoStoreTradeIds, Is.SubsetOf(hftTradeIds));
            }
        }

        [Test, Description("AL-602")]
        [Category("AlgoStore")]
        public async Task CheckGetUserInstances()
        {
            // Get all wallets of user
            List<ClientWalletDataDTO> myWallets = (await GetAllWalletsOfUser()).Select(w => new ClientWalletDataDTO()
            {
                Id = w.Id,
                Name = w.Name
            }).ToList();

            // Create algo
            var algoData = await CreateAlgo();

            // Create Live instance
            var liveInstanceData = await SaveInstance(algoData, AlgoInstanceType.Live);
            // Wait up to 3 minutes for the instance to start
            await WaitAlgoInstanceToStart(liveInstanceData.InstanceId);

            // Create Demo instance
            var demoInstanceData = await SaveInstance(algoData, AlgoInstanceType.Demo);
            // Wait up to 3 minutes for the instance to start
            await WaitAlgoInstanceToStart(demoInstanceData.InstanceId);

            // Create Test instance
            var testInstanceData = await SaveInstance(algoData, AlgoInstanceType.Test);
            // Wait up to 3 minutes for the instance to start
            await WaitAlgoInstanceToStart(testInstanceData.InstanceId);

            // Get userInstances from service
            var myInstances = await Consumer.ExecuteRequest(ApiPaths.ALGO_STORE_GET_ALL_INSTANCES_OF_USER, Helpers.EmptyDictionary, null, Method.GET);
            Assert.That(myInstances.Status, Is.EqualTo(HttpStatusCode.OK));
            List<UserInstanceDTO> actualResult = JsonUtils.DeserializeJson<List<UserInstanceDTO>>(myInstances.ResponseJson);

            // TODO: Get userInstances from DB
            List<ClientInstanceEntity> allUserInstances = await ClientInstanceRepository.GetAllAsync(t => t.PartitionKey == $"client_{algoData.ClientId}") as List<ClientInstanceEntity>;
            var expectedResult = allUserInstances.Select(i => new UserInstanceDTO()
            {
                InstanceId = i.Id,
                AlgoClientId = i.AlgoClientId,
                AlgoId = i.AlgoId,
                InstanceName = i.InstanceName,
                CreateDate = i.AlgoInstanceCreateDate,
                RunDate = i.AlgoInstanceRunDate,
                StopDate = i.AlgoInstanceStopDate,
                InstanceStatus = i.AlgoInstanceStatus,
                Wallet = myWallets.FirstOrDefault(w => w.Id == i.WalletId),
                InstanceType = i.AlgoInstanceType
            }).ToList();

            // Sort actual and expected results by instanceId
            actualResult.Sort((x, y) => x.InstanceId.CompareTo(y.InstanceId));
            expectedResult.Sort((x, y) => x.InstanceId.CompareTo(y.InstanceId));

            // Assert expected and actual results
            Assert.That(actualResult.Count, Is.EqualTo(expectedResult.Count));
            Assert.Multiple(() =>
            {
                for (int i = 0; i < actualResult.Count; i++)
                {
                    Assert.Multiple(() =>
                    {
                        var message = $"Expected: {expectedResult[i]}, but was {actualResult[i]}";
                        Assert.That(actualResult[i].InstanceId, Is.EqualTo(expectedResult[i].InstanceId), message);
                        Assert.That(actualResult[i].AlgoClientId, Is.EqualTo(expectedResult[i].AlgoClientId), message);
                        Assert.That(actualResult[i].AlgoId, Is.EqualTo(expectedResult[i].AlgoId), message);
                        Assert.That(actualResult[i].InstanceName, Is.EqualTo(expectedResult[i].InstanceName), message);
                        Assert.That(actualResult[i].CreateDate, Is.EqualTo(expectedResult[i].CreateDate), message);
                        Assert.That(actualResult[i].RunDate, Is.EqualTo(expectedResult[i].RunDate), message);
                        Assert.That(actualResult[i].StopDate, Is.EqualTo(expectedResult[i].StopDate), message);
                        if (actualResult[i].Wallet == null)
                        {
                            Assert.That(actualResult[i].Wallet, Is.EqualTo(expectedResult[i].Wallet), message);
                        }
                        else
                        {
                            Assert.That(actualResult[i].Wallet.Id, Is.EqualTo(expectedResult[i].Wallet.Id), message);
                            Assert.That(actualResult[i].Wallet.Name, Is.EqualTo(expectedResult[i].Wallet.Name), message);
                        }
                        Assert.That(actualResult[i].InstanceStatus, Is.EqualTo(expectedResult[i].InstanceStatus), message);
                        Assert.That(actualResult[i].InstanceType, Is.EqualTo(expectedResult[i].InstanceType), message);
                    });
                }
            });
        }

        [Ignore("This will most likely be changed in the near future")]
        [Test, Description("AL-523")]
        [Category("AlgoStore")]
        [TestCase(AlgoInstanceType.Test)]
        [TestCase(AlgoInstanceType.Demo)]
        [TestCase(AlgoInstanceType.Live)]
        public async Task CheckHistoryCandlesNeagtiveTests(AlgoInstanceType algoInstanceType)
        {
            // EndOn should be before the Indicator end date
            // StartFrom should be after the Indicator start date
            // StartFrom should be before EndOn
        }

        [Test, Description("AL-523")]
        [Category("AlgoStore")]
        [TestCase(AlgoInstanceType.Test)]
        [TestCase(AlgoInstanceType.Demo)]
        [TestCase(AlgoInstanceType.Live)]
        public async Task CheckHistoryCandles(AlgoInstanceType algoInstanceType)
        {
            int daysBack = -2;

            // Create an algo
            AlgoDataDTO algoData = await CreateAlgo();

            // Start an instance
            var instanceData = await SaveInstance(algoData, algoInstanceType);

            // Wait up to 3 minutes for the instance to start
            await WaitAlgoInstanceToStart(instanceData.InstanceId);

            // Build History Api candles url
            var historyApiCandlesUrl = $"{BaseUrl.AlgoStoreHistoryApiBaseUrl}{ApiPaths.ALGO_STORE_HISTORY_API_CANDLES}";

            // Build params dictionary
            Dictionary<string, string> historyApiCandlesRequestParams = new Dictionary<string, string>
            {
                { "StartFrom", InstanceDataBuilder.FunctionsDictionary[FunctionType.SMA_Short.ToString()].StartingDate.ToString(GlobalConstants.ISO_8601_DATE_FORMAT) },
                { "EndOn", InstanceDataBuilder.FunctionsDictionary[FunctionType.SMA_Long.ToString()].EndingDate.AddDays(daysBack).ToString(GlobalConstants.ISO_8601_DATE_FORMAT) },
                { "IndicatorName", FunctionType.SMA_Short.ToString() }
            };

            // Get instance data from DB
            ClientInstanceEntity instanceDataFromDB = await ClientInstanceRepository.TryGetAsync(t => t.Id == instanceData.InstanceId) as ClientInstanceEntity;

            // Get instance history candles
            var histiryApiCandles = await Consumer.ExecuteRequestCustomEndpoint(historyApiCandlesUrl, historyApiCandlesRequestParams, null, Method.GET, instanceDataFromDB.AuthToken);
            Assert.That(histiryApiCandles.Status, Is.EqualTo(HttpStatusCode.OK));
            List<CandleDTO> actualResult = JsonUtils.DeserializeJson<List<CandleDTO>>(histiryApiCandles.ResponseJson);

            // Build Api v2 candles url
            var apiV2CandlesUrl = $"{BaseUrl.ApiV2BaseUrl}{ApiPaths.API_V2_CANDLES_HISTORY}";

            // Build params dictionary
            Dictionary<string, string> apiV2CandlesRequestParams = new Dictionary<string, string>
            {
                { "Type", "Spot" },
                { "PriceType", "Mid" },
                { "AssetPairId", InstanceDataBuilder.FunctionsDictionary[FunctionType.SMA_Short.ToString()].AssetPair },
                { "TimeInterval", InstanceDataBuilder.FunctionsDictionary[FunctionType.SMA_Short.ToString()].CandleTimeInterval.ToString() },
                { "FromMoment", InstanceDataBuilder.FunctionsDictionary[FunctionType.SMA_Long.ToString()].StartingDate.ToString(GlobalConstants.ISO_8601_DATE_FORMAT) },
                { "ToMoment", InstanceDataBuilder.FunctionsDictionary[FunctionType.SMA_Long.ToString()].EndingDate.AddDays(daysBack).ToString(GlobalConstants.ISO_8601_DATE_FORMAT) }
            };

            // Get expected history candles
            var apiV2Candles = await Consumer.ExecuteRequestCustomEndpoint(apiV2CandlesUrl, apiV2CandlesRequestParams, null, Method.GET);
            Assert.That(apiV2Candles.Status, Is.EqualTo(HttpStatusCode.OK));
            ApiV2HistoryCandles apiV2HistoryCandles = JsonUtils.DeserializeJson<ApiV2HistoryCandles>(apiV2Candles.ResponseJson);

            // Get expected result
            List<CandleDTO> expectedResult = apiV2HistoryCandles.History.Select(c => new CandleDTO()
            {
                DateTime = c.DateTime,
                Open = c.Open,
                Close = c.Close,
                High = c.High,
                Low = c.Low
            }).ToList();

            // Sort actual and expected result by DateTime
            actualResult.Sort((x, y) => x.DateTime.CompareTo(y.DateTime));
            expectedResult.Sort((x, y) => x.DateTime.CompareTo(y.DateTime));

            // Assert actual result equals expected result
            Assert.That(actualResult.Count, Is.EqualTo(expectedResult.Count));
            Assert.Multiple(() =>
            {
                for (int i = 0; i < actualResult.Count; i++)
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(actualResult[i].DateTime, Is.EqualTo(expectedResult[i].DateTime));
                        Assert.That(actualResult[i].Open, Is.EqualTo(expectedResult[i].Open));
                        Assert.That(actualResult[i].Close, Is.EqualTo(expectedResult[i].Close));
                        Assert.That(actualResult[i].High, Is.EqualTo(expectedResult[i].High));
                        Assert.That(actualResult[i].Low, Is.EqualTo(expectedResult[i].Low));
                    });
                }
            });
        }

        [Test, Description("AL-578")]
        [Category("AlgoStore")]
        [TestCase("Lykke.AlgoStore.CSharp.Algo.Implemention.ExecutableClass.Test")]
        [TestCase("Lykke.AlgoStore.CSharp.Algo.Implemention.ExecutableClassTest")]
        [TestCase("Lykke.AlgoStore.CSharp.Algo.Implemention.Test")]
        [TestCase("Lykke.AlgoStore.CSharp.Algo")]
        [TestCase("Lykke.AlgoStore.CSharp.Algo.ExecutableClass")]
        [TestCase(null)]
        [TestCase("")]
        [TestCase(" ")]
        [TestCase("     ")]
        [TestCase("Lykke.AlgoStore.CSharp.Algo.Implemention..ExecutableClass")]
        public async Task CheckInvalidNamespaces(string invalidNamespace)
        {
            // Create timestamp that will be used in algo name
            var algoCreationTimestamp = Helpers.GetTimestampIso8601();
            // Replace the default namespace with an invalid one
            var algoString = DummyAlgoString.Replace(GlobalConstants.AlgoDefaultNamespace, invalidNamespace);

            // Create algo object
            CreateAlgoDTO algoData = new CreateAlgoDTO()
            {
                Name = $"{algoCreationTimestamp}{GlobalConstants.AutoTest}_AlgoName",
                Description = $"{algoCreationTimestamp}{GlobalConstants.AutoTest}_AlgoName - Description",
                Content = Base64Helpers.EncodeToBase64(algoString)
            };

            // Create algo
            var createAlgoResponse = await Consumer.ExecuteRequest(ApiPaths.ALGO_STORE_CREATE_ALGO, Helpers.EmptyDictionary, JsonUtils.SerializeObject(algoData), Method.POST);
            var message = $"POST {ApiPaths.ALGO_STORE_CREATE_ALGO} returned status: {createAlgoResponse.Status} and response: {createAlgoResponse.ResponseJson}. Expected: {HttpStatusCode.BadRequest}";

            // Get user algos
            var userAlgos = (await GetUserAlgos()).Select(a => a.Name).ToList();

            // Assert algo is not created
            Assert.Multiple(() =>
            {
                Assert.That(createAlgoResponse.Status, Is.EqualTo(HttpStatusCode.BadRequest), message);
                Assert.That(createAlgoResponse.ResponseJson, Does.Contain("The provided namespace is not allowed").Or.Contains("Identifier expected"));
                Assert.That(userAlgos, Does.Not.Contain(algoData.Name));
            });
        }

        [Test, Description("AL-690")]
        [Category("AlgoStore")]
        [TestCase("OnStartUp", "The instance is being stopped because OnStartUp took too long to execute.")]
        [TestCase("OnQuoteReceived", "The instance is being stopped because OnQuoteReceived took too long to execute.")]
        [TestCase("OnCandleReceived", "The instance is being stopped because OnCandleReceived took too long to execute.")]
        // TODO: Add test cases for the other algos as well
        public async Task CheckMonitoringService(string whilePosition, string message)
        {
            // Set algoString
            var algoString = File.ReadAllText(String.Format(DummyAlgoWhileLoop, whilePosition));

            // Create algo
            AlgoDataDTO algoData = await CreateAlgo(algoString);

            // Start an instance
            var instanceData = await SaveInstance(algoData, AlgoInstanceType.Demo);

            // Wait up to 3 minutes for the instance to start
            await WaitAlgoInstanceToStart(instanceData.InstanceId);

            // Wait for up to 5 minutes so that the algo can be stopped
            int maxWaitTime = 5 * 60 * 1000; // 5 minutes
            int waitTime = 5 * 1000; // 5 seconds

            while (maxWaitTime > 0)
            {
                Wait.ForPredefinedTime(waitTime);
                maxWaitTime -= waitTime;

                var instanceStatus = await GetInstanceStatus(instanceData.InstanceId);

                if (instanceStatus == AlgoInstanceStatus.Stopped)
                {
                    break;
                }
            }

            // Get instance log
            var instanceLog = await GetInstanceTailLogFromLoggingService(postInstanceData);
            var instanceMessages = instanceLog.Select(x => x.Message).ToList();

            // Assert message added to log
            Assert.That(instanceMessages, Does.Contain(message));
        }
    }
}
