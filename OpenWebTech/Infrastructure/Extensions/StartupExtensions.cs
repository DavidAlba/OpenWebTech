using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenWebTech.Infrastructure.Filters;
using OpenWebTech.Models;
using WebMVC.Infrastructure.Claims;

namespace OpenWebTech.Infrastructure.Extensions
{
    public static class StartupExtensions
    {
        public static IServiceCollection AddCustomRepository(this IServiceCollection services, IHostingEnvironment environment, ILogger logger)
        {
            logger.LogInformation($"Registering {nameof(IContactsRepository)} dependency injection ({environment.ApplicationName})...");
            logger.LogInformation($"Registering {nameof(ISkillsRepository)} dependency injection ({environment.ApplicationName})...");

            if (environment.IsDevelopment())
            {
                // Register repository as a Singleton life time 
                services.AddSingleton<IContactsRepository, FakeContactsRepository>();
                services.AddSingleton<ISkillsRepository, FakeSkillsRepository>();
            }
            else
            {
                // Register repository as a Singleton life time 
                services.AddSingleton<IContactsRepository, FakeContactsRepository>();
                services.AddSingleton<ISkillsRepository, FakeSkillsRepository>();
            }
            return services;
        }

        public static IServiceCollection AddCustomMVC(this IServiceCollection services, IHostingEnvironment environment, ILogger logger)
        {

            logger.LogInformation($"Adding Custom MVC ({environment.ApplicationName})...");

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices();

            //services.AddApiVersioning(options =>
            //{                
            //    options.ApiVersionReader = new HeaderApiVersionReader("api-version");
            //});

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                    .SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IHostingEnvironment environment, ILogger logger)
        {
            logger.LogInformation($"Adding Swagger ({environment.ApplicationName})...");
            services.AddSwaggerGen(options =>
            {
                options.DescribeAllEnumsAsStrings();
                options.SwaggerDoc(
                    "v1",
                    new Swashbuckle.AspNetCore.Swagger.Info
                    {
                        Title = $"{environment.ApplicationName} Web API",
                        Version = "v1",
                        Description = $"The {environment.ApplicationName} Microservice HTTP API",
                        TermsOfService = "Terms Of Service"
                    });
            });

            return services;
        }

        public static IServiceCollection AddCustomSecurity(this IServiceCollection services, IHostingEnvironment environment, ILogger logger)
        {
            logger.LogInformation($"Adding Security ({environment.ApplicationName})...");

            //services.AddSingleton<IClaimsTransformation, ContactClaimsProvider>();
           

            return services;
        }

    }
}
