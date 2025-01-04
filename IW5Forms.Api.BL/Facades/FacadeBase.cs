using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;


namespace IW5Forms.Api.BL.Facades
{
    public class FacadeBase<TRepository, TEntity>
        where TRepository : IRepository<TEntity>
        where TEntity : IEntity
    {
        private readonly TRepository repository;

        public FacadeBase(TRepository repository)
        {
            this.repository = repository;
        }

        public virtual void ThrowIfWrongOwner(Guid id, string? ownerId)
        {
            if (ownerId is not null
                && repository.GetById(id)?.IdentityOwnerId != ownerId)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}