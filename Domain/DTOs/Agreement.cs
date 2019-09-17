namespace Domain.DTOs
{
    public class Agreement
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public string BaseRateCode { get; set; }
        public decimal Margin { get; set; }
        public int AgreementDuration { get; set; }
    }
}
