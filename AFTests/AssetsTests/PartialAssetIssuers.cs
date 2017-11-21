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
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<AssetIssuerDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetIssuerDTO>>(response.ResponseJson);

            for (int i = 0; i < this.AllAssetIssuersFromDB.Count; i++)
            {
                this.AllAssetIssuersFromDB[i].ShouldBeEquivalentTo(parsedResponse.Where(p => p.Id == this.AllAssetIssuersFromDB[i].Id).FirstOrDefault(),
                o => o.ExcludingMissingMembers());
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("Issuers")]
        [Category("IssuersGet")]
        public async Task GetSingleIssuers()
        {
            string url = ApiPaths.ISSUERS_BASE_PATH + "/" + this.TestAssetIssuer.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            AssetIssuerDTO parsedResponse = JsonUtils.DeserializeJson<AssetIssuerDTO>(response.ResponseJson);

            this.TestAssetIssuer.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers());
        }

        [Test]
        [Category("Smoke")]
        [Category("Issuers")]
        [Category("IssuersGet")]
        public async Task CheckIfIssuerExists()
        {
            string url = ApiPaths.ISSUERS_BASE_PATH + "/" + this.TestAssetIssuer.Id + "/exists";
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
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
            AssetIssuerDTO createdIssuer = await this.CreateTestAssetIssuer();
            Assert.NotNull(createdIssuer);

            AssetIssuersEntity entity = await this.AssetIssuersManager.TryGetAsync(createdIssuer.Id) as AssetIssuersEntity;
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
                Id = this.TestAssetIssuerUpdate.Id,
                IconUrl = this.TestAssetIssuerUpdate.IconUrl + Helpers.Random.Next(1000,9999).ToString() + GlobalConstants.AutoTest,
                Name = this.TestAssetIssuerUpdate.Name + Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest,
            };
            string editParam = JsonUtils.SerializeObject(editIssuer);

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, editParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            AssetIssuersEntity entity = await this.AssetIssuersManager.TryGetAsync(this.TestAssetIssuerUpdate.Id) as AssetIssuersEntity;
            entity.ShouldBeEquivalentTo(editIssuer, o => o
            .ExcludingMissingMembers());

        }

        [Test]
        [Category("Smoke")]
        [Category("Issuers")]
        [Category("IssuersDelete")]
        public async Task DeleteAssetIssuer()
        {
            string url = ApiPaths.ISSUERS_BASE_PATH + "/" + this.TestAssetIssuerDelete.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            AssetIssuersEntity entity = await this.AssetIssuersManager.TryGetAsync(this.TestAssetIssuerDelete.Id) as AssetIssuersEntity;
            Assert.Null(entity);
        }
    }
}
