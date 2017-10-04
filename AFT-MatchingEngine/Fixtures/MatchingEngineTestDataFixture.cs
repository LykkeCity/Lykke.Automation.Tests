using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using XUnitTestCommon.DTOs.RabbitMQ;
using XUnitTestCommon.RabbitMQ;

namespace AFTMatchingEngine.Fixtures
{
    public class MatchingEngineTestDataFixture : IDisposable
    {
        public MatchingEngineConsumer Consumer;
        private ConfigBuilder _configBuilder;

        //public List<RabbitMQHttpApiQueueResultDTO> allQueues;
        //public RabbitMQHttpApiQueueResultDTO testQueue;
        //public RabbitMQHttpApiQueueResultDTO badTestQueue;

        //private string queueName = "lykke.cashinout.automation_functional_tests";
        //private string exchangeName = "lykke.cashinout";

        public MatchingEngineTestDataFixture()
        {
            this._configBuilder = new ConfigBuilder("MatchingEngine");
            prepareConsumer();

            //RabbitMQHttpApiConsumer.Setup(_configBuilder);

            //this.allQueues = null;
            //this.allQueues = Task.Run(async () =>
            //{
            //    return await RabbitMQHttpApiConsumer.GetAllQueuesAsync("%2f");
            //}).Result;

            //this.testQueue = Task.Run(async () =>
            //{
            //    return await RabbitMQHttpApiConsumer.GetQueueByNameAsync("lykke.cashinout.TransactionsTracker");
            //}).Result;

            //this.badTestQueue = Task.Run(async () =>
            //{
            //    return await RabbitMQHttpApiConsumer.GetQueueByNameAsync("thisQueue.doesnot.exist.hopefully");
            //}).Result;

            //var test = Task.Run(async () => { return await RabbitMQHttpApiConsumer.GetAllNodesJson(); }).Result;



            //bool IsCreated = Task.Run(async () => { return await RabbitMQHttpApiConsumer.CreateQueueAsync(queueName); }).Result;
            //if (IsCreated)
            //{
            //    bool IsBinded = Task.Run(async () => { return await RabbitMQHttpApiConsumer.BindQueueAsync(exchangeName, queueName); }).Result;
            //}

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

        public void Dispose()
        {
            //bool IsDeleted = Task.Run(async () => { return await RabbitMQHttpApiConsumer.DeleteQueueAsync(queueName); }).Result;
        }
    }
}
