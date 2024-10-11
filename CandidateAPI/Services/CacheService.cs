using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace CandidateAPI.Services
{
    public class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        public CacheService(IMemoryCache memoryCache)
        {
            _memoryCache= memoryCache;
            
        }
        public T Get<T>(string key)
        {
           _memoryCache.TryGetValue(key, out T value);
           return value;
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void Set<T>(string key, T value, TimeSpan expiration)
        {
            var cacheOption = new MemoryCacheEntryOptions().SetAbsoluteExpiration(expiration);
            _memoryCache.Set(key, value, cacheOption);
        }
    }
}