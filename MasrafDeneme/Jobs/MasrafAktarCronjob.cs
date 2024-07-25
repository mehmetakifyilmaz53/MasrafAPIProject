using EasyCronJob.Abstractions;
using MasrafDeneme.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MasrafDeneme.Jobs
{
    public class MasrafAktarCronjob : CronJobService
    {
        private readonly ILogger<MasrafAktarCronjob> _logger;
        private readonly IServiceProvider _serviceProvider;

        public MasrafAktarCronjob(ICronConfiguration<MasrafAktarCronjob> cronConfiguration, ILogger<MasrafAktarCronjob> logger, IServiceProvider serviceProvider)
            : base(cronConfiguration.CronExpression, cronConfiguration.TimeZoneInfo)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Working MasrafAktarCronJob Start Time : " + DateTime.Now);

            using (var scope = _serviceProvider.CreateScope())
            {
                var aggregate = scope.ServiceProvider.GetRequiredService<Aggregate>();
                aggregate.DailyAggregate();
                aggregate.WeeklyAggregate();
            }

            return base.DoWork(cancellationToken);
        }
    }
}
