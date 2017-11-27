using AssetsData.DTOs.Assets;
using AssetsData.Fixtures;
using FluentAssertions;
using RestSharp;
using System.Collections.Generic;
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
        #region predefined
        [Test]
        [Category("Smoke")]
        [Category("WatchList")]
        [Category("WatchListGet")]
        public async Task GetAllWatchListsPredefined()
        {
            string url = ApiPaths.WATCH_LIST_PREDEFINED_PATH;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<WatchListDTO> parsedResponse = JsonUtils.DeserializeJson<List<WatchListDTO>>(response.ResponseJson);

            foreach (WatchListEntity entity in this.AllWatchListsFromDBPredefined)
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

        [Test]
        [Category("Smoke")]
        [Category("WatchList")]
        [Category("WatchListGet")]
        public async Task GetSingleWatchListsPredefined()
        {
            string url = ApiPaths.WATCH_LIST_PREDEFINED_PATH + "/" + this.TestWatchListPredefined.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            WatchListDTO parsedResponse = JsonUtils.DeserializeJson<WatchListDTO>(response.ResponseJson);
            this.TestWatchListPredefined.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers()
            .Excluding(e => e.AssetIds)
            .Excluding(e => e.ReadOnly));

            Assert.True(parsedResponse.ReadOnly);

            this.TestWatchListPredefined.AssetIDsList.Should().HaveSameCount(parsedResponse.AssetIds);

            foreach (string assetId in this.TestWatchListPredefined.AssetIDsList)
            {
                parsedResponse.AssetIds.Should().Contain(assetId);
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("WatchList")]
        [Category("WatchListPost")]
        public async Task CreatePredefinedWatchList()
        {
            WatchListDTO createdDTO = await this.CreateTestWatchList();
            Assert.NotNull(createdDTO);

            WatchListEntity entity = await this.WatchListRepository.TryGetAsync("PublicWatchList", createdDTO.Id) as WatchListEntity;
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

        [Test]
        [Category("Smoke")]
        [Category("WatchList")]
        [Category("WatchListPut")]
        public async Task UpdatePredefinedWatchList()
        {
            string url = ApiPaths.WATCH_LIST_PREDEFINED_PATH;
            WatchListDTO updateWatchList = new WatchListDTO()
            {
                Id = this.TestWatchListPredefinedUpdate.Id,
                Name = this.TestWatchListPredefinedUpdate.Name + Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest,
                Order = Helpers.Random.Next(1, 100),
                ReadOnly = this.TestWatchListPredefinedUpdate.ReadOnly,
                AssetIds = this.TestWatchListPredefinedUpdate.AssetIds
            };
            updateWatchList.AssetIds.Add("AutoTest");
            string updateParam = JsonUtils.SerializeObject(updateWatchList);

            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, updateParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            WatchListEntity entity = await this.WatchListRepository.TryGetAsync("PublicWatchList", updateWatchList.Id) as WatchListEntity;
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

        [Test]
        [Category("Smoke")]
        [Category("WatchList")]
        [Category("WatchListDelete")]
        public async Task DeletePredefinedWatchList()
        {
            string url = ApiPaths.WATCH_LIST_PREDEFINED_PATH + "/" + this.TestWatchListPredefinedDelete.Id;
            var response = await this.Consumer.ExecuteRequest(url, Helpers.EmptyDictionary, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            WatchListEntity entity = await this.WatchListRepository.TryGetAsync("PublicWatchList", this.TestWatchListPredefinedDelete.Id) as WatchListEntity;
            Assert.Null(entity);
        }
        #endregion

        #region Custom
        [Test]
        [Category("Smoke")]
        [Category("WatchList")]
        [Category("WatchListGet")]
        public async Task GetAllWatchListsCustom()
        {
            string url = ApiPaths.WATCH_LIST_CUSTOM_PATH;
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                { "userId", this.TestWatchListCustom.PartitionKey }
            };

            var response = await this.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<WatchListDTO> parsedResponse = JsonUtils.DeserializeJson<List<WatchListDTO>>(response.ResponseJson);
            List<WatchListEntity> userWatchLists = this.AllWatchListsFromDBCustom.Where(w => w.PartitionKey == this.TestWatchListCustom.PartitionKey).ToList();

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
                    if (!string.IsNullOrEmpty(assetId))
                        parsedMatch.AssetIds.Should().Contain(assetId);
                }
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("WatchList")]
        [Category("WatchListGet")]
        public async Task GetSingleWatchListsCustom()
        {
            string url = ApiPaths.WATCH_LIST_CUSTOM_PATH  + "/" + this.TestWatchListCustom.Id;
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                { "userId", this.TestWatchListCustom.PartitionKey }
            };

            var response = await this.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            WatchListDTO parsedResponse = JsonUtils.DeserializeJson<WatchListDTO>(response.ResponseJson);
            this.TestWatchListCustom.ShouldBeEquivalentTo(parsedResponse, o => o
            .ExcludingMissingMembers()
            .Excluding(e => e.AssetIds)
            .Excluding(e => e.ReadOnly));

            Assert.False(parsedResponse.ReadOnly);

            foreach (string assetId in this.TestWatchListCustom.AssetIDsList)
            {
                parsedResponse.AssetIds.Should().Contain(assetId);
            }
        }

        [Test]
        [Category("Smoke")]
        [Category("WatchList")]
        [Category("WatchListPost")]
        public async Task CreateCustomWatchList()
        {
            WatchListDTO createdDTO = await this.CreateTestWatchList(this.TestAccountId);
            Assert.NotNull(createdDTO);

            WatchListEntity entity = await this.WatchListRepository.TryGetAsync(this.TestAccountId, createdDTO.Id) as WatchListEntity;
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

        [Test]
        [Category("Smoke")]
        [Category("WatchList")]
        [Category("WatchListPut")]
        public async Task UpdateCustomWatchList()
        {
            string url = ApiPaths.WATCH_LIST_CUSTOM_PATH;
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                { "userId", this.TestAccountId }
            };
            WatchListDTO updateWatchList = new WatchListDTO()
            {
                Id = this.TestWatchListCustomUpdate.Id,
                Name = this.TestWatchListCustomUpdate.Name + Helpers.Random.Next(1000, 9999).ToString() + GlobalConstants.AutoTest,
                Order = Helpers.Random.Next(1, 100),
                ReadOnly = this.TestWatchListCustomUpdate.ReadOnly,
                AssetIds = this.TestWatchListCustomUpdate.AssetIds
            };
            updateWatchList.AssetIds.Add("AutoTest");
            string updateParam = JsonUtils.SerializeObject(updateWatchList);

            var response = await this.Consumer.ExecuteRequest(url, queryParams, updateParam, Method.PUT);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            WatchListEntity entity = await this.WatchListRepository.TryGetAsync(this.TestAccountId, updateWatchList.Id) as WatchListEntity;
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

        [Test]
        [Category("Smoke")]
        [Category("WatchList")]
        [Category("WatchListDelete")]
        public async Task DeleteCustomWatchList()
        {
            string url = ApiPaths.WATCH_LIST_CUSTOM_PATH + "/" + this.TestWatchListCustomDelete.Id;
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                { "userId", this.TestAccountId }
            };

            var response = await this.Consumer.ExecuteRequest(url, queryParams, null, Method.DELETE);
            Assert.True(response.Status == HttpStatusCode.NoContent);

            WatchListEntity entity = await this.WatchListRepository.TryGetAsync(this.TestAccountId, this.TestWatchListCustomDelete.Id) as WatchListEntity;
            Assert.Null(entity);
        }
        #endregion

        [Test]
        [Category("Smoke")]
        [Category("WatchList")]
        [Category("WatchListGet")]
        public async Task GetAllWatchLists()
        {
            string url = ApiPaths.WATCH_LIST_ALL_PATH;
            Dictionary<string, string> queryParams = new Dictionary<string, string>
            {
                { "userId", this.TestWatchListCustom.PartitionKey }
            };

            var response = await this.Consumer.ExecuteRequest(url, queryParams, null, Method.GET);
            Assert.True(response.Status == HttpStatusCode.OK);

            List<WatchListDTO> parsedResponse = JsonUtils.DeserializeJson<List<WatchListDTO>>(response.ResponseJson);
            List<WatchListEntity> userWatchLists = this.AllWatchListsFromDBCustom.Where(w => w.PartitionKey == this.TestWatchListCustom.PartitionKey).ToList();
            userWatchLists.AddRange(this.AllWatchListsFromDBPredefined);

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
