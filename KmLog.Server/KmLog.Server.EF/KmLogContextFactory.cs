using System.IO;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace KmLog.Server.EF
{
    public class KmLogContextFactory : IDesignTimeDbContextFactory<KmLogContext>
    {
        private const string API_PROJECT = "KmLog.Server.WebApi";

        public KmLogContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), $"../{API_PROJECT}"))
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString(nameof(KmLogContext));
            return new KmLogContext(connectionString);
        }
    }
}
