using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IW5Forms.Common.Models;

namespace IW5Forms.Api.DAL.Common.Entities
{
    public record UserEntity : EntityBase
    {
        public required string Name { get; set; }
        public string? PhotoUrl { get; set; }

        // Role is either normal user (User) or Admin
        public required UserRoles Role { get; set; }

        // Forms contain all accessible forms by this user and bool whether the form was filled by the user
        public ICollection<FormEntity> AvailableForms { get; set; } = new List<FormEntity>();

        public ICollection<FormEntity> OwnedForms { get; set; } = new List<FormEntity>();
    }

    public class UserEntityMapperProfile : Profile
    {
        public UserEntityMapperProfile()
        {
            CreateMap<UserEntity, UserEntity>();
        }
    }
}
