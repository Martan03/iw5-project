using IW5Forms.Common.BL.Facades;
using IW5Forms.IdentityProvider.BL.Models.AppUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Common.Models.User;

namespace IW5Forms.IdentityProvider.BL.Facades.Interfaces
{
    public interface IAppUserFacade : IAppFacade
    {
        Task<Guid?> CreateAppUserAsync(AppUserCreateModel appUserModel);
        Task<bool> ValidateCredentialsAsync(string userName, string password);
        Task<Guid> GetUserIdByUserNameAsync(string userName);
        public Task<AppUserDetailModel?> GetUserByIdAsync(Guid id);

        Task<IList<UserListModel>> SearchAsync(string searchString);
        Task<AppUserDetailModel?> GetUserByUserNameAsync(string userName);
        Task<AppUserDetailModel?> GetAppUserByExternalProviderAsync(string provider, string providerIdentityKey);
        Task<AppUserDetailModel> CreateExternalAppUserAsync(AppUserExternalCreateModel appUserModel);
        Task<bool> ActivateUserAsync(string securityCode, string email);
    }
}
