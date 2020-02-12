using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace UserService.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwaggerDocumentation(this IServiceCollection services) 
        {
            services.AddSwaggerGen(c => { 
                c.SwaggerDoc("v1", new Info
                {
                    Title = "User Service API", Version = "v1"
                });
            });
        }

        public static void UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                s.SwaggerEndpoint("v1/swagger.json", "API v1.0");
                s.EnableFilter();
                s.DocExpansion(DocExpansion.None);
            });
        }
    }
}