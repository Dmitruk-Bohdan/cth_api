namespace CTHelper.Presentation.Security
{
    public static class CorsSetup
    {
        public static IServiceCollection AddCorsPolicy(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("FrontendPolicy", policy =>
                {
                    policy
                        .WithOrigins(
                            "",
                            ""
                        )
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials(); 
                });
            });

            return services;
        }
    }
}
