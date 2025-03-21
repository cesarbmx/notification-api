﻿using CesarBmx.Shared.Api.Configuration;


namespace CesarBmx.Notification.Api.Configuration
{
    public static class ElasticConfig
    {
        public static IServiceCollection ConfigureElastic(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigureSharedElastic(configuration);

            return services;
        }
    }
}
