using IW5Forms.Common.BL.Facades;
using IW5Forms.Common.Installers;
using Microsoft.Extensions.DependencyInjection;
using System;

public class ApiBLInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.Scan(selector =>
            selector.FromAssemblyOf<ApiBLInstaller>()
                    .AddClasses(classes => classes.AssignableTo<IAppFacade>())
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime());
    }
}
