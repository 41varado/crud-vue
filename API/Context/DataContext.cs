using API.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace API.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Accounting> Accountants { get; set; }

        public DataContext(DbContextOptions options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Accounting>(Accountant =>
            {
                Accountant.Property(a => a.Name)
                    .IsRequired();

                Accountant.Property(a => a.BirthDate)
                    .IsRequired();
            });
        }
    }
}
