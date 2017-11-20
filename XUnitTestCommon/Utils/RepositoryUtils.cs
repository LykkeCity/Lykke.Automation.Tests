using Autofac;
using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains;
using XUnitTestData.Repositories;

namespace XUnitTestCommon.Utils
{
    public class RepositoryUtils
    {
        public static GenericRepository<TEntity, TInterface> ResolveGenericRepository<TEntity, TInterface>(IContainer container)
            where TEntity : TableEntity, TInterface, new()
            where TInterface : IDictionaryItem
        {
            return container.Resolve<IDictionaryRepository<TInterface>>() as GenericRepository<TEntity, TInterface>;
        }
    }
}
