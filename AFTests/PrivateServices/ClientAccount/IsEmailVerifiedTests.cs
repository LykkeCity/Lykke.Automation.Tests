using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using NUnit.Framework;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.DataGenerators;
using LykkeAutomationPrivate.Resources.ClientAccountResource;

namespace AFTests.PrivateApiTests
{
    class IsEmailVerifiedTest : PrivateApiBaseTest
    {
        string partnerId = "NewTestPartner";
        ClientAccountInformation clientAccount;

        [SetUp]
        public void CreateClient()
        {
            ClientRegistrationModel clientRegistration = new ClientRegistrationModel().GetTestModel();
            clientRegistration.PartnerId = partnerId;
            clientAccount = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration).GetResponseObject();
        }

        [TearDown]
        public void DeleteClient() =>
            lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(clientAccount.Id);


        [Test]
        [Category("IsEmailVerified"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetIsEmailVerifiedTest()
        {
            var getIsEmailVerified = lykkeApi.ClientAccount.IsEmailVerified
                .PostIsEmailVerified(new VerifiedEmailModel()
                {
                    Email = clientAccount.Email,
                    PartnerId = partnerId
                });
            Assert.That(getIsEmailVerified.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getIsEmailVerified.GetResponseObject(), Is.False, 
                "New registered client email is verified");
        }
    }
}
