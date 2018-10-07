using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Lykke.Client.ApiV2.Models;
using NUnit.Framework;

namespace AFTests.ApiV2
{
    public class ApiV2TokenBaseTest : ApiV2BaseTest
    {
        #region setup teardown

        protected string token = "";

        [OneTimeSetUp]
        public void CashOutSetUp()
        {
            var response = apiV2.Client.PostClientAuth(new AuthRequestModel
            {
                Email = wallet.WalletAddress,
                Password = wallet.WalletKey,
                PartnerId = "lykke"
            });

            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK), "Could not get token for user.");

            token = response.GetResponseObject().AccessToken;
        }
        #endregion
    }
}
