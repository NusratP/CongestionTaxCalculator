using CongestionTaxCalculator.DataAccess.Dto;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.DataAccess.Models.Contexts
{
    public class CongestionTaxContext : DbContext
    {
        public CongestionTaxContext(DbContextOptions<CongestionTaxContext> options) : base(options) { }

        public DbSet<TollFeeHour> TollFeeHours { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TollFeeHour>().HasData(
                new TollFeeHour { Id = 1, StartHour = 6, StartMinute = 0, EndHour = 6, EndMinute = 29, Fee = 8 },
                new TollFeeHour { Id = 2, StartHour = 6, StartMinute = 30, EndHour = 6, EndMinute = 59, Fee = 13 },
                new TollFeeHour { Id = 3, StartHour = 7, StartMinute = 0, EndHour = 7, EndMinute = 59, Fee = 18 },
                new TollFeeHour { Id = 4, StartHour = 8, StartMinute = 0, EndHour = 8, EndMinute = 29, Fee = 13 },
                new TollFeeHour { Id = 5, StartHour = 8, StartMinute = 30, EndHour = 14, EndMinute = 59, Fee = 8 },
                new TollFeeHour { Id = 6, StartHour = 15, StartMinute = 0, EndHour = 15, EndMinute = 29, Fee = 13 },
                new TollFeeHour { Id = 7, StartHour = 15, StartMinute = 30, EndHour = 16, EndMinute = 59, Fee = 18 },
                new TollFeeHour { Id = 8, StartHour = 17, StartMinute = 0, EndHour = 17, EndMinute = 59, Fee = 13 },
                new TollFeeHour { Id = 9, StartHour = 18, StartMinute = 0, EndHour = 18, EndMinute = 29, Fee = 8 }
            );
        }
    }
}
