using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace IW5Forms.Api.DAL.Common.Entities
{
    public record AnswerEntity : EntityBase
    {
        // Users answer
        public required string Text { get; set; }

        // UserId is nullable, in case incognito mode is enabled
        public Guid? UserId { get; set; }
    }

    public class AnswerEntityMapperProfile : Profile
    {
        public AnswerEntityMapperProfile()
        {
            CreateMap<AnswerEntity, AnswerEntity>();
        }
    }
}
