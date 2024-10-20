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
    public class RepositoryBase<TEntity> : IRepository<TEntity>, IDisposable
        where TEntity : class, IEntity
    {
        protected readonly FormsDbContext dbContext;

        public RepositoryBase(FormsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public virtual IList<TEntity> GetAll()
        {
            return dbContext.Set<TEntity>().ToList();
        }

        public virtual TEntity? GetById(Guid id)
        {
            return dbContext.Set<TEntity>().SingleOrDefault(entity => entity.Id == id);
        }

        public virtual Guid Insert(TEntity entity)
        {
            var createdEntity = dbContext.Set<TEntity>().Add(entity);
            dbContext.SaveChanges();

            return createdEntity.Entity.Id;
        }

        public virtual Guid? Update(TEntity entity)
        {
            if (Exists(entity.Id))
            {
                dbContext.Set<TEntity>().Attach(entity);
                var updatedEntity = dbContext.Set<TEntity>().Update(entity);
                dbContext.SaveChanges();

                return updatedEntity.Entity.Id;
            }
            else return null;
        }

        public virtual bool Exists(Guid id)
        {
            return dbContext.Set<TEntity>().Any(entity => entity.Id == id);
        }

        public virtual void Remove(Guid id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                dbContext.Set<TEntity>().Remove(entity);
                dbContext.SaveChanges();
            }
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }
    }
}
