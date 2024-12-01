using IW5Forms.Common.Installers;
using IW5Forms.Web.DAL.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IW5Forms.Web.DAL.Installers;

public class WebDALInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<LocalDb>();
        serviceCollection.Scan(scan =>
            scan.FromAssemblyOf<WebDALInstaller>()
                .AddClasses(classes =>
                    classes.AssignableTo(typeof(IWebRepository<>))
                )
                .AsSelf()
                .WithSingletonLifetime());
    }
}