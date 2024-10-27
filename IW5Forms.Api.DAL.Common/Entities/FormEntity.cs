using IW5Forms.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace IW5Forms.Api.DAL.Common.Entities
{
    public record FormEntity : EntityBase
    {
        public required string Name { get; set; }
        public required DateTime BeginTime { get; set; }
        public required DateTime EndTime { get; set; }
        public required bool Incognito { get; set; }
        public required bool SingleTry { get; set; }
        public IList<Guid>? CompletedUsersId { get; set; } = new List<Guid>();
        public Guid? OwnerId { get; set; }
        public UserEntity? Owner { get; set; }
        public ICollection<QuestionEntity> Questions { get; set; } = new List<QuestionEntity>();
    }

    public class FormEntityMapperProfile : Profile
    {
        public FormEntityMapperProfile()
        {
            CreateMap<FormEntity, FormEntity>();
        }
    }
}
