using Domain.Entities;
using Domain.Responses;
using System.Threading.Tasks;

namespace Domain.Contracts.Services
{
    public interface IInterestRateService
    {
        Task<ImpactOfBaseRateChange> CalculateImpactOfBaseRateChange(long userId, long agreementId, BaseRateCodeEnum newBaseRate);
    }
}
