using AssetsData.Fixtures;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;
using XUnitTestCommon;

namespace AFTests.AssetsTests
{

    [Trait("Category", "FullRegression")]
    [Trait("Category", "AssetsService")]
    public partial class AssetsTest : IClassFixture<AssetsTestDataFixture>
    {
        private AssetsTestDataFixture fixture;

        public AssetsTest(AssetsTestDataFixture fixture)
        {
            this.fixture = fixture;
        }

        #region IsAlive
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "IsAlive")]
        [Trait("Category", "IsAliveGet")]
        public async void IsAlive()
        {
            string url = fixture.ApiEndpointNames["assetIsAlive"];
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            Assert.True(response.ResponseJson.Contains("\"Env\":"));
            Assert.True(response.ResponseJson.Contains("\"Version\":"));
        }
        #endregion

        //[Fact]
        //public async void CleanUp()
        //{
        //    var allAutoTestAssets = fixture.AllAssetsFromDB.Where(a => a.Id.EndsWith("_AutoTest")).ToList();
        //    var allAutoTestAssetGroups = fixture.AllAssetGroupsFromDB.Where(a => a.Name.EndsWith("_AutoTest")).ToList();
        //    var allAutoTestAssetPairs = fixture.AllAssetPairsFromDB.Where(a => a.Name.EndsWith("_AutoTest")).ToList();
        //    var allAutoTestIssuers = fixture.AllAssetIssuersFromDB.Where(a => a.Id.EndsWith("_AutoTest")).ToList();


        //    List<Task<bool>> deleteTasks = new List<Task<bool>>();
        //    foreach (var asset in allAutoTestAssets) { deleteTasks.Add(fixture.DeleteTestAsset(asset.Id)); }
        //    foreach (var group in allAutoTestAssetGroups) { deleteTasks.Add(fixture.DeleteTestAssetGroup(group.Name)); }
        //    foreach (var pair in allAutoTestAssetPairs) { deleteTasks.Add(fixture.DeleteTestAssetPair(pair.Id)); }
        //    foreach (var issuer in allAutoTestIssuers) { deleteTasks.Add(fixture.DeleteTestAssetIssuer(issuer.Id)); }

        //    Task.WhenAll(deleteTasks).Wait();
        //}
    }
}
