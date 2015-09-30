using System;
using Cirrious.CrossCore.Platform;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using Cirrious.CrossCore;

namespace Strainer.Extensions
{
    public static class HttpClientExtensions
    {
        public static Task<HttpResponseMessage> PostJson(this HttpClient thisClient, string pathInfo, object request, IMvxJsonConverter serializer = null)
        {
            var s = serializer ?? DefaultSerializer;
            var requestJson = s.SerializeObject(request);

            return thisClient.PostAsync(pathInfo, new StringContent(requestJson, Encoding.UTF8, "application/json"));
        }



        private static IMvxJsonConverter DefaultSerializer
        {
            get
            {
                return Mvx.Resolve<IMvxJsonConverter>();
            }
        }
    }
}

