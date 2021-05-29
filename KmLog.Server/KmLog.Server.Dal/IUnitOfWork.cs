using System.Threading.Tasks;
using KmLog.Server.Model;
using Microsoft.EntityFrameworkCore.Storage;

namespace KmLog.Server.Dal
{
    public interface IUnitOfWork
    {
        IRepository<Car> CarRepository { get; }

        IRepository<RefuelEntry> RefuelEntryRepository { get; }

        IRepository<ServiceEntry> ServiceEntryRepository { get; }

        IRepository<Group> GroupRepository { get; }

        IDbContextTransaction BeginTransaction();

        Task Save();
    }
}
