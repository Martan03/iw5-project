using System;

public class ApiBLInstaller : IInstaller
{
    public void Install(IServiceCollection serviceCollection)
    {
        serviceCollection.Scan(selector =>
            selector.FromAssemblyOf<ApiBLInstaller>()
                    .AddClasses(classes => classes.AssignableTo<object>())
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime());
    }
}
