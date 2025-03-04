using CesarBmx.Shared.Api.Configuration;


namespace CesarBmx.Notification.Api.Configuration
{
    public static class ElasticConfig
    {
        public static IServiceCollection ConfigureElastic(this IServiceCollection services)
        {
            services.ConfigureSharedElastic();

            return services;
        }
    }
}
