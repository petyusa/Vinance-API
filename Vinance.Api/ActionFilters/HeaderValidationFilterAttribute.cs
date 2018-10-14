using System;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Vinance.Api.ActionFilters
{
    using Contracts.Exceptions;

    public class HeaderValidationFilterAttribute : IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (!context.HttpContext.Request.Method.Equals("get", StringComparison.OrdinalIgnoreCase) &&
                context.HttpContext.Request.ContentType != "application/json")
            {
                throw new HeaderContentTypeException("Content-type must be 'application/json'");
            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}