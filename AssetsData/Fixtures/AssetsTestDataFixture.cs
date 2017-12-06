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
using XUnitTestCommon.Tests;

namespace AssetsData.Fixtures
{
    [TestFixture]
    public partial class AssetsTestDataFixture: BaseTest
    {
        public ApiConsumer Consumer;

        public IMapper mapper;
        private IContainer container;

        private ConfigBuilder _configBuilder;

        [OneTimeSetUp]
        public void Initialize()
        {
            _configBuilder = new ConfigBuilder("Assets");

            this.Consumer = new ApiConsumer(_configBuilder);

            prepareDependencyContainer();
            prepareTestData().Wait();
        }

        private void prepareDependencyContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AssetsTestModule(_configBuilder));
            container = builder.Build();

            AssetRepository = RepositoryUtils.ResolveGenericRepository<AssetEntity, IAsset>(container);
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

        }
    }
}
