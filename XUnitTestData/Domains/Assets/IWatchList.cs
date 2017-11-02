using System.Collections.Generic;

namespace XUnitTestData.Domains.Assets
{
    public interface IWatchList : IDictionaryItem
    {
        string AssetIds { get; set; }
        string Name { get; set; }
        int Order { get; set; }
        bool ReadOnly { get; set; }
    }
}