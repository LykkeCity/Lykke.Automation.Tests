using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Lykke.Client.ApiV2.Models;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    public class ApiV2WalletsTests : ApiV2TokenBaseTest
    {
        [Test]
        [Category("ApiV2")]
        public void GetWalletsTest()
        {
            Step("Make GET /api/wallets and validate response", () => 
            {
                var response = apiV2.wallets.GetWallets(token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.GetResponseObject(), Is.Not.Null);
                Assert.That(response.GetResponseObject().First().Id, Is.Not.Null.Or.Empty);
                Assert.That(response.GetResponseObject().First().Name, Is.Not.Null.Or.Empty);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWalletInvalidTokenTest()
        {
            Step("Make GET /api/wallets with invalid token and validate response", () => 
            {
                var response = apiV2.wallets.GetWallets(Guid.NewGuid().ToString());

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWalletsByIdTest()
        {
            var walletId = "wrong wallet id";

            Step("Make GET /api/wallets and find walletId", () => 
            {
                var response = apiV2.wallets.GetWallets(token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                walletId = response.GetResponseObject().First().Id;
            });

            Step($"Make GET /api/wallets/{walletId} with valid wallet-id and validate response", () => 
            {
                var response = apiV2.wallets.GetWalletsId(walletId, token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                Assert.That(response.GetResponseObject().Id, Is.EqualTo(walletId));
                Assert.That(response.GetResponseObject().Name, Is.Not.Null.Or.Empty);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWalletsbalanceTest()
        {
            Step("Make GET /api/wallets/balances and validate response", () => 
            {
                var response = apiV2.wallets.GetWalletsBalances(token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.GetResponseObject().First().Balances, Is.Not.Null);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetTradingWalletsBalancesTest()
        {
            Step("Make GET /api/wallets/trading/balances and validate response", () => 
            {
                var response = apiV2.wallets.GetWalletsTradingBalance(token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                Assert.That(response.GetResponseObject().First()?.AssetId, Is.Not.Null.Or.Empty);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWalletsWalletIdBalanceTest()
        {
            var walletId = "wrong wallet id";
            Step("Make GET /api/wallets and find walletId", () => 
            {
                var response = apiV2.wallets.GetWallets(token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                walletId = response.GetResponseObject().First().Id;
            });

            Step($"Make GET /api/wallets/{walletId}/balances and validate response", () =>
            {
                var response = apiV2.wallets.GetWalletsWalletIdBalance(walletId, token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWalletsBalanceAssetIdTest()
        { 
            var assetId = "";

            Step("Make GET /assets request. Take id of first asset in response", () =>
            {
                var assets = apiV2.Assets.GetAssets();
                Assert.That(assets.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                assetId = assets.GetResponseObject().Assets[0].Id;
            });

            Step($"Make GET /api/wallets/balances/{assetId}", () => 
            {
                var response = apiV2.wallets.GetWalletsBalancesAssetId(assetId, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetTradingBalancesWalletIdTest()
        {
            var assetId = "BTC";

            Step($"Make GET /api/wallets/trading/balances/{assetId} and validate response", () => 
            {
                var response = apiV2.wallets.GetWalletsTradingBalanceAssetId(assetId, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.GetResponseObject()?.AssetId, Is.Not.Null);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWalletBalanceAssetIdTest()
        {
            var walletId = "wrong wallet id";
            var assetId = "asset id";
            Step("Make GET /assets request. Take id of first asset in response", () =>
            {
                var assets = apiV2.Assets.GetAssets();
                Assert.That(assets.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                assetId = assets.GetResponseObject().Assets[0].Id;
            });

            Step("Make GET /api/wallets and find walletId", () =>
            {
                var response = apiV2.wallets.GetWallets(token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                walletId = response.GetResponseObject().First().Id;
            });

            Step($"Make GET /api/wallets/{walletId}/balances/{assetId} and validate response", () => 
            {
                var response = apiV2.wallets.GetWalletsBalanceAssetId(walletId, assetId, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.GetResponseObject().First().AssetId, Is.Not.Null.Or.Empty);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostWalletsTradingTest()
        {
            var walletId = "";

            Step("Make POST /api/wallets and validate response ", () => 
            {
                var model = new CreateWalletRequest
                {
                    Description = "Wallet created during test",
                    Name = "Autotest wallet.Remove me",
                    Type = WalletType.Trading
                };
                var response = apiV2.wallets.PostWallets(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                walletId = response.GetResponseObject().Id;
            });

            if (walletId != "")
                Step($"Make DELETE /api/wallets/{walletId} and remove wallet", () => 
                {
                    var response = apiV2.wallets.DeleteWallet(walletId, token);
                });
        }

        [Test]
        [Category("ApiV2")]
        public void PostWalletTrustedTest()
        {
            var walletId = "";

            Step("Make POST /api/wallets and validate response ", () =>
            {
                var model = new CreateWalletRequest
                {
                    Description = "Wallet created during test",
                    Name = "Autotest wallet.Remove me",
                    Type = WalletType.Trusted
                };
                var response = apiV2.wallets.PostWallets(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                walletId = response.GetResponseObject().Id;
            });

            if (walletId != "")
                Step($"Make DELETE /api/wallets/{walletId} and remove wallet", () =>
                {
                    var response = apiV2.wallets.DeleteWallet(walletId, token);
                });
        }

        [Test]
        [Category("ApiV2")]
        public void PostWalletWithoutTypeTest()
        {
            var walletId = "";

            Step("Make POST /api/wallets and validate response ", () =>
            {
                var model = new CreateWalletRequest
                {
                    Description = "Wallet created during test",
                    Name = "Autotest wallet.Remove me"
                };
                var response = apiV2.wallets.PostWallets(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                walletId = response.GetResponseObject().Id;
            });

            if (walletId != "")
                Step($"Make DELETE /api/wallets/{walletId} and remove wallet", () =>
                {
                    var response = apiV2.wallets.DeleteWallet(walletId, token);
                });
        }

        [Test]
        [Category("ApiV2")]
        public void PostWalletEmptyBodyTest()
        {
            Step("Make POST /api/wallets and validate response ", () =>
            {
                var model = new CreateWalletRequest
                {  
                };
                var response = apiV2.wallets.PostWallets(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostWalletInvalidTokenTest()
        {
            Step("Make POST /api/wallets and validate response ", () =>
            {
                var model = new CreateWalletRequest
                {
                    Description = "Wallet created during test",
                    Name = "Autotest wallet.Remove me",
                    Type = WalletType.Trading
                };
                var response = apiV2.wallets.PostWallets(model, Guid.NewGuid().ToString());

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));     
            });
        }

        [Test]
        [Category("ApiV2")]
        public void DeleteWalletIdTest()
        {
            var walletId = "";

            Step("Make POST /api/wallets and validate response ", () =>
            {
                var model = new CreateWalletRequest
                {
                    Description = "Wallet created during test",
                    Name = "Autotest wallet.Remove me",
                    Type = WalletType.Trading
                };
                var response = apiV2.wallets.PostWallets(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                walletId = response.GetResponseObject().Id;
            });

            Step($"Make DELETE /api/wallets/{walletId} and remove wallet", () =>
            {
                var response = apiV2.wallets.DeleteWallet(walletId, token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void DeleteWalletIdInvalidTokenTest()
        {
            var walletId = "";

            Step("Make POST /api/wallets and validate response ", () =>
            {
                var model = new CreateWalletRequest
                {
                    Description = "Wallet created during test",
                    Name = "Autotest wallet.Remove me",
                    Type = WalletType.Trading
                };
                var response = apiV2.wallets.PostWallets(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                walletId = response.GetResponseObject().Id;
            });

            Step($"Make DELETE /api/wallets/{walletId} with invalid token and validate", () => 
            {
                var response = apiV2.wallets.DeleteWallet(walletId, Guid.NewGuid().ToString());
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });

            Step($"Make DELETE /api/wallets/{walletId} and remove wallet", () =>
            {
                var response = apiV2.wallets.DeleteWallet(walletId, token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostWalletsHFTest()
        {
            var walletId = "";

            Step("Make POST /api/wallets/hft and validate response", () => 
            {
                var model = new CreateApiKeyRequest
                {
                    Description = "Autotest HFT description",
                    Name = "Autotest HFT description, delete me"
                };

                var response = apiV2.wallets.PostWalletHFT(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

                Assert.That(response.GetResponseObject().WalletId, Is.Not.Null.Or.Empty);
                Assert.That(response.GetResponseObject().ApiKey, Is.Not.Null.Or.Empty);
                walletId = response.GetResponseObject().WalletId;
            });

            if (walletId != "")
                Step($"Make DELETE /api/wallets/{walletId} and remove wallet", () =>
                {
                    var response = apiV2.wallets.DeleteWallet(walletId, token);
                });
        }

        [Test]
        [Category("ApiV2")]
        public void PostWalletsHFTInvalidTokenTest()
        {
            Step("Make POST /api/wallets/hft and validate response", () =>
            {
                var model = new CreateApiKeyRequest
                {
                    Description = "Autotest HFT description",
                    Name = "Autotest HFT description, delete me"
                };

                var response = apiV2.wallets.PostWalletHFT(model, Guid.NewGuid().ToString());

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));      
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostWalletsHFTEmptyObjectTest()
        {
            Step("Make POST /api/wallets/hft and validate response", () =>
            {
                var model = new CreateApiKeyRequest
                {
                };

                var response = apiV2.wallets.PostWalletHFT(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });    
        }
    }
}
