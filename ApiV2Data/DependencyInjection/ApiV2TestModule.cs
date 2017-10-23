using Autofac;
using AzureStorage.Tables;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestCommon;
using XUnitTestCommon.Utils;
using XUnitTestData.Domains;
using XUnitTestData.Domains.ApiV2;
using XUnitTestData.Repositories.ApiV2;

namespace ApiV2Data.DependencyInjection
{
    public class ApiV2TestModule : Module
    {
        private ConfigBuilder _configBuilder;

        public ApiV2TestModule(ConfigBuilder configBuilder)
        {
            this._configBuilder = configBuilder;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new PledgesRepository(
                    new AzureTableStorage<PledgeEntity>(
                        _configBuilder.Config["MainConnectionString"], "Pledges", null)))
                .As<IDictionaryRepository<IPledgeEntity>>();

            RepositoryUtils.RegisterDictionaryManager<IPledgeEntity>(builder);
        }
    }
}
