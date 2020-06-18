using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace KmLog.Server.Dal
{
    public interface IUnitOfWork
    {
        ICarRepository CarRepository { get; }
        
        IUserRepository UserRepository { get; }

        IRefuelEntryRepository RefuelEntryRepository { get; }

        IDbContextTransaction BeginTransaction();

        Task Save();
    }
}
