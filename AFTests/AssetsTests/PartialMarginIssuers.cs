using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Linq;
using System.Net;
using NUnit.Framework;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
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
        [Category("MarginIssuers")]
        [Category("MarginIssuersGet")]
        public async Task GetAllMarginIssuers()
        {
            string url = ApiPaths.MARGIN_ISSUERS_PATH;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            MarginIssuerReturnDTO parsedResponse = JsonUtils.DeserializeJson<MarginIssuerReturnDTO>(response.ResponseJson);

            for (int i = 0; i < this.AllMarginIssuersFromDB.Count; i++)
            {
                this.AllMarginIssuersFromDB[i].ShouldBeEquivalentTo(parsedResponse.Items.Where(p => p.Id == this.AllMarginIssuersFromDB[i].Id).FirstOrDefault(),
                o => o.ExcludingMissingMembers());
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginIssuers")]
        [Category("MarginIssuersGet")]
        public async Task GetSingleMarginIssuer()
        {
            string url = ApiPaths.MARGIN_ISSUERS_PATH + "/" + this.TestMarginIssuer.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            MarginIssuerDTO parsedResponse = JsonUtils.DeserializeJson<MarginIssuerDTO>(response.ResponseJson);

            this.TestMarginIssuer.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("MarginIssuers")]
        [Category("MarginIssuersGet")]
        public async Task CheckIfMarginIssuerExists()
        {
            string url = ApiPaths.MARGIN_ISSUERS_PATH + "/" + this.TestMarginIssuer.Id + "/exists";
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
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
            MarginIssuerDTO createdIssuer = await this.CreateTestMarginIssuer();
            Assert.NotNull(createdIssuer);

            MarginIssuerEntity entity = await this.MarginIssuerManager.TryGetAsync(createdIssuer.Id) as MarginIssuerEntity;
            entity.ShouldBeEquivalentTo(createdIssuer, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("Issuers")]
        [Category("IssuersPut")]
        public async Task UpdateMarginIssuer()
        {
            string url = ApiPaths.MARGIN_ISSUERS_PATH;
            MarginIssuerDTO editIssuer = new MarginIssuerDTO()
            {
                Id = this.TestMarginIssuerUpdate.Id,
                IconUrl = this.TestMarginIssuerUpdate.IconUrl + Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest,
                Name = this.TestMarginIssuerUpdate.Name + Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest,
            };
            string editParam = JsonUtils.SerializeObject(editIssuer);

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, editParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.OK); //HttpStatusCode.NoContent

            MarginIssuerEntity entity = await this.MarginIssuerManager.TryGetAsync(this.TestMarginIssuerUpdate.Id) as MarginIssuerEntity;
            entity.ShouldBeEquivalentTo(editIssuer, o => o
            .ExcludingMissingMembers());

        }

        [Test]
        [Category("Smoke")]
        [Category("Issuers")]
        [Category("IssuersDelete")]
        public async Task DeleteMarginIssuer()
        {
            string url = ApiPaths.MARGIN_ISSUERS_PATH + "/" + this.TestMarginIssuerDelete.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            MarginIssuerEntity entity = await this.MarginIssuerManager.TryGetAsync(this.TestMarginIssuerDelete.Id) as MarginIssuerEntity;
            Assert.Null(entity);
        }
    }
}
