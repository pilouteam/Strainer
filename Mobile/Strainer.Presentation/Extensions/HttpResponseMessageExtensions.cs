using System;
using System.Threading.Tasks;
using System.Net.Http;
using Cirrious.CrossCore.Platform;
using Cirrious.CrossCore;
using System.Net;
using Strainer.Http;
using Strainer.Infrastructure;

namespace Strainer.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static async Task<T> Deserialize<T>(this Task<HttpResponseMessage> task, IMvxJsonConverter serializer = null)
        {
            serializer = serializer ?? DefaultSerializer;

            var response = await task.ConfigureAwait(false);

            // We must have a successful response
            if (!response.IsSuccessStatusCode)
            {
                throw new ServerException(response);
            }

            var json = await response.Content.ReadAsStringAsync()
                .ConfigureAwait(false);
            try
            {
                return serializer.DeserializeObject<T>(json);
            }
            catch(Exception e)
            {
                throw new Exception("Can't Deserialize: " + json);
            }
        }

        public static async Task<HttpResponseMessage> HandleError(this Task<HttpResponseMessage> thisMessage)
        {
            var message = await thisMessage;

            if(!message.IsSuccessStatusCode)
            {
                await ErrorCodeHandlerSerivce.HandleResponse(message);
            }
            return message;
        }

        static IMvxJsonConverter DefaultSerializer
        {
            get { return Mvx.Resolve<IMvxJsonConverter> (); }
        }
        static IErrorHandler ErrorCodeHandlerSerivce
        {
            get { return Mvx.Resolve<IErrorHandler> (); }
        }
    }
}

