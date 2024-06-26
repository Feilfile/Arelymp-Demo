using BusinessDomain.Services.Abstract;
using BusinessDomain.Services;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using BusinessDomain.ExternaServices;
using Microsoft.Extensions.Caching.Memory;

namespace ProjectMobileApi.StartupExtensions
{
    public static class ServiceExtensions
    {
        public static void AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMemoryCache();
            services.AddDbContext<GameDbContext>(o => o.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IInventoryService, InventoryService>();
            services.AddScoped<IGoogleApiService, GoogleApiService>();
            services.AddScoped<IMatchmakingService, MatchmakingService>();
            services.AddScoped<ICharacterLoadoutService, CharacterLoadoutService>();
            services.AddScoped<ILevelingService, LevelingService>();

            services.AddHttpClient<IGoogleApiService, GoogleApiService>();
            services.AddHttpClient<IMatchmakingService, MatchmakingService>();
        }
    }
}
