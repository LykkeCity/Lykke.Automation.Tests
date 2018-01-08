using Lykke.Client.AutorestClient.Models;
using LykkeAutomation.ApiModels.RegistrationModels;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using XUnitTestCommon.TestsData;

namespace AFTests.WalletApiTests.BitcoinCash
{
    class BitcoinCashTests
    {

        public class GetBitcoinCashMultisigBalance : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetBitcoinCashMultisigBalanceTest()
            {
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.BitcoinCash.GetBitcoinCashMultiSigBalance(registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class GetBitcoinCashMultisigBalanceInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@$%(8")]
            [Category("WalletApi")]
            public void GetBitcoinCashMultisigBalanceInvalidTokenTest(string token)
            {
                var response = walletApi.BitcoinCash.GetBitcoinCashMultiSigBalance(token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        public class GetBitcoinCashMultiSigTransaction : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetBitcoinCashMultiSigTransactionTest()
            {
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.BitcoinCash.GetBitcoinCashMultiSigTransaction(registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class GetBitcoinCashMultiSigTransactionInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@%^&(0")]
            [Category("WalletApi")]
            public void GetBitcoinCashMultiSigTransactionInvalidTokenTest(string token)
            {
                var response = walletApi.BitcoinCash.GetBitcoinCashMultiSigTransaction(token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        public class GetGetBitcoinCashPrivateBalance : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetGetBitcoinCashPrivateBalanceTest()
            {
                Assert.Ignore("Get valid address");
                var address = TestData.GenerateString(8);

                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var response = walletApi.BitcoinCash.GetBitcoinCashPrivateBalance(address, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class GetGetBitcoinCashPrivateBalanceInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("1234567")]
            [TestCase("testToken")]
            [TestCase("!@%^&(0")]
            [Category("WalletApi")]
            public void GetGetBitcoinCashPrivateBalanceInvalidTokenTest(string token)
            {
                var address = TestData.GenerateString(8);

                var response = walletApi.BitcoinCash.GetBitcoinCashPrivateBalance(address, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        public class GetBitcoinCashPrivateTransaction : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetBitcoinCashPrivateTransactionTest()
            {
                Assert.Ignore("Get valid source, dest, fee");
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var source = TestData.GenerateString(6);
                var dest = TestData.GenerateString(6);
                var fee = 5d;

                var response = walletApi.BitcoinCash.GetBitcoinCashPrivateTransaction(source, dest, fee, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class GetBitcoinCashPrivateTransactionInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("testToken")]
            [TestCase("1234567")]
            [TestCase("!@$&(0")]
            [Category("WalletApi")]
            public void GetBitcoinCashPrivateTransactionInvalidTokenTest(string token)
            {
                var source = TestData.GenerateString(6);
                var dest = TestData.GenerateString(6);
                var fee = 5d;

                var response = walletApi.BitcoinCash.GetBitcoinCashPrivateTransaction(source, dest, fee, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }

        public class PostBitcoinCashBroadcast : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostBitcoinCashBroadcastTest()
            {
                Assert.Ignore("Get Valid transaction");
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var model = new BccBroadcastRequest() {Transaction = TestData.GenerateLetterString(6) };

                var response = walletApi.BitcoinCash.PostBitcoinCashBroadcast(model, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Error, Is.Null);
            }
        }

        public class PostBitcoinCashBroadcastInvalidAddress : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void PostBitcoinCashBroadcastInvalidAddressTest()
            {
                var newUser = new AccountRegistrationModel();
                var registeredClient = walletApi.Registration.PostRegistrationResponse(newUser).GetResponseObject();

                var model = new BccBroadcastRequest() { Transaction = TestData.GenerateLetterString(6) };

                var response = walletApi.BitcoinCash.PostBitcoinCashBroadcast(model, registeredClient.Result.Token);
                response.Validate.StatusCode(HttpStatusCode.InternalServerError);
            }
        }

        public class PostBitcoinCashBroadcastInvalidToken : WalletApiBaseTest
        {
            [TestCase("")]
            [TestCase("testToken")]
            [TestCase("1234567")]
            [TestCase("!@%667")]
            [Category("WalletApi")]
            public void PostBitcoinCashBroadcastInvalidTokenTest(string token)
            {
                var model = new BccBroadcastRequest() { Transaction = TestData.GenerateLetterString(6) };

                var response = walletApi.BitcoinCash.PostBitcoinCashBroadcast(model, token);
                response.Validate.StatusCode(HttpStatusCode.Unauthorized);
            }
        }
    }
}
