using BusinessDomain.Constants;
using BusinessDomain.Extensions;
using Microsoft.AspNetCore.Authentication;
using SharedLibrary.Enum;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ProjectMobileApi.Extensions
{
    public static class HttpContextExtensions
    {
        public static string GetCompleteUserId(this HttpContext httpContext)
        {
            var platform = httpContext.GetPlatform();
            return httpContext.GetRawUserId()
                .AddPlatformPrefix(platform);
        }

        public static string GetRawUserId(this HttpContext httpContext)
        {
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null) 
            { 
                throw new ArgumentNullException(nameof(userId)); 
            }

            return userId;
        }

        public static Platform GetPlatform(this HttpContext httpContext)
        {
            var platformString = httpContext.User.FindFirstValue(RequestHeaders.Platform);

            if (platformString == null) { throw new ArgumentNullException(nameof(platformString)); }

            if (!Enum.TryParse(platformString, out Platform platform))
            {
                throw new FormatException($"{platform} could not be parsed to {typeof(Platform)}");
            }

            return platform;
        }
    }
}
