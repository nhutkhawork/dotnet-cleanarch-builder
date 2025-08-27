using Temp.Domain.Interfaces.Repositories.Base;
using Temp.Domain.Models;
using Temp.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Temp.Infrastructure.Repositories.Base;

public class Repository<T>(DbFactory dbFactory) : IRepository<T> where T : class
{
    protected DbSet<T> DbSet = dbFactory.DbContext.Set<T>();

    public async Task AddAsync(T model)
    {
        await DbSet.AddAsync(model);
    }

    public async Task AddRangeAsync(IEnumerable<T> models)
    {
        await DbSet.AddRangeAsync(models);
    }

    public void PermanentlyDelete(T model)
    {
        DbSet.Remove(model);
    }

    public void PermanentlyDeleteRange(IEnumerable<T> models)
    {
        DbSet.RemoveRange(models);
    }

    public async Task<T?> GetByIdAsync(params object[] keyValues)
    {
        return await DbSet.FindAsync(keyValues);
    }

    public async Task<List<T>> ListAsync()
    {
        return await DbSet.ToListAsync();
    }

    public void Update(T model)
    {
        DbSet.Update(model);
    }

    protected static async Task<PaginationResponse<T>> ToPagedList(
        IQueryable<T> source,
        PaginationRequest pagination,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isDescending = false,
        Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        if (filter != null)
        {
            source = source.Where(filter);
        }

        if (include != null)
        {
            source = include(source);
        }

        if (orderBy != null)
        {
            source = isDescending ? source.OrderByDescending(orderBy) : source.OrderBy(orderBy);
        }

        var count = await source.CountAsync();
        var items = await source
            .Skip((pagination.Page - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PaginationResponse<T>
        {
            CurrentPage = pagination.Page,
            PageSize = pagination.PageSize,
            TotalCount = count,
            TotalPages = (int)Math.Ceiling(count / (decimal)pagination.PageSize),
            Result = items
        };
    }

    public async Task<PaginationResponse<T>> PaginateAsync(
        PaginationRequest pagination,
        Expression<Func<T, bool>>? filter = null,
        Expression<Func<T, object>>? orderBy = null,
        bool isDescending = false,
        Func<IQueryable<T>, IQueryable<T>>? include = null)
    {
        return await ToPagedList(DbSet, pagination, filter, orderBy, isDescending, include);
    }
}
