using KmLog.Server.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace KmLog.Server.EF
{
    public class KmLogContext : IdentityDbContext<User>
    {
        private readonly string _connectionString;

        public KmLogContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Car> Cars { get; set; }

        public DbSet<RefuelEntry> RefuelEntries { get; set; }

        public DbSet<ServiceEntry> ServiceEntries { get; set; }

        public DbSet<Group> Groups { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            //builder.Entity<User>()
            //    .HasIndex(u => u.Email)
            //    .IsUnique();

            //builder.Entity<User>()
            //    .HasKey(u => u.Id);

            builder.Entity<Car>()
                .HasIndex(c => c.LicensePlate)
                .IsUnique();

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName());
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }
    }
}
