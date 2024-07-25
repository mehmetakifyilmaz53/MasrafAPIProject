using MasrafDeneme.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MasrafDeneme.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Person> People { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }

}
