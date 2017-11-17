using ApiV2Data.Fixtures;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    [Category("FullRegression")]
    [Category("ApiV2Service")]
    public partial class ApiV2Tests
    {
        private ApiV2TestDataFixture _fixture;

        public ApiV2Tests()
        {
            this._fixture = new ApiV2TestDataFixture();
        }
    }
}
