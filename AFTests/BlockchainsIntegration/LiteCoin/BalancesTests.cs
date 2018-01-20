using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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

                if (litecoinApi.Balances.GetBalances(take, null).GetResponseObject().Items.ToList().Any(a => a.Address == WALLET_ADDRESS))
                {
                    litecoinApi.Balances.DeleteBalances(WALLET_ADDRESS);
                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5));
                }

                //enable observation
                var pResponse = litecoinApi.Balances.PostBalances(WALLET_ADDRESS);


                Assert.That(() => litecoinApi.Balances.GetBalances(take, null).GetResponseObject().Items.ToList().Any(a => a.Address == WALLET_ADDRESS), 
                    Is.True.After(10*60 * 1000, 1 * 1000), "Wallet is not present in Get Balances after 10 minutes");

                //disable
                var dResponse = litecoinApi.Balances.DeleteBalances(WALLET_ADDRESS);

                Assert.That(() => litecoinApi.Balances.GetBalances(take, null).GetResponseObject().Items.ToList().Any(a => a.Address == WALLET_ADDRESS),
                    Is.False.After(1 * 60 * 1000, 1 * 1000), "Wallet still present in Get Balances after Delete");
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

        public class CheckBalanceIsZeroBeforeGetBalance : LitecoinBaseTest
        {
            [Test]
            [Category("Litecoin")]
            public void CheckBalanceIsZeroBeforeGetBalanceTest()
            {
                Assert.Ignore("Get valid blockchain address");
                var response = litecoinApi.Balances.DeleteBalances("1");
                response.Validate.StatusCode(HttpStatusCode.OK);
            }
        }

        public class DeleteBalancesInvalidAddress : LitecoinBaseTest
        {
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
