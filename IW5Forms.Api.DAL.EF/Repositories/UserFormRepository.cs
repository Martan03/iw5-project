using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using IW5Forms.API.DAL;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;


namespace IW5Forms.Api.DAL.EF.Repositories
{
    public class UserFormRepository : RepositoryBase<UserFormEntity>, IUserFormRepository
    {
        public UserFormRepository(FormsDbContext dbContext) : base(dbContext)
        {
        }
    }
}
