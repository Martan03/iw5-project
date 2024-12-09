using IW5Forms.IdentityProvider.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.IdentityProvider.DAL.Repositories
{
    public interface IAppUserRepository
    {
        Task<IList<AppUserEntity>> SearchAsync(string searchString);
    }
}
