using IW5Forms.Common.Installers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Common.BL.Facades;
using IW5Forms.IdentityProvider.BL.MapperProfiles;

namespace IW5Forms.IdentityProvider.BL.Installers
{
    public class IdentityProviderBLInstaller : IInstaller
    {
        public void Install(IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(AppUserMapperProfile));

            serviceCollection.Scan(selector =>
                selector.FromAssemblyOf<IdentityProviderBLInstaller>()
                    .AddClasses(classes => classes.AssignableTo<IAppFacade>())
                    .AsSelfWithInterfaces()
                    .WithScopedLifetime());
        }
    }
}
