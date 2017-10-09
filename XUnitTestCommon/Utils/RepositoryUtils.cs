using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using XUnitTestData.Services;

namespace XUnitTestCommon.Utils
{
    public class RepositoryUtils
    {
        public static void RegisterDictionaryManager<T>(ContainerBuilder builder, double cacheServiceTimeout = 60.0) where T : IDictionaryItem
        {
            builder.RegisterType<DictionaryCacheService<T>>()
                .As<IDictionaryCacheService<T>>()
                .SingleInstance();

            builder.RegisterType<DictionaryManager<T>>()
                .As<IDictionaryManager<T>>()
                .WithParameter(new TypedParameter(typeof(TimeSpan),
                    TimeSpan.FromSeconds(cacheServiceTimeout)))
                .SingleInstance();
        }

        public static IDictionaryManager<T> PrepareRepositoryManager<T>(IContainer container) where T : IDictionaryItem
        {
            var repository = container.Resolve<IDictionaryRepository<T>>();
            var cacheService = container.Resolve<IDictionaryCacheService<T>>();
            var dateTimeProvider = new DateTimeProvider();


            var manager = container.Resolve<IDictionaryManager<T>>(
                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IDictionaryRepository<T>) && pi.Name == "repository",
                    (pi, ctx) => repository),

                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IDictionaryCacheService<T>) && pi.Name == "cache",
                    (pi, ctx) => cacheService),

                new ResolvedParameter(
                    (pi, ctx) => pi.ParameterType == typeof(IDateTimeProvider) && pi.Name == "dateTimeProvider",
                    (pi, ctx) => dateTimeProvider)
                    );

            return manager;
        }
    }
}
