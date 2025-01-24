using System.Security.Claims;
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
using IW5Forms.Common;
using IW5Forms.Common.Extentions;
using IW5Forms.Common.Models.Answer;
using IW5Forms.Common.Models.Form;
using IW5Forms.Common.Models.Question;
using IW5Forms.Common.Models.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

            // builder.Services.AddAuthentication();

            ConfigureCors(builder.Services);
            ConfigureOpenApiDocuments(builder.Services);
            ConfigureDependencies(builder.Services, builder.Configuration);
            ConfigureAutoMapper(builder.Services);

            ConfigureAuthentication(builder.Services, builder.Configuration.GetSection("IdentityServer")["Url"]!);

            var app = builder.Build();


            ValidateAutoMapperConfiguration(app.Services);

            UseDevelopmentSettings(app);
            UseSecurityFeatures(app);
            UseRouting(app);
            UseAuthorization(app);
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
                        // .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });
        }

        private static void ConfigureOpenApiDocuments(IServiceCollection serviceCollection)
        {
            serviceCollection.AddEndpointsApiExplorer();
            serviceCollection.AddOpenApiDocument();
        }

        private static void ConfigureAuthentication(IServiceCollection serviceCollection, string identityServerUrl)
        {
            serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = identityServerUrl;
                    options.TokenValidationParameters.ValidateAudience = false;
                });

            serviceCollection.AddAuthorization(
                options =>
                {
                    options.AddPolicy(ApiPolicies.FormsAdmin, policy => policy.RequireRole(AppRoles.Admin));
                }
                );
            serviceCollection.AddHttpContextAccessor();
        }

        private static void ConfigureDependencies(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            //EF stuff
            var connectionString = configuration.GetConnectionString("AZURE_SQL_CONNECTION_STRING")
                                   ?? configuration.GetConnectionString("TestConnection1");

            serviceCollection.AddDbContext<FormsDbContext>(options =>
            {

                options.UseSqlServer(connectionString);
            });

            serviceCollection.AddInstaller<RepositoriesInstaller>();

            serviceCollection.AddInstaller<ApiBLInstaller>();

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

            searchEndpoints.MapGet("/user", (IUserFacade userFacade, string name) => userFacade.SearchByName(name)).RequireAuthorization(ApiPolicies.FormsAdmin);
            searchEndpoints.MapGet("/question/text", (IQuestionFacade questionFacade, string text) => questionFacade.SearchByText(text)).RequireAuthorization(ApiPolicies.FormsAdmin);
            searchEndpoints.MapGet("/question/description", (IQuestionFacade questionFacade, string description) => questionFacade.SearchByDescription(description)).RequireAuthorization(ApiPolicies.FormsAdmin);
        }

        private static void UseUserEndpoints(RouteGroupBuilder routeGroupBuilder)
        {
            var userEndpoints = routeGroupBuilder.MapGroup("user")
                .WithTags("user");

            // get all users - require admin
            userEndpoints.MapGet("", (IUserFacade userFacade) => userFacade.GetAll())
                .RequireAuthorization(ApiPolicies.FormsAdmin);

            // get user by id - require login
            userEndpoints.MapGet("{id:guid}", Results<Ok<UserDetailModel>, NotFound<string>> (Guid id, IUserFacade userFacade)
                => userFacade.GetById(id) is { } user
                    ? TypedResults.Ok(user)
                    : TypedResults.NotFound("User with id:" + id + " was not found."))
                .RequireAuthorization();

            // create new user - require login
            userEndpoints.MapPost("", (UserDetailModel user, IUserFacade userFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);
                return userFacade.Create(user, userId);
            }).RequireAuthorization();

            // update user - require login
            userEndpoints.MapPut("", (UserDetailModel user, IUserFacade userFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                return userFacade.Update(user, userId);
            }).RequireAuthorization();

            // upsert user - require login
            userEndpoints.MapPost("upsert", (UserDetailModel user, IUserFacade userFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                return userFacade.CreateOrUpdate(user, userId);
            }).RequireAuthorization();

            //delete user - require login
            userEndpoints.MapDelete("{id:guid}", (Guid id, IUserFacade userFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                userFacade.Delete(id, userId);
            }).RequireAuthorization();

        }

        private static void UseFormEndpoints(RouteGroupBuilder routeGroupBuilder)
        {
            var formEndpoints = routeGroupBuilder.MapGroup("form")
                .WithTags("form");

            // get all forms - require admin
            formEndpoints.MapGet("", (IFormFacade formFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                return formFacade.GetAll();

                //var isAdmin = IsAdmin(httpContextAccessor);
                //if (isAdmin != null && (isAdmin! == true))
                //{
                //     formFacade.GetAll();
                //}
                //else formFacade.GetAllOwned(GetUserId(httpContextAccessor));  
            }).RequireAuthorization();

            // get form by id - require login
            formEndpoints.MapGet("{id:guid}", Results<Ok<FormDetailModel>, NotFound<string>> (Guid id, IFormFacade formFacade)
                => formFacade.GetById(id) is { } form
                    ? TypedResults.Ok(form)
                    : TypedResults.NotFound("Form with id:" + id + " was not found."))
                .RequireAuthorization();

            // create new form - reauire login
            formEndpoints.MapPost("", (FormDetailModel form, IFormFacade formFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                return formFacade.Create(form, userId);
            }).RequireAuthorization();

            //update form - require login
            formEndpoints.MapPut("", (FormDetailModel form, IFormFacade formFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                return formFacade.Update(form, userId);
            });

            //upsert form - require login
            formEndpoints.MapPost("upsert", (FormDetailModel form, IFormFacade formFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                return formFacade.CreateOrUpdate(form, userId);
            }).RequireAuthorization();

            //delete form - require login
            formEndpoints.MapDelete("{id:guid}", (Guid id, IFormFacade formFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                formFacade.Delete(id, userId);
            }).RequireAuthorization();
        }
        private static void UseAuthorization(WebApplication application)
        {
            application.UseAuthentication();
            application.UseAuthorization();
        }
        private static void UseAnswerEndpoints(RouteGroupBuilder routeGroupBuilder)
        {
            var answerEndpoints = routeGroupBuilder.MapGroup("answer")
                .WithTags("answer");

            //get all answers - require admin
            answerEndpoints.MapGet("", (IAnswerFacade answerFacade) => answerFacade.GetAll()).RequireAuthorization(ApiPolicies.FormsAdmin);

            //get answer by id - require login
            answerEndpoints.MapGet("{id:guid}", Results<Ok<AnswerListAndDetailModel>, NotFound<string>> (Guid id, IAnswerFacade answerFacade)
                => answerFacade.GetById(id) is { } answer
                    ? TypedResults.Ok(answer)
                    : TypedResults.NotFound("Answer with id:" + id + " was not found."))
                .RequireAuthorization();

            //create new answer - no requirements
            answerEndpoints.MapPost("", (AnswerListAndDetailModel answer, IAnswerFacade answerFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                return answerFacade.Create(answer, userId);
            });

            //update answer - no requirements
            answerEndpoints.MapPut("", (AnswerListAndDetailModel answer, IAnswerFacade answerFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                return answerFacade.Update(answer, userId);
            });

            //upsert answer - no requirements
            answerEndpoints.MapPost("upsert", (AnswerListAndDetailModel answer, IAnswerFacade answerFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                return answerFacade.CreateOrUpdate(answer, userId);
            });

            //delete answer - no requirements
            answerEndpoints.MapDelete("{id:guid}", (Guid id, IAnswerFacade answerFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                answerFacade.Delete(id, userId);
            });
        }

        private static void UseQuestionEndpoints(RouteGroupBuilder routeGroupBuilder)
        {
            var questionEndpoints = routeGroupBuilder.MapGroup("question")
                .WithTags("question");

            //get all questions - require admin
            questionEndpoints.MapGet("", (IQuestionFacade questionFacade) => questionFacade.GetAll()).RequireAuthorization(ApiPolicies.FormsAdmin);

            //get question by id - require login
            questionEndpoints.MapGet("{id:guid}", Results<Ok<QuestionDetailModel>, NotFound<string>> (Guid id, IQuestionFacade questionFacade)
                => questionFacade.GetById(id) is { } question
                    ? TypedResults.Ok(question)
                    : TypedResults.NotFound("Question with id:" + id + " was not found.")).RequireAuthorization();

            //create question - require login
            questionEndpoints.MapPost("", (QuestionDetailModel question, IQuestionFacade questionFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                return questionFacade.Create(question, userId);
            }).RequireAuthorization();

            //update question - require login
            questionEndpoints.MapPut("", (QuestionDetailModel question, IQuestionFacade questionFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                return questionFacade.Update(question, userId);
            }).RequireAuthorization();

            //upsert question - require login
            questionEndpoints.MapPost("upsert", (QuestionDetailModel question, IQuestionFacade questionFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                return questionFacade.CreateOrUpdate(question, userId);
            }).RequireAuthorization();

            //delete question - require login
            questionEndpoints.MapDelete("{id:guid}", (Guid id, IQuestionFacade questionFacade, IHttpContextAccessor httpContextAccessor) =>
            {
                var userId = GetUserId(httpContextAccessor);

                questionFacade.Delete(id, userId);
            }).RequireAuthorization();

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

        public static string? GetUserId(IHttpContextAccessor httpContextAccessor)
        {
            var idClaim = httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return idClaim?.Value;
        }

        public static bool? IsAdmin(IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor.HttpContext?.User.IsInRole("forms-admin");
        }
    }
}
