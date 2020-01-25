using KmLog.Server.Model;
using Microsoft.EntityFrameworkCore;

namespace KmLog.Server.EF
{
    public class KmLogContext : DbContext
    {
        private readonly string _connectionString;

        public KmLogContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Journey> Journeys { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }
    }
}
