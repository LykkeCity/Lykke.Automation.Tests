using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestData.Domains;

namespace XUnitTestData.Services
{
    public interface IDictionaryCacheService<TDictionaryItem>
        where TDictionaryItem : IDictionaryItem
    {
        void Update(IEnumerable<TDictionaryItem> item);
        TDictionaryItem TryGet(string id);
        IReadOnlyCollection<TDictionaryItem> GetAll();
    }
}
