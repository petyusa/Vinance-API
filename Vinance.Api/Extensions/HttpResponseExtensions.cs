using Microsoft.AspNetCore.Http;

namespace Vinance.Api.Extensions
{
    public static class HttpResponseExtensions
    {
        public static bool IsSuccessStatusCode(this HttpResponse response)
        {
            return response.StatusCode >= 200 && response.StatusCode <= 299;
        }

        public static bool IsUnAuthorizedStatusCode(this HttpResponse response)
        {
            return response.StatusCode == 401;
        }

        public static bool IsRedirectStatusCode(this HttpResponse response)
        {
            return response.StatusCode >= 300 && response.StatusCode <= 399;
        }

        public static bool IsClientErrorStatusCode(this HttpResponse response)
        {
            return response.StatusCode >= 400 && response.StatusCode <= 499 && response.StatusCode != 401;
        }

        public static bool IsServerErrorStatusCode(this HttpResponse response)
        {
            return response.StatusCode >= 500 && response.StatusCode <= 599;
        }
    }
}