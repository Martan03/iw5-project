using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Api.DAL.Common.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace IW5Forms.Api.DAL.EF.Installers
{
    public class RepositoriesInstaller
    {
        public void Install(IServiceCollection serviceCollection)
        {
            serviceCollection.Scan(selector =>
                selector.FromAssemblyOf<RepositoriesInstaller>()
                    .AddClasses(classes => classes.AssignableTo(typeof(IRepository<>)))
                    .AsMatchingInterface()
                    .WithScopedLifetime());
        }
    }
}
