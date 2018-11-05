using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Vinance.Api.ActionFilters
{
    using Contracts;
    using Contracts.Exceptions;

    public class HeaderValidationFilter : IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!context.HttpContext.Request.Method.Equals("get", StringComparison.OrdinalIgnoreCase) &&
                !context.HttpContext.Request.ContentType.Contains(Constants.ApplicationJson, StringComparison.OrdinalIgnoreCase))
            {
                throw new HeaderContentTypeException($"Content-type must be {Constants.ApplicationJson}");
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}