using Frontend.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ArelympApi.Security
{
    public class ApiKeyAuthFilter : IAuthorizationFilter
    {
        private readonly IApiKeyValidation _apiKeyValidation;

        public ApiKeyAuthFilter(IApiKeyValidation apiKeyValidation)
        {
            _apiKeyValidation = apiKeyValidation;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var clientApiKey = context.HttpContext.Request.Headers[RequestHeaders.ApiKeyHeaderName];

            if (string.IsNullOrEmpty(clientApiKey))
            {
                context.Result = new BadRequestResult();

                return;
            }

            if (!_apiKeyValidation.IsValidApiKey(clientApiKey))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
