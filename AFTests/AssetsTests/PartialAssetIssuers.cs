using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Xunit;
using XUnitTestCommon.Utils;
using XUnitTestCommon;
using XUnitTestData.Entitites.ApiV2.Assets;

namespace AFTests.AssetsTests
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "AssetsService")]
    public partial class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Issuers")]
        [Trait("Category", "IssuersGet")]
        public async void GetAllIssuers()
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

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Issuers")]
        [Trait("Category", "IssuersGet")]
        public async void GetSingleIssuers()
        {
            string url = ApiPaths.ISSUERS_BASE_PATH + "/" + fixture.TestAssetIssuer.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            AssetIssuerDTO parsedResponse = JsonUtils.DeserializeJson<AssetIssuerDTO>(response.ResponseJson);

            fixture.TestAssetIssuer.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Issuers")]
        [Trait("Category", "IssuersGet")]
        public async void CheckIfIssuerExists()
        {
            string url = ApiPaths.ISSUERS_BASE_PATH + "/" + fixture.TestAssetIssuer.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);

            Assert.True(parsedResponse);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Issuers")]
        [Trait("Category", "IssuersPost")]
        public async void CreateAssetIssuer()
        {
            AssetIssuerDTO createdIssuer = await fixture.CreateTestAssetIssuer();
            Assert.NotNull(createdIssuer);

            AssetIssuersEntity entity = await fixture.AssetIssuersManager.TryGetAsync(createdIssuer.Id) as AssetIssuersEntity;
            entity.ShouldBeEquivalentTo(createdIssuer, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Issuers")]
        [Trait("Category", "IssuersPut")]
        public async void UpdateAssetIssuer()
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

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Issuers")]
        [Trait("Category", "IssuersDelete")]
        public async void DeleteAssetIssuer()
        {
            string url = ApiPaths.ISSUERS_BASE_PATH + "/" + fixture.TestAssetIssuerDelete.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            AssetIssuersEntity entity = await fixture.AssetIssuersManager.TryGetAsync(fixture.TestAssetIssuerDelete.Id) as AssetIssuersEntity;
            Assert.Null(entity);
        }
    }
}
