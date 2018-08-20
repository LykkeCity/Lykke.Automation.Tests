using HFT;
using LykkeAutomationPrivate.Api;
using XUnitTestCommon.Tests;

namespace AFTests.PrivateApiTests
{
    public class PrivateApiBaseTest : BaseTest
    {
        protected LykkeApi lykkeApi = new LykkeApi();
        protected Hft hft = new Hft();
    }
}
