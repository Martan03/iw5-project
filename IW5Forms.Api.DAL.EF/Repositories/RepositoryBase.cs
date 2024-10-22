using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IW5Forms.API.DAL;
using IW5Forms.Api.DAL.Common.Entities;
using IW5Forms.Api.DAL.Common.Repositories;

namespace IW5Forms.Api.DAL.EF.Repositories
{
    public class RepositoryBase<TEntity>(FormsDbContext dbContext) : IRepository<TEntity>, IDisposable
        where TEntity : class, IEntity
    {
        protected readonly FormsDbContext DbContext = dbContext;

        public virtual IList<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>().ToList();
        }

        public virtual TEntity? GetById(Guid id)
        {
            return DbContext.Set<TEntity>().SingleOrDefault(entity => entity.Id == id);
        }

        public virtual Guid Insert(TEntity entity)
        {
            var createdEntity = DbContext.Set<TEntity>().Add(entity);
            DbContext.SaveChanges();

            return createdEntity.Entity.Id;
        }

        public virtual Guid? Update(TEntity questionEntity)
        {
            if (Exists(questionEntity.Id))
            {
                DbContext.Set<TEntity>().Attach(questionEntity);
                var updatedEntity = DbContext.Set<TEntity>().Update(questionEntity);
                DbContext.SaveChanges();

                return updatedEntity.Entity.Id;
            }
            else return null;
        }

        public virtual bool Exists(Guid id)
        {
            return DbContext.Set<TEntity>().Any(entity => entity.Id == id);
        }

        public virtual void Remove(Guid id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                DbContext.Set<TEntity>().Remove(entity);
                DbContext.SaveChanges();
            }
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
