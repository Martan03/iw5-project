using IW5Forms.Common.Enums;
using IW5Forms.Common.Models.Answer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Common.Models.Question
{
    public record QuestionListModel : IWithId
    {
        public required Guid Id { get; init; }

        public required QuestionTypes QuestionType { get; set; }

        public required string Text { get; set; }
    }
}
