using IW5Forms.Common.Enums;
using IW5Forms.Common.Models.Answer;
using IW5Forms.Common.Models.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Common.Models.User
{
    public record UserDetailFormModel
    {
        public required Guid Id { get; init; }

        public required FormListModel Form { get; set; }

        public required FormRelationTypes FormRelationTypes { get; set; }
    }
}
