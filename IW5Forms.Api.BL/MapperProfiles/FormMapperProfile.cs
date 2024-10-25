using AutoMapper;
using IW5Forms.Common.Models.Answer;
using IW5Forms.Common.Models.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Api.DAL.Common.Entities;

namespace IW5Forms.Api.BL.MapperProfiles
{
    public class FormMapperProfile : Profile
    {
        public FormMapperProfile()
        {
            CreateMap<FormEntity, FormListModel>();
            CreateMap<FormEntity, FormDetailModel>()
                .ForMember(dst => dst.CompletedUsersId, config => config.MapFrom(src => src.CompletedUsersId));

            CreateMap<FormDetailModel, FormEntity>();
        }
    }
}
