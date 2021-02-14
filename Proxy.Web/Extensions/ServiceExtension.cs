using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Polly;
using Polly.Extensions.Http;
using Proxy.Domain.Interface.Managers;
using Proxy.Domain.Interface.Repository;
using Proxy.Domain.Interface.Services;
using Proxy.Domain.Managers;
using Proxy.Infrastructure.Repositories;
using Proxy.Infrastructure.Services;
using Proxy.Web.Interface.Services;
using Proxy.Web.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Threading.Tasks;

namespace Proxy.Web.Extensions
{
    public static class ServiceExtension
    {
        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            //Add Managers
            services.AddScoped<IAccountManager, AccountManager>();

            //Add Repositories
            services.AddScoped<IInMemoryUserRepository, InMemoryUserRepository>();
            
            //Add Services
            var jwtOptions = configuration.GetSection("Jwt").Get<TokenService.Options>();
            services.AddScoped<ITokenService, TokenService>(_ => new TokenService(jwtOptions));
            services.AddScoped<ILogMessageManager, LogMessageManager>();
            services.AddScoped<ILogMessageService, LogMessageService>();
            services.AddHttpClient("externalservice", client =>
            {
                client.BaseAddress = new Uri(configuration["ExternalUrl:BaseUrl"]);
                client.DefaultRequestHeaders.Add("accept", "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration["ExternalUrl:ApiKey"]);
            }).AddPolicyHandler(GetRetryPolicy());
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Proxy API", Version = "v1" });
                c.CustomSchemaIds((type) => type.IsNested ? type.FullName : type.Name);

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Specify the authorization token.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                };

                OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                {
                    { securityDefinition, Array.Empty<string>()},
                };

                c.AddSecurityRequirement(securityRequirements);
            });
        }

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }
    }
}
