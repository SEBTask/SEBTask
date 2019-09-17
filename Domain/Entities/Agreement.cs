namespace Domain.Entities
{
    public class Agreement : BaseEntity
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public BaseRateCodeEnum BaseRateCodeId { get; set; }
        public decimal Margin { get; set; }
        public int AgreementDuration { get; set; }
        public long UserId { get; set; }

        public virtual BaseRateCode BaseRateCode { get; set; }
        public virtual User User { get; set; }

        public decimal CalculateInterestRate(decimal baseRate)
        {
            return baseRate + this.Margin;
        }
    }
}
