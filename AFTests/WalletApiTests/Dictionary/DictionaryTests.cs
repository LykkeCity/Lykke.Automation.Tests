using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace AFTests.WalletApiTests.Dictionary
{
    class DictionaryTests
    {

        public class GetDictionary : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetDictionaryTest()
            {
                var response = walletApi.Dictionary.GetDictionary();
                response.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(response.GetResponseObject().Result.Count, Is.GreaterThanOrEqualTo(1), "Result count is less than 1");
            }
        }

        public class GetDictionaryKey : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetDictionaryKeyTest()
            {
                var response = walletApi.Dictionary.GetDictionary();
                response.Validate.StatusCode(HttpStatusCode.OK);

                var key = response.GetResponseObject().Result[0].Key;

                var keyResponse = walletApi.Dictionary.GetDictionaryKey(key);
                keyResponse.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(keyResponse.GetResponseObject().Result.Key, Is.EqualTo(key));
                Assert.That(keyResponse.GetResponseObject().Result.Value, Is.Not.Null, $"Value is null for key {key}");
            }
        }

        public class GetDictionaryAllKeys : WalletApiBaseTest
        {
            [Test]
            [Category("WalletApi")]
            public void GetDictionaryAllKeysTest()
            {
                var response = walletApi.Dictionary.GetDictionary();
                response.Validate.StatusCode(HttpStatusCode.OK);

                var keyList = response.GetResponseObject().Result;

                foreach (var pair in keyList)
                {
                    var keyResponse = walletApi.Dictionary.GetDictionaryKey(pair.Key);
                    keyResponse.Validate.StatusCode(HttpStatusCode.OK);
                    Assert.That(keyResponse.GetResponseObject().Result.Key, Is.EqualTo(pair.Key));
                    Assert.That(keyResponse.GetResponseObject().Result.Value, Is.Not.Null, $"Value is null for key {pair.Key}");
                }
            }
        }

        public class GetDictionaryInvalidKey : WalletApiBaseTest
        {
            [TestCase("1234567")]
            [TestCase("testKey")]
            [TestCase("!@%^&*()")]
            [Category("WalletApi")]
            public void GetDictionaryInvalidKeyTest(string key)
            {
                var keyResponse = walletApi.Dictionary.GetDictionaryKey(key);
                keyResponse.Validate.StatusCode(HttpStatusCode.OK);
                Assert.That(keyResponse.GetResponseObject().Result, Is.Null);
                Assert.That(keyResponse.GetResponseObject().Error, Is.Null);
            }
        }
    }
}
