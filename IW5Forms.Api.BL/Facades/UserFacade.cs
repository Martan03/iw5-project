using AutoMapper;
using IW5Forms.Common.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Api.DAL.Common.Repositories;
using IW5Forms.Common.Models.Form;
using IW5Forms.Api.DAL.Common.Entities;

namespace IW5Forms.Api.BL.Facades
{    
    public class UserFacade(IUserRepository userRepository, IMapper mapper) : IUserFacade
    {
        public List<UserListModel> GetAll()
        {
            return mapper.Map<List<UserListModel>>(userRepository.GetAll());

        }

        public List<UserListModel> SearchByName(string name)
        {
            return new List<UserListModel>();
        }

        public UserDetailModel? GetById(Guid id)
        {
            var userEntity = userRepository.GetById(id);
            return mapper.Map<UserDetailModel>(userEntity);
        }

        public Guid CreateOrUpdate(UserDetailModel userModel) 
        {
            return userRepository.Exists(userModel.Id)
                ? Update(userModel)!.Value
                : Create(userModel);
        }

        public Guid Create(UserDetailModel userModel) 
        {
            var userEntity = mapper.Map<UserEntity>(userModel);
            return userRepository.Insert(userEntity);
        }

        public Guid? Update(UserDetailModel userModel)
        {
            var userEntity = mapper.Map<UserEntity>(userModel);
            return userRepository.Update(userEntity);
        }

        public void Delete(Guid id)
        {
            userRepository.Remove(id);
        }
    }
}
