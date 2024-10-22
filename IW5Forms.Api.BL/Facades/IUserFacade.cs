using IW5Forms.Common.BL.Facades;
using IW5Forms.Common.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Api.BL.Facades
{
    public interface IUserFacade : IAppFacade
    {
        List<UserListModel> GetAll();
        List<UserListModel> SearchByName(string name);
        UserDetailModel? GetById(Guid id);
        Guid CreateOrUpdate(UserDetailModel userModel);
        Guid Create(UserDetailModel userModel);
        Guid? Update(UserDetailModel userModel);
        void Delete(Guid id);
    }
}
