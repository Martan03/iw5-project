using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.Api.DAL.Common.Entities;

namespace IW5Forms.Api.DAL.Common.Repositories
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        IList<TEntity> GetAll();
        TEntity? GetById(Guid id);
        Guid Insert(TEntity entity);
        Guid Update(TEntity entity);
        void Delete(TEntity entity);
        bool Exists(Guid id);
    }
}
