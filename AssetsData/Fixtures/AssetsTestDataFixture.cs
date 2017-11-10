using Autofac;
using XUnitTestCommon;
using XUnitTestCommon.Consumers;
using AssetsData.DependencyInjection;
using XUnitTestData.Domains.Assets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XUnitTestData.Repositories.Assets;
using XUnitTestData.Domains;
using XUnitTestCommon.Utils;
using AssetsData.DTOs.Assets;
using AutoMapper;

namespace AssetsData.Fixtures
{
    public partial class AssetsTestDataFixture : IDisposable
    {
        public ApiConsumer Consumer;
        public Dictionary<string, string> ApiEndpointNames;

        public IMapper mapper;
        private IContainer container;

        private ConfigBuilder _configBuilder;

        public AssetsTestDataFixture()
        {
            this._configBuilder = new ConfigBuilder("Assets");
            this.Consumer = new ApiConsumer(_configBuilder.Config["UrlPefix"], _configBuilder.Config["BaseUrl"], Boolean.Parse(_configBuilder.Config["IsHttps"]));

            prepareDependencyContainer();
            prepareTestData().Wait();
        }

        private void prepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AssetsTestModule(_configBuilder));
            this.container = builder.Build();

            this.AssetManager = RepositoryUtils.PrepareRepositoryManager<IAsset>(this.container);
            this.AssetExtendedInfosManager = RepositoryUtils.PrepareRepositoryManager<IAssetExtendedInfo>(this.container);
            this.AssetCategoryManager = RepositoryUtils.PrepareRepositoryManager<IAssetCategory>(this.container);
            this.AssetAttributesManager = RepositoryUtils.PrepareRepositoryManager<IAssetAttributes>(this.container);
            this.AssetGroupsManager = RepositoryUtils.PrepareRepositoryManager<IAssetGroup>(this.container);
            this.AssetAttributesRepository = (AssetAttributesRepository)this.container.Resolve<IDictionaryRepository<IAssetAttributes>>();
            this.AssetGroupsRepository = (AssetGroupsRepository)this.container.Resolve<IDictionaryRepository<IAssetGroup>>();
            this.AssetPairManager = RepositoryUtils.PrepareRepositoryManager<IAssetPair>(this.container);
            this.AssetSettingsManager = RepositoryUtils.PrepareRepositoryManager<IAssetSettings>(this.container);
            this.AssetIssuersManager = RepositoryUtils.PrepareRepositoryManager<IAssetIssuers>(this.container);
            this.MarginAssetPairManager = RepositoryUtils.PrepareRepositoryManager<IMarginAssetPairs>(this.container);
            this.MarginAssetManager = RepositoryUtils.PrepareRepositoryManager<IMarginAsset>(this.container);
            this.MarginIssuerManager = RepositoryUtils.PrepareRepositoryManager<IMarginIssuer>(this.container);
            this.WatchListRepository = (WatchListRepository)this.container.Resolve<IDictionaryRepository<IWatchList>>();
        }

        public void Dispose()
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
