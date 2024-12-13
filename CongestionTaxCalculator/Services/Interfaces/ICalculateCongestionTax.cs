using CongestionTaxCalculator.Enums;

namespace CongestionTaxCalculator.services.Interfaces
{
    public interface ICalculateCongestionTax
    {
        public int GetTax(VehicleTypes vehicle, DateTime[] dates);
    }
}