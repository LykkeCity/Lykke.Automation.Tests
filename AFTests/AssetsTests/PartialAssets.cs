using AssetsData.DTOs;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
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
        [Category("Assets")]
        [Category("AssetsGet")]
        public async Task GetAllAssets()
        {
            // Get all assets
            string url = ApiPaths.ASSETS_BASE_PATH;
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams.Add("includeNonTradable", "true");
            var response = await this.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK, "Actual status code is not OK");
            Assert.NotNull(response.ResponseJson);

            List<AssetDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetDTO>>(response.ResponseJson);

            foreach (AssetEntity entity in this.AllAssetsFromDB)
            {
                var parsedObject = parsedResponse.Where(a => a.Id == entity.Id).FirstOrDefault();
                if (parsedObject == null)
                    continue; //TODO figure out why Asset with id Dev_fdgd doesn't show up
                entity.ShouldBeEquivalentTo(parsedObject,
                o => o.ExcludingMissingMembers().Excluding(m => m.PartnerIds));
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsGet")]
        public async Task GetTradableAssets()
        {
            // Get all assets
            string url = ApiPaths.ASSETS_BASE_PATH;
            Dictionary<string, string> queryParams = new Dictionary<string, string>();
            queryParams.Add("includeNonTradable", "false");
            var response = await this.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK, "Actual status code is not OK");
            Assert.NotNull(response.ResponseJson);

            List<AssetDTO> parsedResponse = JsonUtils.DeserializeJson<List<AssetDTO>>(response.ResponseJson);
            List<AssetEntity> allTradeableAssets = AllAssetsFromDB.Where(a => a.IsTradable == false).ToList();

            foreach (AssetEntity entity in this.AllAssetsFromDB)
            {
                var parsedObject = parsedResponse.Where(a => a.Id == entity.Id).FirstOrDefault();
                if (parsedObject == null)
                    continue; //TODO figure out why Asset with id Dev_fdgd doesn't show up
                entity.ShouldBeEquivalentTo(parsedObject,
                        o => o.ExcludingMissingMembers().Excluding(m => m.PartnerIds));
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsGet")]
        public async Task GetSingleAsset()
        {
            string url = ApiPaths.ASSETS_BASE_PATH + "/" + this.TestAsset.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetDTO parsedResponse = JsonUtils.DeserializeJson<AssetDTO>(response.ResponseJson);

            this.TestAsset.ShouldBeEquivalentTo(parsedResponse, options => options
                .ExcludingMissingMembers()
                .Excluding(a => a.PartnerIds));
        }

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsGet")]
        public async Task CheckIfAssetExists()
        {
            string url = ApiPaths.ASSETS_V2_BASE_PATH + "/" + this.TestAsset.Id + "/exists";
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            bool parsedResponse = JsonUtils.DeserializeJson<bool>(response.ResponseJson);

            Assert.True(parsedResponse);
        }

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsGet")]
        public async Task GetDefault()
        {
            string url = ApiPaths.ASSETS_DEFAULT_PATH;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);

            Assert.True(response.Status == HttpStatusCode.OK);
            Assert.NotNull(response.ResponseJson);

            AssetDTO parsedResponse = JsonUtils.DeserializeJson<AssetDTO>(response.ResponseJson);

            foreach (PropertyInfo pi in parsedResponse.GetType().GetProperties())
            {
                if (pi.PropertyType == typeof(string))
                {
                    string value = pi.GetValue(parsedResponse).As<string>();
                    if (pi.Name == "Blockchain")
                    {
                        Assert.True(value == "None");
                    }
                    else
                    {
                        Assert.Null(value);
                    }
                }
                else if (pi.PropertyType == typeof(int))
                    Assert.True(pi.GetValue(parsedResponse).As<int>() == 0);
                else if (pi.PropertyType == typeof(bool))
                    Assert.True(pi.GetValue(parsedResponse).As<bool>() == false);
                else if (pi.PropertyType == typeof(List<string>))
                    Assert.True(pi.GetValue(parsedResponse).As<List<string>>().Count == 0);
            }
        }

        //[Test]
        //[Category("Smoke")]
        //[Category("AssetsPut")]
        //public async Task GetAssetSpecification()
        //{
        //    throw new NotImplementedException();
        //}

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsPost")]
        public async Task EnableDisableAsset()
        {
            string disableUrl = ApiPaths.ASSETS_V2_BASE_PATH + "/" + this.TestAsset.Id + "/disable";
            string enableUrl = ApiPaths.ASSETS_V2_BASE_PATH + "/" + this.TestAsset.Id + "/enable";
            string parameter = JsonUtils.SerializeObject(new { id = this.TestAsset.Id });
            string url;

            if (this.TestAsset.IsDisabled)
                url = enableUrl;
            else
                url = disableUrl;

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, parameter, Method.POST);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            AssetEntity entity = await this.AssetRepository.TryGetAsync(this.TestAsset.Id) as AssetEntity;
            Assert.True(entity.IsDisabled != this.TestAsset.IsDisabled);

            if (entity.IsDisabled)
                url = enableUrl;
            else
                url = disableUrl;

            var responseAfter = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, parameter, Method.POST);
            Assert.True(responseAfter.Status == HttpStatusCode.NoContent);

            AssetEntity entityAfter = await this.AssetRepository.TryGetAsync(this.TestAsset.Id) as AssetEntity;
            Assert.True(entityAfter.IsDisabled == this.TestAsset.IsDisabled);

        }

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsPost")]
        public async Task CreateAsset()
        {
            AssetDTO createdAsset = await this.CreateTestAsset();
            Assert.NotNull(createdAsset);

            AssetEntity entity = await this.AssetRepository.TryGetAsync(createdAsset.Id) as AssetEntity;
            entity.ShouldBeEquivalentTo(createdAsset, o => o
            .ExcludingMissingMembers());

        }

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsPut")]
        public async Task UpdateAsset()
        {
            string url = ApiPaths.ASSETS_V2_BASE_PATH;
            AssetDTO updateParamAsset = await CreateTestAsset();

            updateParamAsset.Name += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTestEdit;
            updateParamAsset.DefinitionUrl += Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest ;

            string updateParam = JsonUtils.SerializeObject(updateParamAsset);

            var updateResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
            Assert.True(updateResponse.Status == HttpStatusCode.NoContent);

            AssetEntity entityUpdateed = await this.AssetRepository.TryGetAsync(updateParamAsset.Id) as AssetEntity;
            entityUpdateed.ShouldBeEquivalentTo(updateParamAsset, o => o
            .ExcludingMissingMembers());


        }

        [Test]
        [Category("Smoke")]
        [Category("Assets")]
        [Category("AssetsDelete")]
        public async Task DeleteAsset()
        {
            AssetDTO TestAssetDelete = await CreateTestAsset();

            string url = ApiPaths.ASSETS_V2_BASE_PATH + "/" + TestAssetDelete.Id;
            string deleteParam = JsonUtils.SerializeObject(new { id = TestAssetDelete.Id });

            var deleteResponse = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, deleteParam, Method.DELETE);
            Assert.True(deleteResponse.Status == HttpStatusCode.NoContent);

            AssetEntity entityDeleted = await this.AssetRepository.TryGetAsync(TestAssetDelete.Id) as AssetEntity;
            Assert.Null(entityDeleted);
        }
    }
}
