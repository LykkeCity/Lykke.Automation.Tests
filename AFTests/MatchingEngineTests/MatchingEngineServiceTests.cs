using System;
using System.Collections.Generic;
using System.Text;
using AFTMatchingEngine.Fixtures;
using Xunit;
using Lykke.MatchingEngine.Connector.Abstractions.Models;
using AFTMatchingEngine.DTOs.RabbitMQ;
using System.Linq;
using System.Threading;
using System.Diagnostics;

namespace AFTMatchingEngine
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "MatchingEngine")]
    public class MatchingEngineServiceTests : IClassFixture<MatchingEngineTestDataFixture>
    {
        private MatchingEngineTestDataFixture fixture;

        public MatchingEngineServiceTests(MatchingEngineTestDataFixture fixture)
        {
            this.fixture = fixture;
        }


        [Fact]
        [Trait("Category", "Smoke")]
        public async void CashInOut()
        {
            Assert.NotNull(fixture.Consumer.Client);
            Assert.True(fixture.Consumer.Client.IsConnected);

            //TODO
            string newId = Guid.NewGuid().ToString();
            string clientId = "test";
            string assetId = "test";
            double ammount = 0;


            MeResponseModel meResponse = await fixture.Consumer.Client.CashInOutAsync(newId, clientId, assetId, ammount);
            Assert.True(meResponse.Status == MeStatusCodes.Ok);

            Stopwatch stopWatch = new Stopwatch();
            CashOperation message = null;
            bool messsegeIsIn = false;

            stopWatch.Start();
            while (!messsegeIsIn && stopWatch.Elapsed.TotalMilliseconds < 10000)
            {
                message = fixture.CashInOutMessages.Where(m => m.id == newId).FirstOrDefault();

                if (message != null)
                {
                    messsegeIsIn = true;
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
            stopWatch.Stop();

            Assert.NotNull(message);

            double parsedVolume = -1;
            Double.TryParse(message.volume, out parsedVolume);

            Assert.Equal(message.clientId, clientId);
            Assert.Equal(message.asset, assetId);
            Assert.Equal(parsedVolume, ammount);

        }

    }
}
