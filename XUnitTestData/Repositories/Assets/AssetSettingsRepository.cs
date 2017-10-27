using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestData.Domains;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Repositories.Assets
{
    public class AssetSettingsEntity : TableEntity, IAssetSettings
    {
        public static string GeneratePartitionKey()
        {
            return "Asset";
        }

        public string Id => RowKey;
        public string CashinCoef { get; set; }
        public string ChangeWallet { get; set; }
        public string Dust { get; set; }
        public string HotWallet { get; set; }
        public int MaxOutputsCount { get; set; }
        public int MaxOutputsCountInTx { get; set; }
        public string MinBalance { get; set; }
        public int MinOutputsCount { get; set; }
        public string OutputSize { get; set; }
        public int PrivateIncrement { get; set; }
        public string MaxBalance { get; set; }
    }

    public class AssetSettingsRepository : IDictionaryRepository<IAssetSettings>
    {
        private readonly INoSQLTableStorage<AssetSettingsEntity> _tableStorage;

        public AssetSettingsRepository(INoSQLTableStorage<AssetSettingsEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IAssetSettings> TryGetAsync(string id)
        {
            string partitionKey = AssetSettingsEntity.GeneratePartitionKey();

            return (await _tableStorage.GetDataAsync(partitionKey, id));
        }

        public async Task<IEnumerable<IAssetSettings>> GetAllAsync()
        {
            string partitionKey = AssetSettingsEntity.GeneratePartitionKey();

            return (await _tableStorage.GetDataAsync(partitionKey));
        }
    }
}
