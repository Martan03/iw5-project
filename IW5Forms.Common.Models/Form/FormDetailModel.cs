using IW5Forms.Common.Models.Question;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Common.Models.Form
{
    public record FormDetailModel : IWithId
    {
        public required Guid Id { get; init; }
        public required string Name { get; set; }
        public required DateTime BeginTime { get; set; }

        public required DateTime EndTime { get; set; }
        public required bool Incognito { get; set; }
        public required bool SingleTry { get; set; }
        public string? IdentityOwnerId { get; set; }
        public List<Guid> CompletedUsersId { get; set; } = [];
        public List<QuestionListModel> Questions { get; set; } = [];
    }
}
