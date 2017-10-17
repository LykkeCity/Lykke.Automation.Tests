using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.DTOs.RabbitMQ;
using Autofac;
using XUnitTestCommon.Utils;
using MatchingEngineData.DependencyInjection;
using XUnitTestData.Services;
using XUnitTestData.Domains;
using XUnitTestData.Repositories;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using MatchingEngineData.DTOs.RabbitMQ;
using XUnitTestData.Repositories.MatchingEngine;
using XUnitTestData.Domains.MatchingEngine;
using XUnitTestData.Domains.Assets;
using XUnitTestData.Repositories.Assets;
using System.Linq.Expressions;

namespace AFTMatchingEngine.Fixtures
{
    public class MatchingEngineTestDataFixture : IDisposable
    {
        public MatchingEngineConsumer Consumer;

        //public IDictionaryManager<IAccount> AccountManager;
        public AccountRepository AccountRepository;
        public CashSwapRepository CashSwapRepository;
        public MarketOrdersRepository MarketOrdersRepository;
        public LimitOrderRepository LimitOrdersRepository;

        private IDictionaryManager<IAssetPair> AssetPairsManager;

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
            this._configBuilder = new ConfigBuilder("MatchingEngine");
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

            this.AccountRepository = (AccountRepository)this.container.Resolve<IDictionaryRepository<IAccount>>();
            this.CashSwapRepository = (CashSwapRepository)this.container.Resolve<IDictionaryRepository<ICashSwap>>();
            this.AssetPairsManager = RepositoryUtils.PrepareRepositoryManager<IAssetPair>(this.container);
            this.MarketOrdersRepository = (MarketOrdersRepository)this.container.Resolve<IDictionaryRepository<IMarketOrderEntity>>();
            this.LimitOrdersRepository = (LimitOrderRepository)this.container.Resolve<IDictionaryRepository<ILimitOrderEntity>>();
        }

        private void prepareRabbitMQConnections()
        {
            RabbitMqMessages = new Dictionary<Type, List<IRabbitMQOperation>>();

            CashInOutSubscription = new RabbitMQConsumer<CashOperation>(
                _configBuilder, "cashinout", "automation_functional_tests", handleOperationMessages);

            CashTransferSubscription = new RabbitMQConsumer<CashTransferOperation>(
                _configBuilder, "transfers", "automation_functional_tests", handleOperationMessages);

            CashSwapSubscription = new RabbitMQConsumer<CashSwapOperation>(
                _configBuilder, "cashswap", "automation_functional_tests", handleOperationMessages);

            BalanceUpdateSubscription = new RabbitMQConsumer<BalanceUpdate>(
                _configBuilder, "balanceupdate", "automation_functional_tests", handleOperationMessages);

            LimitOrderSubscription = new RabbitMQConsumer<LimitOrdersResponse>(
                _configBuilder, "limitorders.clients", "automation_functional_tests", handleOperationMessages);

            TradesOrderSubscription = new RabbitMQConsumer<MarketOrderWithTrades>(
                _configBuilder, "trades", "automation_functional_tests", handleOperationMessages);

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
            RabbitMQHttpApiConsumer.Setup(_configBuilder);

            
            if(!Int32.TryParse(_configBuilder.Config["RabbitMQMessageWait"], out waitForRabbitMQMessage))
                waitForRabbitMQMessage = 20000;

            List<Task<bool>> createQueueTasks = new List<Task<bool>>();
            createQueueTasks.Add(createQueue("lykke.cashinout", "lykke.cashinout.automation_functional_tests"));
            createQueueTasks.Add(createQueue("lykke.transfers", "lykke.transfers.automation_functional_tests"));
            createQueueTasks.Add(createQueue("lykke.cashswap", "lykke.cashswap.automation_functional_tests"));
            createQueueTasks.Add(createQueue("lykke.balanceupdate", "lykke.balanceupdate.automation_functional_tests"));
            createQueueTasks.Add(createQueue("lykke.limitorders.clients", "lykke.limitorders.clients.automation_functional_tests"));
            createQueueTasks.Add(createQueue("lykke.trades", "lykke.trades.automation_functional_tests"));

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
            TestAccountId1 = "AFTest_Client1";
            TestAccountId2 = "AFTest_Client2";
            TestAsset1 = "LKK";
            TestAsset2 = "USD";

            if (!Int32.TryParse(_configBuilder.Config["AssetPrecission"], out AssetPrecission))
                AssetPrecission = 2;

            this.TestAssetPair = (AssetPairEntity)Task.Run(async () =>
            {
                return await this.AssetPairsManager.TryGetAsync(TestAsset1 + TestAsset2);
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
