namespace Domain.Mappers
{
    public static class AgreementMapper
    {
        public static DTOs.Agreement ToDto(this Entities.Agreement agreement)
        {
            return new DTOs.Agreement
            {
                Id = agreement.Id,
                Amount = agreement.Amount,
                BaseRateCode = agreement.BaseRateCodeId.ToString(),
                Margin = agreement.Margin,
                AgreementDuration = agreement.AgreementDuration
            };
        }
    }
}
