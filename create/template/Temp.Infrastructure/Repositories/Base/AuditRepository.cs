using Temp.Domain.Entities.Base;
using Temp.Domain.Interfaces.Repositories.Base;
using Temp.Domain.Models;
using Temp.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Temp.Infrastructure.Repositories.Base;

public class AuditRepository<T>(DbFactory dbFactory) : Repository<T>(dbFactory), IAuditRepository<T> where T : AuditableEntity
{
    public void Delete(T model)
    {
        model.IsDeleted = true;
        Update(model);
    }

    public async Task<List<T>> ListSoftDeletedAsync()
    {
        var res = await SoftFiltered(true)
            .Where(x => x.IsDeleted)
            .ToListAsync();

        return res;
    }

    public async Task<PaginationResponse<T>> PaginateSoftFilter(
        PaginationRequest pagination,
        bool includeDeleted = false,
        Func<IQueryable<T>, IQueryable<T>>? include = null,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isDescending = false)
    {
        return await ToPagedList(SoftFiltered(includeDeleted), pagination, filter, orderBy, isDescending, include);
    }

    protected virtual IQueryable<T> SoftFiltered(bool includeDeleted = false)
    {
        return includeDeleted ? DbSet.IgnoreQueryFilters() : DbSet.IgnoreQueryFilters().Where(x => !x.IsDeleted);
    }
}
