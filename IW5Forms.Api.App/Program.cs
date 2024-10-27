using AutoMapper;
using IW5Forms.API.DAL;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;
using IW5Forms.Api.DAL.EF.Repositories;
using IW5Forms.Common.Models;
using Microsoft.EntityFrameworkCore;
using IW5Forms.Api.DAL.EF.Installers;
using Microsoft.Extensions.DependencyInjection;
using IW5Forms.Api.BL.Facades;
using IW5Forms.Common.Models.Answer;
using IW5Forms.Common.Models.Form;
using IW5Forms.Common.Models.Question;
using IW5Forms.Common.Models.User;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Localization;
using ServiceCollectionExtensions = IW5Forms.Common.Extentions.ServiceCollectionExtensions;

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

        static void SeedData(IHost app)
        {
            var facorry = app.Services.GetService<IServiceScopeFactory>();

            using (var scope = facorry.CreateScope())
            {
                var service = scope.ServiceProvider.GetService<SeedScript>();
                service.SeedData();
            }
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

            serviceCollection.AddTransient<SeedScript>();
            serviceCollection.AddDbContext<FormsDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("TestConnection"));
            });

            ServiceCollectionExtensions.AddInstaller<ApiBLInstaller>(serviceCollection);
        }

        private static void ConfigureAutoMapper(IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(EntityBase), typeof(ApiBLInstaller));
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

            var searchEndpoints = routeGroupBuilder.MapGroup("search")
                .WithTags("search");

            searchEndpoints.MapGet("/user", (IUserFacade userFacade, string name) => userFacade.SearchByName(name));
            searchEndpoints.MapGet("/question/text", (IQuestionFacade questionFacade, string text) => questionFacade.SearchByText(text));
            searchEndpoints.MapGet("/question/description", (IQuestionFacade questionFacade, string description) => questionFacade.SearchByDescription(description));
        }

        private static void UseUserEndpoints(RouteGroupBuilder routeGroupBuilder)
        {
            var userEndpoints = routeGroupBuilder.MapGroup("user")
                .WithTags("user");

            userEndpoints.MapGet("", (IUserFacade userFacade) => userFacade.GetAll());

            userEndpoints.MapGet("{id:guid}", Results<Ok<UserDetailModel>, NotFound<string>> (Guid id, IUserFacade userFacade)
                => userFacade.GetById(id) is { } user
                    ? TypedResults.Ok(user)
                    : TypedResults.NotFound("User with id:" + id + " was not found."));

            userEndpoints.MapPost("", (UserDetailModel user, IUserFacade userFacade) => userFacade.Create(user));
            userEndpoints.MapPut("", (UserDetailModel user, IUserFacade userFacade) => userFacade.Update(user));
            userEndpoints.MapPost("upsert", (UserDetailModel user, IUserFacade userFacade) => userFacade.CreateOrUpdate(user));
            userEndpoints.MapDelete("{id:guid}", (Guid id, IUserFacade userFacade) => userFacade.Delete(id));

        }

        private static void UseFormEndpoints(RouteGroupBuilder routeGroupBuilder)
        {
            var formEndpoints = routeGroupBuilder.MapGroup("form")
                .WithTags("form");

            formEndpoints.MapGet("", (IFormFacade formFacade) => formFacade.GetAll());

            formEndpoints.MapGet("{id:guid}", Results<Ok<FormDetailModel>, NotFound<string>> (Guid id, IFormFacade formFacade)
                => formFacade.GetById(id) is { } form
                    ? TypedResults.Ok(form)
                    : TypedResults.NotFound("Form with id:" + id + " was not found."));

            formEndpoints.MapPost("", (FormDetailModel form, IFormFacade formFacade) => formFacade.Create(form));
            formEndpoints.MapPut("", (FormDetailModel form, IFormFacade formFacade) => formFacade.Update(form));
            formEndpoints.MapPost("upsert", (FormDetailModel form, IFormFacade formFacade) => formFacade.CreateOrUpdate(form));
            formEndpoints.MapDelete("{id:guid}", (Guid id, IFormFacade formFacade) => formFacade.Delete(id));
        }

        private static void UseAnswerEndpoints(RouteGroupBuilder routeGroupBuilder)
        {
            var answerEndpoints = routeGroupBuilder.MapGroup("answer")
                .WithTags("answer");

            answerEndpoints.MapGet("", (IAnswerFacade answerFacade) => answerFacade.GetAll());

            answerEndpoints.MapGet("{id:guid}", Results<Ok<AnswerListAndDetailModel>, NotFound<string>> (Guid id, IAnswerFacade answerFacade)
                => answerFacade.GetById(id) is { } answer
                    ? TypedResults.Ok(answer)
                    : TypedResults.NotFound("Answer with id:" + id + " was not found."));

            answerEndpoints.MapPost("", (AnswerListAndDetailModel answer, IAnswerFacade answerFacade) => answerFacade.Create(answer));
            answerEndpoints.MapPut("", (AnswerListAndDetailModel answer, IAnswerFacade answerFacade) => answerFacade.Update(answer));
            answerEndpoints.MapPost("upsert", (AnswerListAndDetailModel answer, IAnswerFacade answerFacade) => answerFacade.CreateOrUpdate(answer));
            answerEndpoints.MapDelete("{id:guid}", (Guid id, IAnswerFacade answerFacade) => answerFacade.Delete(id));
        }

        private static void UseQuestionEndpoints(RouteGroupBuilder routeGroupBuilder)
        {
            var questionEndpoints = routeGroupBuilder.MapGroup("question")
                .WithTags("question");

            questionEndpoints.MapGet("", (IQuestionFacade questionFacade) => questionFacade.GetAll());

            questionEndpoints.MapGet("{id:guid}", Results<Ok<QuestionDetailModel>, NotFound<string>> (Guid id, IQuestionFacade questionFacade)
                => questionFacade.GetById(id) is { } question
                    ? TypedResults.Ok(question)
                    : TypedResults.NotFound("Question with id:" + id + " was not found."));

            questionEndpoints.MapPost("", (QuestionDetailModel question, IQuestionFacade questionFacade) => questionFacade.Create(question));
            questionEndpoints.MapPut("", (QuestionDetailModel question, IQuestionFacade questionFacade) => questionFacade.Update(question));
            questionEndpoints.MapPost("upsert", (QuestionDetailModel question, IQuestionFacade questionFacade) => questionFacade.CreateOrUpdate(question));
            questionEndpoints.MapDelete("{id:guid}", (Guid id, IQuestionFacade questionFacade) => questionFacade.Delete(id));

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
