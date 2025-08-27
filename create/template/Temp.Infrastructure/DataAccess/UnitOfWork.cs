using Temp.Domain.Interfaces.DataAccess;

namespace Temp.Infrastructure.DataAccess;

public class UnitOfWork(DbFactory dbFactory) : BaseUnitOfWork(dbFactory), IUnitOfWork
{
}
