using AutoMapper;
using IW5Forms.Common.Models.Question;

namespace IW5Forms.Web.BL.MapperProfiles;

public class QuestionMapperProfile : Profile
{
    public QuestionMapperProfile()
    {
        CreateMap<QuestionDetailModel, QuestionListModel>();
    }
}
