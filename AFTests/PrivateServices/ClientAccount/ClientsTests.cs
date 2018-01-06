using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.DataGenerators;
using System.Linq;

namespace AFTests.PrivateApiTests
{
    class ClientsBaseTest : PrivateApiBaseTest
    {
        protected ClientRegistrationModel clientRegistration;
        protected Partner partner;

        [OneTimeSetUp]
        public void CreateUnregisteredClientAndPartner()
        {
            clientRegistration = new ClientRegistrationModel().GetTestModel();
            partner = new Partner().GetTestModel();
            lykkeApi.ClientAccount.Partners.PostPartners(partner);
        }

        [OneTimeTearDown]
        public void RemoveClientAndPartner()
        {
            //Remove client if registered
            var client = lykkeApi.ClientAccount.ClientAccountInformation
                .GetClientsByEmail(clientRegistration.Email)
                .GetResponseObject().FirstOrDefault();
            if (client != null)
                lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(client.Id);

            lykkeApi.ClientAccount.Partners.DeleteRemovePartner(partner.InternalId);
        }
    }

    class RegisterTests : ClientsBaseTest
    {
        [Test]
        [Category("Clients"), Category("ClientAccount"), Category("ServiceAll")]
        public void RegisterNewUserTest()
        {
            clientRegistration.PartnerId = partner.PublicId;

            var register = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration);
            Assert.That(register.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var registeredClient = register.GetResponseObject();
            Assert.That(registeredClient, Is.EqualTo(clientRegistration));

            Assert.That(registeredClient.Id, Is.Not.Null, "No Id in response");
            Assert.That(registeredClient.IsReviewAccount, Is.False);
            Assert.That(registeredClient.IsTrusted, Is.False);
            Assert.That(registeredClient.NotificationsId, Is.Not.Null, "No NotificationId id response");
            Assert.That(registeredClient.Pin, Is.Null);
            Assert.That(registeredClient.Registered, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromMinutes(10)));
        }
    }

    class RegisterithOutPartnerIdTests : ClientsBaseTest
    {
        [Test]
        [Category("Clients"), Category("ClientAccount"), Category("ServiceAll")]
        public void RegisterNewUserWithOutPartnerIdTest()
        {
            var register = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration);
            Assert.That(register.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var registeredClient = register.GetResponseObject();
            Assert.That(registeredClient, Is.EqualTo(clientRegistration));

            Assert.That(registeredClient.Id, Is.Not.Null, "No Id in response");
            Assert.That(registeredClient.IsReviewAccount, Is.False);
            Assert.That(registeredClient.IsTrusted, Is.False);
            Assert.That(registeredClient.NotificationsId, Is.Not.Null, "No NotificationId id response");
            Assert.That(registeredClient.Pin, Is.Null);
            Assert.That(registeredClient.Registered, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromMinutes(10)));
        }
    }

    class GenerateNotificationsIdTests : ClientsBaseTest
    {
        [Test]
        [Category("Clients"), Category("ClientAccount"), Category("ServiceAll")]
        public void GenerateNotificationsIdTest()
        {
            clientRegistration.PartnerId = partner.PublicId;
            var account = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration).GetResponseObject();

            var notificationsId = lykkeApi.ClientAccount.Clients.GenerateNotificationsId(account.Id);
            Assert.That(notificationsId.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(notificationsId.GetResponseObject(), Is.Not.EqualTo(account.NotificationsId), 
                "Same NotificationsId aftre generated new");
            Assert.That(lykkeApi.ClientAccount.ClientAccountInformation.GetClientAccountInformation(account.Id)
                .GetResponseObject().NotificationsId, Is.EqualTo(notificationsId.GetResponseObject()),
                "NotificationsId has not been changed");
        }
    }

    class GenerateNotificationsIdWithOutPartnerTests : ClientsBaseTest
    {
        [Test]
        [Category("Clients"), Category("ClientAccount"), Category("ServiceAll")]
        public void GenerateNotificationsIdWithOutPartnerTest()
        {
            var account = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration).GetResponseObject();

            var notificationsId = lykkeApi.ClientAccount.Clients.GenerateNotificationsId(account.Id);
            Assert.That(notificationsId.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(notificationsId.GetResponseObject(), Is.Not.EqualTo(account.NotificationsId),
                "Same NotificationsId aftre generated new");
            Assert.That(lykkeApi.ClientAccount.ClientAccountInformation.GetClientAccountInformation(account.Id)
                .GetResponseObject().NotificationsId, Is.EqualTo(notificationsId.GetResponseObject()),
                "NotificationsId has not been changed");
        }
    }
}
