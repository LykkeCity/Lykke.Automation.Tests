using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon.Consumers.Models;

namespace XUnitTestCommon.Consumers
{
    public class RabbitMQConsumer<T>
    {
        private RabbitMqSubscriber<T> _connector;
        private RabbitMQSettings _settings;

        public RabbitMQConsumer(RabbitMQSettings settings, Func<T, Task> callback)
        {
            string connectionString = getConnectionStringFromSettings(settings);

            RabbitMqSubscriptionSettings subscriberSettings =
                RabbitMqSubscriptionSettings.CreateForSubscriber(connectionString, settings.SourceEndpoint, settings.Endpoint);

            subscriberSettings.MakeDurable();
            subscriberSettings.DeadLetterExchangeName = "";

            Setup(subscriberSettings);
            SubscribeMessageHandler(callback);
            Start();

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

        private string getConnectionStringFromSettings(RabbitMQSettings settings)
        {
            StringBuilder connectionstrinSb = new StringBuilder("amqp://");
            connectionstrinSb.Append(settings.Username);
            connectionstrinSb.Append(":");
            connectionstrinSb.Append(settings.Password);
            connectionstrinSb.Append("@");
            connectionstrinSb.Append(settings.Hostname);
            connectionstrinSb.Append(":");
            connectionstrinSb.Append(settings.Port);
            connectionstrinSb.Append("/");
            connectionstrinSb.Append("%2f");

            return connectionstrinSb.ToString();
        }
    }
}
