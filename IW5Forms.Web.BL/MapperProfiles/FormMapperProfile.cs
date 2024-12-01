using AutoMapper;
using IW5Forms.Common.Models.Form;

namespace IW5Forms.Web.BL.MapperProfiles;

public class FormMapperProfile : Profile
{
    public FormMapperProfile()
    {
        CreateMap<FormDetailModel, FormListModel>();
    }
}
