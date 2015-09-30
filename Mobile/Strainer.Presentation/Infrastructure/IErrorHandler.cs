using System;
using System.Threading.Tasks;
using System.Net.Http;

namespace Strainer.Infrastructure
{
    public interface IErrorHandler
    {
        Task HandleResponse(HttpResponseMessage message);

        void Handle(Exception ex);
    }
}