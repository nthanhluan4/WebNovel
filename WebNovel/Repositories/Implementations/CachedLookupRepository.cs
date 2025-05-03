using Microsoft.Extensions.Caching.Memory;
using WebNovel.Repositories.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    public class CachedLookupRepository : ILookupRepository
    {
        private const string GENRE_KEY = "AllGenres";
        private const string TAG_KEY = "AllTags";

        private readonly ILookupRepository _inner;
        private readonly IMemoryCache _cache;
        private readonly MemoryCacheEntryOptions _cacheOpts
            = new() { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24) };

        public CachedLookupRepository(ILookupRepository inner, IMemoryCache cache)
        {
            _inner = inner;
            _cache = cache;
        }

        public Task<Dictionary<int, string>> GetAllGenresAsync()
            => _cache.GetOrCreateAsync(GENRE_KEY,
                entry =>
                {
                    entry.SetOptions(_cacheOpts);
                    return _inner.GetAllGenresAsync();
                });

        public Task<Dictionary<int, string>> GetAllTagsAsync()
            => _cache.GetOrCreateAsync(TAG_KEY,
                entry =>
                {
                    entry.SetOptions(_cacheOpts);
                    return _inner.GetAllTagsAsync();
                });

        // Nếu muốn bust cache khi có thay đổi, bạn có thể expose public methods:
        public void RemoveGenreCache() => _cache.Remove(GENRE_KEY);
        public void RemoveTagCache() => _cache.Remove(TAG_KEY);
    }

}
