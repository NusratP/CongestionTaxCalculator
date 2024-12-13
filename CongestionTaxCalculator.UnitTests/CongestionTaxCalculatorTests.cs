using CongestionTaxCalculator.DataAccess.Dto;
using CongestionTaxCalculator.DataAccess.Models.Contexts;
using CongestionTaxCalculator.Enums;
using CongestionTaxCalculator.services;
using Microsoft.EntityFrameworkCore;

namespace CongestionTaxCalculator.UnitTests
{
    public class CongestionTaxCalculatorTests
    {
        private DbContextOptions<CongestionTaxContext> GetInMemoryDbContextOptions()
        {
            return new DbContextOptionsBuilder<CongestionTaxContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
        }

        private void SeedDatabase(CongestionTaxContext context)
        {
            if (!context.TollFeeHours.Any())
            {
                context.TollFeeHours.AddRange(
                    new TollFeeHour { Id = 1, StartHour = 6, StartMinute = 0, EndHour = 6, EndMinute = 29, Fee = 8 },
                    new TollFeeHour { Id = 2, StartHour = 6, StartMinute = 30, EndHour = 6, EndMinute = 59, Fee = 13 },
                    new TollFeeHour { Id = 3, StartHour = 7, StartMinute = 0, EndHour = 7, EndMinute = 59, Fee = 18 },
                    new TollFeeHour { Id = 4, StartHour = 8, StartMinute = 0, EndHour = 8, EndMinute = 29, Fee = 13 },
                    new TollFeeHour { Id = 5, StartHour = 8, StartMinute = 30, EndHour = 14, EndMinute = 59, Fee = 8 },
                    new TollFeeHour { Id = 6, StartHour = 15, StartMinute = 0, EndHour = 15, EndMinute = 29, Fee = 13 },
                    new TollFeeHour
                    { Id = 7, StartHour = 15, StartMinute = 30, EndHour = 16, EndMinute = 59, Fee = 18 },
                    new TollFeeHour { Id = 8, StartHour = 17, StartMinute = 0, EndHour = 17, EndMinute = 59, Fee = 13 },
                    new TollFeeHour { Id = 9, StartHour = 18, StartMinute = 0, EndHour = 18, EndMinute = 29, Fee = 8 }
                );
                context.SaveChanges();
            }
        }


        [Fact]
        public void GetTaxForCar_ShouldReturnCorrectTax_ForGivenDates()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using var context = new CongestionTaxContext(options);
            SeedDatabase(context);

            var calculator = new CalculateCongestionTax(context);
            var vehicle = VehicleTypes.Car;
            var dates = new DateTime[]
            {
                new DateTime(2013, 1, 2, 6, 0, 0), // 8 SEK
                new DateTime(2013, 1, 2, 7, 0, 0), // 18 SEK
                new DateTime(2013, 1, 2, 8, 0, 0)  // 13 SEK
            };

            // Act
            int tax = calculator.GetTax(vehicle, dates);

            // Assert
            Assert.Equal(39, tax);
        }

        [Fact]
        public void GetTaxForCar_ShouldReturnCorrectTax_HighestTollWithinInterval()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using var context = new CongestionTaxContext(options);
            SeedDatabase(context);

            var calculator = new CalculateCongestionTax(context);
            var vehicle = VehicleTypes.Car;
            var dates = new DateTime[]
            {
                new DateTime(2013, 1, 2, 6, 0, 0), // 8 SEK
                new DateTime(2013, 1, 2, 6, 59, 59, 999), // 13 SEK
            };

            // Act
            int tax = calculator.GetTax(vehicle, dates);

            // Assert
            Assert.Equal(13, tax);
        }

        [Fact]
        public void GetTaxForMotorcycle_ShouldReturnZero_ForTollFreeVehicle()
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using var context = new CongestionTaxContext(options);
            SeedDatabase(context);

            var calculator = new CalculateCongestionTax(context);
            var vehicle = VehicleTypes.Motorcycle;
            var dates = new DateTime[]
            {
                new DateTime(2013, 1, 1, 6, 0, 0),
                new DateTime(2013, 1, 1, 7, 0, 0),
                new DateTime(2013, 1, 1, 8, 0, 0)
            };

            // Act
            int tax = calculator.GetTax(vehicle, dates);

            // Assert
            Assert.Equal(0, tax); // Total tax should be 0 SEK for toll-free vehicles
        }


        [Theory]
        [InlineData("2013-01-14 21:00:00", 0)]
        [InlineData("2013-01-15 21:00:00", 0)]
        [InlineData("2013-02-07 06:23:27", 8)]
        [InlineData("2013-02-07 15:27:00", 13)]
        [InlineData("2013-02-08 06:27:00", 8)]
        [InlineData("2013-02-08 06:20:27", 8)]
        [InlineData("2013-02-08 14:35:00", 8)]
        [InlineData("2013-02-08 15:29:00", 13)]
        [InlineData("2013-02-08 15:47:00", 18)]
        [InlineData("2013-02-08 16:01:00", 18)]
        [InlineData("2013-02-08 16:48:00", 18)]
        [InlineData("2013-02-08 17:49:00", 13)]
        [InlineData("2013-02-08 18:29:00", 8)]
        [InlineData("2013-02-08 18:35:00", 0)]
        [InlineData("2013-03-26 14:25:00", 8)]
        [InlineData("2013-03-28 14:07:27", 0)] //public holiday
        public void GetTax_ShouldCalculateCorrectly(string dateString, int tollTax)
        {
            // Arrange
            var options = GetInMemoryDbContextOptions();
            using var context = new CongestionTaxContext(options);
            SeedDatabase(context);

            var calculator = new CalculateCongestionTax(context);
            var vehicle = VehicleTypes.Car;
            var dates = new DateTime[] { DateTime.Parse(dateString) };

            // Act
            int tax = calculator.GetTax(vehicle, dates);

            // Assert
            Assert.Equal(tollTax, tax);
        }
    }
}