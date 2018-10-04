using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Lykke.Client.ApiV2.Models;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    class ApiV2AffiliateTests : ApiV2TokenBaseTest
    {    
        [Test]
        [Category("ApiV2")]
        public void GetAffiliateLinkTest()
        {
            Step($"Make GET /api/Affiliate/link and validate response", () =>
            {
                var response = apiV2.Affiliate.GetAffiliateLink(token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void PostAffiliateCreate()
        {
            Step("Make POST /api/Affiliate/create and validate response", () => 
            {
                var response = apiV2.Affiliate.PostAffiliateCreate(token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.GetResponseObject(), Is.Not.Null);
                Assert.That(response.GetResponseObject().RedirectUrl, Is.Not.Null);
                Assert.That(response.GetResponseObject().Url, Is.Not.Null);
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetAffiliateWrongCredentialsStats()
        {
            Step("Make GET /api/Affiliate/stats with invalid parameter and validate response", () => 
            {
                var response = apiV2.Affiliate.GetAffiliateStats("wrong wallet id", "BTC", token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        [Category("ApiV2")]
        public void GetAffiliateStats()
        {
            var walletId = "";

            Step("Make GET /api/wallets and get walletId. Validate response", () => 
            {
                var response = apiV2.wallets.GetWallets(token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                walletId = response.GetResponseObject()[1].Id;
            });

            Step("Make GET /api/Affiliate/stats and validate response", () =>
            {
                var response = apiV2.Affiliate.GetAffiliateStats(walletId, "BTC", token);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}
