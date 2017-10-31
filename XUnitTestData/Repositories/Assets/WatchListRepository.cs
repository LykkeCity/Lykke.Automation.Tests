using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestData.Domains;
using XUnitTestData.Domains.Assets;
using System.Threading.Tasks;
using AzureStorage;
using System.Linq;

namespace XUnitTestData.Repositories.Assets
{
    public class WatchListEntity : TableEntity, IWatchList
    {
        public string Id => RowKey;
        public string Name { get; set; }
        public int Order { get; set; }
        public bool ReadOnly { get; set; }
        public string AssetIds
        {
            get
            {
                return this._assetIdsString;
            }
            set
            {
                this._assetIdsString = value;
                this._assetIDsList = value.Split(",").ToList();
            }
        }
        public List<string> AssetIDsList
        {
            get
            {
                return this._assetIDsList;
            }
            set
            {
                this._assetIDsList = value;
                this._assetIdsString = String.Join(",", value);

            }
        }

        private string _assetIdsString;
        private List<string> _assetIDsList;

    }

    public class WatchListRepository : IDictionaryRepository<IWatchList>
    {
        private readonly INoSQLTableStorage<WatchListEntity> _tableStorage;

        public WatchListRepository(INoSQLTableStorage<WatchListEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IEnumerable<IWatchList>> GetAllAsync(string partitionKey)
        {
            return await _tableStorage.GetDataAsync(partitionKey);
        }

        public async Task<IEnumerable<IWatchList>> GetAllAsync()
        {
            return await _tableStorage.GetDataAsync();
        }

        public async Task<IWatchList> TryGetAsync(string partitionKey, string id)
        {
            return await _tableStorage.GetDataAsync(partitionKey, id);
        }
    }
}
