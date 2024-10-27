using IW5Forms.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IW5Forms.Common.Enums;

namespace IW5Forms.Api.DAL.Common.Entities
{
    public record QuestionEntity : EntityBase
    {
        public required QuestionTypes QuestionType { get; set; }
        public required string Text { get; set; }
        public string? Description { get; set; }
        public IList<string> Options { get; set; } = new List<string>();
        public IList<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();
        public Guid FormId { get; set; }
        public FormEntity? Form { get; set; }
    }

    public class QuestionEntityMapperProfile : Profile
    {
        public QuestionEntityMapperProfile()
        {
            CreateMap<QuestionEntity, QuestionEntity>();
        }
    }
}
