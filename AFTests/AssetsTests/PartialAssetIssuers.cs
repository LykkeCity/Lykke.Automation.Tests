using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using NUnit.Framework;
using XUnitTestCommon.Utils;
using XUnitTestCommon;
using System.Threading.Tasks;
using XUnitTestData.Entities.Assets;

namespace AFTests.AssetsTests
{
    [Category("FullRegression")]
    [Category("AssetsService")]
    public partial class AssetsTest
    {
        [Test]
        [Category("Smoke")]
        [Category("Issuers")]
        [Category("IssuersGet")]
        public async Task GetAllIssuers()
        {
            string url = ApiPaths.ISSUERS_BASE_PATH;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<AssetIssuerDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetIssuerDTO>>(response.ResponseJson);

            for (int i = 0; i < fixture.AllAssetIssuersFromDB.Count; i++)
            {
                fixture.AllAssetIssuersFromDB[i].ShouldBeEquivalentTo(parsedResponse.Where(p => p.Id == fixture.AllAssetIssuersFromDB[i].Id).FirstOrDefault(),
                o => o.ExcludingMissingMembers());
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("Issuers")]
        [Category("IssuersGet")]
        public async Task GetSingleIssuers()
        {
            string url = ApiPaths.ISSUERS_BASE_PATH + "/" + fixture.TestAssetIssuer.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            AssetIssuerDTO parsedResponse = JsonUtils.DeserializeJson<AssetIssuerDTO>(response.ResponseJson);

            fixture.TestAssetIssuer.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("Issuers")]
        [Category("IssuersGet")]
        public async Task CheckIfIssuerExists()
        {
            string url = ApiPaths.ISSUERS_BASE_PATH + "/" + fixture.TestAssetIssuer.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);

            Assert.True(parsedResponse);
        }

        [Test]
        [Category("Smoke")]
        [Category("Issuers")]
        [Category("IssuersPost")]
        public async Task CreateAssetIssuer()
        {
            AssetIssuerDTO createdIssuer = await fixture.CreateTestAssetIssuer();
            Assert.NotNull(createdIssuer);

            AssetIssuersEntity entity = await fixture.AssetIssuersManager.TryGetAsync(createdIssuer.Id) as AssetIssuersEntity;
            entity.ShouldBeEquivalentTo(createdIssuer, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("Issuers")]
        [Category("IssuersPut")]
        public async Task UpdateAssetIssuer()
        {
            string url = ApiPaths.ISSUERS_BASE_PATH;
            AssetIssuerDTO editIssuer = new AssetIssuerDTO()
            {
                Id = fixture.TestAssetIssuerUpdate.Id,
                IconUrl = fixture.TestAssetIssuerUpdate.IconUrl + Helpers.Random.Next(1000,9999).ToString() + GlobalConstants.AutoTest,
                Name = fixture.TestAssetIssuerUpdate.Name + Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest,
            };
            string editParam = JsonUtils.SerializeObject(editIssuer);

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, editParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            AssetIssuersEntity entity = await fixture.AssetIssuersManager.TryGetAsync(fixture.TestAssetIssuerUpdate.Id) as AssetIssuersEntity;
            entity.ShouldBeEquivalentTo(editIssuer, o => o
            .ExcludingMissingMembers());

        }

        [Test]
        [Category("Smoke")]
        [Category("Issuers")]
        [Category("IssuersDelete")]
        public async Task DeleteAssetIssuer()
        {
            string url = ApiPaths.ISSUERS_BASE_PATH + "/" + fixture.TestAssetIssuerDelete.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            AssetIssuersEntity entity = await fixture.AssetIssuersManager.TryGetAsync(fixture.TestAssetIssuerDelete.Id) as AssetIssuersEntity;
            Assert.Null(entity);
        }
    }
}
