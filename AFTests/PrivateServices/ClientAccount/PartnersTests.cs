using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Net;
using LykkeAutomationPrivate.Models.ClientAccount.Models;
using LykkeAutomationPrivate.DataGenerators;
using LykkeAutomationPrivate.Resources.ClientAccountResource;
using XUnitTestCommon.TestsData;

namespace AFTests.PrivateApiTests
{
    class PartnerComparer : IComparer<Partner>
    {
        public int Compare(Partner x, Partner y)
        {
            if (x.PublicId == y.PublicId && x.InternalId == y.InternalId &&
                x.Name == y.Name && x.AssetPrefix == y.AssetPrefix &&
                x.RegisteredUsersCount == y.RegisteredUsersCount)
                return 0;
            return -1;
        }
    }

    class PartnerBaseTest : PrivateApiBaseTest
    {
        protected Partner partner;
        protected Partners api;
        protected IComparer<Partner> partnerComparer = new PartnerComparer();

        [OneTimeSetUp]
        public void GenerateTestPartnerAndApi()
        {
            partner = new Partner().GetTestModel();
            api = lykkeApi.ClientAccount.Partners;
        }

        [OneTimeTearDown]
        public void RemovePartner()
        {
            api.DeleteRemovePartner(partner.InternalId);
        }
    }

    class PostPartnersTests : PartnerBaseTest
    {
        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void CreatePartnerTest()
        {
            var postPartners = api.PostPartners(partner);
            Assert.That(postPartners.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetPartners().GetResponseObject(), 
                Does.Contain(partner).Using(partnerComparer));
        }
    }

    class GetPartnersTests : PartnerBaseTest
    {
        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetPartnersTest()
        {
            api.PostPartners(partner);

            var getPartners = api.GetPartners();
            Assert.That(getPartners.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartners.GetResponseObject(),
                Does.Contain(partner).Using(partnerComparer));
        }
    }

    class DeletePartnersRemovePartnerTests : PartnerBaseTest
    {
        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeletePartnerTest()
        {
            api.PostPartners(partner);
            Assert.That(api.GetPartners().GetResponseObject(),
                Does.Contain(partner).Using(partnerComparer));

            var deletePartner = api.DeleteRemovePartner(partner.InternalId);
            Assert.That(deletePartner.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetPartners().GetResponseObject(),
                Does.Not.Contain(partner).Using(partnerComparer));
        }
    }

    class PutPartnersTests : PartnerBaseTest
    {
        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PutPartnerTest()
        {
            var putPartners = api.PutPartners(partner);
            Assert.That(putPartners.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetPartners().GetResponseObject(),
                Does.Contain(partner).Using(partnerComparer));
        }
    }

    class GetPartnersAllTests : PartnerBaseTest
    {
        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetPartnersAllTest()
        {
            api.PostPartners(partner);

            var getPartnersAll = api.GetPartnersAll();
            Assert.That(getPartnersAll.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnersAll.GetResponseObject(),
                Does.Contain(partner).Using(partnerComparer));
        }
    }

    class PostPartnersFilterByPublicIds : PartnerBaseTest
    {
        Partner secondPartner;

        [OneTimeSetUp]
        public void CreatePartners()
        {
            secondPartner = new Partner().GetTestModel();
            api.PostPartners(partner);
            api.PostPartners(secondPartner);
        }

        [OneTimeTearDown]
        public void RemoveSecondPartner() => api.DeleteRemovePartner(secondPartner.InternalId);

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostExistedPartnersBySinglePublicIdTest()
        {
            var ids = new List<string>() { partner.PublicId };
            var postPartnersByPublicId = api.PostPartnersByPublicIds(ids);
            Assert.That(postPartnersByPublicId.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(postPartnersByPublicId.GetResponseObject(), Has.Count.EqualTo(1));
            Assert.That(postPartnersByPublicId.GetResponseObject(),
                Does.Contain(partner).Using(partnerComparer));
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostExistedPartnersByFewPublicIdsTest()
        {
            var ids = new List<string>() { partner.PublicId, secondPartner.PublicId };
            var getPartnersByPublicId = api.PostPartnersByPublicIds(ids);
            Assert.That(getPartnersByPublicId.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnersByPublicId.GetResponseObject(), Has.Count.EqualTo(2));
            Assert.That(getPartnersByPublicId.GetResponseObject(),
                Does.Contain(partner).Using(partnerComparer)
                .And.Contain(secondPartner).Using(partnerComparer));
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostPartnersByExistedAndNoExistedPublicIdsTest()
        {
            var nonExistedPartner = new Partner().GetTestModel();
            var ids = new List<string>() { partner.PublicId, nonExistedPartner.PublicId };
            var getPartnersByPublicId = api.PostPartnersByPublicIds(ids);
            Assert.That(getPartnersByPublicId.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnersByPublicId.GetResponseObject(), Has.Count.EqualTo(1));
            Assert.That(getPartnersByPublicId.GetResponseObject(),
                Does.Contain(partner).Using(partnerComparer));
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostNotExistedPartnersByPublicIdsTest()
        {
            var nonExistedPartner = new Partner().GetTestModel();
            var ids = new List<string>() { nonExistedPartner.PublicId };
            var getPartnersByPublicId = api.PostPartnersByPublicIds(ids);
            Assert.That(getPartnersByPublicId.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnersByPublicId.GetResponseObject(), Has.Count.EqualTo(0));
        }
    }

    class PostPartnersFilterByIds : PartnerBaseTest
    {
        Partner secondPartner;

        [OneTimeSetUp]
        public void CreatePartners()
        {
            secondPartner = new Partner().GetTestModel();
            api.PostPartners(partner);
            api.PostPartners(secondPartner);
        }

        [OneTimeTearDown]
        public void RemoveSecondPartner() => api.DeleteRemovePartner(secondPartner.InternalId);

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostExistedPartnersBySingleIdTest()
        {
            var internalIds = new List<string>() { partner.InternalId };
            var getPartnersById = api.PostPartnersByIds(internalIds);
            Assert.That(getPartnersById.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnersById.GetResponseObject(), Has.Count.EqualTo(1));
            Assert.That(getPartnersById.GetResponseObject(),
                Does.Contain(partner).Using(partnerComparer));
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostExistedPartnersByFewIdsTest()
        {
            var internalIds = new List<string>() { partner.InternalId, secondPartner.InternalId };
            var getPartnersById = api.PostPartnersByIds(internalIds);
            Assert.That(getPartnersById.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnersById.GetResponseObject(), Has.Count.EqualTo(2));
            Assert.That(getPartnersById.GetResponseObject(),
                Does.Contain(partner).Using(partnerComparer)
                .And.Contain(secondPartner).Using(partnerComparer));
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostPartnersByExistedAndNoExistedIdsTest()
        {
            var nonExistedPartner = new Partner().GetTestModel();
            var internalIds = new List<string>() { partner.InternalId, nonExistedPartner.InternalId };
            var getPartnersById = api.PostPartnersByIds(internalIds);
            Assert.That(getPartnersById.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnersById.GetResponseObject(), Has.Count.EqualTo(1));
            Assert.That(getPartnersById.GetResponseObject(),
                Does.Contain(partner).Using(partnerComparer));
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostNotExistedPartnersByIdsTest()
        {
            var nonExistedPartner = new Partner().GetTestModel();
            var internalIds = new List<string>() { nonExistedPartner.InternalId };
            var getPartnersById = api.PostPartnersByIds(internalIds);
            Assert.That(getPartnersById.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnersById.GetResponseObject(), Has.Count.EqualTo(0));
        }
    }

    class GetPartnerByIdTest : PartnerBaseTest
    {
        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetExistingPatnerByIdTest()
        {
            api.PostPartners(partner);
            var getPartnerById = api.GetPartnerById(partner.InternalId);
            Assert.That(getPartnerById.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnerById.GetResponseObject(),
                Is.EqualTo(partner).Using(partnerComparer));
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetNonExistingPatnerByIdTest()
        {
            var nonExistingPartner = new Partner().GetTestModel();
            var getPartnerById = api.GetPartnerById(nonExistingPartner.InternalId);
            Assert.That(getPartnerById.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getPartnerById.GetResponseObject(), Has.Count.EqualTo(0));
        }
    }

    class GetUsersCountTest : PartnerBaseTest
    {
        ClientAccountInformation clientAccount;

        [OneTimeSetUp]
        public void CreatePartnerAndClient()
        {
            api.PostPartners(partner);
        }

        [OneTimeTearDown]
        public void RemoveClient()
        {
            lykkeApi.ClientAccount.ClientAccount.DeleteClientAccount(clientAccount.Id);
        }

        [Test]
        [Order(1)]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetNewPartnerUsersCountTest()
        {
            var getUserCount = api.GetPartnerUserCount(partner.PublicId);
            Assert.That(getUserCount.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getUserCount.GetResponseObject().Count, Is.EqualTo(0));
        }

        [Test]
        [Order(2)]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void GetPartnerWithUsersCountTest()
        {
            var clientRegistration = new ClientRegistrationModel().GetTestModel(partner.PublicId);
            clientAccount = lykkeApi.ClientAccount.Clients.PostRegister(clientRegistration).GetResponseObject();

            var getUserCount = api.GetPartnerUserCount(partner.PublicId);
            Assert.That(getUserCount.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(getUserCount.GetResponseObject().Count, Is.EqualTo(1));
        }
    }

    class GetPartnersFindByPublicIdsAsyncTests : PartnerBaseTest
    {
        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostExistedPartnersBySinglePublicIdAsyncTest()
        {
            api.PostPartners(partner);
            var postParnersFilterByPublicIdsAsync = api
                .PostPartnersByPublicIdsAsync(new PartnersPublicIdsRequestModel()
                {
                    PublicIds = new List<string> { partner.PublicId }
                });
            Assert.That(postParnersFilterByPublicIdsAsync.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(postParnersFilterByPublicIdsAsync.GetResponseObject(), Has.Count.EqualTo(1));
            Assert.That(postParnersFilterByPublicIdsAsync.GetResponseObject(),
                Does.Contain(partner).Using(partnerComparer));
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void PostNotExistedPartnersBySinglePublicIdAsyncTest()
        {
            var notExistedPartner = new Partner().GetTestModel();
            var postParnersFilterByPublicIdsAsync = api
                .PostPartnersByPublicIdsAsync(new PartnersPublicIdsRequestModel()
                {
                    PublicIds = new List<string> { notExistedPartner.PublicId }
                });
            Assert.That(postParnersFilterByPublicIdsAsync.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(postParnersFilterByPublicIdsAsync.GetResponseObject(), Has.Count.EqualTo(0));
        }
    }

    class DeleteRemovePartnerTest : PartnerBaseTest
    {
        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void DeletePartnerTest()
        {
            api.PostPartners(partner);
            var deleteRemovePartner = api.DeleteRemovePartner(partner.InternalId);
            Assert.That(deleteRemovePartner.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.PostPartnersByPublicIds(new List<string> { partner.PublicId })
                .GetResponseObject(), Has.Count.EqualTo(0));
        }
    }

    class PostUpdatePartnerTests : PartnerBaseTest
    {
        [OneTimeSetUp]
        public void CreatePartner()
        {
            api.PostPartners(partner);
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void UpdatePartnerChangeNameTest()
        {
            var newName = TestData.GenerateLetterString(7);
            partner.Name = newName;

            var postUpdatePartner = api.PostUpdatePartner(partner);
            Assert.That(postUpdatePartner.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetPartnerById(partner.InternalId).GetResponseObject(), 
                Is.EqualTo(partner).Using(partnerComparer));
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void UpdatePartnersChangeAssetPrefix()
        {
            var newAssetPrefix = TestData.GenerateLetterString(5) + "_";
            partner.AssetPrefix = newAssetPrefix;

            var postUpdatePartner = api.PostUpdatePartner(partner);
            Assert.That(postUpdatePartner.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetPartnerById(partner.InternalId).GetResponseObject(),
                Is.EqualTo(partner).Using(partnerComparer));
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void UpdatePartnersChangeRegisteredUsersCount()
        {
            var newRegisteredUsersCount = 7;
            partner.RegisteredUsersCount = newRegisteredUsersCount;

            var postUpdatePartner = api.PostUpdatePartner(partner);
            Assert.That(postUpdatePartner.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetPartnerById(partner.InternalId).GetResponseObject(),
                Is.EqualTo(partner).Using(partnerComparer));
        }

        [Test]
        [Category("Partners"), Category("ClientAccount"), Category("ServiceAll")]
        public void UpdatePartnersChangePublicId()
        {
            var newPublicId = TestData.GenerateLetterString(10);
            partner.PublicId = newPublicId;

            var postUpdatePartner = api.PostUpdatePartner(partner);
            Assert.That(postUpdatePartner.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(api.GetPartnerById(partner.InternalId).GetResponseObject(),
                Is.EqualTo(partner).Using(partnerComparer));
        }
    }
}
