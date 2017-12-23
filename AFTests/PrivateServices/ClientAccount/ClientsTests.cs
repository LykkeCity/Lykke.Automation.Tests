using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.DataGenerators;
using LykkeAutomationPrivate.Validators;

namespace AFTests.PrivateApiTests
{
    class Register : PrivateApiBaseTest
    {
        [Test]
        [Category("Clients"), Category("ClientAccount"), Category("ServiceAll")]
        public void RegisterNewUserTest()
        {
            string partnerId = "NewTestPartner";
            ClientRegistrationModel clientRegistration = new ClientRegistrationModel().GetTestModel(partnerId);

            var register = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration);
            Assert.That(register.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var registeredClient = register.GetResponseObject();
            Assert.That(registeredClient.Email, Is.EqualTo(clientRegistration.Email), "Wrong email");
            Assert.That(registeredClient.Id, Is.Not.Null, "No Id in response");
            Assert.That(registeredClient.IsReviewAccount, Is.False);
            Assert.That(registeredClient.IsTrusted, Is.False);
            Assert.That(registeredClient.NotificationsId, Is.Not.Null, "No NotificationId id response");
            Assert.That(registeredClient.PartnerId, Is.EqualTo(partnerId), "Wrong PartnerId");
            Assert.That(registeredClient.Pin, Is.Null);
            Assert.That(registeredClient.Registered, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromMinutes(10)));
        }

        [Test]
        [Category("Clients"), Category("ClientAccount"), Category("ServiceAll")]
        public void RegisterNewUserWithOutPartnerIdTest()
        {
            ClientRegistrationModel clientRegistration = new ClientRegistrationModel().GetTestModel();

            var register = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration);
            Assert.That(register.StatusCode, Is.EqualTo(HttpStatusCode.OK));

            var registeredClient = register.GetResponseObject();
            Assert.That(registeredClient.Email, Is.EqualTo(clientRegistration.Email), "Wrong email");
            Assert.That(registeredClient.Id, Is.Not.Null, "No Id in response");
            Assert.That(registeredClient.IsReviewAccount, Is.False);
            Assert.That(registeredClient.IsTrusted, Is.False);
            Assert.That(registeredClient.NotificationsId, Is.Not.Null, "No NotificationId id response");
            Assert.That(registeredClient.PartnerId, Is.Null, "Wrong PartnerId");
            Assert.That(registeredClient.Pin, Is.Null);
            Assert.That(registeredClient.Registered, Is.EqualTo(DateTime.UtcNow).Within(TimeSpan.FromMinutes(10)));
        }
    }

    class GenerateNotificationsId : PrivateApiBaseTest
    {
        [Test]
        [Category("Clients"), Category("ClientAccount"), Category("ServiceAll")]
        public void GenerateNotificationsIdTest()
        {
            ClientRegistrationModel clientRegistration = new ClientRegistrationModel().GetTestModel();
            var account = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration).GetResponseObject();

            var notificationsId = lykkeApi.ClientAccount.Clients.GenerateNotificationsId(account.Id);
            Assert.That(notificationsId.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(account.NotificationsId, Is.Not.EqualTo(notificationsId.GetResponseObject()), 
                "Same NotificationsId aftre generated new");
            Assert.That(lykkeApi.ClientAccount.ClientAccountInformation.GetClientAccountInformation(account.Id)
                .GetResponseObject().NotificationsId, Is.EqualTo(notificationsId.GetResponseObject()),
                "NotificationsId has not been changed");
        }
    }
}
