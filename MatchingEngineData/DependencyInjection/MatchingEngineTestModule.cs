using System;
using System.Collections.Generic;
using Autofac;
using System.Text;
using XUnitTestCommon;
using XUnitTestData.Domains;
using XUnitTestData.Services;
using XUnitTestData.Repositories;
using XUnitTestCommon.Utils;
using AzureStorage.Tables;

namespace MatchingEngineData.DependencyInjection
{
    class MatchingEngineTestModule : Module
    {
        private ConfigBuilder _configBuilder;

        public MatchingEngineTestModule(ConfigBuilder configBuilder)
        {
            this._configBuilder = configBuilder;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c => new AccountRepository(
                    new AzureTableStorage<AccountEntity>(
                        _configBuilder.Config["BalancesInfoConnectionString"], "Accounts", null)))
                .As<IDictionaryRepository<IAccount>>();

            //RepositoryUtils.RegisterDictionaryManager<IAccount>(builder);

        }
    }
}
