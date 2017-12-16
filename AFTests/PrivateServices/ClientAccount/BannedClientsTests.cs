using LykkeAutomationPrivate.Models.Registration.Models;
using LykkeAutomationPrivate.DataGenerators;
using NUnit.Framework;
using System.Net;
using System;
using System.Collections.Generic;
using System.Text;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using System.Linq;

namespace LykkeAutomationPrivate.Tests.ClientAccount
{
    class PutClientBan : BaseTest
    {
        string clientId;

        [SetUp]
        public void CreateClient()
        {
            clientId = lykkeApi.Registration.PostRegistration(new AccountRegistrationModel().GetTestModel()).Account.Id;
        }

        [TearDown]
        public void UnBanClient()
        {
            if (clientId != null)
                lykkeApi.ClientAccount.BannedClients.DeleteBannedClients(clientId);
        }

        [Test]
        [Category("BannedClients"), Category("ClientAccount"), Category("ServiceAll")]
        public void PutClientBanTest()
        {
            var putClientBan = lykkeApi.ClientAccount.BannedClients.PutBannedClients(clientId);
            Assert.That(putClientBan.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var getIsClientBanned = lykkeApi.ClientAccount.BannedClients.GetBannedClientsIsBanned(clientId);
            Assert.That(getIsClientBanned.GetResponseObject(), Is.True);
        }
    }

    class DeleteClientsBan : BaseTest
    {
        string clientId;

        [SetUp]
        public void CreateClient()
        {
            clientId = lykkeApi.Registration.PostRegistration(new AccountRegistrationModel().GetTestModel()).Account.Id;
        }

        [TearDown]
        public void UnBanClientAtTheEnd()
        {
            if (clientId != null)
                lykkeApi.ClientAccount.BannedClients.DeleteBannedClients(clientId);
        }

        [Test]
        [Category("BannedClients"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeleteClientsBanTest()
        {
            var putClientBan = lykkeApi.ClientAccount.BannedClients.PutBannedClients(clientId);
            var getIsClientBanned = lykkeApi.ClientAccount.BannedClients.GetBannedClientsIsBanned(clientId);
            Assert.That(getIsClientBanned.GetResponseObject(), Is.True);

            var putUnBanClient = lykkeApi.ClientAccount.BannedClients.DeleteBannedClients(clientId);
            getIsClientBanned = lykkeApi.ClientAccount.BannedClients.GetBannedClientsIsBanned(clientId);
            Assert.That(getIsClientBanned.GetResponseObject(), Is.False);
        }
    }

    class GetBannedClients : BaseTest
    {
        string clientId;

        [SetUp]
        public void CreateClient()
        {
            clientId = lykkeApi.Registration.PostRegistration(new AccountRegistrationModel().GetTestModel()).Account.Id;
        }

        [TearDown]
        public void UnBanClientAtTheEnd()
        {
            if (clientId != null)
                lykkeApi.ClientAccount.BannedClients.DeleteBannedClients(clientId);
        }

        [Test]
        [Category("BannedClients"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetBannedClientsTest()
        {
            var postBannedClientsList = lykkeApi.ClientAccount.BannedClients.GetBannedClients();
            Assert.That(postBannedClientsList.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(postBannedClientsList.GetResponseObject().Select(c => c.ClientId), 
                Does.Not.Contain(clientId));

            lykkeApi.ClientAccount.BannedClients.PutBannedClients(clientId);
            postBannedClientsList = lykkeApi.ClientAccount.BannedClients.GetBannedClients();
            Assert.That(postBannedClientsList.GetResponseObject().Select(c => c.ClientId), 
                Does.Contain(clientId));
        }
    }

    public class PostBannedCleintsFilterByIds : BaseTest
    {
        string clientId;

        [SetUp]
        public void CreateClient()
        {
            clientId = lykkeApi.Registration.PostRegistration(new AccountRegistrationModel().GetTestModel()).Account.Id;
        }

        [TearDown]
        public void UnBanClientAtTheEnd()
        {
            if (clientId != null)
                lykkeApi.ClientAccount.BannedClients.DeleteBannedClients(clientId);
        }

        [Test]
        [Category("BannedClients"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostBannedCleintsFilterByIdsTest()
        {
            var postBannedCleintsFilterByIds = lykkeApi.ClientAccount.BannedClients
                .PostBannedCleintsFilterByIds(new List<string> { clientId });
            Assert.That(postBannedCleintsFilterByIds.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(postBannedCleintsFilterByIds.GetResponseObject(), Is.Empty);

            lykkeApi.ClientAccount.BannedClients.PutBannedClients(clientId);
            postBannedCleintsFilterByIds = lykkeApi.ClientAccount.BannedClients
                .PostBannedCleintsFilterByIds(new List<string> { clientId });
            Assert.That(postBannedCleintsFilterByIds.GetResponseObject().Select(c => c.ClientId),
                Does.Contain(clientId));
        }
    }

    class GetClientIsBanned : BaseTest
    {
        string clientId;
        string clientIdToBan;

        [SetUp]
        public void CreateClient()
        {
            clientId = lykkeApi.Registration.PostRegistration(new AccountRegistrationModel().GetTestModel()).Account.Id;
            clientIdToBan = lykkeApi.Registration.PostRegistration(new AccountRegistrationModel().GetTestModel()).Account.Id;
        }

        [TearDown]
        public void UnBanClientAtTheEnd()
        {
            if (clientIdToBan != null)
                lykkeApi.ClientAccount.BannedClients.DeleteBannedClients(clientIdToBan);
        }

        [Test]
        [Category("BannedClients"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetClientIsBannedTest()
        {
            var putClientBan = lykkeApi.ClientAccount.BannedClients.PutBannedClients(clientIdToBan);

            var getBannedClient = lykkeApi.ClientAccount.BannedClients.GetBannedClientsIsBanned(clientIdToBan);
            Assert.That(getBannedClient.GetResponseObject(), Is.True);

            var getNotBannedClient = lykkeApi.ClientAccount.BannedClients.GetBannedClientsIsBanned(clientId);
            Assert.That(getNotBannedClient.GetResponseObject(), Is.False);
        }
    }
}
