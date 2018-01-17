using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.BlockchainsIntegration.LiteCoin
{
    class BalancesTests
    {

        public class GetBalances : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void GetBalancesTest()
            {
                var take = "1";
                var response = litecoinApi.Balances.GetBalances(take, null);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class GetBalancesInvalidTake : LitecoinBaseTest
        {
            [TestCase("")]
            [TestCase("testTake")]
            [TestCase("325.258")]
            [TestCase("!@&*()")]
            [Category("Litecoin")]
            public void GetBalancesInvalidTakeTest(string take)
            {
                var response = litecoinApi.Balances.GetBalances(take, null);
                response.Validate.StatusCode(HttpStatusCode.BadRequest, $"Unexpected Status code {response.StatusCode} for take: {take}");
            }
        }

        public class GetBalancesContinuation : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void GetBalancesContinuationTest()
            {
                var take = "1";
                var continuation = TestData.GenerateString();
                var response = litecoinApi.Balances.GetBalances(take, continuation);
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class PostBalances : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void PostBalancesTest()
            {
                Assert.Ignore("Get valid blockchain address");
                var address = TestData.GenerateString(8);
                var response = litecoinApi.Balances.PostBalances(address);
            }
        }

        public class PostBalancesInvalidAddress : LitecoinBaseTest
        {
            [TestCase("")]
            [TestCase("testAddress")]
            [TestCase("!@&*()")]
            [TestCase("352.58")]
            [Category("Litecoin")]
            public void PostBalancesInvalidAddressTest(string address)
            {
                var response = litecoinApi.Balances.PostBalances(address);
                response.Validate.StatusCode(HttpStatusCode.NotFound);
            }
        }

        public class DeleteBalances : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void DeleteBalancesTest()
            {
                Assert.Ignore("Get valid blockchain address");
                var response = litecoinApi.Balances.DeleteBalances("1");
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class DeleteBalancesInvalidAddress : LitecoinBaseTest
        {
            [TestCase("")]
            [TestCase("testAddress")]
            [TestCase("!@&*()")]
            [TestCase("352.58")]
            [Category("Litecoin")]
            public void DeleteBalancesInvalidAddressTest(string address)
            {
                var response = litecoinApi.Balances.DeleteBalances(address);
                response.Validate.StatusCode(HttpStatusCode.BadRequest);
            }
        }
    }
}
