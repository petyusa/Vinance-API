using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Vinance.Api.ActionFilters
{
    using Contracts;

    public class SwaggerContentTypeOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            operation.Produces = new List<string> { Constants.ApplicationJson };

            if(operation.Parameters.Any(p => p.GetType() == typeof(IFormFile)))
            {
                operation.Consumes = new List<string> { Constants.MultipartFormData };
            }
            else
            {
                operation.Consumes = new List<string> { Constants.ApplicationJson };
            }
        }
    }
}