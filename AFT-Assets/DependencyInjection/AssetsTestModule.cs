using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using XUnitTestData.Repositories.Assets;
using AzureStorage.Tables;
using XUnitTestCommon;
using XUnitTestData.Domains;
using XUnitTestData.Domains.Assets;
using Common.Log;
using Lykke.SettingsReader;
using System.Threading.Tasks;
using XUnitTestData.Services;

namespace FirstXUnitTest.DependencyInjection
{
    class AssetsTestModule : Module
    {

        public AssetsTestModule()
        {
            
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new AssetsRepository(
                    new AzureTableStorage<AssetEntity>(
                        ConfigBuilder.Config["DictionariesConnectionString"], "Dictionaries", null)))
                .As<IDictionaryRepository<IAsset>>();

            RegisterDictionaryManager<IAsset>(builder);



            builder.Register(c => new AssetDescriptionRepository(
                    new AzureTableStorage<AssetDescriptionEntity>(
                        ConfigBuilder.Config["DictionariesConnectionString"], "Dictionaries", null)))
                .As<IDictionaryRepository<IAssetDescription>>();

            RegisterDictionaryManager<IAssetDescription>(builder);

            builder.Register(c => new AssetCategoryRepository(
                    new AzureTableStorage<AssetCategoryEntity>(
                        ConfigBuilder.Config["DictionariesConnectionString"], "AssetCategories", null)))
                .As<IDictionaryRepository<IAssetCategory>>();

            RegisterDictionaryManager<IAssetCategory>(builder);

            builder.Register(c => new AssetAttributesRepository(
                    new AzureTableStorage<AssetAttributesEntity>(
                        ConfigBuilder.Config["DictionariesConnectionString"], "AssetAttributes", null)))
                .As<IDictionaryRepository<IAssetAttributes>>();

            RegisterDictionaryManager<IAssetAttributes>(builder);

            base.Load(builder);
        }

        private void RegisterDictionaryManager<T>(ContainerBuilder builder) where T : IDictionaryItem
        {
            builder.RegisterType<DictionaryCacheService<T>>()
                .As<IDictionaryCacheService<T>>()
                .SingleInstance();

            builder.RegisterType<DictionaryManager<T>>()
                .As<IDictionaryManager<T>>()
                .WithParameter(new TypedParameter(typeof(TimeSpan), 
                    TimeSpan.FromSeconds(Double.Parse(ConfigBuilder.Config["DictionaryCacheExpirationSeconds"]))))
                .SingleInstance();
        }
    }
}
