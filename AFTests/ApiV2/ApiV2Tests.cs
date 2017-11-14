using AFTMatchingEngine.Fixtures;
using ApiV2Data.Fixtures;
using Xunit;

namespace AFTests.ApiV2
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "ApiV2Service")]
    public partial class ApiV2Tests : IClassFixture<ApiV2TestDataFixture>, IClassFixture<MatchingEngineTestDataFixture>
    {
        private ApiV2TestDataFixture _fixture;
        private MatchingEngineTestDataFixture _matchingEngineFixture;

        public ApiV2Tests(ApiV2TestDataFixture fixture, MatchingEngineTestDataFixture matchingEngineFixture)
        {
            _fixture = fixture;
            _matchingEngineFixture = matchingEngineFixture;
        }
    }
}
