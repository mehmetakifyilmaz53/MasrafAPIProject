using MasrafDeneme.Data;
using MasrafDeneme.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace MasrafDeneme.Helpers
{
    public class Aggregate
    {
        private readonly AppDbContext _context;

        public Aggregate(AppDbContext context)
        {
            _context = context;
        }

        public void DailyAggregate()
        {
            try
            {
                var aggregates = _context.Transactions
                        .Where(t => t.Date >= DateTime.Now.Date && t.Date < DateTime.Now.Date.AddDays(1))
                        .GroupBy(t => new { t.PersonId, t.Person.Name })
                        .Select(g => new TransactionAggregate
                        {
                            PersonId = g.Key.PersonId,
                            PersonName = g.Key.Name,
                            TotalAmount = g.Sum(t => t.Amount),
                            TransactionCount = g.Count()
                        })
                        .ToList();

                Console.WriteLine(aggregates);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void WeeklyAggregate()
        {
            try
            {
                var startOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek);
                var endOfWeek = startOfWeek.AddDays(7);

                var aggregates = _context.Transactions
                        .Where(t => t.Date >= startOfWeek && t.Date < endOfWeek)
                        .GroupBy(t => new { t.PersonId, t.Person.Name })
                        .Select(g => new TransactionAggregate
                        {
                            PersonId = g.Key.PersonId,
                            PersonName = g.Key.Name,
                            TotalAmount = g.Sum(t => t.Amount),
                            TransactionCount = g.Count()
                        })
                        .ToList();

                Console.WriteLine(aggregates);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
