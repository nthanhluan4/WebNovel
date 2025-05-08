using Microsoft.Extensions.Caching.Memory;
using WebNovel.Models;
using WebNovel.Repositories.Interfaces;

namespace WebNovel.Repositories.Implementations
{
    public class CachedLookupRepository : ILookupRepository
    {
        private const string GENRE_KEY = "AllGenres";
        private const string GENRE_LIST_KEY = "Genres";
        private const string TAG_KEY = "AllTags";
        private const string TAG_LIST_KEY = "Tags";

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

        public Task<Dictionary<int, Genre>> GetGenresAsync()
         => _cache.GetOrCreateAsync(GENRE_LIST_KEY,
                entry =>
                {
                    entry.SetOptions(_cacheOpts);
                    return _inner.GetGenresAsync();
                });

        public Task<Dictionary<int, Tag>> GetTagsAsync()
         => _cache.GetOrCreateAsync(TAG_LIST_KEY,
                entry =>
                {
                    entry.SetOptions(_cacheOpts);
                    return _inner.GetTagsAsync();
                });

        // Nếu muốn bust cache khi có thay đổi, bạn có thể expose public methods:
        public void RemoveGenreCache()
        {
            _cache.Remove(GENRE_KEY);
            _cache.Remove(GENRE_LIST_KEY);
        }
        public void RemoveTagCache()
        {
            _cache.Remove(TAG_KEY);
            _cache.Remove(TAG_LIST_KEY);
        }
    }

}
