using ApiV2Data.Fixtures;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    [Category("FullRegression")]
    [Category("ApiV2Service")]
    public partial class ApiV2Tests : ApiV2TestDataFixture
    {
        private ApiV2TestDataFixture fixture;

        public ApiV2Tests() { } // Default constructor is needed for nUnit

        public ApiV2Tests(ApiV2TestDataFixture fixture)
        {
            this.fixture = fixture;
        }
    }
}
