using IW5Forms.Common.Enums;
using IW5Forms.Common.Models.Answer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Common.Models.User
{
    public record UserDetailModel : IWithId
    {
        public required Guid Id { get; init; }

        public required string Name { get; set; }

        public string? PhotoUrl { get; set; }

        public string? Description { get; set; }
        public required RoleTypes Role { get; set; }

        public List<UserDetailFormModel>? Forms { get; set; } = [];
    }
}
