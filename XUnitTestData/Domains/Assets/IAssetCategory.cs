using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XUnitTestData.Domains.Assets
{
    public interface IAssetCategory : IDictionaryItem
    {
        string Id { get; }
        string Name { get; }
        string IosIconUrl { get; set; }
        string AndroidIconUrl { get; set; }
        int SortOrder { get; set; }
    }
}
