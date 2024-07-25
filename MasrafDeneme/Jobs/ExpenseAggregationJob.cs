using MasrafDeneme.Data;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace MasrafDeneme.Jobs
{
    public class ExpenseAggregationJob : IJob
    {
        private readonly AppDbContext _context;

        public ExpenseAggregationJob(AppDbContext context)
        {
            _context = context;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var now = DateTime.UtcNow;
            var oneDayAgo = now.AddDays(-1);
            var oneWeekAgo = now.AddDays(-7);
            var oneMonthAgo = now.AddMonths(-1);

            var dailyExpenses = await _context.Transactions
                .Where(t => t.Date >= oneDayAgo)
                .GroupBy(t => t.PersonId)
                .Select(g => new { PersonId = g.Key, TotalAmount = g.Sum(t => t.Amount) })
                .ToListAsync();

            var weeklyExpenses = await _context.Transactions
                .Where(t => t.Date >= oneWeekAgo)
                .GroupBy(t => t.PersonId)
                .Select(g => new { PersonId = g.Key, TotalAmount = g.Sum(t => t.Amount) })
                .ToListAsync();

            var monthlyExpenses = await _context.Transactions
                .Where(t => t.Date >= oneMonthAgo)
                .GroupBy(t => t.PersonId)
                .Select(g => new { PersonId = g.Key, TotalAmount = g.Sum(t => t.Amount) })
                .ToListAsync();

            // Bu verileri kullanarak gerekli işlemleri yapabilirsiniz
        }

    }
}
