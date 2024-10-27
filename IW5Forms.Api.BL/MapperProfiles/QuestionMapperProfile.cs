using AutoMapper;
using IW5Forms.Common.Models.Form;
using IW5Forms.Common.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Api.DAL.Common.Entities;

namespace IW5Forms.Api.BL.MapperProfiles
{
    public class QuestionMapperProfile : Profile
    {
        public QuestionMapperProfile()
        {
            CreateMap <QuestionEntity, QuestionDetailModel > ();
            CreateMap <QuestionEntity, QuestionListModel > ();

            //CreateMap<QuestionDetailModel, QuestionEntity>();
        }
    }
}
