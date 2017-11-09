using BlueApiData.Fixtures;
using Xunit;

namespace AFTests.BlueApi
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "BlueApiService")]
    public partial class BlueApiTests : IClassFixture<BlueApiTestDataFixture>
    {
        private readonly BlueApiTestDataFixture _fixture;

        public BlueApiTests(BlueApiTestDataFixture fixture)
        {
            _fixture = fixture;
        }
    }
}
