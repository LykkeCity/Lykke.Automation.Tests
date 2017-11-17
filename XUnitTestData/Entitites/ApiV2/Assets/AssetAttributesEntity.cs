using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;
using XUnitTestData.Domains.Assets;

namespace XUnitTestData.Entitites.ApiV2.Assets
{
    public class AssetAttributesEntity : TableEntity, IAssetAttributes
    {
        public string Id => PartitionKey;
        public string AssetId
        {
            get { return PartitionKey; }
            set { PartitionKey = value; }
        }
        public string Key
        {
            get { return RowKey; }
            set { RowKey = value; }
        }
        public string Value { get; set; }
        public IEnumerable<IAssetAttributesKeyValue> Attributes { get; set; }

        public static AssetAttributesEntity Create(string assetId, IAssetAttributesKeyValue keyValue)
        {
            return new AssetAttributesEntity
            {
                RowKey = keyValue.Key,
                PartitionKey = assetId,
                Value = keyValue.Value
            };
        }
    }
}
