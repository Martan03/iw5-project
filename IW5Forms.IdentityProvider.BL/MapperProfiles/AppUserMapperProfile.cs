using AutoMapper;
using IW5Forms.Common.Extentions;
using IW5Forms.Common.Models.User;
using IW5Forms.IdentityProvider.BL.Models.AppUser;
using IW5Forms.IdentityProvider.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.IdentityProvider.BL.MapperProfiles
{
    public class AppUserMapperProfile : Profile
    {
        public AppUserMapperProfile()
        {
            CreateMap<AppUserCreateModel, AppUserEntity>()
                .Ignore(entity => entity.Active)
                .Ignore(entity => entity.Id)
                .Ignore(entity => entity.NormalizedUserName)
                .Ignore(entity => entity.NormalizedEmail)
                .Ignore(entity => entity.EmailConfirmed)
                .Ignore(entity => entity.PasswordHash)
                .Ignore(entity => entity.SecurityStamp)
                .Ignore(entity => entity.ConcurrencyStamp)
                .Ignore(entity => entity.PhoneNumber)
                .Ignore(entity => entity.PhoneNumberConfirmed)
                .Ignore(entity => entity.TwoFactorEnabled)
                .Ignore(entity => entity.LockoutEnd)
                .Ignore(entity => entity.LockoutEnabled)
                .Ignore(entity => entity.AccessFailedCount);

            CreateMap<AppUserEntity, AppUserDetailModel>();
            CreateMap<AppUserEntity, UserListModel>()
                .Ignore(entity => entity.Name)
                .Ignore(entity => entity.PhotoUrl);
        }
    }
}
