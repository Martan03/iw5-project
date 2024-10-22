using AutoMapper;
using IW5Forms.Common.Models.Answer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Api.BL.MapperProfiles
{
    public class AnswerMapperProfile : Profile
    {
        public AnswerMapperProfile()
        {
            // přidat při merge s DAL
            CreateMap </*AnswerEntity*/, AnswerListAndDetailModel>();
            CreateMap<AnswerListAndDetailModel, /*AnswerEntity*/>();
        }
    }
}
