using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Net;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.DataGenerators;
using LykkeAutomationPrivate.Resources.ClientAccountResource;
using FluentAssertions;

namespace AFTests.PrivateApiTests
{
    class WithPartnerBase : PrivateApiBaseTest
    {
        protected Partner partner;
        protected Partners partnersApi;

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

        protected int Compare(Partner x, Partner y)
        {
            if (x.PublicId == y.PublicId && x.InternalId == y.InternalId)
                return 0;
            return -1;
        }
    }

    class PostPartnersTests : WithPartnerBase
    {
        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void CreatePartnerTest()
        {
            var postPartners = partnersApi.PostPartners(partner);
            Assert.That(postPartners.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(partnersApi.GetPartners().GetResponseObject(), 
                Does.Contain(partner).Using<Partner>(Compare));
        }
    }

    class GetPartnersTests : WithPartnerBase
    {
        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetPartnersTest()
        {
            partnersApi.PostPartners(partner);

            var getPartners = partnersApi.GetPartners();
            Assert.That(getPartners.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartners.GetResponseObject(),
                Does.Contain(partner).Using<Partner>(Compare));
        }
    }

    class DeletePartnersRemovePartnerTests : WithPartnerBase
    {
        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeletePartnerTest()
        {
            partnersApi.PostPartners(partner);
            Assert.That(partnersApi.GetPartners().GetResponseObject(),
                Does.Contain(partner).Using<Partner>(Compare));

            var deletePartner = partnersApi.DeleteRemovePartner(partner.InternalId);
            Assert.That(deletePartner.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(partnersApi.GetPartners().GetResponseObject(),
                Does.Not.Contain(partner).Using<Partner>(Compare));
        }
    }

    class PutPartnersTests : WithPartnerBase
    {
        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PutPartnerTest()
        {
            var putPartners = partnersApi.PutPartners(partner);
            Assert.That(putPartners.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(partnersApi.GetPartners().GetResponseObject(),
                Does.Contain(partner).Using<Partner>(Compare));
        }
    }

    class GetPartnersAllTests : WithPartnerBase
    {
        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetPartnersAllTest()
        {
            partnersApi.PostPartners(partner);

            var getPartnersAll = partnersApi.GetPartnersAll();
            Assert.That(getPartnersAll.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnersAll.GetResponseObject(),
                Does.Contain(partner).Using<Partner>(Compare));
        }
    }

    class PostPartnersFilterByPublicIds : WithPartnerBase
    {
        Partner secondPartner;

        [OneTimeSetUp]
        public void CreatePartners()
        {
            secondPartner = new Partner().GetTestModel();
            partnersApi.PostPartners(partner);
            partnersApi.PostPartners(secondPartner);
        }

        [OneTimeTearDown]
        public void RemoveSecondPartner() => partnersApi.DeleteRemovePartner(secondPartner.InternalId);

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostExistedPartnersBySinglePublicIdTest()
        {
            var ids = new List<string>() { partner.PublicId };
            var getPartnersByPublicId = partnersApi.PostPartnersByPublicIds(ids);
            Assert.That(getPartnersByPublicId.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnersByPublicId.GetResponseObject(), Has.Count.EqualTo(1));
            Assert.That(getPartnersByPublicId.GetResponseObject(),
                Does.Contain(partner).Using<Partner>(Compare));
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostExistedPartnersByFewPublicIdsTest()
        {
            var ids = new List<string>() { partner.PublicId, secondPartner.PublicId };
            var getPartnersByPublicId = partnersApi.PostPartnersByPublicIds(ids);
            Assert.That(getPartnersByPublicId.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnersByPublicId.GetResponseObject(), Has.Count.EqualTo(2));
            Assert.That(getPartnersByPublicId.GetResponseObject(),
                Does.Contain(partner).Using<Partner>(Compare)
                .And.Contain(secondPartner).Using<Partner>(Compare));
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostPartnersByExistedAndNoExistedPublicIdsTest()
        {
            var nonExistedPartner = new Partner().GetTestModel();
            var ids = new List<string>() { partner.PublicId, nonExistedPartner.PublicId };
            var getPartnersByPublicId = partnersApi.PostPartnersByPublicIds(ids);
            Assert.That(getPartnersByPublicId.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnersByPublicId.GetResponseObject(), Has.Count.EqualTo(1));
            Assert.That(getPartnersByPublicId.GetResponseObject(),
                Does.Contain(partner).Using<Partner>(Compare));
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostNotExistedPartnersByPublicIdsTest()
        {
            var nonExistedPartner = new Partner().GetTestModel();
            var ids = new List<string>() { nonExistedPartner.PublicId };
            var getPartnersByPublicId = partnersApi.PostPartnersByPublicIds(ids);
            Assert.That(getPartnersByPublicId.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnersByPublicId.GetResponseObject(), Has.Count.EqualTo(0));
        }
    }
}
