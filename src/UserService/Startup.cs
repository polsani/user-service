using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserService.Configurations;
using UserService.Data.Contexts;
using UserService.Data.Repositories;
using UserService.Data.UnitOfWork;
using UserService.Domain.Data.Repositories;
using UserService.Domain.Data.UnitOfWork;
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
        private readonly MessagingService _messagingService;

        public Startup(IConfiguration configuration)
        {
            RabbitMqConfiguration.Configure(configuration);
            UserServiceConfiguration.Configure(configuration);

            _messagingService = MessagingServiceSingleton.Instance;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddHealthChecks()
                .AddCheck<DatabaseHealthCheck>("Database");

            services.AddDbContext<DefaultContext>();
            
            services.AddScoped<ISupportedMediaHelper, SupportedMediaHelper>();
            services.AddScoped<IImportService, ImportService>() ;
            services.AddScoped<IUserService, Services.UserService>();
            services.AddScoped<IMediaMapperFactory, MediaMapperFactory>();
            services.AddScoped<IMediaMapper, CsvMediaMapper>();
            services.AddScoped<IMediaMapper, ExcelMediaMapper>();
            services.AddScoped<IImportMapper, ImportMapper>();
            services.AddScoped<IUserMapper, UserMapper>();

            services.AddScoped<IUserToImportMapper, UserToImportMapper>();

            services.AddScoped<IImportRepository, ImportRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPreviousImportItemRepository, PreviousImportItemRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton(_messagingService);

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
            app.UseUserConsumer(_messagingService);
            
            using (var scope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetService<DefaultContext>().Database.Migrate();
            }
        }
    }
}