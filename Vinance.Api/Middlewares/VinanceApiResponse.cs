using System.Net;

namespace Vinance.Api.Middlewares
{
    public class VinanceApiResponse
    {
        public VinanceApiResponse(HttpStatusCode statusCode, object data = null, string errorMessage = null)
        {
            StatusCode = (int)statusCode;
            Data = data;
            ErrorMessage = errorMessage;
        }

        public int StatusCode { get; }

        public string ErrorMessage { get; }

        public object Data { get; }
    }

}