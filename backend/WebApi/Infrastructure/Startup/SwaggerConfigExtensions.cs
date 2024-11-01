using Microsoft.OpenApi.Models;

namespace WebApi.Infrastructure.Startup
{
    public static class SwaggerConfigExtensions
    {
        public static void AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "API",
                    Description = "API de teste",
                });

                options.CustomSchemaIds(t => t.FullName?.Replace("+", "."));
            });
        }

        public static void UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "docs";
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
            });
        }
    }
}
