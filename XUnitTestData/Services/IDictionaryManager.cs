using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestData.Domains;

namespace XUnitTestData.Services
{
    public interface IDictionaryManager<TDictionaryItem>
        where TDictionaryItem : IDictionaryItem
    {
        Task<TDictionaryItem> TryGetAsync(string id);
        Task<IEnumerable<TDictionaryItem>> GetAllAsync();
        Task UpdateCacheAsync();
    }
}
