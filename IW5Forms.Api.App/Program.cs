using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using IW5Forms.Common.Extentions;
using ServiceCollectionExtensions = IW5Forms.Common.Extentions.ServiceCollectionExtensions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Localization;

namespace IW5Forms.Api.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();

            ConfigureCors(builder.Services);
            ConfigureOpenApiDocuments(builder.Services);
            ConfigureDependencies(builder.Services, builder.Configuration);
            ConfigureAutoMapper(builder.Services);

            var app = builder.Build();

            ValidateAutoMapperConfiguration(app.Services);

            UseDevelopmentSettings(app);
            UseSecurityFeatures(app);
            UseRouting(app);
            UseEndpoints(app);
            UseOpenApi(app);

            app.Run();
        }

        private static void ConfigureCors(IServiceCollection serviceCollection)
        {
            serviceCollection.AddCors(options =>
            {
                options.AddDefaultPolicy(o =>
                    o.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
        }

        private static void ConfigureOpenApiDocuments(IServiceCollection serviceCollection)
        {
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddOpenApiDocument();
        }

        private static void ConfigureDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //EF stuff

            ServiceCollectionExtensions.AddInstaller<ApiBLInstaller>(serviceCollection);
        }

        private static void ConfigureAutoMapper(IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(ApiBLInstaller));
        }

        private static void ValidateAutoMapperConfiguration(IServiceProvider serviceProvider)
        {
            var mapper = serviceProvider.GetRequiredService<IMapper>();
            mapper.ConfigurationProvider.AssertConfigurationIsValid();
        }

        private static void UseEndpoints(WebApplication application)
        {
            var endpointsBase = application.MapGroup("api")
                .WithOpenApi();

            UseSearchEndpoints(endpointsBase);
            UseUserEndpoints(endpointsBase);
            UseFormEndpoints(endpointsBase);
            UseAnswerEndpoints(endpointsBase);
            UseQuestionEndpoints(endpointsBase);
        }

        private static void UseSearchEndpoints(RouteGroupBuilder routeGroupBuilder)
        {
         
        }

        private static void UseUserEndpoints(RouteGroupBuilder routeGroupBuilder)
        {

        }

        private static void UseFormEndpoints(RouteGroupBuilder routeGroupBuilder)
        {

        }

        private static void UseAnswerEndpoints(RouteGroupBuilder routeGroupBuilder)
        {

        }

        private static void UseQuestionEndpoints(RouteGroupBuilder routeGroupBuilder)
        {

        }

        private static void UseDevelopmentSettings(WebApplication application)
        {
            var environment = application.Services.GetRequiredService<IWebHostEnvironment>();

            if (environment.IsDevelopment())
            {
                application.UseDeveloperExceptionPage();
            }
        }

        private static void UseSecurityFeatures(IApplicationBuilder application)
        {
            application.UseCors();
            application.UseHttpsRedirection();
        }

        private static void UseRouting(IApplicationBuilder application)
        {
            application.UseRouting();
        }

        private static void UseOpenApi(IApplicationBuilder application)
        {
            application.UseOpenApi();
            application.UseSwaggerUi();
        }
    }
}
