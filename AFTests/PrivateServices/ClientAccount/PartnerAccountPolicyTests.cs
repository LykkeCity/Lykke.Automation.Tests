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
    class PartnerAccountPolicyBaseTest : PrivateApiBaseTest
    {
        protected Partner partner;

        [OneTimeSetUp]
        public void CreatePartner()
        {
            partner = new Partner().GetTestModel();
            lykkeApi.ClientAccount.Partners.PostPartners(partner);
        }

        [OneTimeTearDown]
        public void RemovePartner()
        {
            lykkeApi.ClientAccount.Partners.DeleteRemovePartner(partner.InternalId);
        }
    }

    class PartnerAccountPolicyTests : PartnerAccountPolicyBaseTest
    {
        //TODO: 404 for now
        [Test]
        [Category("PartnerAccountPolicy"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetPartnerAccountTest()
        {
            var getPartnerAccountPolicy = lykkeApi.ClientAccount.PartnerAccountPolicy
                .GetPartnerAccountPolicy(partner.PublicId);
            Assert.That(getPartnerAccountPolicy.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            var partnerAccountPolicy = getPartnerAccountPolicy.GetResponseObject();
            Assert.That(partnerAccountPolicy.PublicId, Is.EqualTo(partner.PublicId));
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
