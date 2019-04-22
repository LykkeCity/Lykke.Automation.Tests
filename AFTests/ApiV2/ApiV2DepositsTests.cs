using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lykke.Client.ApiV2.Models;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    public class ApiV2DepositsTests : ApiV2TokenBaseTest
    {
        [Test]
        [Category("ApiV2")]
        public void GetDepositsFxpaygateLastTest()
        {
            Step("Make GET /api/Deposits/fxpaygate/last and validate response", () =>
            {
                var response = apiV2.Deposits.GetDepositsFXPayGateLast(token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                Assert.That(response.ResponseObject, Is.Not.Null);
                Assert.That(response.ResponseObject.Email, Is.Not.Null);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetDepositsFxpaygateLastInvalidTokenTest()
        {
            Step("Make GET /api/Deposits/fxpaygate/last with invalid token and validate response", () =>
            {
                var response = apiV2.Deposits.GetDepositsFXPayGateLast(Guid.NewGuid().ToString());

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetDepositsFxpaygateFeeTest()
        {
            Step("Make GET /api/Deposits/fxpaygate/fee and validate response", () =>
            {
                var response = apiV2.Deposits.GetDepositsFXPayGateFee(token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetDepositsFxpaygateFeeInvalidTokenTest()
        {
            Step("Make GET /api/Deposits/fxpaygate/fee with invalid token and validate response", () =>
            {
                var response = apiV2.Deposits.GetDepositsFXPayGateFee(Guid.NewGuid().ToString());
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostDepositsFxpayGateTest()
        {
            Step("Make POST /api/Deposits/fxpaygate and validate response", () =>
            {
                var model = new FxPaygatePaymentUrlRequestModel
                {
                    Address = "123 Downing street",
                    Amount = 1.1,
                    AssetId = "BTC",
                    CancelUrl = "http://lykke.com",
                    City = "Boston",
                    Country = "USA",
                    Email = wallet.WalletAddress,
                    FailUrl = "http://lykkex.com",
                    FirstName = "Ly",
                    LastName = "Kke",
                    OkUrl = "OkUrl",
                    Phone = "+130026",
                    WalletId = Guid.NewGuid().ToString(),
                    Zip = "654321"
                };

                var response = apiV2.Deposits.PostDepositsFXPayGate(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.ResponseObject.Url, Is.Not.Null);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostDepositsFxpayGateInvalidTokenTest()
        {
            Step("Make POST /api/Deposits/fxpaygate with invalid token and validate response", () =>
            {
                var model = new FxPaygatePaymentUrlRequestModel
                {
                    Address = "123 Downing street",
                    Amount = 1.1,
                    AssetId = "BTC",
                    CancelUrl = "http://lykke.com",
                    City = "Boston",
                    Country = "USA",
                    Email = wallet.WalletAddress,
                    FailUrl = "http://lykkex.com",
                    FirstName = "Ly",
                    LastName = "Kke",
                    OkUrl = "OkUrl",
                    Phone = "+130026",
                    WalletId = Guid.NewGuid().ToString(),
                    Zip = "654321"
                };

                var response = apiV2.Deposits.PostDepositsFXPayGate(model, Guid.NewGuid().ToString());

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostDepositsFxpayGateInvalidEmailTest()
        {
            Step("Make POST /api/Deposits/fxpaygate with invalid token and validate response", () =>
            {
                var model = new FxPaygatePaymentUrlRequestModel
                {
                    Address = "123 Downing street",
                    Amount = 1.1,
                    AssetId = "BTC",
                    CancelUrl = "http://lykke.com",
                    City = "Boston",
                    Country = "USA",
                    Email = "lykke@email.com",
                    FailUrl = "http://lykkex.com",
                    FirstName = "Ly",
                    LastName = "Kke",
                    OkUrl = "OkUrl",
                    Phone = "+130026",
                    WalletId = Guid.NewGuid().ToString(),
                    Zip = "654321"
                };

                var response = apiV2.Deposits.PostDepositsFXPayGate(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostDepositsFxpayGateInvalidPhoneTest()
        {
            Step("Make POST /api/Deposits/fxpaygate with invalid token and validate response", () =>
            {
                var model = new FxPaygatePaymentUrlRequestModel
                {
                    Address = "123 Downing street",
                    Amount = 1.1,
                    AssetId = "BTC",
                    CancelUrl = "http://lykke.com",
                    City = "Boston",
                    Country = "USA",
                    Email = wallet.WalletAddress,
                    FailUrl = "http://lykkex.com",
                    FirstName = "Ly",
                    LastName = "Kke",
                    OkUrl = "OkUrl",
                    Phone = "+1300267",
                    WalletId = Guid.NewGuid().ToString(),
                    Zip = "654321"
                };

                var response = apiV2.Deposits.PostDepositsFXPayGate(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void POSTDepositsSwiftAssetIdEmailTest()
        {
            Step("Make POST /api/Deposits/swift/{assetId}/email and validate response", () =>
            {
                var model = new SwiftDepositEmailModel
                {
                    Amount = 1
                };

                var response = apiV2.Deposits.PostSwiftAssetIdEmail(model, "BTC", token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostDepositsSwiftAssetIdEmailainvalidTokenTest()
        {
            Step("Make POST /api/Deposits/swift/{assetId}/email and validate response", () =>
            {
                var model = new SwiftDepositEmailModel
                {
                    Amount = 1
                };

                var response = apiV2.Deposits.PostSwiftAssetIdEmail(model, "BTC", Guid.NewGuid().ToString());
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostDepositsSwiftAssetIdEmailEmptyObjectTest()
        {
            var assetId = "";

            Step("Make GET /assets request. Take id of first asset in response", () =>
            {
                var assets = apiV2.Assets.GetAssets();
                Assert.That(assets.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                assetId = assets.GetResponseObject().Assets[0].Id;
            });

            Step($"Make POST /api/Deposits/swift/{assetId}/email and validate response", () =>
            {
                var model = new SwiftDepositEmailModel
                {
                };

                var response = apiV2.Deposits.PostSwiftAssetIdEmail(model, assetId, token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetDepositsSwiftAssetIdRequisitesTest()
        {
            Step("Make GET /api/Deposits/swift/{assetId}/requisites and validate response", () =>
            {
                var response = apiV2.Deposits.GetSwiftAssetIdRequisites("BTC", token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                Assert.That(response.ResponseObject.AccountName, Is.Not.Null);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetDepositsSwiftAssetIdRequisitesInvalidTokenTest()
        {
            Step("Make GET /api/Deposits/swift/{assetId}/requisites and validate response", () =>
            {
                var response = apiV2.Deposits.GetSwiftAssetIdRequisites("BTC", Guid.NewGuid().ToString());
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetDepositsSwiftAssetIdRequisitesInvalidAssedIdTest()
        {
            Step("Make GET /api/Deposits/swift/{assetId}/requisites with invalid assetId and validate response", () =>
            {
                var response = apiV2.Deposits.GetSwiftAssetIdRequisites(Guid.NewGuid().ToString(), token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostDepositsCryptoAssetIdAddressTest()
        {
            Step("Make GET /api/Deposits/crypto/{assetId}/address and validate response", () =>
            {
                var response = apiV2.Deposits.PostCryptoAssetIdAddress("BTC", token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostDepositsCryptoAssetIdAddressInvalidTokenTest()
        {
            Step("Make GET /api/Deposits/crypto/{assetId}/address and validate response", () =>
            {
                var response = apiV2.Deposits.PostCryptoAssetIdAddress("BTC", Guid.NewGuid().ToString());

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostDepositsCryptoAssetIdAddressInvalidAssetIdTest()
        {
            Step("Make GET /api/Deposits/crypto/{assetId}/address with invalid assetId and validate response", () =>
            {
                var response = apiV2.Deposits.PostCryptoAssetIdAddress(Guid.NewGuid().ToString(), token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetDepositsCryptoAssetIdAddress()
        {
            Step("Make GET /api/Deposits/crypto/{assetId}/address and validate response", () => 
            {
                var response = apiV2.Deposits.GetCryptoAssetIdAddress("BTC", token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.ResponseObject, Is.Not.Null);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetDepositsCryptoAssetIdAddressInvalidAssetTest()
        {
            Step("Make GET /api/Deposits/crypto/{assetId}/address with invalid assetId and validate response ", () => 
            {
                var response = apiV2.Deposits.GetCryptoAssetIdAddress(Guid.NewGuid().ToString(), token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetDepositsCryptoAssetIdAddressInvalidAssetInvalidToken()
        {
            Step("Make GET /api/Deposits/crypto/{assetId}/address with invalid token and validate response ", () =>
            {
                var response = apiV2.Deposits.GetCryptoAssetIdAddress("BTC", Guid.NewGuid().ToString());
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }
}
}
