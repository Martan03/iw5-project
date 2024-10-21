using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Common.Models.Answer
{
    public record AnswerListAndDetailModel
    {
        public required Guid Id { get; init; }

        public required string Text { get; set; }

        public required Guid FormResponder { get; set; }
    }
}
