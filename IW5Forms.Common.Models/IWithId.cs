using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.Common.Models
{
    public interface IWithId
    {
        Guid Id { get; init; }
    }
}
