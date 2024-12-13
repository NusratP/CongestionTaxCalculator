using CongestionTaxCalculator.DataAccess.Models.Contexts;
using CongestionTaxCalculator.Enums;
using CongestionTaxCalculator.services.Interfaces;
namespace CongestionTaxCalculator.services
{
    public class CalculateCongestionTax : ICalculateCongestionTax
    {        
        //Scenario 1: A car passing a toll at 7:00:00 and then at 7:59:59 will be considered as inside the 1 hour time period and highest toll amount will be considered.
        //Scenario 2: A car passing a toll at 7:00:00 and then at 8:00:00 will be considered as outside the 1 hour time period and toll will be added for both the time period.
        private readonly CongestionTaxContext _context;
        public CalculateCongestionTax(CongestionTaxContext context)
        {
            _context = context;
        }
        public int GetTax(VehicleTypes vehicle, DateTime[] dates)
        {
            try
            {
                if (IsTollFreeVehicle(vehicle))
                    return 0;

                DateTime intervalStart = dates[0];
                int totalFee = 0;
                int highestFeeInInterval = 0;
                foreach (DateTime date in dates)
                {
                    int nextFee = GetTollFee(date);
                    long minutes = (long)(date - intervalStart).TotalMinutes;

                    if (minutes < 60)
                    {
                        if (nextFee > highestFeeInInterval)
                        {
                            highestFeeInInterval = nextFee;
                        }
                    }
                    else
                    {
                        totalFee += highestFeeInInterval;
                        intervalStart = date;
                        highestFeeInInterval = nextFee;
                    }
                }

                // Add fee for last interval
                totalFee += highestFeeInInterval;

                if (totalFee > 60) totalFee = 60;
                return totalFee;

            }
            catch (Exception e)
            {
                //TODO: logging
                Console.WriteLine(e);
                throw;
            }
        }

        private bool IsTollFreeVehicle(VehicleTypes vehicle)
        {
            try
            {
                return Enum.IsDefined(typeof(TollFreeVehicles), vehicle.ToString());
            }
            catch (Exception e)
            {
                //TODO: logging
                Console.WriteLine(e);
                throw;
            }
        }

        private int GetTollFee(DateTime date)
        {
            if (IsTollFreeDate(date)) return 0;

            int hour = date.Hour;
            int minute = date.Minute;


            #region TollFees from Database

            var tollFeeHours = _context.TollFeeHours.ToList();

            foreach (var tollFeeHour in tollFeeHours)
            {
                if ((hour > tollFeeHour.StartHour || (hour == tollFeeHour.StartHour && minute >= tollFeeHour.StartMinute)) &&
                    (hour < tollFeeHour.EndHour || (hour == tollFeeHour.EndHour && minute <= tollFeeHour.EndMinute)))
                {
                    return tollFeeHour.Fee;
                }
            }

            return 0;

            #endregion

            #region OldLogic

            //if (hour == 6 && minute >= 0 && minute <= 29) return 8;
            //else if (hour == 6 && minute >= 30 && minute <= 59) return 13;
            //else if (hour == 7 && minute >= 0 && minute <= 59) return 18;
            //else if (hour == 8 && minute >= 0 && minute <= 29) return 13;
            //else if ((hour == 8 && minute >= 30) || (hour > 8 && hour < 15) || (hour == 14 && minute <= 59)) return 8;
            //else if (hour == 15 && minute >= 0 && minute <= 29) return 13;
            //else if ((hour == 15 && minute >= 30) || (hour == 16 && minute <= 59)) return 18;
            //else if (hour == 17 && minute >= 0 && minute <= 59) return 13;
            //else if (hour == 18 && minute >= 0 && minute <= 29) return 8;
            //else return 0;

            #endregion
        }

        //The public holiday dates are considered to be correct
        private bool IsTollFreeDate(DateTime date)
        {
            int year = date.Year;
            int month = date.Month;
            int day = date.Day;

            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) return true;

            if (year == 2013)
            {
                if (month == 1 && day == 1 ||
                    month == 3 && (day == 28 || day == 29) ||
                    month == 4 && (day == 1 || day == 30) ||
                    month == 5 && (day == 1 || day == 8 || day == 9) ||
                    month == 6 && (day == 5 || day == 6 || day == 21) ||
                    month == 7 ||
                    month == 11 && day == 1 ||
                    month == 12 && (day == 24 || day == 25 || day == 26 || day == 31))
                {
                    return true;
                }
            }

            return false;
        }

    }
}