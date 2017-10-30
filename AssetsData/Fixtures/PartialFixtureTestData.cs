﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XUnitTestData.Repositories.Assets;
using System.Linq;
using XUnitTestCommon.Utils;
using AutoMapper;
using AssetsData.DTOs;
using AssetsData.DTOs.Assets;

namespace AssetsData.Fixtures
{
    public partial class AssetsTestDataFixture : IDisposable
    {
        private async Task prepareTestData()
        {
            Mapper.Initialize(cfg =>
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

            });

            this.AssetsToDelete = new List<string>();
            this.AssetAtributesToDelete = new List<AssetAttributeIdentityDTO>();
            this.AssetCategoriesToDelete = new List<string>();
            this.AssetExtendedInfosToDelete = new List<string>();
            this.AssetGroupsToDelete = new List<string>();
            this.AssetPairsToDelete = new List<string>();
            this.AssetIssuersToDelete = new List<string>();

            this.ApiEndpointNames = new Dictionary<string, string>();
            ApiEndpointNames["assets"] = "/api/v2/assets";
            ApiEndpointNames["assetAttributes"] = "/api/v2/asset-attributes";
            ApiEndpointNames["assetCategories"] = "/api/v2/asset-categories";
            ApiEndpointNames["assetExtendedInfos"] = "/api/v2/asset-extended-infos";
            ApiEndpointNames["assetGroups"] = "/api/v2/asset-groups";
            ApiEndpointNames["assetPairs"] = "/api/v2/asset-pairs";
            ApiEndpointNames["assetSettings"] = "/api/v2/asset-settings";
            ApiEndpointNames["assetClients"] = "/api/v2/clients";
            ApiEndpointNames["assetIsAlive"] = "/api/IsAlive";
            ApiEndpointNames["assetIssuers"] = "/api/v2/issuers";
            ApiEndpointNames["marginAssetPairs"] = "/api/v2/margin-asset-pairs";

            var assetsFromDB = AssetManager.GetAllAsync();
            var AssetExtInfoFromDB = AssetExtendedInfosManager.GetAllAsync();
            var assetsAttrFromDB = AssetAttributesManager.GetAllAsync();
            var assetsCatsFromDB = AssetCategoryManager.GetAllAsync();
            var assetsGroupsFromDB = AssetGroupsManager.GetAllAsync();
            var assetPairsFromDB = AssetPairManager.GetAllAsync();
            var assetSettingsFromDB = AssetSettingsManager.GetAllAsync();
            var assetIssuersFromDB = AssetIssuersManager.GetAllAsync();
            var marginAssetPairsFromDB = MarginAssetPairManager.GetAllAsync();

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
            //TestAssetSettingsUpdate
            //TestAssetSettingsDelete

            this.AllAssetIssuersFromDB = (await assetIssuersFromDB).Cast<AssetIssuersEntity>().ToList();
            this.TestAssetIssuer = EnumerableUtils.PickRandom(AllAssetIssuersFromDB);
            this.TestAssetIssuerUpdate = await CreateTestAssetIssuer();
            this.TestAssetIssuerDelete = await CreateTestAssetIssuer();

            this.AllMarginAssetPairsFromDB = (await marginAssetPairsFromDB).Cast<MarginAssetPairsEntity>().ToList();
            this.TestMarginAssetPair = EnumerableUtils.PickRandom(AllMarginAssetPairsFromDB);
        }
    }
}