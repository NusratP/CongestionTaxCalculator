using Microsoft.AspNetCore.Mvc;
using CongestionTaxCalculator.enums;
using CongestionTaxCalculator.services.Interfaces;

namespace CongestionTaxCalculator.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class CongestionCalculatorController : ControllerBase
    {
        private readonly ICalculateCongestionTax _calculateCongestionTax;

        public CongestionCalculatorController(ICalculateCongestionTax calculateCongestionTax)
        {
            _calculateCongestionTax = calculateCongestionTax;
        }

        [HttpPost]
        [Route("api/calculateTax/{vehicle}")]
        public Task<int> CalculateTax(string vehicle, [FromBody] DateTime[] dates)
        {
            //Validation
            if (dates.Any(date => date < DateTime.Parse("2013-01-01") || date >= DateTime.Parse("2014-01-01")))
            {
                throw new Exception("Congestion tax calculation is only done for the year 2013!");
            }
            if (!Enum.TryParse(vehicle, true, out VehicleTypes vehicleType))
            {
                throw new Exception("Invalid vehicle type!");
            }

            var result = _calculateCongestionTax.GetTax(vehicleType, dates);
            return Task.FromResult(result);
        }
    }
}