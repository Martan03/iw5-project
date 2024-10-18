using IW5Forms.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace IW5Forms.Api.DAL.Common.Entities
{
    public record QuestionEntity : EntityBase
    {
        public record QuestionModel
        {
            public required QuestionType QuestionType { get; set; }

            // text of the question (the actual question)
            public required string Text { get; set; }

            // further description of the question (optional)
            public string? Description { get; set; }
            public List<AnswerEntity> Answers { get; set; } = new List<AnswerEntity>();
        }
    }

    public class QuestionEntityMapperProfile : Profile
    {
        public QuestionEntityMapperProfile()
        {
            CreateMap<QuestionEntity, QuestionEntity>();
        }
    }
}
