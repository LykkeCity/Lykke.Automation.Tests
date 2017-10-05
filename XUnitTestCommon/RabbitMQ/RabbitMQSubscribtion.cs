using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTestCommon.RabbitMQ
{
    public class RabbitMQSubscribtion<T>
    {
        private RabbitMqSubscriber<T> _connector;
        public RabbitMQSubscribtion(RabbitMqSubscriptionSettings settings)
        {
            var logger = new LogToConsole();

            _connector =
                new RabbitMqSubscriber<T>(settings, new DefaultErrorHandlingStrategy(logger, settings))
                  .SetMessageDeserializer(new JsonMessageDeserializer<T>())
                  .SetLogger(logger)
                  .SetConsole(new writeToSystemConsole())
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

        //public Task MessageHandler(T msg)
        //{
        //    var test = msg;
        //    return Task.FromResult<T>(msg);
        //}
    }

    public class writeToSystemConsole : IConsole
    {
        public void WriteLine(string line)
        {
            Console.WriteLine(line ?? "[null]");
        }
    }
}
