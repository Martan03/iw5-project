using System.Globalization;
using AutoMapper;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;
using IW5Forms.Common.Models.User;

namespace IW5Forms.Api.BL.Facades
{
    public class UserFacade : FacadeBase<IUserRepository, UserEntity>, IUserFacade
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserFacade(IUserRepository userRepository, IMapper mapper) : base(userRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public List<UserListModel> GetAll()
        {
            return _mapper.Map<List<UserListModel>>(_userRepository.GetAll());

        }

        public List<UserListModel> SearchByName(string name)
        {
            var users = _mapper.Map<List<UserListModel>>(_userRepository.GetAll());
            users.RemoveAll(u => !u.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
            return users;
        }

        public UserDetailModel? GetById(Guid id)
        {
            var userEntity = _userRepository.GetById(id);
            return _mapper.Map<UserDetailModel>(userEntity);
        }

        public Guid CreateOrUpdate(UserDetailModel userModel, string? ownerId)
        {
            return _userRepository.Exists(userModel.Id)
                ? Update(userModel, ownerId)!.Value
                : Create(userModel, ownerId);
        }

        public Guid Create(UserDetailModel userModel, string? ownerId)
        {
            var userEntity = NewUserFromModel(userModel);
            userEntity.IdentityOwnerId = ownerId;
            return _userRepository.Insert(userEntity);
        }

        public Guid? Update(UserDetailModel userModel, string? ownerId)
        {
            ThrowIfWrongOwner(userModel.Id, ownerId);
            var userEntity = NewUserFromModel(userModel);
            return _userRepository.Update(userEntity);
        }

        public void Delete(Guid id, string? ownerId)
        {
            ThrowIfWrongOwner(id, ownerId);
            _userRepository.Remove(id);
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
