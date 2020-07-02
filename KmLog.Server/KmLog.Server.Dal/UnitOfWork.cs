using System.Threading.Tasks;
using KmLog.Server.EF;
using KmLog.Server.Model;
using Microsoft.EntityFrameworkCore.Storage;

namespace KmLog.Server.Dal
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly KmLogContext _context;

        public UnitOfWork(KmLogContext kmLogContext,
                          IRepository<Car> carRepository, IRepository<User> userRepository,
                          IRepository<RefuelEntry> refuelEntryRepository, IRepository<ServiceEntry> serviceEntryRepository, 
                          IRepository<Group> groupRepository)
        {
            _context = kmLogContext;
            CarRepository = carRepository;
            UserRepository = userRepository;
            RefuelEntryRepository = refuelEntryRepository;
            ServiceEntryRepository = serviceEntryRepository;
            GroupRepository = groupRepository;
        }

        public IRepository<Car> CarRepository { get; }

        public IRepository<User> UserRepository { get; }

        public IRepository<RefuelEntry> RefuelEntryRepository { get; }

        public IRepository<ServiceEntry> ServiceEntryRepository { get; }

        public IRepository<Group> GroupRepository { get; }

        public IDbContextTransaction BeginTransaction() => _context.Database.BeginTransaction();

        public async Task Save() => await _context.SaveChangesAsync();
    }
}
