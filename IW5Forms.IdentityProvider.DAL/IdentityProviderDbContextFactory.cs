using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace IW5Forms.IdentityProvider.DAL
{
    public class IdentityProviderDbContextFactory : IDesignTimeDbContextFactory<IdentityProviderDbContext>, IDbContextFactory<IdentityProviderDbContext>
    {
        private readonly Assembly startupAssembly;

        public IdentityProviderDbContextFactory()
        {
            startupAssembly = Assembly.GetEntryAssembly()!;
        }

        public IdentityProviderDbContext CreateDbContext(string[] args)
            => CreateDbContext();

        public IdentityProviderDbContext CreateDbContext()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings2.json")
                .AddUserSecrets<IdentityProviderDbContextFactory>(optional: true)
                .AddUserSecrets(startupAssembly, optional: true)
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<IdentityProviderDbContext>();
            var connectionString = configuration.GetConnectionString("AZURE_SQL_IDENTITY_CONNECTION_STRING")??configuration.GetConnectionString("DefaultConnection");
            optionsBuilder.UseSqlServer(connectionString);
            return new IdentityProviderDbContext(optionsBuilder.Options);
        }
    }
}
