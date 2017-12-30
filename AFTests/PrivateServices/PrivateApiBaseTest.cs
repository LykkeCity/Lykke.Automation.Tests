using LykkeAutomationPrivate.Api;
using XUnitTestCommon.Tests;

namespace AFTests.PrivateApiTests
{
    public class PrivateApiBaseTest : BaseTest
    {
        protected LykkeApi lykkeApi = new LykkeApi();
        protected ResponseValidator responseValidator = new ResponseValidator();
    }
}
