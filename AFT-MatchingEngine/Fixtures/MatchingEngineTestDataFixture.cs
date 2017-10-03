using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon;

namespace AFTMatchingEngine.Fixtures
{
    public class MatchingEngineTestDataFixture : IDisposable
    {
        public MatchingEngineConsumer Consumer;
        private ConfigBuilder _configBuilder;

        public MatchingEngineTestDataFixture()
        {
            this._configBuilder = new ConfigBuilder("MatchingEngine");

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
            
        }
    }
}
