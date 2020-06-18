using System.Threading.Tasks;
using KmLog.Server.EF;
using Microsoft.EntityFrameworkCore.Storage;

namespace KmLog.Server.Dal
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KmLogContext _context;

        public UnitOfWork(KmLogContext kmLogContext,
                          ICarRepository carRepository, IUserRepository userRepository, IRefuelEntryRepository refuelEntryRepository)
        {
            _context = kmLogContext;
            CarRepository = carRepository;
            UserRepository = userRepository;
            RefuelEntryRepository = refuelEntryRepository;
        }

        public ICarRepository CarRepository { get; }

        public IUserRepository UserRepository { get; }

        public IRefuelEntryRepository RefuelEntryRepository { get; }

        public IDbContextTransaction BeginTransaction() => _context.Database.BeginTransaction();

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
