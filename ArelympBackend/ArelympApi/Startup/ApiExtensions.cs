using ArelympApi.Attributes;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Unchase.Swashbuckle.AspNetCore.Extensions.Extensions;

namespace ProjectMobileApi.StartupExtensions
{
    public static class ApiExtensions
    {
        public static void AddApiServices(this IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(o =>
            {
                o.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0.1", new OpenApiInfo { Title = "ArelympBackend", Version = "v0.1" });
                c.AddSecurityDefinition("X-API-Key", new OpenApiSecurityScheme
                {
                    Description = "ApiKey must apper in header for GameServer requests",
                    Type = SecuritySchemeType.ApiKey,
                    Name = "X-API-Key",
                    In = ParameterLocation.Header,
                    Scheme = "ApiKeyScheme"
                });
                var key = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "X-API-Key"
                    },
                    In = ParameterLocation.Header,
                };
                var requirement = new OpenApiSecurityRequirement
                {
                    {key, new List<string>() }
                };
                c.AddSecurityRequirement(requirement);
                c.AddEnumsWithValuesFixFilters();
            });
        }
    }

}
