using AutoMapper;
using IW5Forms.Common.Models.User;

namespace IW5Forms.Web.BL.MapperProfiles;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<UserDetailModel, UserListModel>();
    }
}
