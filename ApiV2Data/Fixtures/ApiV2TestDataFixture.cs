using System;
using System.Collections.Generic;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;

namespace ApiV2Data.Fixtures
{
    public class ApiV2TestDataFixture : IDisposable
    {
        private ConfigBuilder _configBuilder;


        public Dictionary<string, string> ApiEndpointNames;
        public ApiConsumer Consumer;

        public ApiV2TestDataFixture()
        {
            this._configBuilder = new ConfigBuilder("ApiV2");
            this.Consumer = new ApiConsumer(this._configBuilder);

            ApiEndpointNames = new Dictionary<string, string>();
            ApiEndpointNames["Pledges"] = "/api/pledges";
        }

        public void Dispose()
        {
            
        }
    }
}
