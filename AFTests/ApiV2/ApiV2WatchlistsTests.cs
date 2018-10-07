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
            Step("Make POST /api/watchlists and validate request", () => 
            {
                var model = new WatchListCreateModel
                {
                    AssetPairIds = new List<string> { "BTCUSD" },
                    Name = "Autotest",
                    Order = 1
                };

                var response = apiV2.Watchlists.PostWatchLists(model, token);

                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

       
    }
}
