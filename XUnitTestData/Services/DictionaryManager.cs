using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XUnitTestData.Domains;

namespace XUnitTestData.Services
{
    [UsedImplicitly]
    public class DictionaryManager<TDictionaryItem> : IDictionaryManager<TDictionaryItem>
        where TDictionaryItem : IDictionaryItem
    {
        private readonly IDictionaryRepository<TDictionaryItem> _repository;
        private readonly IDictionaryCacheService<TDictionaryItem> _cache;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly TimeSpan _cacheExpirationPeriod;
        private DateTime _cacheExpirationMoment;

        public DictionaryManager(
            IDictionaryRepository<TDictionaryItem> repository,
            IDictionaryCacheService<TDictionaryItem> cache,
            IDateTimeProvider dateTimeProvider,
            TimeSpan cacheExpirationPeriod)
        {
            _repository = repository;
            _cache = cache;
            _dateTimeProvider = dateTimeProvider;
            _cacheExpirationPeriod = cacheExpirationPeriod;

            _cacheExpirationMoment = DateTime.MinValue;
        }

        public async Task<TDictionaryItem> TryGetAsync(string id)
        {
            await EnsureCacheIsUpdatedAsync();

            return _cache.TryGet(id);
        }

        public async Task<IEnumerable<TDictionaryItem>> GetAllAsync()
        {
            await EnsureCacheIsUpdatedAsync();

            return _cache.GetAll();
        }

        public async Task UpdateCacheAsync()
        {
            var items = await _repository.GetAllAsync();

            _cache.Update(items);

            _cacheExpirationMoment = _dateTimeProvider.UtcNow + _cacheExpirationPeriod;
        }

        private async Task EnsureCacheIsUpdatedAsync()
        {
            if (_cacheExpirationMoment < _dateTimeProvider.UtcNow)
            {
                await UpdateCacheAsync();
            }
        }
    }
}
