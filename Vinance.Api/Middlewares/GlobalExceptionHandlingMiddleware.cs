using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Vinance.Contracts.Exceptions;

namespace Vinance.Api.Middlewares
{
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

            switch (exception)
            {
                case UserNotFoundException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    return;
            }

            var result = JsonConvert.SerializeObject(new { message = exception.Message });
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(result);
        }
    }
}