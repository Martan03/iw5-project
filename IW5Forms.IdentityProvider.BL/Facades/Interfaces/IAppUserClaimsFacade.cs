using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Common.BL.Facades;
using IW5Forms.IdentityProvider.BL.Models.AppUserClaim;

namespace IW5Forms.IdentityProvider.BL.Facades.Interfaces
{
    public interface IAppUserClaimsFacade : IAppFacade
    {
        Task<IEnumerable<AppUserClaimListModel>> GetAppUserClaimsByUserIdAsync(Guid userId);
    }
}
