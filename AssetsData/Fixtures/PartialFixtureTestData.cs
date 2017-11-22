﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using XUnitTestCommon.Utils;
using AutoMapper;
using AssetsData.DTOs;
using AssetsData.DTOs.Assets;
using XUnitTestData.Entities.Assets;

namespace AssetsData.Fixtures
{
    public partial class AssetsTestDataFixture
    {
        private async Task prepareTestData()
        {
            MapperConfiguration config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AssetEntity, AssetDTO>();
                cfg.CreateMap<AssetDTO, AssetEntity>();
                cfg.CreateMap<AssetCategoryEntity, AssetCategoryDTO>();
                cfg.CreateMap<AssetCategoryDTO, AssetCategoryEntity>();
                cfg.CreateMap<AssetExtendedInfosEntity, AssetExtendedInfoDTO>();
                cfg.CreateMap<AssetExtendedInfoDTO, AssetExtendedInfosEntity>();
                cfg.CreateMap<AssetGroupDTO, AssetGroupEntity>();
                cfg.CreateMap<AssetGroupEntity, AssetGroupDTO>();
                cfg.CreateMap<AssetPairDTO, AssetPairEntity>();
                cfg.CreateMap<AssetPairEntity, AssetPairDTO>();
                cfg.CreateMap<AssetIssuerDTO, AssetIssuersEntity>();
                cfg.CreateMap<AssetIssuersEntity, AssetIssuerDTO>();
                cfg.CreateMap<MarginAssetPairDTO, MarginAssetPairsEntity>();
                cfg.CreateMap<MarginAssetPairsEntity, MarginAssetPairDTO>();
                cfg.CreateMap<MarginAssetDTO, MarginAssetEntity>();
                cfg.CreateMap<MarginAssetEntity, MarginAssetDTO>();
                cfg.CreateMap<MarginIssuerDTO, MarginIssuerEntity>();
                cfg.CreateMap<MarginIssuerEntity, MarginIssuerDTO>();
                cfg.CreateMap<WatchListDTO, WatchListEntity>();
                cfg.CreateMap<WatchListEntity, WatchListDTO>();
                cfg.CreateMap<AssetSettingsDTO, AssetSettingsEntity>();
                cfg.CreateMap<AssetSettingsEntity, AssetSettingsDTO>();
                cfg.CreateMap<AssetSettingsDTO, AssetSettingsCreateDTO>();
                cfg.CreateMap<AssetSettingsCreateDTO, AssetSettingsDTO>();
            });

            this.mapper = config.CreateMapper();

            this.AssetsToDelete = new List<string>();
            this.AssetAtributesToDelete = new List<AssetAttributeIdentityDTO>();
            this.AssetCategoriesToDelete = new List<string>();
            this.AssetExtendedInfosToDelete = new List<string>();
            this.AssetGroupsToDelete = new List<string>();
            this.AssetPairsToDelete = new List<string>();
            this.AssetIssuersToDelete = new List<string>();
            this.MarginAssetPairsToDelete = new List<string>();
            this.MarginAssetsToDelete = new List<string>();
            this.MarginIssuersToDelete = new List<string>();
            this.WatchListsToDelete = new Dictionary<string, string>();
            this.AssetSettingsToDelete = new List<string>();

            var assetsFromDB = AssetManager.GetAllAsync();
            var AssetExtInfoFromDB = AssetExtendedInfosManager.GetAllAsync();
            var assetsAttrFromDB = AssetAttributesManager.GetAllAsync();
            var assetsCatsFromDB = AssetCategoryManager.GetAllAsync();
            var assetsGroupsFromDB = AssetGroupsManager.GetAllAsync();
            var assetPairsFromDB = AssetPairManager.GetAllAsync();
            var assetSettingsFromDB = AssetSettingsManager.GetAllAsync();
            var assetIssuersFromDB = AssetIssuersManager.GetAllAsync();
            var marginAssetPairsFromDB = MarginAssetPairManager.GetAllAsync();
            var marginAssetsFromDB = MarginAssetManager.GetAllAsync();
            var marginIssuersFromDB = MarginIssuerManager.GetAllAsync();
            var watchListsFromDB = WatchListRepository.GetAllAsync();

            this.AllAssetsFromDB = (await assetsFromDB).Cast<AssetEntity>().ToList();
            this.TestAsset = EnumerableUtils.PickRandom(AllAssetsFromDB);
            this.TestAssetUpdate = await CreateTestAsset();
            this.TestAssetDelete = await CreateTestAsset();

            this.AllAssetExtendedInfosFromDB = (await AssetExtInfoFromDB).Cast<AssetExtendedInfosEntity>().ToList();
            this.TestAssetExtendedInfo = EnumerableUtils.PickRandom(AllAssetExtendedInfosFromDB);
            this.TestAssetExtendedInfoUpdate = await CreateTestAssetExtendedInfo();
            this.TestAssetExtendedInfoDelete = await CreateTestAssetExtendedInfo();

            this.AllAssetAttributesFromDB = (await assetsAttrFromDB).Cast<AssetAttributesEntity>().ToList();
            this.TestAssetAttribute = EnumerableUtils.PickRandom(AllAssetAttributesFromDB);
            this.TestAssetAttributeUpdate = await CreateTestAssetAttribute();
            this.TestAssetAttributeDelete = await CreateTestAssetAttribute();

            this.AllAssetCategoriesFromDB = (await assetsCatsFromDB).Cast<AssetCategoryEntity>().ToList();
            this.TestAssetCategory = EnumerableUtils.PickRandom(AllAssetCategoriesFromDB);
            this.TestAssetCategoryUpdate = await CreateTestAssetCategory();
            this.TestAssetCategoryDelete = await CreateTestAssetCategory();

            this.AllAssetGroupsFromDB = (await assetsGroupsFromDB).Cast<AssetGroupEntity>().ToList();
            this.TestAssetGroup = EnumerableUtils.PickRandom(AllAssetGroupsFromDB);
            this.TestAssetGroupUpdate = await CreateTestAssetGroup();
            this.TestAssetGroupDelete = await CreateTestAssetGroup();

            this.AllAssetPairsFromDB = (await assetPairsFromDB).Cast<AssetPairEntity>().ToList();
            this.TestAssetPair = EnumerableUtils.PickRandom(AllAssetPairsFromDB);
            this.TestAssetPairUpdate = await CreateTestAssetPair();
            this.TestAssetPairDelete = await CreateTestAssetPair();

            this.TestGroupForGroupRelationAdd = await CreateTestAssetGroup();
            this.TestAssetForGroupRelationAdd = await CreateTestAsset();
            this.TestGroupForGroupRelationDelete = await CreateTestAssetGroup();
            this.TestAssetForGroupRelationDelete = await CreateTestAsset();
            this.TestGroupForClientRelationAdd = await CreateTestAssetGroup();
            this.TestGroupForClientRelationDelete = await CreateTestAssetGroup();
            this.TestAccountId = "AFTest_Client1";

            this.TestGroupForClientEndpoint = await CreateTestAssetGroup();
            this.TestAssetForClientEndpoint = await CreateTestAsset();
            this.TestAccountIdForClientEndpoint = "AFTest_Client2";
            await AddClientToGroup(TestAccountIdForClientEndpoint, TestGroupForClientEndpoint.Name);
            await AddAssetToGroup(TestAssetForClientEndpoint.Id, TestGroupForClientEndpoint.Name);

            this.AllAssetSettingsFromDB = (await assetSettingsFromDB).Cast<AssetSettingsEntity>().ToList();
            this.TestAssetSettings = EnumerableUtils.PickRandom(AllAssetSettingsFromDB);
            this.TestAssetSettingsUpdate = await CreateTestAssetSettings();
            this.TestAssetSettingsDelete = await CreateTestAssetSettings();

            this.AllAssetIssuersFromDB = (await assetIssuersFromDB).Cast<AssetIssuersEntity>().ToList();
            this.TestAssetIssuer = EnumerableUtils.PickRandom(AllAssetIssuersFromDB);
            this.TestAssetIssuerUpdate = await CreateTestAssetIssuer();
            this.TestAssetIssuerDelete = await CreateTestAssetIssuer();

            this.AllMarginAssetPairsFromDB = (await marginAssetPairsFromDB).Cast<MarginAssetPairsEntity>().ToList();
            this.TestMarginAssetPair = EnumerableUtils.PickRandom(AllMarginAssetPairsFromDB);
            this.TestMarginAssetPairUpdate = await CreateTestMarginAssetPair();
            this.TestMarginAssetPairDelete = await CreateTestMarginAssetPair();

            this.AllMarginAssetsFromDB = (await marginAssetsFromDB).Cast<MarginAssetEntity>().ToList();
            this.TestMarginAsset = EnumerableUtils.PickRandom(AllMarginAssetsFromDB);
            this.TestMarginAssetUpdate = await CreateTestMarginAsset();
            this.TestMarginAssetDelete = await CreateTestMarginAsset();

            this.AllMarginIssuersFromDB = (await marginIssuersFromDB).Cast<MarginIssuerEntity>().ToList();
            this.TestMarginIssuer = EnumerableUtils.PickRandom(AllMarginIssuersFromDB);
            this.TestMarginIssuerUpdate = await CreateTestMarginIssuer();
            this.TestMarginIssuerDelete = await CreateTestMarginIssuer();

            this.AllWatchListsFromDB = (await watchListsFromDB).Cast<WatchListEntity>().ToList();
            this.AllWatchListsFromDBPredefined = AllWatchListsFromDB.Where(e => e.PartitionKey == "PublicWatchList").ToList();
            this.TestWatchListPredefined = EnumerableUtils.PickRandom(AllWatchListsFromDBPredefined);
            this.TestWatchListPredefinedUpdate = await CreateTestWatchList();
            this.TestWatchListPredefinedDelete = await CreateTestWatchList();

            this.AllWatchListsFromDBCustom = AllWatchListsFromDB.Where(e => e.PartitionKey != "PublicWatchList").ToList();
            this.TestWatchListCustom = EnumerableUtils.PickRandom(AllWatchListsFromDBCustom);
            this.TestWatchListCustomUpdate = await CreateTestWatchList(TestAccountId);
            this.TestWatchListCustomDelete = await CreateTestWatchList(TestAccountId);
        }
    }
}
