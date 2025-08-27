namespace Temp.Domain.Interfaces.DataAccess;

public interface IBaseUnitOfWork
{
    Task<int> CommitAsync();
}
