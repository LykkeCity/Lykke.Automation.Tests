using BlueApiData.Fixtures;
using NUnit.Framework;

namespace AFTests.BlueApi
{
    [Category("FullRegression")]
    [Category("BlueApiService")]
    public partial class BlueApiTests : BlueApiTestDataFixture
    {
        private readonly BlueApiTestDataFixture _fixture;

        public BlueApiTests() { } // Default constructor is needed for nUnit

        public BlueApiTests(BlueApiTestDataFixture fixture)
        {
            _fixture = fixture;
        }
    }
}
