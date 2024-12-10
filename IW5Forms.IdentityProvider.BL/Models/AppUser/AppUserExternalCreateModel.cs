using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.IdentityProvider.BL.Models.AppUser
{
    public class AppUserExternalCreateModel
    {
        public required string? Email { get; set; }
        public required string Provider { get; set; }
        public required string ProviderIdentityKey { get; set; }
        public IEnumerable<Claim> Claims { get; set; } = new List<Claim>();
    }
}
