using IW5Forms.Api.DAL.Common.Repositories;
using IW5Forms.Common.Installers;
using Microsoft.Extensions.DependencyInjection;

namespace IW5Forms.Api.DAL.Memory.Installers;

public class ApiDALMemoryInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.Scan(selector =>
            selector.FromAssemblyOf<ApiDALMemoryInstaller>()
                .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
                    .AsMatchingInterface()
                    .WithTransientLifetime()
                .AddClasses(classes => classes.AssignableTo<Storage>())
                    .AsSelf()
                    .WithSingletonLifetime()
        );
    }
}
