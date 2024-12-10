using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Common.Models.Question;

namespace IW5Forms.Common.Models.Answer
{
    public record AnswerListAndDetailModel : IWithId
    {
        public required Guid Id { get; init; }

        public required string Text { get; set; }

        public required Guid ResponderId { get; set; }

        public Guid? QuestionId { get; set; }
    }
}
