using System.Threading.Tasks;
using KmLog.Server.Model;
using Microsoft.EntityFrameworkCore.Storage;

namespace KmLog.Server.Dal
{
    public interface IUnitOfWork
    {
        IRepository<Car> CarRepository { get; }

        IRepository<User> UserRepository { get; }

        IRepository<RefuelEntry> RefuelEntryRepository { get; }

        IRepository<Group> GroupRepository { get; }

        IDbContextTransaction BeginTransaction();

        Task Save();
    }
}
