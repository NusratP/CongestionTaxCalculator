namespace CongestionTaxCalculator.DataAccess.Dto
{
    public class TollFeeHour
    {
        public int Id { get; set; }
        public int StartHour { get; set; }
        public int StartMinute { get; set; }
        public int EndHour { get; set; }
        public int EndMinute { get; set; }
        public int Fee { get; set; }
    }
}
