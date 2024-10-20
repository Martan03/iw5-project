using IW5Forms.Api.DAL.EF;
using IW5Forms.Api.DAL.Common;
using IW5Forms.API.DAL;

namespace IW5Forms.Api.App
{
    public class SeedScript
    {
        private readonly FormsDbContext dbContext;

        public SeedScript(FormsDbContext context)
        {
            dbContext = context;
        }

        public void SeedData()
        {

        }
    }
}
