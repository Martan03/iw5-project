using Microsoft.EntityFrameworkCore;

namespace IW5Forms.API.DAL
{
    public class FormsDbContext : DbContext
    {
        
        public FormsDbContext(DbContextOptions<FormsDbContext> options) : base(options)
        {
            
        }
    }
}
