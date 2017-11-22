using ApiV2Data.Fixtures;
using NUnit.Framework;
using XUnitTestCommon.Tests;

namespace AFTests.ApiV2
{
    [Category("FullRegression")]
    [Category("ApiV2Service")]
    public partial class ApiV2Tests: BaseTest
    {
        private ApiV2TestDataFixture _fixture;

        public ApiV2Tests()
        {
            this._fixture = new ApiV2TestDataFixture();
        }
    }
}
