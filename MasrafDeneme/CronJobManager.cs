using Hangfire;
using MasrafDeneme.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MasrafDeneme.Helpers
{
    public class CronJobManager
    {
        private readonly AppDbContext _context;

        public CronJobManager(AppDbContext context)
        {
            _context = context;
        }
        public  string _logPath => Path.Combine(Directory.GetCurrentDirectory(), "Log");

        public  void CreateFolder(string route)
        {
            if (!Directory.Exists(route)) Directory.CreateDirectory(route);
        }
        public void SetHangFire()
        {
            RecurringJob.AddOrUpdate("Masraf Günlük Cronjob", () => new Aggregate(_context).DailyAggregate(), "0 23 * * *", new RecurringJobOptions { TimeZone = TimeZoneInfo.FindSystemTimeZoneById("Turkey Standard Time") });
        }

    }
}
