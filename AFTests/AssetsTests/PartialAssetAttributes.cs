using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;
using XUnitTestCommon.Utils;
using XUnitTestCommon;
using System.Threading.Tasks;

namespace AFTests.AssetsTests
{
    [Category("FullRegression")]
    [Category("AssetsService")]
    public partial class AssetsTest
    {
        [Test]
        [Category("Smoke")]
        [Category("AssetAttributes")]
        [Category("AssetsAttributesGet")]
        public async Task GetAllAssetAttributes()
        {
            string url = ApiPaths.ASSET_ATTRIBUTES_PATH;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            List<AssetAttributesReturnDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetAttributesReturnDTO>>(response.ResponseJson);


            for (int i = 0; i < this.AllAssetAttributesFromDB.Count; i++)
            {
                this.AllAssetAttributesFromDB[i].ShouldBeEquivalentTo(parsedResponse[i], o => o
                .ExcludingMissingMembers()
                .Excluding(a => a.Attributes));
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetAttributes")]
        [Category("AssetsAttributesGet")]
        public async Task GetSingleAssetAttributes()
        {
            string url = ApiPaths.ASSET_ATTRIBUTES_PATH + "/" + this.TestAssetAttribute.AssetId;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetAttributesReturnDTO parsedResponse = JsonUtils.DeserializeJson<AssetAttributesReturnDTO>(response.ResponseJson);

            this.TestAssetAttribute.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers()
            .Excluding(p => p.Attributes));
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetAttributes")]
        [Category("AssetsAttributesGet")]
        public async Task GetSingleAssetAttribute()
        {
            string url = ApiPaths.ASSET_ATTRIBUTES_PATH + "/" + this.TestAssetAttribute.AssetId + "/" + this.TestAssetAttribute.Key;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetAttributeDTO parsedResponse = JsonUtils.DeserializeJson<AssetAttributeDTO>(response.ResponseJson);

            Assert.True(this.TestAssetAttribute.Value == parsedResponse.Value);
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetAttributes")]
        [Category("AssetsAttributesPost")]
        public async Task CreateAssetAttribute()
        {
            AssetAttributeIdentityDTO newAssetAttr = await this.CreateTestAssetAttribute();
            Assert.NotNull(newAssetAttr);

            var checkDb = await this.AssetAttributesRepository.TryGetAsync(newAssetAttr.AssetId, newAssetAttr.Key);
            Assert.True(checkDb.Value == newAssetAttr.Value);
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetAttributes")]
        [Category("AssetsAttributesPut")]
        public async Task UpdateAssetAttribute()
        {
            AssetAttributeIdentityDTO TestAssetAttributeUpdate = await CreateTestAssetAttribute();

            string url = ApiPaths.ASSET_ATTRIBUTES_PATH + "/" + TestAssetAttributeUpdate.AssetId;
            string updateValue = TestAssetAttributeUpdate.Value + "_AutoTestEdit";
            string updateParameter = JsonUtils.SerializeObject(
                new AssetAttributeDTO() { Key = TestAssetAttributeUpdate.Key, Value = updateValue });
            var updateResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParameter, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            var checkDbUpdated = await this.AssetAttributesRepository.TryGetAsync(TestAssetAttributeUpdate.AssetId, TestAssetAttributeUpdate.Key);
            Assert.True(checkDbUpdated.Value == updateValue);
        }

        [Test]
        [Category("Smoke")]
        [Category("AssetAttributes")]
        [Category("AssetsAttributesDelete")]
        public async Task DeleteAssetAttribute()
        {
            AssetAttributeIdentityDTO TestAssetAttributeDelete = await CreateTestAssetAttribute();

            string deleteUrl = ApiPaths.ASSET_ATTRIBUTES_PATH + "/" + TestAssetAttributeDelete.AssetId + "/" + TestAssetAttributeDelete.Key;
            var deleteResponse = await this.Consumer.ExecuteRequest(deleteUrl, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            var checkDbDeleted = await this.AssetAttributesRepository.TryGetAsync(TestAssetAttributeDelete.AssetId, TestAssetAttributeDelete.Key);
            Assert.Null(checkDbDeleted);
        }
    }
}
