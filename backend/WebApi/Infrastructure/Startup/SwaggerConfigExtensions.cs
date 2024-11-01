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

                options.CustomSchemaIds(type =>
                {
                    var fullNameWithouPlusSign = type
                        .FullName
                        .Replace("+", "");

                    //se for generico
                    if (fullNameWithouPlusSign.Contains('['))
                    {
                        var pattern = new System.Text.RegularExpressions.Regex(@"\.(\w+)`[^,]*\.(\w+),");
                        var groups = pattern.Match(fullNameWithouPlusSign).Groups;
                        return $"{groups[2]}{groups[1]}";
                    }

                    var lastName = fullNameWithouPlusSign
                        .Substring(
                            fullNameWithouPlusSign.LastIndexOf('.') + 1);

                    return lastName;
                });
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
