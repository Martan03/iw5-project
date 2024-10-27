using AutoMapper;
using IW5Forms.Common.Models.Answer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Api.DAL.Common.Entities;

namespace IW5Forms.Api.BL.MapperProfiles
{
    public class AnswerMapperProfile : Profile
    {
        public AnswerMapperProfile()
        {
            
            CreateMap<AnswerEntity, AnswerListAndDetailModel>();
            CreateMap<AnswerListAndDetailModel, AnswerEntity>();
        }
    }
}
