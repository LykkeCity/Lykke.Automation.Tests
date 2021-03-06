﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.DTOs.RabbitMQ;
using Autofac;
using XUnitTestCommon.Utils;
using XUnitTestCommon.Consumers.Models;
using MatchingEngineData.DependencyInjection;
using XUnitTestData.Domains;
using XUnitTestData.Repositories;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using MatchingEngineData.DTOs.RabbitMQ;
using XUnitTestData.Domains.MatchingEngine;
using XUnitTestData.Domains.Assets;
using MatchingEngineData;
using XUnitTestData.Entities.MatchingEngine;
using XUnitTestData.Entities;
using XUnitTestData.Entities.Assets;
using NUnit.Framework;
using XUnitTestCommon.Tests;
using XUnitTestCommon.GlobalActions;
using XUnitTestCommon.Settings.AutomatedFunctionalTests;
using Lykke.SettingsReader;
using XUnitTestCommon.Settings;

namespace AFTMatchingEngine.Fixtures
{
    [TestFixture]
    public class MatchingEngineTestDataFixture : BaseTest
    {
        public MatchingEngineConsumer Consumer;

        public GenericRepository<AccountEntity, IAccount> AccountRepository;
        public GenericRepository<CashSwapEntity, ICashSwap> CashSwapRepository;
        public GenericRepository<MarketOrderEntity, IMarketOrderEntity> MarketOrdersRepository;
        public GenericRepository<LimitOrderEntity, ILimitOrderEntity> LimitOrdersRepository;

        private GenericRepository<AssetPairEntity, IAssetPair> AssetPairsRepository;

        public string TestAccountId1;
        public string TestAccountId2;
        public string TestAsset1;
        public string TestAsset2;

        public int AssetPrecission;

        public AssetPairEntity TestAssetPair;

        private Dictionary<Type, List<IRabbitMQOperation>> RabbitMqMessages;

        private int waitForRabbitMQMessage;

        private RabbitMQConsumer<CashOperation> CashInOutSubscription;
        private RabbitMQConsumer<CashTransferOperation> CashTransferSubscription;
        private RabbitMQConsumer<CashSwapOperation> CashSwapSubscription;
        private RabbitMQConsumer<BalanceUpdate> BalanceUpdateSubscription;
        private RabbitMQConsumer<LimitOrdersResponse> LimitOrderSubscription;
        private RabbitMQConsumer<MarketOrderWithTrades> TradesOrderSubscription;

        private IReloadingManager<AppSettings> _configBuilder;
        private MatchingEngineSettings _matchingEngineSettings;

        private List<string> _createdQueues;
        private IContainer container;

        [OneTimeSetUp]
        public void Initialize()
        {
            _configBuilder = new ConfigBuilder().ReloadingManager;
            _matchingEngineSettings = _configBuilder.CurrentValue.AutomatedFunctionalTests.MatchingEngine;
            prepareConsumer();
            prepareRabbitQueues();
            prepareRabbitMQConnections();
            prepareDependencyContainer();

            prepareTestData().Wait();
        }

        private void prepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new MatchingEngineTestModule(_configBuilder));
            this.container = builder.Build();

            this.AccountRepository = RepositoryUtils.ResolveGenericRepository<AccountEntity, IAccount>(this.container);
            this.CashSwapRepository = RepositoryUtils.ResolveGenericRepository<CashSwapEntity, ICashSwap>(this.container);
            this.AssetPairsRepository = RepositoryUtils.ResolveGenericRepository<AssetPairEntity, IAssetPair>(this.container);
            this.MarketOrdersRepository = RepositoryUtils.ResolveGenericRepository<MarketOrderEntity, IMarketOrderEntity>(this.container);
            this.LimitOrdersRepository = RepositoryUtils.ResolveGenericRepository<LimitOrderEntity, ILimitOrderEntity>(this.container);
        }

        private void prepareRabbitMQConnections()
        {
            RabbitMqMessages = new Dictionary<Type, List<IRabbitMQOperation>>();

            CashInOutSubscription = new RabbitMQConsumer<CashOperation>(
                new RabbitMQSettings(_matchingEngineSettings, "cashinout", Constants.TestQueueName), handleOperationMessages);

            CashTransferSubscription = new RabbitMQConsumer<CashTransferOperation>(
                new RabbitMQSettings(_matchingEngineSettings, "transfers", Constants.TestQueueName), handleOperationMessages);

            CashSwapSubscription = new RabbitMQConsumer<CashSwapOperation>(
                new RabbitMQSettings(_matchingEngineSettings, "cashswap", Constants.TestQueueName), handleOperationMessages);

            BalanceUpdateSubscription = new RabbitMQConsumer<BalanceUpdate>(
                new RabbitMQSettings(_matchingEngineSettings, "balanceupdate", Constants.TestQueueName), handleOperationMessages);

            LimitOrderSubscription = new RabbitMQConsumer<LimitOrdersResponse>(
                new RabbitMQSettings(_matchingEngineSettings, "limitorders.clients", Constants.TestQueueName), handleOperationMessages);

            TradesOrderSubscription = new RabbitMQConsumer<MarketOrderWithTrades>(
                new RabbitMQSettings(_matchingEngineSettings, "trades", Constants.TestQueueName), handleOperationMessages);

        }

        private void prepareConsumer()
        {
            if (Int32.TryParse(_matchingEngineSettings.Port, out int port))
            {
                Consumer = new MatchingEngineConsumer(_matchingEngineSettings.BaseUrl, port);
            }
            else
            {
                throw new FormatException();
            }
        }

        private void prepareRabbitQueues()
        {
            RabbitMQHttpApiConsumer.Setup(new RabbitMQHttpApiSettings(_matchingEngineSettings));

            
            if(!Int32.TryParse(_matchingEngineSettings.RabbitMQMessageWait, out waitForRabbitMQMessage))
                waitForRabbitMQMessage = 20000;

            List<Task<bool>> createQueueTasks = new List<Task<bool>>
            {
                createQueue("lykke.cashinout", "lykke.cashinout." + Constants.TestQueueName),
                createQueue("lykke.transfers", "lykke.transfers." + Constants.TestQueueName),
                createQueue("lykke.cashswap", "lykke.cashswap." + Constants.TestQueueName),
                createQueue("lykke.balanceupdate", "lykke.balanceupdate." + Constants.TestQueueName),
                createQueue("lykke.limitorders.clients", "lykke.limitorders.clients." + Constants.TestQueueName),
                createQueue("lykke.trades", "lykke.trades." + Constants.TestQueueName)
            };

            Task.WhenAll(createQueueTasks).Wait();
        }

        private async Task<bool> createQueue(string exchangeName, string queueName)
        {
            RabbitMQHttpApiQueueResultDTO queueModel = Task.Run(async () =>
            {
                return await RabbitMQHttpApiConsumer.GetQueueByNameAsync(queueName);
            }).Result;

            if (queueModel != null)
            {
                await RabbitMQHttpApiConsumer.DeleteQueueAsync(queueName);
            }

            bool IsBinded = false;

            bool IsCreated = await RabbitMQHttpApiConsumer.CreateQueueAsync(queueName);
            if (IsCreated)
            {
                IsBinded = await RabbitMQHttpApiConsumer.BindQueueAsync(exchangeName, queueName);
            }

            if (IsBinded)
            {
                AddOneTimeCleanupAction(async () => await RabbitMQHttpApiConsumer.DeleteQueueAsync(queueName));
            }

            return IsBinded;
        }

        private async Task prepareTestData()
        {
            ApiV2Settings apiv2Config = new ConfigBuilder().ReloadingManager.CurrentValue.AutomatedFunctionalTests.ApiV2;
            ApiConsumer registerConsumer1 = new ApiConsumer(apiv2Config);
            ApiConsumer registerConsumer2 = new ApiConsumer(apiv2Config);
            var registerTestAccount1 = registerConsumer1.RegisterNewUser();
            var registerTestAccount2 = registerConsumer2.RegisterNewUser();

            TestAsset1 = Constants.TestAsset1;
            TestAsset2 = Constants.TestAsset2;

            TestAccountId1 = (await registerTestAccount1)?.Account.Id;
            TestAccountId2 = (await registerTestAccount2)?.Account.Id;

            AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(TestAccountId1));
            AddOneTimeCleanupAction(async () => await ClientAccounts.DeleteClientAccount(TestAccountId2));

            //give test clients some cash to work with
            List<Task> giveCashTasks = new List<Task>()
            {
                Consumer.Client.UpdateBalanceAsync(Guid.NewGuid().ToString(), TestAccountId1, TestAsset1, Constants.InitialAssetAmmount),
                Consumer.Client.UpdateBalanceAsync(Guid.NewGuid().ToString(), TestAccountId1, TestAsset2, Constants.InitialAssetAmmount),
                Consumer.Client.UpdateBalanceAsync(Guid.NewGuid().ToString(), TestAccountId2, TestAsset1, Constants.InitialAssetAmmount),
                Consumer.Client.UpdateBalanceAsync(Guid.NewGuid().ToString(), TestAccountId2, TestAsset2, Constants.InitialAssetAmmount)
            };

            if (!Int32.TryParse(_matchingEngineSettings.AssetPrecission, out AssetPrecission))
                AssetPrecission = 2;

            this.TestAssetPair = (AssetPairEntity)Task.Run(async () =>
            {
                return await this.AssetPairsRepository.TryGetAsync(TestAsset1 + TestAsset2);
            }).Result;

            await Task.WhenAll(giveCashTasks);
        }

        public Task<IRabbitMQOperation> WaitForRabbitMQ<T>(Func<T, bool> lambda) where T : IRabbitMQOperation
        {
            Stopwatch stopWatch = new Stopwatch();
            IRabbitMQOperation message = default(IRabbitMQOperation);

            stopWatch.Start();
            while (message == default(IRabbitMQOperation) && stopWatch.Elapsed.TotalMilliseconds < waitForRabbitMQMessage)
            {
                if (RabbitMqMessages.ContainsKey(typeof(T)))
                {
                    List<T> rabbotMessagesCopy = RabbitMqMessages[typeof(T)].Cast<T>().ToList(); //avoid read/write lock
                    message = rabbotMessagesCopy.Where(lambda).FirstOrDefault();
                }
                if (message == default(IRabbitMQOperation))
                {
                    Thread.Sleep(100);
                }
            }
            stopWatch.Stop();

            return Task.FromResult(message);
        }

        private Task handleOperationMessages(IRabbitMQOperation msg)
        {
            if (!RabbitMqMessages.ContainsKey(msg.GetType()))
            {
                RabbitMqMessages[msg.GetType()] = new List<IRabbitMQOperation>();
            }

            RabbitMqMessages[msg.GetType()].Add(msg);

            return Task.FromResult(msg);
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            CashInOutSubscription.Stop();
            CashTransferSubscription.Stop();
            CashSwapSubscription.Stop();
            BalanceUpdateSubscription.Stop();
            LimitOrderSubscription.Stop();
            TradesOrderSubscription.Stop();

        }
    }
}
