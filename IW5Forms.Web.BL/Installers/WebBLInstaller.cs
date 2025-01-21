using IW5Forms.Common.BL.Facades;
using Microsoft.Extensions.DependencyInjection;

namespace IW5Forms.Web.BL.Installers;

public class WebBLInstaller
{
    public void Install(
        IServiceCollection serviceCollection
    ) {
        serviceCollection.AddScoped<IAnswerApiClient, AnswerApiClient>();
        serviceCollection.AddScoped<IFormApiClient, FormApiClient>();
        serviceCollection.AddScoped<IQuestionApiClient, QuestionApiClient>();
        serviceCollection.AddScoped<ISearchApiClient, SearchApiClient>();
        serviceCollection.AddScoped<IUserApiClient, UserApiClient>();


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
