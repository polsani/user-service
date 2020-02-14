using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Configurations;
using UserService.Data.Contexts;
using UserService.Data.Repositories;
using UserService.Domain.Data;
using UserService.Domain.Factories;
using UserService.Domain.Helpers;
using UserService.Domain.Mappers;
using UserService.Domain.Services;
using UserService.Extensions;
using UserService.Factories;
using UserService.HealthChecks;
using UserService.Helpers;
using UserService.Mappers;
using UserService.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace UserService
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
            RabbitMqConfiguration.Configure(configuration);
            UserServiceConfiguration.Configure(configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("Database");

            services.AddDbContext<DefaultContext>();
            
            services.AddScoped<ISupportedMediaHelper, SupportedMediaHelper>();
            services.AddScoped<IImportService, ImportService>() ;
            services.AddScoped<IMediaMapperFactory, MediaMapperFactory>();
            services.AddScoped<IMediaMapper, CsvMediaMapper>();
            services.AddScoped<IMediaMapper, ExcelMediaMapper>();

            services.AddScoped<IImportRepository, ImportRepository>();

            services.AddSwaggerDocumentation();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
            app.UseHealthChecks();
            app.UseSwaggerDocumentation();
            
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<DefaultContext>().Database.Migrate();
            }
        }
    }
}