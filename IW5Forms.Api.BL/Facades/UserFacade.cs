using AutoMapper;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;
using IW5Forms.Common.Models.User;

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
            var users = mapper.Map<List<UserListModel>>(userRepository.GetAll());
            users.RemoveAll(u => !u.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            return users;
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
            var userEntity = NewUserFromModel(userModel);
            return userRepository.Insert(userEntity);
        }

        public Guid? Update(UserDetailModel userModel)
        {
            var userEntity = NewUserFromModel(userModel);
            return userRepository.Update(userEntity);
        }

        public void Delete(Guid id)
        {
            userRepository.Remove(id);
        }

        public UserEntity NewUserFromModel(UserDetailModel userModel)
        {
            var userEntity = new UserEntity()
            {
                Description = userModel.Description,
                Forms = new List<UserFormEntity>(),
                Id = userModel.Id,
                Name = userModel.Name,
                PhotoUrl = userModel.PhotoUrl,
                Role =  userModel.Role
            };

            if (userModel.Forms != null)
            {
                foreach (var t in userModel.Forms)
                {
                    userEntity.Forms.Add(new UserFormEntity()
                    {
                        Id = t.Id,
                        FormId = t.Form.Id,
                        FormRelationTypes = t.FormRelationTypes,
                        User = userEntity,
                        UserId = userEntity.Id
                    });
                }
            }

            return userEntity;
        }
    }
}
