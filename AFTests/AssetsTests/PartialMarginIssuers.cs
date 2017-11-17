using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Xunit;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using XUnitTestData.Entitites.ApiV2.Assets;

namespace AFTests.AssetsTests
{
    [Trait("Category", "FullRegression")]
    [Trait("Category", "AssetsService")]
    public partial class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "MarginIssuers")]
        [Trait("Category", "MarginIssuersGet")]
        public async void GetAllMarginIssuers()
        {
            string url = ApiPaths.MARGIN_ISSUERS_PATH;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            MarginIssuerReturnDTO parsedResponse = JsonUtils.DeserializeJson<MarginIssuerReturnDTO>(response.ResponseJson);

            for (int i = 0; i < fixture.AllMarginIssuersFromDB.Count; i++)
            {
                fixture.AllMarginIssuersFromDB[i].ShouldBeEquivalentTo(parsedResponse.Items.Where(p => p.Id == fixture.AllMarginIssuersFromDB[i].Id).FirstOrDefault(),
                o => o.ExcludingMissingMembers());
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "MarginIssuers")]
        [Trait("Category", "MarginIssuersGet")]
        public async void GetSingleMarginIssuer()
        {
            string url = ApiPaths.MARGIN_ISSUERS_PATH + "/" + fixture.TestMarginIssuer.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            MarginIssuerDTO parsedResponse = JsonUtils.DeserializeJson<MarginIssuerDTO>(response.ResponseJson);

            fixture.TestMarginIssuer.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "MarginIssuers")]
        [Trait("Category", "MarginIssuersGet")]
        public async void CheckIfMarginIssuerExists()
        {
            string url = ApiPaths.MARGIN_ISSUERS_PATH + "/" + fixture.TestMarginIssuer.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);

            Assert.True(parsedResponse);
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Issuers")]
        [Trait("Category", "IssuersPost")]
        public async void CreateMarginIssuer()
        {
            MarginIssuerDTO createdIssuer = await fixture.CreateTestMarginIssuer();
            Assert.NotNull(createdIssuer);

            MarginIssuerEntity entity = await fixture.MarginIssuerManager.TryGetAsync(createdIssuer.Id) as MarginIssuerEntity;
            entity.ShouldBeEquivalentTo(createdIssuer, o => o
            .ExcludingMissingMembers());
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Issuers")]
        [Trait("Category", "IssuersPut")]
        public async void UpdateMarginIssuer()
        {
            string url = ApiPaths.MARGIN_ISSUERS_PATH;
            MarginIssuerDTO editIssuer = new MarginIssuerDTO()
            {
                Id = fixture.TestMarginIssuerUpdate.Id,
                IconUrl = fixture.TestMarginIssuerUpdate.IconUrl + Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest,
                Name = fixture.TestMarginIssuerUpdate.Name + Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest,
            };
            string editParam = JsonUtils.SerializeObject(editIssuer);

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, editParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.OK); //HttpStatusCode.NoContent

            MarginIssuerEntity entity = await fixture.MarginIssuerManager.TryGetAsync(fixture.TestMarginIssuerUpdate.Id) as MarginIssuerEntity;
            entity.ShouldBeEquivalentTo(editIssuer, o => o
            .ExcludingMissingMembers());

        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "Issuers")]
        [Trait("Category", "IssuersDelete")]
        public async void DeleteMarginIssuer()
        {
            string url = ApiPaths.MARGIN_ISSUERS_PATH + "/" + fixture.TestMarginIssuerDelete.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            MarginIssuerEntity entity = await fixture.MarginIssuerManager.TryGetAsync(fixture.TestMarginIssuerDelete.Id) as MarginIssuerEntity;
            Assert.Null(entity);
        }
    }
}
