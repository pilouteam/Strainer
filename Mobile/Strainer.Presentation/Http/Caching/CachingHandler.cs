using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Threading;

namespace Strainer.Http.Caching
{
    public class CachingHandler: DelegatingHandler
    {
        readonly ICacheStore _store;

        public CachingHandler(ICacheStore store)
        {
            this._store = store;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var cached = _store.Get(request.RequestUri);
            if (cached != null)
            {
                return new HttpResponseMessage
                {
                    Content = new ByteArrayContent(cached),
                };
            }

            var response = await base.SendAsync(request, cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                await AddToCache(request.RequestUri, response);
            }

            return response;
        }

        async Task AddToCache(Uri requestUri, HttpResponseMessage response)
        {
            if(response.Headers.CacheControl != null && response.Headers.CacheControl.MaxAge.HasValue)
            {
                var maxAge = response.Headers.CacheControl.MaxAge;
                var expiresAtUtc = DateTime.UtcNow.Add(maxAge.Value);
                _store.Set(requestUri, await response.Content.ReadAsByteArrayAsync(), expiresAtUtc);
            }
        }
    }
}

