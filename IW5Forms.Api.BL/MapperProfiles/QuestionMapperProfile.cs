using AutoMapper;
using IW5Forms.Common.Models.Form;
using IW5Forms.Common.Models.Question;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Api.BL.MapperProfiles
{
    public class QuestionMapperProfile : Profile
    {
        public QuestionMapperProfile()
        {
            // přidat při merge s DAL
            CreateMap </*QuestionEntity*/, QuestionDetailModel > ();
            CreateMap </*QuestionEntity*/, QuestionListModel > ();

            CreateMap<QuestionDetailModel, /*QuestionEntity*/>();
        }
    }
}
