using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Vinance.Api.Middlewares
{
    using Contracts.Exceptions;
    using Contracts.Exceptions.Base;

    public class GlobalExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<GlobalExceptionHandlingMiddleware>();
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogError(exception, exception.Message);

            var message = JsonConvert.SerializeObject(exception.Message);

            if (exception is VinanceException)
            {
                switch (exception)
                {
                    case VinanceNotFoundException ex:
                        context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    case HeaderContentTypeException ex:
                        context.Response.StatusCode = (int) HttpStatusCode.UnsupportedMediaType;
                        break;
                    case UserNotAuthenticatedException ex:
                        context.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        break;
                }
            }

            await context.Response.WriteAsync(message);
        }
    }
}