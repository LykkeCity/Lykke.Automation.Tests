using Autofac;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using AssetsData.DependencyInjection;
using XUnitTestData.Domains.Assets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XUnitTestData.Domains;
using XUnitTestCommon.Utils;
using AssetsData.DTOs.Assets;
using AutoMapper;
using XUnitTestData.Domains.Authentication;
using XUnitTestData.Repositories.ApiV2;
using XUnitTestData.Entities.Assets;
using NUnit.Framework;

namespace AssetsData.Fixtures
{
    [TestFixture]
    public partial class AssetsTestDataFixture
    {
        public ApiConsumer Consumer;

        public IMapper mapper;
        private IContainer container;

        private ConfigBuilder _configBuilder;

        [OneTimeSetUp]
        public void Initialize()
        {
            _configBuilder = new ConfigBuilder("Assets");

            var oAuthConsumer = new OAuthConsumer(_configBuilder);

            this.Consumer = new ApiConsumer(_configBuilder, oAuthConsumer);

            prepareDependencyContainer();
            prepareTestData().Wait();
        }

        private void prepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AssetsTestModule(_configBuilder));
            container = builder.Build();

            AssetManager = RepositoryUtils.ResolveGenericRepository<AssetEntity, IAsset>(container);
            AssetExtendedInfosManager = RepositoryUtils.ResolveGenericRepository<AssetExtendedInfosEntity, IAssetExtendedInfo>(container);
            AssetCategoryManager = RepositoryUtils.ResolveGenericRepository<AssetCategoryEntity, IAssetCategory>(container);
            AssetAttributesManager = container.Resolve<IDictionaryRepository<IAssetAttributes>>() as AssetAttributesRepository;
            AssetGroupsManager = RepositoryUtils.ResolveGenericRepository<AssetGroupEntity, IAssetGroup>(container);

            AssetAttributesRepository = container.Resolve<IDictionaryRepository<IAssetAttributes>>() as AssetAttributesRepository;
            AssetGroupsRepository = RepositoryUtils.ResolveGenericRepository<AssetGroupEntity, IAssetGroup>(container);

            AssetPairManager = RepositoryUtils.ResolveGenericRepository<AssetPairEntity, IAssetPair>(container);
            AssetSettingsManager = RepositoryUtils.ResolveGenericRepository<AssetSettingsEntity, IAssetSettings>(container);
            AssetIssuersManager = RepositoryUtils.ResolveGenericRepository<AssetIssuersEntity, IAssetIssuers>(container);
            MarginAssetPairManager = RepositoryUtils.ResolveGenericRepository<MarginAssetPairsEntity, IMarginAssetPairs>(container);
            MarginAssetManager = RepositoryUtils.ResolveGenericRepository<MarginAssetEntity, IMarginAsset>(container);
            MarginIssuerManager = RepositoryUtils.ResolveGenericRepository<MarginIssuerEntity, IMarginIssuer>(container);

            WatchListRepository = container.Resolve<IDictionaryRepository<IWatchList>>() as WatchListRepository;
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            List<Task<bool>> deleteTasks = new List<Task<bool>>();

            foreach (string assetId in AssetsToDelete) { deleteTasks.Add(DeleteTestAsset(assetId)); }
            foreach (AssetAttributeIdentityDTO attrDTO in AssetAtributesToDelete) { deleteTasks.Add(DeleteTestAssetAttribute(attrDTO.AssetId, attrDTO.Key)); }
            foreach (string catId in AssetCategoriesToDelete) { deleteTasks.Add(DeleteTestAssetCategory(catId)); }
            foreach (string infoId in AssetExtendedInfosToDelete) { deleteTasks.Add(DeleteTestAssetExtendedInfo(infoId)); }
            foreach (string groupName in AssetGroupsToDelete) { deleteTasks.Add(DeleteTestAssetGroup(groupName)); }
            foreach (string pairId in AssetPairsToDelete) { deleteTasks.Add(DeleteTestAssetPair(pairId)); }
            foreach (string issuerId in AssetIssuersToDelete) { deleteTasks.Add(DeleteTestAssetIssuer(issuerId)); }
            foreach (string pairId in MarginAssetPairsToDelete) { deleteTasks.Add(DeleteTestMarginAssetPair(pairId)); }
            foreach (string assetId in MarginAssetsToDelete) { deleteTasks.Add(DeleteTestMarginAsset(assetId)); }
            foreach (string issuerId in MarginIssuersToDelete) { deleteTasks.Add(DeleteTestMarginIssuer(issuerId)); }
            foreach (KeyValuePair<string, string> watchListIDs in WatchListsToDelete) { deleteTasks.Add(DeleteTestWatchList(watchListIDs)); }
            foreach (string assetId in AssetSettingsToDelete) { deleteTasks.Add(DeleteTestAssetSettings(assetId)); }

            Task.WhenAll(deleteTasks).Wait();
        }
    }
}
