using AutoMapper;
using IW5Forms.IdentityProvider.BL.Models.AppUserClaim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.IdentityProvider.BL.MapperProfiles
{
    public class AppUserClaimMapperProfile : Profile
    {
        public AppUserClaimMapperProfile()
        {
            CreateMap<Claim, AppUserClaimListModel>()
                .ForMember(dest => dest.ClaimType, opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.ClaimValue, opt => opt.MapFrom(src => src.Value));
        }
    }
}
