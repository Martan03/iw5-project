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

        // Incognito mode doesn't store UserId in Answers
        public required bool Incognito { get; set; }

        // SingleTry allows only one try per user
        public required bool SingleTry { get; set; }

        // if SingleTry is true, UsersCompleted will store users which already completed the form
        public IList<Guid>? CompletedUsersId { get; set; } = new List<Guid>();

        // public form doesnt require logged user
        //public required bool Public { get; set; }
        // stores Users which have access to this form, if Public is true
        //public ICollection<UserEntity>? UsersWithAccess { get; set; } = new List<UserEntity>();

        //public required Guid OwnerId { get; set; }
        //public UserEntity? Owner { get; set; }

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
