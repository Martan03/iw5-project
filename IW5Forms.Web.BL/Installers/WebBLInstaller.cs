using IW5Forms.Common.BL.Facades;
using Microsoft.Extensions.DependencyInjection;

namespace IW5Forms.Web.BL.Installers;

public class WebBLInstaller
{
    public void Install(
        IServiceCollection serviceCollection,
        string apiBaseUrl
    ) {
        serviceCollection.AddTransient<IFormApiClient, FormApiClient>(
            provider => {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new FormApiClient(client, apiBaseUrl);
            }
        );

        serviceCollection.AddTransient<IQuestionApiClient, QuestionApiClient>(
            provider => {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new QuestionApiClient(client, apiBaseUrl);
            }
        );

        serviceCollection.AddTransient<IAnswerApiClient, AnswerApiClient>(
            provider => {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new AnswerApiClient(client, apiBaseUrl);
            }
        );

        serviceCollection.AddTransient<IUserApiClient, UserApiClient>(
            provider => {
                var client = CreateApiHttpClient(provider, apiBaseUrl);
                return new UserApiClient(client, apiBaseUrl);
            }
        );

        serviceCollection.Scan(
            selector => selector.FromAssemblyOf<WebBLInstaller>()
                .AddClasses(classes => classes.AssignableTo<IAppFacade>())
                .AsSelfWithInterfaces()
                .WithTransientLifetime()
        );
    }

    public HttpClient CreateApiHttpClient(
        IServiceProvider serviceProvider,
        string apiBaseUrl
    ) {
        var client = new HttpClient() {
            BaseAddress = new Uri(apiBaseUrl)
        };
        client.BaseAddress = new Uri(apiBaseUrl);
        return client;
    }
}
