using Microsoft.AspNetCore.Mvc;

namespace ArelympApi.Security
{
    public class ApiKeyAttribute : ServiceFilterAttribute
    {
        public ApiKeyAttribute() : base (typeof(ApiKeyAuthFilter)) 
        { }
    }
}
