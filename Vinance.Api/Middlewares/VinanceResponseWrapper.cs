using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace Vinance.Api.Middlewares
{
    using Contracts;
    using Extensions;
    using Viewmodels;

    public class VinanceResponseWrapper
    {
        private readonly RequestDelegate _next;

        public VinanceResponseWrapper(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (IsSwagger(context))
            {
                await _next(context);
            }
            else
            {
                var currentBody = context.Response.Body;

                using (var memoryStream = new MemoryStream())
                {
                    context.Response.Body = memoryStream;

                    await _next(context);

                    context.Response.Body = currentBody;
                    memoryStream.Seek(0, SeekOrigin.Begin);

                    object objectResult = null;
                    object errorMessage = null;
                    var readToEnd = new StreamReader(memoryStream).ReadToEnd();

                    if (context.Response.IsSuccessStatusCode())
                    {
                        objectResult = JsonConvert.DeserializeObject(readToEnd);
                    }

                    if (context.Response.IsClientErrorStatusCode())
                    {
                        errorMessage = readToEnd;
                    }

                    if (context.Response.IsUnAuthorizedStatusCode())
                    {
                        errorMessage = "You are not authorized to make this request";
                    }

                    if (context.Response.IsServerErrorStatusCode())
                    {
                        errorMessage = "Internal server error";
                    }

                    var response = new VinanceApiResponse((HttpStatusCode)context.Response.StatusCode, objectResult, errorMessage);
                    context.Response.ContentType = Constants.ApplicationJson;
                    var settings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(response, settings));
                }
            }
        }

        private bool IsSwagger(HttpContext context)
        {
            return context.Request.Path.StartsWithSegments("/swagger");
        }
    }
}