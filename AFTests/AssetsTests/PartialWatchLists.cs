using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        #region predefined
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "WatchList")]
        [Trait("Category", "WatchListGet")]
        public async void GetAllWatchListsPredefined()
        {
            string url = ApiPaths.WATCH_LIST_PREDEFINED_PATH;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<WatchListDTO> parsedResponse = JsonUtils.DeserializeJson<List<WatchListDTO>>(response.ResponseJson);

            foreach (WatchListEntity entity in fixture.AllWatchListsFromDBPredefined)
            {
                WatchListDTO parsedMatch = parsedResponse.Where(p => p.Id == entity.Id).FirstOrDefault();
                entity.ShouldBeEquivalentTo(parsedMatch, o => o
                .ExcludingMissingMembers()
                .Excluding(e => e.AssetIds)
                .Excluding(e => e.ReadOnly));

                Assert.True(parsedMatch.ReadOnly);

                entity.AssetIDsList.Should().HaveSameCount(parsedMatch.AssetIds);

                foreach (string assetId in entity.AssetIDsList)
                {
                    parsedMatch.AssetIds.Should().Contain(assetId);
                }
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "WatchList")]
        [Trait("Category", "WatchListGet")]
        public async void GetSingleWatchListsPredefined()
        {
            string url = ApiPaths.WATCH_LIST_PREDEFINED_PATH + "/" + fixture.TestWatchListPredefined.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            WatchListDTO parsedResponse = JsonUtils.DeserializeJson<WatchListDTO>(response.ResponseJson);
            fixture.TestWatchListPredefined.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers()
            .Excluding(e => e.AssetIds)
            .Excluding(e => e.ReadOnly));

            Assert.True(parsedResponse.ReadOnly);

            fixture.TestWatchListPredefined.AssetIDsList.Should().HaveSameCount(parsedResponse.AssetIds);

            foreach (string assetId in fixture.TestWatchListPredefined.AssetIDsList)
            {
                parsedResponse.AssetIds.Should().Contain(assetId);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "WatchList")]
        [Trait("Category", "WatchListPost")]
        public async void CreatePredefinedWatchList()
        {
            WatchListDTO createdDTO = await fixture.CreateTestWatchList();
            Assert.NotNull(createdDTO);

            WatchListEntity entity = await fixture.WatchListRepository.TryGetAsync("PublicWatchList", createdDTO.Id) as WatchListEntity;
            Assert.NotNull(entity);
            entity.ShouldBeEquivalentTo(createdDTO, o => o
            .ExcludingMissingMembers()
            .Excluding(e => e.AssetIds)
            .Excluding(e => e.ReadOnly));

            entity.AssetIDsList.Should().HaveSameCount(createdDTO.AssetIds);

            foreach (string assetId in entity.AssetIDsList)
            {
                createdDTO.AssetIds.Should().Contain(assetId);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "WatchList")]
        [Trait("Category", "WatchListPut")]
        public async void UpdatePredefinedWatchList()
        {
            string url = ApiPaths.WATCH_LIST_PREDEFINED_PATH;
            WatchListDTO updateWatchList = new WatchListDTO()
            {
                Id = fixture.TestWatchListPredefinedUpdate.Id,
                Name = fixture.TestWatchListPredefinedUpdate.Name + Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest,
                Order = Helpers.Random.Next(1, 100),
                ReadOnly = fixture.TestWatchListPredefinedUpdate.ReadOnly,
                AssetIds = fixture.TestWatchListPredefinedUpdate.AssetIds
            };
            updateWatchList.AssetIds.Add("AutoTest");
            string updateParam = JsonUtils.SerializeObject(updateWatchList);

            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            WatchListEntity entity = await fixture.WatchListRepository.TryGetAsync("PublicWatchList", updateWatchList.Id) as WatchListEntity;
            Assert.NotNull(entity);
            entity.ShouldBeEquivalentTo(updateWatchList, o => o
            .ExcludingMissingMembers()
            .Excluding(e => e.AssetIds)
            .Excluding(e => e.ReadOnly));

            entity.AssetIDsList.Should().HaveSameCount(updateWatchList.AssetIds);

            foreach (string assetId in entity.AssetIDsList)
            {
                updateWatchList.AssetIds.Should().Contain(assetId);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "WatchList")]
        [Trait("Category", "WatchListDelete")]
        public async void DeletePredefinedWatchList()
        {
            string url = ApiPaths.WATCH_LIST_PREDEFINED_PATH + "/" + fixture.TestWatchListPredefinedDelete.Id;
            var response = await fixture.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            WatchListEntity entity = await fixture.WatchListRepository.TryGetAsync("PublicWatchList", fixture.TestWatchListPredefinedDelete.Id) as WatchListEntity;
            Assert.Null(entity);
        }
        #endregion

        #region Custom
        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "WatchList")]
        [Trait("Category", "WatchListGet")]
        public async void GetAllWatchListsCustom()
        {
            string url = ApiPaths.WATCH_LIST_CUSTOM_PATH;
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                { "userId", fixture.TestWatchListCustom.PartitionKey }
            };

            var response = await fixture.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<WatchListDTO> parsedResponse = JsonUtils.DeserializeJson<List<WatchListDTO>>(response.ResponseJson);
            List<WatchListEntity> userWatchLists = fixture.AllWatchListsFromDBCustom.Where(w => w.PartitionKey == fixture.TestWatchListCustom.PartitionKey).ToList();

            foreach (WatchListEntity entity in userWatchLists)
            {
                WatchListDTO parsedMatch = parsedResponse.Where(p => p.Id == entity.Id).FirstOrDefault();
                entity.ShouldBeEquivalentTo(parsedMatch, o => o
                .ExcludingMissingMembers()
                .Excluding(e => e.AssetIds)
                .Excluding(e => e.ReadOnly));

                Assert.False(parsedMatch.ReadOnly);

                foreach (string assetId in entity.AssetIDsList)
                {
                    parsedMatch.AssetIds.Should().Contain(assetId);
                }
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "WatchList")]
        [Trait("Category", "WatchListGet")]
        public async void GetSingleWatchListsCustom()
        {
            string url = ApiPaths.WATCH_LIST_CUSTOM_PATH  + "/" + fixture.TestWatchListCustom.Id;
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                { "userId", fixture.TestWatchListCustom.PartitionKey }
            };

            var response = await fixture.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            WatchListDTO parsedResponse = JsonUtils.DeserializeJson<WatchListDTO>(response.ResponseJson);
            fixture.TestWatchListCustom.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers()
            .Excluding(e => e.AssetIds)
            .Excluding(e => e.ReadOnly));

            Assert.False(parsedResponse.ReadOnly);

            foreach (string assetId in fixture.TestWatchListCustom.AssetIDsList)
            {
                parsedResponse.AssetIds.Should().Contain(assetId);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "WatchList")]
        [Trait("Category", "WatchListPost")]
        public async void CreateCustomWatchList()
        {
            WatchListDTO createdDTO = await fixture.CreateTestWatchList(fixture.TestAccountId);
            Assert.NotNull(createdDTO);

            WatchListEntity entity = await fixture.WatchListRepository.TryGetAsync(fixture.TestAccountId, createdDTO.Id) as WatchListEntity;
            Assert.NotNull(entity);
            entity.ShouldBeEquivalentTo(createdDTO, o => o
            .ExcludingMissingMembers()
            .Excluding(e => e.AssetIds)
            .Excluding(e => e.ReadOnly));

            entity.AssetIDsList.Should().HaveSameCount(createdDTO.AssetIds);

            foreach (string assetId in entity.AssetIDsList)
            {
                createdDTO.AssetIds.Should().Contain(assetId);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "WatchList")]
        [Trait("Category", "WatchListPut")]
        public async void UpdateCustomWatchList()
        {
            string url = ApiPaths.WATCH_LIST_CUSTOM_PATH;
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                { "userId", fixture.TestAccountId }
            };
            WatchListDTO updateWatchList = new WatchListDTO()
            {
                Id = fixture.TestWatchListCustomUpdate.Id,
                Name = fixture.TestWatchListCustomUpdate.Name + Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest,
                Order = Helpers.Random.Next(1, 100),
                ReadOnly = fixture.TestWatchListCustomUpdate.ReadOnly,
                AssetIds = fixture.TestWatchListCustomUpdate.AssetIds
            };
            updateWatchList.AssetIds.Add("AutoTest");
            string updateParam = JsonUtils.SerializeObject(updateWatchList);

            var response = await fixture.Consumer.ExecuteRequest(url, queryParams, updateParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            WatchListEntity entity = await fixture.WatchListRepository.TryGetAsync(fixture.TestAccountId, updateWatchList.Id) as WatchListEntity;
            Assert.NotNull(entity);
            entity.ShouldBeEquivalentTo(updateWatchList, o => o
            .ExcludingMissingMembers()
            .Excluding(e => e.AssetIds)
            .Excluding(e => e.ReadOnly));

            entity.AssetIDsList.Should().HaveSameCount(updateWatchList.AssetIds);

            foreach (string assetId in entity.AssetIDsList)
            {
                updateWatchList.AssetIds.Should().Contain(assetId);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "WatchList")]
        [Trait("Category", "WatchListDelete")]
        public async void DeleteCustomWatchList()
        {
            string url = ApiPaths.WATCH_LIST_CUSTOM_PATH + "/" + fixture.TestWatchListCustomDelete.Id;
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                { "userId", fixture.TestAccountId }
            };

            var response = await fixture.Consumer.ExecuteRequest(url, queryParams, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            WatchListEntity entity = await fixture.WatchListRepository.TryGetAsync(fixture.TestAccountId, fixture.TestWatchListCustomDelete.Id) as WatchListEntity;
            Assert.Null(entity);
        }
        #endregion

        [Fact]
        [Trait("Category", "Smoke")]
        [Trait("Category", "WatchList")]
        [Trait("Category", "WatchListGet")]
        public async void GetAllWatchLists()
        {
            string url = ApiPaths.WATCH_LIST_ALL_PATH;
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                { "userId", fixture.TestWatchListCustom.PartitionKey }
            };

            var response = await fixture.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<WatchListDTO> parsedResponse = JsonUtils.DeserializeJson<List<WatchListDTO>>(response.ResponseJson);
            List<WatchListEntity> userWatchLists = fixture.AllWatchListsFromDBCustom.Where(w => w.PartitionKey == fixture.TestWatchListCustom.PartitionKey).ToList();
            userWatchLists.AddRange(fixture.AllWatchListsFromDBPredefined);

            foreach (WatchListEntity entity in userWatchLists)
            {
                WatchListDTO parsedMatch = parsedResponse.Where(p => p.Id == entity.Id).FirstOrDefault();
                entity.ShouldBeEquivalentTo(parsedMatch, o => o
                .ExcludingMissingMembers()
                .Excluding(e => e.AssetIds)
                .Excluding(e => e.ReadOnly));

                foreach (string assetId in parsedMatch.AssetIds)
                {
                    entity.AssetIDsList.Should().Contain(assetId);
                }
            }
        }
    }
}
