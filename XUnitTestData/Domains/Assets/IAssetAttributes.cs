
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTestData.Domains.Assets
{
    public interface IAssetAttributes : IDictionaryItem
    {
        string AssetId { get; set; }
        IEnumerable<IAssetAttributesKeyValue> Attributes { get; set; }
    }

    public interface IAssetAttributesKeyValue
    {
        string Key { get; set; }
        string Value { get; set; }
    }

    public class KeyValue : IAssetAttributesKeyValue
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    //public class AssetAttributes : IAssetAttributes
    //{
    //    public string AssetId { get; set; }
    //    public IEnumerable<IAssetAttributesKeyValue> Attributes { get; set; }
    //    public string Id { get; set; }
    //}

    public interface IAssetAttributesRepository
    {
        Task AddAsync(string assetId, IAssetAttributesKeyValue keyValue);
        Task EditAsync(string assetId, IAssetAttributesKeyValue keyValue);
        Task RemoveAsync(string assetId, string key);
        Task<IAssetAttributesKeyValue> GetAsync(string assetId, string key);
        Task<IAssetAttributesKeyValue[]> GetAllAsync(string assetId);

        Task<IEnumerable<IAssetAttributes>> GetAllAsync();
    }
}
