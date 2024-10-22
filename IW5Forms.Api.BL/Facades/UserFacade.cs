using AutoMapper;
using IW5Forms.Common.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Api.BL.Facades
{    // přidat při merge s DAL
    public class UserFacade(/*IUserRepository*/ userRepository, IMapper mapper) : IUserFacade
    {
        public List<UserListModel> GetAll()
        {

        }

        public List<UserListModel> SearchByName(string name) 
        {

        }

        public UserDetailModel? GetById(Guid id)
        {
        
        }

        public Guid CreateOrUpdate(UserDetailModel userModel) 
        {
        
        }

        public Guid Create(UserDetailModel userModel) 
        {
        
        }

        public Guid? Update(UserDetailModel userModel)
        {
        
        }

        public void Delete(Guid id)
        {
        
        }
    }
}
