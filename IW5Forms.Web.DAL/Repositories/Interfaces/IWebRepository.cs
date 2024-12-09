using IW5Forms.Common.Models;

namespace IW5Forms.Web.DAL.Repositories;

public interface IWebRepository<T>
    where T : IWithId
{
    string TableName { get; }
    Task<IList<T>> GetAllAsync();
    Task<T> GetByIdAsync(Guid id);
    Task InsertAsync(T entity);
    Task RemoveAsync(Guid id);
}
