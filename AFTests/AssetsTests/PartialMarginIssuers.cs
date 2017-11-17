using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using NUnit.Framework;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using XUnitTestData.Repositories.Assets;
using System.Threading.Tasks;

namespace AFTests.AssetsTests
{
    [Category("FullRegression")]
    [Category("AssetsService")]
    public partial class AssetsTest : AssetsTestDataFixture
    {
        [Test]
        [Category("Smoke")]
        [Category("MarginIssuers")]
        [Category("MarginIssuersGet")]
        public async Task GetAllMarginIssuers()
        {
            string url = fixture.ApiEndpointNames["marginIssuers"];
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            MarginIssuerReturnDTO parsedResponse = JsonUtils.DeserializeJson<MarginIssuerReturnDTO>(response.ResponseJson);

            for (int i = 0; i < fixture.AllMarginIssuersFromDB.Count; i++)
            {
                fixture.AllMarginIssuersFromDB[i].ShouldBeEquivalentTo(parsedResponse.Items.Where(p => p.Id == fixture.AllMarginIssuersFromDB[i].Id).FirstOrDefault(),
                o => o.ExcludingMissingMembers());
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginIssuers")]
        [Category("MarginIssuersGet")]
        public async Task GetSingleMarginIssuer()
        {
            string url = fixture.ApiEndpointNames["marginIssuers"] + "/" + fixture.TestMarginIssuer.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            MarginIssuerDTO parsedResponse = JsonUtils.DeserializeJson<MarginIssuerDTO>(response.ResponseJson);

            fixture.TestMarginIssuer.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginIssuers")]
        [Category("MarginIssuersGet")]
        public async Task CheckIfMarginIssuerExists()
        {
            string url = fixture.ApiEndpointNames["marginIssuers"] + "/" + fixture.TestMarginIssuer.Id + "/exists";
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);

            Assert.True(parsedResponse);
        }

        [Test]
        [Category("Smoke")]
        [Category("Issuers")]
        [Category("IssuersPost")]
        public async Task CreateMarginIssuer()
        {
            MarginIssuerDTO createdIssuer = await fixture.CreateTestMarginIssuer();
            Assert.NotNull(createdIssuer);

            await fixture.MarginIssuerManager.UpdateCacheAsync();
            MarginIssuerEntity entity = await fixture.MarginIssuerManager.TryGetAsync(createdIssuer.Id) as MarginIssuerEntity;
            entity.ShouldBeEquivalentTo(createdIssuer, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("Issuers")]
        [Category("IssuersPut")]
        public async Task UpdateMarginIssuer()
        {
            string url = fixture.ApiEndpointNames["marginIssuers"];
            MarginIssuerDTO editIssuer = new MarginIssuerDTO()
            {
                Id = fixture.TestMarginIssuerUpdate.Id,
                IconUrl = fixture.TestMarginIssuerUpdate.IconUrl + Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest",
                Name = fixture.TestMarginIssuerUpdate.Name + Helpers.Random.Next(1000, 9999).ToString() + "_AutoTest",
            };
            string editParam = JsonUtils.SerializeObject(editIssuer);

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, editParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.OK); //HttpStatusCode.NoContent

            await fixture.MarginIssuerManager.UpdateCacheAsync();
            MarginIssuerEntity entity = await fixture.MarginIssuerManager.TryGetAsync(fixture.TestMarginIssuerUpdate.Id) as MarginIssuerEntity;
            entity.ShouldBeEquivalentTo(editIssuer, o => o
            .ExcludingMissingMembers());

        }

        [Test]
        [Category("Smoke")]
        [Category("Issuers")]
        [Category("IssuersDelete")]
        public async Task DeleteMarginIssuer()
        {
            string url = fixture.ApiEndpointNames["marginIssuers"] + "/" + fixture.TestMarginIssuerDelete.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            await fixture.MarginIssuerManager.UpdateCacheAsync();
            MarginIssuerEntity entity = await fixture.MarginIssuerManager.TryGetAsync(fixture.TestMarginIssuerDelete.Id) as MarginIssuerEntity;
            Assert.Null(entity);
        }
    }
}
