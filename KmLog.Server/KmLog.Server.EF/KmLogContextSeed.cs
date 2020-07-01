using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KmLog.Server.Domain;
using KmLog.Server.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace KmLog.Server.EF
{
    public class KmLogContextSeed
    {
        public static async Task SeedAsync(KmLogContext context, string[] emails, ILoggerFactory loggerFactory, int retry = 0)
        {
            int retryForAvailability = retry;
            try
            {
                context.Database.Migrate();

                if (emails.Any() && !await context.Users.AnyAsync())
                {
                    // add initial users
                    var users = GetInitialUsers(emails);
                    await context.Users.AddRangeAsync(users);

                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<KmLogContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(context, emails, loggerFactory, retryForAvailability);
                }
                throw;
            }
        }

        private static IEnumerable<User> GetInitialUsers(string[] emails)
        {
            foreach (var email in emails)
            {
                yield return new User
                {
                    Email = email,
                    Role = UserRole.Admin
                };
            }
        }
    }
}
