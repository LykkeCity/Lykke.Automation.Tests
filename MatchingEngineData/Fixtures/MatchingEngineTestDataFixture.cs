using AFTMatchingEngine.DTOs;
using Lykke.RabbitMqBroker.Subscriber;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.DTOs.RabbitMQ;
using XUnitTestCommon.RabbitMQ;
using XUnitTestCommon.Utils;

namespace AFTMatchingEngine.Fixtures
{
    public class MatchingEngineTestDataFixture : IDisposable
    {
        public MatchingEngineConsumer Consumer;
        public List<RabbitMQCashOperation> CashInOutMessages;

        private RabbitMQSubscribtion<RabbitMQCashOperation> CashInOutSubscription;
        private ConfigBuilder _configBuilder;

        private List<string> _createdQueues;

        public MatchingEngineTestDataFixture()
        {
            this._configBuilder = new ConfigBuilder("MatchingEngine");
            prepareConsumer();
            prepareRabbitQueues();
            prepareRabbitMQConnection();


        }

        private void prepareRabbitMQConnection()
        {
            CashInOutMessages = new List<RabbitMQCashOperation>();

            StringBuilder connectionstrinSb = new StringBuilder("amqp://");
            connectionstrinSb.Append(_configBuilder.Config["RabbitMQUsername"]);
            connectionstrinSb.Append(":");
            connectionstrinSb.Append(_configBuilder.Config["RabbitMQPassword"]);
            connectionstrinSb.Append("@");
            connectionstrinSb.Append(_configBuilder.Config["RabbitMQHost"]);
            connectionstrinSb.Append(":");
            connectionstrinSb.Append(_configBuilder.Config["RabbitMQamqpPort"]);
            connectionstrinSb.Append("/");
            connectionstrinSb.Append("%2f"); //vhost


            string nameOfSourceEndpoint = "cashinout";
            string nameOfEndpoint = "automation_functional_tests";

            RabbitMqSubscriptionSettings subscriberSettings =
                RabbitMqSubscriptionSettings.CreateForSubscriber(connectionstrinSb.ToString(), nameOfSourceEndpoint, nameOfEndpoint);

            subscriberSettings.MakeDurable();

            //TODO: set queue argument instead of setting subscriber DeadLetter to none!!!!!!!!!
            subscriberSettings.DeadLetterExchangeName = "";

            CashInOutSubscription = new RabbitMQSubscribtion<RabbitMQCashOperation>(subscriberSettings);
            CashInOutSubscription.SubscribeMessageHandler(handleCashInOutMessages);
            CashInOutSubscription.Start();
        }

        private void prepareConsumer()
        {
            if (Int32.TryParse(_configBuilder.Config["Port"], out int port))
            {
                Consumer = new MatchingEngineConsumer(_configBuilder.Config["BaseUrl"], port);
                Consumer.Connect();
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

            bool CashoutCreated = createQueue("lykke.cashinout", "lykke.cashinout.automation_functional_tests");
        }

        private bool createQueue(string exchangeName, string queueName)
        {
            RabbitMQHttpApiQueueResultDTO queueModel = Task.Run(async () =>
            {
                return await RabbitMQHttpApiConsumer.GetQueueByNameAsync(queueName);
            }).Result;

            if (queueModel != null)
            {
                Task.Run(async () => { return await RabbitMQHttpApiConsumer.DeleteQueueAsync(queueName); });
            }

            bool IsBinded = false;

            bool IsCreated = Task.Run(async () => { return await RabbitMQHttpApiConsumer.CreateQueueAsync(queueName); }).Result;
            if (IsCreated)
            {
                IsBinded = Task.Run(async () => { return await RabbitMQHttpApiConsumer.BindQueueAsync(exchangeName, queueName); }).Result;
            }

            if (IsBinded)
            {
                _createdQueues.Add(queueName);
            }

            return IsBinded;
        }

        private Task handleCashInOutMessages(RabbitMQCashOperation msg)
        {
            CashInOutMessages.Add(msg);

            return Task.FromResult<RabbitMQCashOperation>(msg);
        }

        public void Dispose()
        {
            CashInOutSubscription.Stop();

            foreach (string queueName in _createdQueues)
            {
                Task.Run(async () => { return await RabbitMQHttpApiConsumer.DeleteQueueAsync(queueName); });
            }
            
        }
    }
}
