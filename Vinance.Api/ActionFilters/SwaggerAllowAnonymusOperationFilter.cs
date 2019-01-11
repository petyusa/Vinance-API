using Microsoft.AspNetCore.Mvc.Authorization;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;

namespace Vinance.Api.ActionFilters
{
    public class SwaggerAllowAnonymusOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var hasAllowAnonymous = context.ApiDescription.ActionDescriptor.FilterDescriptors.Any(f => f.Filter.GetType() == typeof(AllowAnonymousFilter));
            if (hasAllowAnonymous)
                return;

            operation.Security = new List<IDictionary<string, IEnumerable<string>>>
            {
                new Dictionary<string, IEnumerable<string>> {{"Bearer", new string[] {}}}
            };
        }
    }
}