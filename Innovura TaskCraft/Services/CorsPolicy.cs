using Microsoft.Extensions.DependencyInjection;

namespace Innovura_TaskCraft.Services
{
    public static class CorsPolicy
    {
        public static void ConfigureCORS(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader().Build());
            });
        }
    }
}
