using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Lykke.Client.ApiV2.Models;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    public class ApiV2WatchlistsTests : ApiV2TokenBaseTest
    {
        [Test]
        [Category("ApiV2")]
        public void GetWatchlistsTest()
        {
            Step("Make GET /api/watchlists and validate response", () => 
            {
                var response = apiV2.Watchlists.GetWatchlists(token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.GetResponseObject()?.FirstOrDefault()?.Id, Is.Not.Empty);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostWatchlistsTest()
        {
            var assetId = "wrong asset id";

            Step("Make GET /assets request. Take id of first asset in response", () =>
            {
                var assets = apiV2.Assets.GetAssets();
                Assert.That(assets.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                assetId = assets.GetResponseObject().Assets[0].Id;
            });

            Step("Make POST /api/watchlists and validate response", () => 
            {
                var model = new WatchListCreateModel
                {
                    AssetPairIds = new List<string> { assetId },
                    Name = "Autotest",
                    Order = 0
                };

                var response = apiV2.Watchlists.PostWatchLists(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostWatchlistsInvalidTokenTest()
        {
            Step("Make POST /api/watchlists with invalid token and validate response", () => 
            {
                var model = new WatchListCreateModel
                {
                    AssetPairIds = new List<string> { "BTCUSD" },
                    Name = "Autotest",
                    Order = 1
                };

                var response = apiV2.Watchlists.PostWatchLists(model, Guid.NewGuid().ToString());

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostWatchlistsEmptyObjectTest()
        {
            Step("Make POST/api/watchlists with invalid token and validate response", () => 
            {
                var model = new WatchListCreateModel
                {
                };

                var response = apiV2.Watchlists.PostWatchLists(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void DeleteWatchlistsTest()
        {
            Step("Make DELETE /api/watchlists/{id} and validate response ", () => 
            {
                var id = Guid.NewGuid().ToString();

                var response = apiV2.Watchlists.DeleteWatchlists(id, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void DeleteWatchlistsInvalidWatchlistIdTest()
        {
            var watchlistId = Guid.NewGuid().ToString();

            Step($"Make DELETE /api/watchlists/{watchlistId} and validate response ", () =>
            {
                var response = apiV2.Watchlists.DeleteWatchlists(watchlistId, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }
        
        [Test]
        [Category("ApiV2")]
        public void DeleteWatchlistsInvalidTokenTest()
        {
            Step("Make /api/watchlists/{watchlistId} with invalid token and validate response is unathorized", () => 
            {
                var response = apiV2.Watchlists.DeleteWatchlists(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWatchlistByIdTest()
        {
            Step("Make GET /api/watchlists/{id} and validate response", () => 
            {
                var watchlist = Guid.NewGuid().ToString(); // get valid watchlist

                var response = apiV2.Watchlists.GetWatchlistsById(watchlist, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWatchlistByInvalidIdTest()
        {
            Step("Make GET /api/watchlists/{id} and validate response", () =>
            {
                var watchlist = Guid.NewGuid().ToString();

                var response = apiV2.Watchlists.GetWatchlistsById(watchlist, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetWatchlistByIdInvalidToken()
        {
            Step("Make GET /api/watchlists/{id} with invalid token and validate response is Unathorized", () => 
            {
                var watchlist = Guid.NewGuid().ToString();

                var response = apiV2.Watchlists.GetWatchlistsById(watchlist, Guid.NewGuid().ToString());

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PutWatchlistTest()
        {
            Step("Make PUT /api/watchlists/{id} and validate response", () => 
            {
                var model = new WatchListUpdateModel
                {
                    AssetPairIds = new List<string> { "BTCUSD" },
                    Name = "Autotest",
                    Order = 0
                };

                var response = apiV2.Watchlists.PutWatchlistsById(model, Guid.NewGuid().ToString(),token); //get valid watchlist id

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PutWatchlistInvalidIdTest()
        {
            Step("Make PUT /api/watchlists/{id} and validate response", () =>
            {
                var model = new WatchListUpdateModel
                {
                    AssetPairIds = new List<string> { "BTCUSD" },
                    Name = "Autotest",
                    Order = 0
                };

                var response = apiV2.Watchlists.PutWatchlistsById(model, Guid.NewGuid().ToString(), token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PutWatchlistInvalidTokenTest()
        {
            Step("Make PUT /api/watchlists/{id} and validate response", () =>
            {
                var model = new WatchListUpdateModel
                {
                    AssetPairIds = new List<string> { "BTCUSD" },
                    Name = "Autotest",
                    Order = 0
                };

                var response = apiV2.Watchlists.PutWatchlistsById(model, Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            });
        }
    }
}
