using IW5Forms.Common.Enums;
using IW5Forms.Common.Models.Answer;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Common.Models.Form;

namespace IW5Forms.Common.Models.Question
{
    public record QuestionDetailModel : IWithId
    {
        public required Guid Id { get; init; }

        public required QuestionTypes QuestionType { get; set; }

        public required string Text { get; set; }

        public string? Description { get; set; }

        // in case the type is ManyOptions, they will be stored here
        public List<string> Options { get; set; } = [];

        public List<AnswerListAndDetailModel> Answers { get; set; } = [];

    }
}
