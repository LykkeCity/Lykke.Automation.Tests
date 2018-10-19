using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    public class ApiV2WithdrawalsTests : ApiV2TokenBaseTest
    {
        [Test]
        [Category("ApiV2")]
        public void GetWithdrawalsCryptoAssetInfoTest()
        {
            Step("Make GET /api/withdrawals/crypto/{assetId}/info and validate response", () => 
            {
                var assetId = "BTC";
                var response = apiV2.Withdrawals.GetWithdrawalsCryptoAssetInfo(assetId, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                Assert.That(response.GetResponseObject(), Is.Not.Null);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWithdrawalsCryptoInvalidAssetTest()
        {
            Step("Make GET /api/withdrawals/crypto/{assetId}/info with invalid assetId and validate response", () => 
            {
                var assetId = Guid.NewGuid().ToString();
                var response = apiV2.Withdrawals.GetWithdrawalsCryptoAssetInfo(assetId, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWithdrawalsCryptoAssetFee()
        {
            Step("Make GET /api/withdrawals/crypto/{assetId}/fee and validate response", () => 
            {
                var assetId = "BTC";
                var response = apiV2.Withdrawals.GetWithdrawalsCryptoAssetIdFee(assetId, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.GetResponseObject(), Is.Not.Null);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWithdrawalsCryptoAssetFeeInvalidAssetId()
        {
            Step("Make GET /api/withdrawals/crypto/{assetId}/fee and validate response", () =>
            {
                var assetId = Guid.NewGuid().ToString();
                var response = apiV2.Withdrawals.GetWithdrawalsCryptoAssetIdFee(assetId, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        [TestCase("BTC", "baseAddress", "addressExtension")]
        [Category("ApiV2")]
        public void GetWithdrawalCryptoAssetValidateAddressTest(string assetId, string baseAddress, string addressExtension)
        {
            Step($"Make  GET /api/withdrawals/crypto/{assetId}/validateAddress and validate request", () => 
            {
                var response = apiV2.Withdrawals.GetWithdrawalsCryptoAssetIdValidateAddress(assetId, baseAddress, addressExtension, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.GetResponseObject().IsValid, Is.False);
            });
        }

        [Test]
        [TestCase("invalidAssetId", "baseAddress", "addressExtension")]
        [Category("ApiV2")]
        public void GetWithdrawalCryptoAssetValidateAddressInvalidAssetTest(string assetId, string baseAddress, string addressExtension)
        {
            Step($"Make  GET /api/withdrawals/crypto/{assetId}/validateAddress and validate request", () =>
            {
                var response = apiV2.Withdrawals.GetWithdrawalsCryptoAssetIdValidateAddress(assetId, baseAddress, addressExtension, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        [TestCase("BTC", "baseAddress", "addressExtension")]
        [Category("ApiV2")]
        public void GetWithdrawalCryptoAssetValidateAddressInvalidtokenTest(string assetId, string baseAddress, string addressExtension)
        {
            Step($"Make  GET /api/withdrawals/crypto/{assetId}/validateAddress and validate request", () =>
            {
                var response = apiV2.Withdrawals.GetWithdrawalsCryptoAssetIdValidateAddress(assetId, baseAddress, addressExtension, Guid.NewGuid().ToString());

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWithdrawalsAvailableTest()
        {
            Step("Make GET /api/withdrawals/available and validate request", () => 
            {
                var response = apiV2.Withdrawals.GetWithdrawalsCryptoAvailable(token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWithdrawalsAvailableInvalidTokenTest()
        {
            Step("Make GET /api/withdrawals/available with invalid token and validate request", () =>
            {
                var response = apiV2.Withdrawals.GetWithdrawalsCryptoAvailable(Guid.NewGuid().ToString());
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWithdrawalsFeeTest()
        {
            Step("Make GET /api/withdrawals/swift/last and validate request", () => 
            {
                var assetId = "BTC";
                var response = apiV2.Withdrawals.GetWithdrawalsCryptoAssetIdFee(assetId, token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.GetResponseObject().Size, Is.Not.Null);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWithdrawalsFeeInvalidTokenTest()
        {
            Step("Make GET /api/withdrawals/swift/last and validate request", () =>
            {
                var assetId = "BTC";
                var response = apiV2.Withdrawals.GetWithdrawalsCryptoAssetIdFee(assetId, Guid.NewGuid().ToString());
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }
    }
}
