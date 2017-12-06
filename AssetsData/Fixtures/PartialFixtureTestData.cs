using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using XUnitTestCommon.Utils;
using AutoMapper;
using AssetsData.DTOs;
using AssetsData.DTOs.Assets;
using XUnitTestData.Entities.Assets;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;

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
                cfg.CreateMap<Erc20TokenEntity, Erc20TokenDto>().ReverseMap();
            });

            this.mapper = config.CreateMapper();

            var assetsFromDB = AssetRepository.GetAllAsync(
                a => a.PartitionKey == AssetEntity.GeneratePartitionKey()
                && a.Type != "Erc20Token");
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

            ConfigBuilder apiv2Config = new ConfigBuilder("ApiV2");
            ApiConsumer registerConsumer1 = new ApiConsumer(apiv2Config);
            ApiConsumer registerConsumer2 = new ApiConsumer(apiv2Config);

            var registeredAccount1 = registerConsumer1.RegisterNewUser();
            var registeredAccount2 = registerConsumer2.RegisterNewUser();

            this.TestAccountId = (await registeredAccount1)?.Account.Id;
            this.TestAccountIdForClientEndpoint = (await registeredAccount2)?.Account.Id;

            this.TestGroupForClientEndpoint = await CreateTestAssetGroup();
            this.TestAssetForClientEndpoint = await CreateTestAsset();

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

            this.TestErcToken = await CreateTestErcToken();
        }
    }
}
