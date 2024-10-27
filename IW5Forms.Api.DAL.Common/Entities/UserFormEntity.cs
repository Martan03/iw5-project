using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IW5Forms.Common.Enums;

namespace IW5Forms.Api.DAL.Common.Entities
{
    public record UserFormEntity : EntityBase
    {

        public Guid UserId { get; set; }
        public UserEntity? User { get; set; }

        public required Guid FormId { get; set; }
        public FormEntity? Form { get; set; }

        public required FormRelationTypes FormRelationTypes { get; set; }
}
    public class UserFormEntityMapperProfile : Profile
    {
        public UserFormEntityMapperProfile()
        {
            CreateMap<UserFormEntity, UserFormEntity>();
        }
    }
}
