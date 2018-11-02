using System.Net;

namespace Vinance.Api.Viewmodels
{
    public class VinanceApiResponse
    {
        public VinanceApiResponse(HttpStatusCode statusCode, object data = null, object error = null)
        {
            StatusCode = (int)statusCode;
            Data = data;
            Error = error;
        }

        public int StatusCode { get; }

        public object Error { get; }

        public object Data { get; }
    }

}