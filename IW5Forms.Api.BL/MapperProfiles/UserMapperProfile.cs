using AutoMapper;
using IW5Forms.Common.Models.Question;
using IW5Forms.Common.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Api.BL.MapperProfiles
{
    public class UserMapperProfile : Profile
    {
        public UserMapperProfile()
        {
            // přidat při merge s DAL
            CreateMap </*UserEntity*/, UserDetailModel > ();
            CreateMap </*UserEntity*/, UserListModel > ();

            CreateMap<UserDetailModel, /*UserEntity*/>();

            CreateMap </*UserFormEntity*/, UserDetailFormModel>();
        }
    }
}
