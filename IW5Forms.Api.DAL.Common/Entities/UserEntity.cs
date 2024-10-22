using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IW5Forms.Common.Enums;
using IW5Forms.Common.Models;

namespace IW5Forms.Api.DAL.Common.Entities
{
    public record UserEntity : EntityBase
    {
        public required string Name { get; set; }
        public string? PhotoUrl { get; set; }
        public string? Description { get; set; }
        public required RoleTypes Role { get; set; }
        public ICollection<UserFormEntity> Forms { get; set; } = new List<UserFormEntity>();
    }

    public class UserEntityMapperProfile : Profile
    {
        public UserEntityMapperProfile()
        {
            CreateMap<UserEntity, UserEntity>();
        }
    }
}
