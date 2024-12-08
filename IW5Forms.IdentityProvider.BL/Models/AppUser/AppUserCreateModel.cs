using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.IdentityProvider.BL.Models.AppUser
{
    public class AppUserCreateModel
    {
        public required string UserName { get; set; }
        public required string Password { get; set; }
        public required string Subject { get; set; }
        public required string Email { get; set; }
    }
}
