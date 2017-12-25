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
    class CreateGetRemovePartnerTests : PrivateApiBaseTest
    {
        Partner partner;
        Partners partnersApi;

        [OneTimeSetUp]
        public void GenerateTestPartnerAndApi()
        {
            partner = new Partner().GetTestModel();
            partnersApi = lykkeApi.ClientAccount.Partners;
        }

        [OneTimeTearDown]
        public void RemovePartner()
        {
            partnersApi.DeleteRemovePartner(partner.InternalId);
        }

        [Test]
        [Order(1)]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void CreatePartnerTest()
        {
            var postPartners = partnersApi.PostPartners(partner);
            Assert.That(postPartners.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        [Order(2)]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetPartnersListTest()
        {
            var getPartners = partnersApi.GetPartners();
            Assert.That(getPartners.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartners.GetResponseObject(), Does.Contain(partner));
        }

        [Test]
        [Order(3)]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void RemovePartnerTest()
        {
            var deletePartner = partnersApi.DeleteRemovePartner(partner.InternalId);
            Assert.That(deletePartner.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }
    }
}
