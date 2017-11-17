using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using XUnitTestData.Domains.Assets;
using XUnitTestData.Entitites.ApiV2.Assets;

namespace XUnitTestData.Repositories.ApiV2
{
    public class WatchListRepository : GenericRepository<WatchListEntity, IWatchList>
    {
        public WatchListRepository(INoSQLTableStorage<WatchListEntity> tableStorage, string partitionKey = null) : base(tableStorage, partitionKey)
        {
        }

        public new async Task<IEnumerable<IWatchList>> GetAllAsync()
        {
            return await TableStorage.GetDataAsync();
        }
    }
}
