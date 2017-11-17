using BlueApiData.Fixtures;
using NUnit.Framework;

namespace AFTests.BlueApi
{
    [Category("FullRegression")]
    [Category("BlueApiService")]
    public partial class BlueApiTests
    {
        private readonly BlueApiTestDataFixture _fixture;

        public BlueApiTests()
        {
            this._fixture = new BlueApiTestDataFixture();
        }
    }
}
