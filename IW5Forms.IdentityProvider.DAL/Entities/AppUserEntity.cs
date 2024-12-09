using Microsoft.AspNetCore.Identity;

namespace IW5Forms.IdentityProvider.DAL.Entities;

public class AppUserEntity : IdentityUser<Guid>
{
    public bool Active { get; set; }
    public string Subject { get; set; }
}
