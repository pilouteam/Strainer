using System;
using System.Net;
using System.Net.Http;

namespace Strainer.Http
{
    public class ServerException : Exception
    {
        public ServerException(HttpResponseMessage response)
            : base("Server as returned an error " + response.StatusCode + " For request: " +response.RequestMessage.RequestUri)
        {
            Response = response;
        }

        public HttpResponseMessage Response{ get ; private set; }

        public HttpStatusCode StatusCode { get { return Response.StatusCode; } }
    }
}

