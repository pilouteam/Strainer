using System;
using System.Threading.Tasks;

namespace Strainer.Infrastructure
{
    public interface ICacheService
    {
        Task<T> Get<T>(string key, T fallback=default(T)) ;

        Task Set<T>(string key, T obj) ;
        Task Set<T>(string key, T obj, DateTime expiresAt) ;

        Task Remove(string key);
        Task ClearAll();
    }
}