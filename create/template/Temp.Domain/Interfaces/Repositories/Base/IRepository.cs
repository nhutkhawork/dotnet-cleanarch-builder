using Temp.Domain.Models;
using System.Linq.Expressions;

namespace Temp.Domain.Interfaces.Repositories.Base;

public interface IRepository<T> where T : class
{
    Task<List<T>> ListAsync();
    Task<T?> GetByIdAsync(params object[] keyValues);
    Task AddAsync(T model);
    void PermanentlyDelete(T model);
    void PermanentlyDeleteRange(IEnumerable<T> models);
    void Update(T model);
    Task<PaginationResponse<T>> PaginateAsync(
            PaginationRequest pagination,
            Expression<Func<T, bool>>? filter = null,
            Expression<Func<T, object>>? orderBy = null,
            bool isDescending = false,
            Func<IQueryable<T>, IQueryable<T>>? include = null);
}
