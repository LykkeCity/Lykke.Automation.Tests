using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Net;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.DataGenerators;
using LykkeAutomationPrivate.Resources.ClientAccountResource;

namespace AFTests.PrivateApiTests
{
    class PartnerAccountPolicyTests : PrivateApiBaseTest
    {
        string partnerId = "NewTestPartner";

        [Test]
        [Category("PartnerAccountPolicy"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetPartnerAccountTest()
        {
            var getPartnerAccount = lykkeApi.ClientAccount.PartnerAccountPolicy.GetPartnerAccountPolicy(partnerId);
            Assert.That(getPartnerAccount.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var partner = getPartnerAccount.GetResponseObject();
            Assert.That(partner.PublicId, Is.EqualTo(partnerId));
            //TODO: Add more assertions?
        }

        [Test]
        [Category("PartnerAccountPolicy"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetNonExistencePartnerAccountTest()
        {
            var getPartnerAccount = lykkeApi.ClientAccount.PartnerAccountPolicy
                .GetPartnerAccountPolicy(Guid.NewGuid().ToString("N"));
            Assert.That(getPartnerAccount.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }
    }
}
