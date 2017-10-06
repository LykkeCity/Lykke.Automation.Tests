using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTestCommon.Consumers
{
    public class RabbitMQConsumer<T>
    {
        private RabbitMqSubscriber<T> _connector;

        public RabbitMQConsumer(ConfigBuilder configBuilder, string sourceEndpoint, string endpoint)
        {
            StringBuilder connectionstrinSb = new StringBuilder("amqp://");
            connectionstrinSb.Append(configBuilder.Config["RabbitMQUsername"]);
            connectionstrinSb.Append(":");
            connectionstrinSb.Append(configBuilder.Config["RabbitMQPassword"]);
            connectionstrinSb.Append("@");
            connectionstrinSb.Append(configBuilder.Config["RabbitMQHost"]);
            connectionstrinSb.Append(":");
            connectionstrinSb.Append(configBuilder.Config["RabbitMQamqpPort"]);
            connectionstrinSb.Append("/");
            connectionstrinSb.Append("%2f");

            RabbitMqSubscriptionSettings subscriberSettings =
                RabbitMqSubscriptionSettings.CreateForSubscriber(connectionstrinSb.ToString(), sourceEndpoint, endpoint);

            subscriberSettings.MakeDurable();
            subscriberSettings.DeadLetterExchangeName = "";

            Setup(subscriberSettings);

        }

        public RabbitMQConsumer(RabbitMqSubscriptionSettings settings)
        {
            Setup(settings);
        }

        private void Setup(RabbitMqSubscriptionSettings settings)
        {
            var logger = new LogToConsole();

            _connector =
                new RabbitMqSubscriber<T>(settings, new DefaultErrorHandlingStrategy(logger, settings))
                  .SetMessageDeserializer(new JsonMessageDeserializer<T>())
                  .SetLogger(logger)
                  .CreateDefaultBinding();
        }

        public void Start()
        {
            _connector.Start();
        }

        public void Stop()
        {
            _connector.Stop();
        }

        public void SubscribeMessageHandler(Func<T, Task> callback)
        {
            _connector.Subscribe(callback);
        }
    }
}
