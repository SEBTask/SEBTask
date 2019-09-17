using Domain.Entities;
using System.Threading.Tasks;

namespace Domain.Contracts.Services
{
    public interface IBaseRateService
    {
        Task<decimal> GetBaseRate(BaseRateCodeEnum baseRateCode);
    }
}
