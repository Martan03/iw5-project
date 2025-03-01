using AutoMapper.Internal;
using IW5Forms.Common.Extentions;
using IW5Forms.Web.App;
using IW5Forms.Web.BL.Extensions;
using IW5Forms.Web.BL.Installers;
using IW5Forms.Web.BL.Options;
using IW5Forms.Web.DAL.Installers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using MudBlazor.Services;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl");

builder.Services.AddInstaller<WebDALInstaller>();
builder.Services.AddInstaller<WebBLInstaller>();
builder.Services.AddTransient<CustomAuthorizationMessageHandler>();

builder.Services.AddHttpClient("api", client => client.BaseAddress = new Uri(apiBaseUrl))
    .AddHttpMessageHandler(serviceProvider
        => serviceProvider?.GetService<CustomAuthorizationMessageHandler>()
            ?.ConfigureHandler(
                authorizedUrls: new[] { apiBaseUrl },
                exceptionUrls: new[] { "/api/form/id/", "/api/answer/upsert" , "/api/form/getAll" },
                scopes: new[] { "iw5api" }));

builder.Services.AddScoped(sp => sp.GetService<IHttpClientFactory>().CreateClient("api"));

//builder.Services.AddScoped<HttpClient>(serviceProvider =>
//{
//    var messageHandler = serviceProvider?.GetService<CustomAuthorizationMessageHandler>()
//        ?.ConfigureHandler(
//            authorizedUrls: new[] { apiBaseUrl },
//            scopes: new[] { "iw5api" });
//    if (messageHandler is not null)
//    {
//        messageHandler.InnerHandler = new HttpClientHandler();
//        var client = new HttpClient(messageHandler);
//        return client;
//    }

//    throw new ArgumentException();
//});

builder.Services.AddAutoMapper(configuration =>
{
    // This is a temporary fix - should be able to remove this when version 11.0.2 comes out
    // More information here: https://github.com/AutoMapper/AutoMapper/issues/3988
    configuration.Internal().MethodMappingEnabled = false;
}, typeof(WebBLInstaller));

builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("IdentityServer", options.ProviderOptions);
    var configurationSection = builder.Configuration.GetSection("IdentityServer");
    var authority = configurationSection["Authority"];

    options.ProviderOptions.Authority = authority;
    options.ProviderOptions.ClientId = "formsclient";
    options.ProviderOptions.DefaultScopes.Add("iw5api");
    options.ProviderOptions.PostLogoutRedirectUri = configurationSection["PostLogoutRedirectUri"];

    options.UserOptions.RoleClaim = "role";
});

builder.Services.Configure<LocalDbOptions>(options => {
    options.IsLocalDbEnabled = bool.Parse(
        builder.Configuration.GetSection(
            nameof(LocalDbOptions)
        )[nameof(LocalDbOptions.IsLocalDbEnabled)]
    );
});



builder.Services.AddMudServices();

await builder.Build().RunAsync();
