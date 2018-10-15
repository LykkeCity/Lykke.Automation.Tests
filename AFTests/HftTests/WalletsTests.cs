namespace AFTests.HftTests
{
    using NUnit.Framework;
    using System.Net;

    class WalletsTests
    {
        public class GetWallets : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            public void GetWalletsTest()
            {
                hft.Wallets.GetWallets(ApiKey).Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class GetWalletsInvalidApiKey : HftBaseTest
        {
            [Test]
            [Category("HFT")]
            [TestCase("!@^&*(%€§", ExpectedResult = HttpStatusCode.BadRequest)]
            [TestCase("invalidApiKey", ExpectedResult = HttpStatusCode.Unauthorized)]
            [TestCase("1234", ExpectedResult = HttpStatusCode.Unauthorized)]
            [TestCase("-125.45", ExpectedResult = HttpStatusCode.Unauthorized)]
            [TestCase(" ", ExpectedResult = HttpStatusCode.Unauthorized)]               
            public HttpStatusCode GetWalletsInvalidApiKeyTest(string apiKey)
            {
                return hft.Wallets.GetWallets(apiKey).StatusCode;
            }
        }

    }
}
