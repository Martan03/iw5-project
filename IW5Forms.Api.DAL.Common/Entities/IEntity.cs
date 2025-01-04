using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Api.DAL.Common.Entities
{
    public interface IEntity
    {
        public Guid Id { get;  init; }
        string? IdentityOwnerId { get; set; }
    }
}
