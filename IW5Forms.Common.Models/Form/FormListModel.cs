using IW5Forms.Common.Models.Question;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Common.Models.Form
{
    public record FormListModel
    {
        public required Guid Id { get; init; }

        public required DateTime AnswerAcceptanceStartTime { get; set; }

        public required DateTime AnswerAcceptanceEndTime { get; set; }    }
}
