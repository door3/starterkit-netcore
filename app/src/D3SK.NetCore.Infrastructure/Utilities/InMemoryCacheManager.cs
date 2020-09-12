using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using D3SK.NetCore.Common.Extensions;
using D3SK.NetCore.Common.Utilities;
using Microsoft.Extensions.Caching.Memory;

namespace D3SK.NetCore.Infrastructure.Utilities
{
    public class InMemoryCacheManager : ICacheManager
    {
        private readonly IMemoryCache _cache;

        public InMemoryCacheManager(IMemoryCache cache)
        {
            _cache = cache;
        }

        public Task<T> GetAsync<T>(string key) =>
            _cache.TryGetValue(key, out var value) ? value.AsTask<T>() : Task.FromResult(default(T));

        public Task<T> GetOrCreateAsync<T>(string key, T value) => GetOrCreateAsync(key, () => value.AsTask());

        public Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> asyncFactory) =>
            _cache.GetOrCreateAsync(key, _ => asyncFactory());

        public Task<T> SetAsync<T>(string key, T value) => _cache.Set(key, value).AsTask();

        public Task RemoveAsync(string key) => Task.CompletedTask.WithAction(() => _cache.Remove(key));
    }
}
