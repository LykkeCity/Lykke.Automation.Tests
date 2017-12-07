using XUnitTestData.Domains.Assets;
using System;
using System.Collections.Generic;
using AssetsData.DTOs;
using AssetsData.DTOs.Assets;
using XUnitTestData.Repositories;
using XUnitTestData.Repositories.ApiV2;
using XUnitTestData.Entities.Assets;

namespace AssetsData.Fixtures
{
    public partial class AssetsTestDataFixture
    {
        public List<AssetEntity> AllAssetsFromDB;
        public AssetEntity TestAsset;

        public List<AssetExtendedInfosEntity> AllAssetExtendedInfosFromDB;
        public AssetExtendedInfosEntity TestAssetExtendedInfo;

        public List<AssetAttributesEntity> AllAssetAttributesFromDB;
        public AssetAttributesEntity TestAssetAttribute;

        public List<AssetCategoryEntity> AllAssetCategoriesFromDB;
        public AssetCategoryEntity TestAssetCategory;

        public List<AssetGroupEntity> AllAssetGroupsFromDB;
        public AssetGroupEntity TestAssetGroup;

        public string TestAccountId;
        public AssetGroupDTO TestGroupForClientEndpoint;
        public AssetDTO TestAssetForClientEndpoint;
        public string TestAccountIdForClientEndpoint;

        public List<AssetPairEntity> AllAssetPairsFromDB;
        public AssetPairEntity TestAssetPair;

        public List<AssetSettingsEntity> AllAssetSettingsFromDB;
        public AssetSettingsEntity TestAssetSettings;

        public List<AssetIssuersEntity> AllAssetIssuersFromDB;
        public AssetIssuersEntity TestAssetIssuer;

        public List<MarginAssetPairsEntity> AllMarginAssetPairsFromDB;
        public MarginAssetPairsEntity TestMarginAssetPair;

        public List<MarginAssetEntity> AllMarginAssetsFromDB;
        public MarginAssetEntity TestMarginAsset;

        public List<MarginIssuerEntity> AllMarginIssuersFromDB;
        public MarginIssuerEntity TestMarginIssuer;

        public List<WatchListEntity> AllWatchListsFromDB;
        public List<WatchListEntity> AllWatchListsFromDBPredefined;
        public List<WatchListEntity> AllWatchListsFromDBCustom;
        public WatchListEntity TestWatchListPredefined;
        public WatchListEntity TestWatchListCustom;

        public Erc20TokenDto TestErcToken;

        public GenericRepository<AssetEntity, IAsset> AssetRepository;
        public GenericRepository<AssetExtendedInfosEntity, IAssetExtendedInfo> AssetExtendedInfosManager;
        public GenericRepository<AssetCategoryEntity, IAssetCategory> AssetCategoryManager;
        public AssetAttributesRepository AssetAttributesManager;
        public GenericRepository<AssetGroupEntity, IAssetGroup> AssetGroupsManager;
        public GenericRepository<AssetPairEntity, IAssetPair> AssetPairManager;
        public GenericRepository<AssetSettingsEntity, IAssetSettings> AssetSettingsManager;
        public GenericRepository<AssetIssuersEntity, IAssetIssuers> AssetIssuersManager;
        public GenericRepository<MarginAssetPairsEntity, IMarginAssetPairs> MarginAssetPairManager;
        public GenericRepository<MarginAssetEntity, IMarginAsset> MarginAssetManager;
        public GenericRepository<MarginIssuerEntity, IMarginIssuer> MarginIssuerManager;

        public AssetAttributesRepository AssetAttributesRepository;
        public GenericRepository<AssetGroupEntity, IAssetGroup> AssetGroupsRepository;
        public WatchListRepository WatchListRepository;


    }
}
