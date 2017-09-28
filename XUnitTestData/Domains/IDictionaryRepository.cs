using System.Collections.Generic;
using System.Threading.Tasks;

namespace XUnitTestData.Domains
{
    public interface IDictionaryRepository<TDictionaryItem>
        where TDictionaryItem : IDictionaryItem
    {
        Task<IEnumerable<TDictionaryItem>> GetAllAsync();
    }
}