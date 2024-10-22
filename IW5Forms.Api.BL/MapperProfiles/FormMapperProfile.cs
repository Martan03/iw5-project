using AutoMapper;
using IW5Forms.Common.Models.Answer;
using IW5Forms.Common.Models.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Api.BL.MapperProfiles
{
    public class FormMapperProfile : Profile
    {
        public FormMapperProfile()
        {
            // přidat při merge s DAL
            CreateMap </*FormEntity*/, FormListModel > ();
            CreateMap </*FormEntity*/, FormDetailModel > ();

            CreateMap<FormDetailModel, /*FormEntity*/>();
        }
    }
}
