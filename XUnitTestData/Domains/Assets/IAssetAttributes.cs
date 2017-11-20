
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
}
