using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Strainer.Http.Caching
{
    public interface ICacheStore
    {
        byte[] Get(Uri key);
        void Set(Uri url, byte[] value, DateTime expiresAtUtc);
    }
}

