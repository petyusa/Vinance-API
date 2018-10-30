using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Vinance.Api.Middlewares
{
    public class VinanceResponseWrapper
    {
        private readonly RequestDelegate _next;

        public VinanceResponseWrapper(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var currentBody = context.Response.Body;

            using (var memoryStream = new MemoryStream())
            {
                context.Response.Body = memoryStream;

                await _next(context);

                context.Response.Body = currentBody;
                memoryStream.Seek(0, SeekOrigin.Begin);

                object objectResult = null;
                string errorMessage = null;
                var readToEnd = new StreamReader(memoryStream).ReadToEnd();

                if (context.Response.StatusCode < 400)
                {
                    objectResult = JsonConvert.DeserializeObject(readToEnd);
                }
                else if (context.Response.StatusCode == 401)
                {
                    errorMessage = "You are unauthorized to make this request";
                }
                else
                {
                    objectResult = JsonConvert.DeserializeObject(readToEnd);
                }

                var response = new VinanceApiResponse((HttpStatusCode)context.Response.StatusCode, objectResult, errorMessage);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
            }
        }

    }
}