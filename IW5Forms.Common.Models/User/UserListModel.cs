using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Common.Models.User
{
    public record UserListModel
    {
        public required Guid Id { get; init; }

        public required string Name { get; set; }

        public string? PhotoUrl { get; set; }
    }
}
