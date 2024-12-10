using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.IdentityProvider.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace IW5Forms.IdentityProvider.DAL.Repositories
{
    internal class AppUserRepository(IdentityProviderDbContext identityProviderDbContext) : IAppUserRepository
    {
        public async Task<IList<AppUserEntity>> SearchAsync(string searchString)
            => await identityProviderDbContext.Users.Where(user => EF.Functions.Like(user.UserName, $"%{searchString}%"))
                .ToListAsync();
    }
}
