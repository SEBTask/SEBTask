using Domain.DTOs;

namespace Domain.Responses
{
    public class ImpactOfBaseRateChange
    {
        public Agreement Agreement { get; set; }
        public User User { get; set; }
        public decimal CurrentInterestRate { get; set; }
        public decimal NewInterestRate { get; set; }
        public decimal InterestRateDifference { get; set; }
    }
}
