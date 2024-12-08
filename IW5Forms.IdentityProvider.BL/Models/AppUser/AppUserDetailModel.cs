using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IW5Forms.IdentityProvider.BL.Models.AppUser
{
    public class AppUserDetailModel
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string UserName { get; set; }
    }
}
