using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace D3SK.NetCore.Common.Utilities
{
    public interface ICaching
    {
        ICacheManager CacheManager { get; }
    }

    public interface ICacheManager
    {
        Task<T> GetAsync<T>(string key);

        Task<T> GetOrCreateAsync<T>(string key, T value);

        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> asyncFactory);

        Task<T> SetAsync<T>(string key, T value);

        Task RemoveAsync(string key);
    }
}
