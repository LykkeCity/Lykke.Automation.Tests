using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.DTOs.RabbitMQ;
using Autofac;
using XUnitTestCommon.Utils;
using XUnitTestCommon.Consumers.Models;
using MatchingEngineData.DependencyInjection;
using XUnitTestData.Services;
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
using XUnitTestData.Entitites.ApiV2.Assets;

namespace AFTMatchingEngine.Fixtures
{
    public class MatchingEngineTestDataFixture : IDisposable
    {
        public MatchingEngineConsumer Consumer;

        //public IDictionaryManager<IAccount> AccountManager;
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

        private ConfigBuilder _configBuilder;

        private List<string> _createdQueues;
        private IContainer container;

        public MatchingEngineTestDataFixture()
        {
            this._configBuilder = new ConfigBuilder(Constants.ConfigItemName);
            prepareConsumer();
            prepareRabbitQueues();
            prepareRabbitMQConnections();
            prepareDependencyContainer();

            prepareTestData();
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
                new RabbitMQSettings(_configBuilder, "cashinout", Constants.TestQueueName), handleOperationMessages);

            CashTransferSubscription = new RabbitMQConsumer<CashTransferOperation>(
                new RabbitMQSettings(_configBuilder, "transfers", Constants.TestQueueName), handleOperationMessages);

            CashSwapSubscription = new RabbitMQConsumer<CashSwapOperation>(
                new RabbitMQSettings(_configBuilder, "cashswap", Constants.TestQueueName), handleOperationMessages);

            BalanceUpdateSubscription = new RabbitMQConsumer<BalanceUpdate>(
                new RabbitMQSettings(_configBuilder, "balanceupdate", Constants.TestQueueName), handleOperationMessages);

            LimitOrderSubscription = new RabbitMQConsumer<LimitOrdersResponse>(
                new RabbitMQSettings(_configBuilder, "limitorders.clients", Constants.TestQueueName), handleOperationMessages);

            TradesOrderSubscription = new RabbitMQConsumer<MarketOrderWithTrades>(
                new RabbitMQSettings(_configBuilder, "trades", Constants.TestQueueName), handleOperationMessages);

        }

        private void prepareConsumer()
        {
            if (Int32.TryParse(_configBuilder.Config["Port"], out int port))
            {
                Consumer = new MatchingEngineConsumer(_configBuilder.Config["BaseUrl"], port);
            }
            else
            {
                throw new FormatException();
            }
        }

        private void prepareRabbitQueues()
        {
            _createdQueues = new List<string>();
            RabbitMQHttpApiConsumer.Setup(new RabbitMQHttpApiSettings(_configBuilder));

            
            if(!Int32.TryParse(_configBuilder.Config["RabbitMQMessageWait"], out waitForRabbitMQMessage))
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
                _createdQueues.Add(queueName);
            }

            return IsBinded;
        }

        private void prepareTestData()
        {
            TestAccountId1 = Constants.TestAccountId1;
            TestAccountId2 = Constants.TestAccountId2;
            TestAsset1 = Constants.TestAsset1;
            TestAsset2 = Constants.TestAsset2;

            if (!Int32.TryParse(_configBuilder.Config["AssetPrecission"], out AssetPrecission))
                AssetPrecission = 2;

            this.TestAssetPair = (AssetPairEntity)Task.Run(async () =>
            {
                return await this.AssetPairsRepository.TryGetAsync(TestAsset1 + TestAsset2);
            }).Result;
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

        public void Dispose()
        {
            if (_createdQueues != null)
            {
                CashInOutSubscription.Stop();
                CashTransferSubscription.Stop();
                CashSwapSubscription.Stop();
                BalanceUpdateSubscription.Stop();
                LimitOrderSubscription.Stop();
                TradesOrderSubscription.Stop();

                List<Task<bool>> deleteTasks = new List<Task<bool>>();

                foreach (string queueName in _createdQueues)
                {
                    deleteTasks.Add(RabbitMQHttpApiConsumer.DeleteQueueAsync(queueName));
                }

                Task.WhenAll(deleteTasks).Wait();
            }
        }
    }
}
