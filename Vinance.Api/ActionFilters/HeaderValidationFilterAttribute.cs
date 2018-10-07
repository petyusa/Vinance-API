using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Vinance.Api.Exceptions;

namespace Vinance.Api.ActionFilters
{
    public class HeaderValidationFilterAttribute : IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            if (context.HttpContext.Request.Method.Equals("put", StringComparison.OrdinalIgnoreCase) ||
                context.HttpContext.Request.Method.Equals("post", StringComparison.OrdinalIgnoreCase))
            {
                if (context.HttpContext.Request.ContentType != "application/json")
                {
                    throw new HeaderContentTypeException("Content-type must be 'application/json'");
                }

            }
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }
}