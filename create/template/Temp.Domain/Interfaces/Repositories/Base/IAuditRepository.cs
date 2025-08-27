using Temp.Domain.Entities.Base;
using Temp.Domain.Models;
using System.Linq.Expressions;

namespace Temp.Domain.Interfaces.Repositories.Base;

public interface IAuditRepository<T> : IRepository<T> where T : AuditableEntity
{
    public void Delete(T model);
    public Task<List<T>> ListSoftDeletedAsync();

    public Task<PaginationResponse<T>> PaginateSoftFilter(
        PaginationRequest pagination,
        bool includeDelete,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isDescending = false);
}
