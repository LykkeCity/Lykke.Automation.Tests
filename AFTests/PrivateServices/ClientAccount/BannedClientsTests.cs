using LykkeAutomationPrivate.DataGenerators;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.Resources.ClientAccountResource;
using System.Net;

namespace AFTests.PrivateApiTests
{
    class BannedClientComparer : IComparer<BannedClient>
    {
        public int Compare(BannedClient x, BannedClient y)
        {
            if (x.ClientId == y.ClientId)
                return 0;
            return -1;
        }
    }

    class BannedClientBaseTest : PrivateApiBaseTest
    {
        protected BannedClients api;
        protected string clientId;
        protected IComparer<BannedClient> bannedClientsComparer = new BannedClientComparer();

        [OneTimeSetUp]
        public void CreateClient()
        {
            api = lykkeApi.ClientAccount.BannedClients;
            clientId = lykkeApi.ClientAccount.Clients
                .PostRegister(new ClientRegistrationModel().GetTestModel())
                .GetResponseObject().Id;
        }

        [OneTimeTearDown]
        public void UnBanAndDeleteClient()
        {
            if (clientId != null)
            {
                lykkeApi.ClientAccount.BannedClients.DeleteBannedClients(clientId);
                lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(clientId);
            }
        }
    }

    class PutClientBanTests : BannedClientBaseTest
    {
        [Test]
        [Category("BannedClients"), Category("ClientAccount"), Category("ServiceAll")]
        public void PutClientBanTest()
        {
            var putClientBan = api.PutBannedClients(clientId);
            putClientBan.Validate.StatusCode(HttpStatusCode.OK);
            Assert.That(api.GetBannedClientsIsBanned(clientId)
                .GetResponseObject(), Is.True);
        }
    }

    class DeleteClientsBanTests : BannedClientBaseTest
    {
        [Test]
        [Category("BannedClients"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteClientsBanTest()
        {
            api.PutBannedClients(clientId);

            var getIsClientBanned = api.GetBannedClientsIsBanned(clientId);
            Assert.That(getIsClientBanned.GetResponseObject(), Is.True);

            var deleteUnBanClient = api.DeleteBannedClients(clientId);
            deleteUnBanClient.Validate.StatusCode(HttpStatusCode.OK);
            Assert.That(api.GetBannedClientsIsBanned(clientId)
                .GetResponseObject(), Is.False);
        }
    }

    class GetBannedClientsTests : BannedClientBaseTest
    {
        [Test]
        [Category("BannedClients"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetBannedClientsTest()
        {
            var bannedClient = new BannedClient { ClientId = clientId };

            var postBannedClientsList = api.GetBannedClients();
            postBannedClientsList.Validate.StatusCode(HttpStatusCode.OK);
            Assert.That(postBannedClientsList.GetResponseObject(), 
                Does.Not.Contain(bannedClient).Using(bannedClientsComparer));

            api.PutBannedClients(clientId);
            postBannedClientsList = api.GetBannedClients();
            Assert.That(postBannedClientsList.GetResponseObject(), 
                Does.Contain(bannedClient).Using(bannedClientsComparer));
        }
    }

    class PostBannedCleintsFilterByIdsTests : BannedClientBaseTest
    {
        [Test]
        [Category("BannedClients"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostBannedCleintsFilterByIdsTest()
        {
            var postBannedCleintsFilterByIds = api
                .PostBannedCleintsFilterByIds(new List<string> { clientId });
            postBannedCleintsFilterByIds.Validate.StatusCode(HttpStatusCode.OK);
            Assert.That(postBannedCleintsFilterByIds.GetResponseObject(), Is.Empty);

            api.PutBannedClients(clientId);
            postBannedCleintsFilterByIds = api
                .PostBannedCleintsFilterByIds(new List<string> { clientId });
            Assert.That(postBannedCleintsFilterByIds.GetResponseObject(),
                Does.Contain(new BannedClient { ClientId = clientId }).Using(bannedClientsComparer));
        }
    }

    class GetClientIsBannedTests : BannedClientBaseTest
    {
        string bannedClientId;

        [OneTimeSetUp]
        public void CreateBannedClient()
        {
            bannedClientId = lykkeApi.ClientAccount.Clients
                .PostRegister(new ClientRegistrationModel().GetTestModel())
                .GetResponseObject().Id;
            api.PutBannedClients(bannedClientId);
        }

        [OneTimeTearDown]
        public void UnBanAndDeleteBannedClient()
        {
            if (bannedClientId != null)
            {
                lykkeApi.ClientAccount.BannedClients.DeleteBannedClients(bannedClientId);
                lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(bannedClientId);
            }
        }

        [Test]
        [Category("BannedClients"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetBannedClientIsBannedTest()
        {
            var getBannedClient = api.GetBannedClientsIsBanned(bannedClientId);
            getBannedClient.Validate.StatusCode(HttpStatusCode.OK);
            Assert.That(getBannedClient.GetResponseObject(), Is.True);
        }

        [Test]
        [Category("BannedClients"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetNotBannedClientIsBannedTest()
        {
            var getNotBannedClient = api.GetBannedClientsIsBanned(clientId);
            getNotBannedClient.Validate.StatusCode(HttpStatusCode.OK);
            Assert.That(getNotBannedClient.GetResponseObject(), Is.False);
        }
    }
}
