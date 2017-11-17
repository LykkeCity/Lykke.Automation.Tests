using XUnitTestData.Domains.Assets;
using System;
using System.Collections.Generic;
using XUnitTestData.Services;
using AssetsData.DTOs;
using AssetsData.DTOs.Assets;
using XUnitTestData.Entitites.ApiV2.Assets;
using XUnitTestData.Repositories;
using XUnitTestData.Repositories.ApiV2;

namespace AssetsData.Fixtures
{
    public partial class AssetsTestDataFixture : IDisposable
    {
        public List<AssetEntity> AllAssetsFromDB;
        public AssetEntity TestAsset;
        public AssetDTO TestAssetUpdate;
        public AssetDTO TestAssetDelete;

        public List<AssetExtendedInfosEntity> AllAssetExtendedInfosFromDB;
        public AssetExtendedInfosEntity TestAssetExtendedInfo;
        public AssetExtendedInfoDTO TestAssetExtendedInfoUpdate;
        public AssetExtendedInfoDTO TestAssetExtendedInfoDelete;

        public List<AssetAttributesEntity> AllAssetAttributesFromDB;
        public AssetAttributesEntity TestAssetAttribute;
        public AssetAttributeIdentityDTO TestAssetAttributeUpdate;
        public AssetAttributeIdentityDTO TestAssetAttributeDelete;

        public List<AssetCategoryEntity> AllAssetCategoriesFromDB;
        public AssetCategoryEntity TestAssetCategory;
        public AssetCategoryDTO TestAssetCategoryUpdate;
        public AssetCategoryDTO TestAssetCategoryDelete;

        public List<AssetGroupEntity> AllAssetGroupsFromDB;
        public AssetGroupEntity TestAssetGroup;
        public AssetGroupDTO TestAssetGroupUpdate;
        public AssetGroupDTO TestAssetGroupDelete;

        public AssetGroupDTO TestGroupForGroupRelationAdd;
        public AssetDTO TestAssetForGroupRelationAdd;
        public AssetGroupDTO TestGroupForGroupRelationDelete;
        public AssetDTO TestAssetForGroupRelationDelete;
        public AssetGroupDTO TestGroupForClientRelationAdd;
        public AssetGroupDTO TestGroupForClientRelationDelete;
        public string TestAccountId;
        public AssetGroupDTO TestGroupForClientEndpoint;
        public AssetDTO TestAssetForClientEndpoint;
        public string TestAccountIdForClientEndpoint;

        public List<AssetPairEntity> AllAssetPairsFromDB;
        public AssetPairEntity TestAssetPair;
        public AssetPairDTO TestAssetPairUpdate;
        public AssetPairDTO TestAssetPairDelete;

        public List<AssetSettingsEntity> AllAssetSettingsFromDB;
        public AssetSettingsEntity TestAssetSettings;
        public AssetSettingsDTO TestAssetSettingsUpdate;
        public AssetSettingsDTO TestAssetSettingsDelete;

        public List<AssetIssuersEntity> AllAssetIssuersFromDB;
        public AssetIssuersEntity TestAssetIssuer;
        public AssetIssuerDTO TestAssetIssuerUpdate;
        public AssetIssuerDTO TestAssetIssuerDelete;

        public List<MarginAssetPairsEntity> AllMarginAssetPairsFromDB;
        public MarginAssetPairsEntity TestMarginAssetPair;
        public MarginAssetPairDTO TestMarginAssetPairUpdate;
        public MarginAssetPairDTO TestMarginAssetPairDelete;

        public List<MarginAssetEntity> AllMarginAssetsFromDB;
        public MarginAssetEntity TestMarginAsset;
        public MarginAssetDTO TestMarginAssetUpdate;
        public MarginAssetDTO TestMarginAssetDelete;

        public List<MarginIssuerEntity> AllMarginIssuersFromDB;
        public MarginIssuerEntity TestMarginIssuer;
        public MarginIssuerDTO TestMarginIssuerUpdate;
        public MarginIssuerDTO TestMarginIssuerDelete;

        public List<WatchListEntity> AllWatchListsFromDB;
        public List<WatchListEntity> AllWatchListsFromDBPredefined;
        public List<WatchListEntity> AllWatchListsFromDBCustom;
        public WatchListEntity TestWatchListPredefined;
        public WatchListDTO TestWatchListPredefinedUpdate;
        public WatchListDTO TestWatchListPredefinedDelete;
        public WatchListEntity TestWatchListCustom;
        public WatchListDTO TestWatchListCustomUpdate;
        public WatchListDTO TestWatchListCustomDelete;

        public GenericRepository<AssetEntity, IAsset> AssetManager;
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

        private List<string> AssetsToDelete;
        private List<AssetAttributeIdentityDTO> AssetAtributesToDelete;
        private List<string> AssetCategoriesToDelete;
        private List<string> AssetExtendedInfosToDelete;
        private List<string> AssetGroupsToDelete;
        private List<string> AssetPairsToDelete;
        private List<string> AssetIssuersToDelete;
        private List<string> MarginAssetPairsToDelete;
        private List<string> MarginAssetsToDelete;
        private List<string> MarginIssuersToDelete;
        private Dictionary<string, string> WatchListsToDelete;
        private List<string> AssetSettingsToDelete;
    }
}
