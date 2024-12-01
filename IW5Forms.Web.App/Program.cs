using AutoMapper.Internal;
using IW5Forms.Common.Extentions;
using IW5Forms.Web.App;
using IW5Forms.Web.BL.Extensions;
using IW5Forms.Web.BL.Installers;
using IW5Forms.Web.BL.Options;
using IW5Forms.Web.DAL.Installers;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Configuration.AddJsonFile("appsettings.json");

var apiBaseUrl = builder.Configuration.GetValue<string>("ApiBaseUrl");

builder.Services.AddInstaller<WebDALInstaller>();
builder.Services.AddInstaller<WebBLInstaller>(apiBaseUrl);
builder.Services.AddScoped(sp => new HttpClient {
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});
builder.Services.AddAutoMapper(configuration =>
{
    // This is a temporary fix - should be able to remove this when version 11.0.2 comes out
    // More information here: https://github.com/AutoMapper/AutoMapper/issues/3988
    configuration.Internal().MethodMappingEnabled = false;
}, typeof(WebBLInstaller));

builder.Services.Configure<LocalDbOptions>(options => {
    options.IsLocalDbEnabled = bool.Parse(
        builder.Configuration.GetSection(
            nameof(LocalDbOptions)
        )[nameof(LocalDbOptions.IsLocalDbEnabled)]
    );
});

builder.Services.AddScoped(sp => new HttpClient {
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddMudServices();

await builder.Build().RunAsync();
