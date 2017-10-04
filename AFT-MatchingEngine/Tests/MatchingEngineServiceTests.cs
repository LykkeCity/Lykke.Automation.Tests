using System;
using System.Collections.Generic;
using System.Text;
using AFTMatchingEngine.Fixtures;
using Xunit;
using RestSharp;
using XUnitTestCommon;
using Lykke.MatchingEngine.Connector.Abstractions.Models;

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
            Guid newId = Guid.NewGuid();

            MeResponseModel test = await fixture.Consumer.Client.CashInOutAsync(newId.ToString(), "test", "test", 0);
            Assert.True(test.Status == MeStatusCodes.Ok);

            //Assert.NotNull(fixture.allQueues);
            //Assert.True(fixture.allQueues.Count > 0);
            //Assert.NotNull(fixture.testQueue);
            //Assert.Null(fixture.badTestQueue);
        }

    }
}
