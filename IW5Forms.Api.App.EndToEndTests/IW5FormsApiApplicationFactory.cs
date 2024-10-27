using System.Reflection;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IW5Forms.Api.App.EndToEndTests;

public class IW5FormsApiApplicationFactory : WebApplicationFactory<Program>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(collection =>
        {
            var controllerAssemblyName = typeof(Program).Assembly.FullName;
            collection
                .AddMvc()
                .AddApplicationPart(Assembly.Load(controllerAssemblyName));
        });
        return base.CreateHost(builder);
    }
}
