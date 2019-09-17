using Domain.Contracts.DataAccess;
using Domain.Contracts.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Mappers;
using Domain.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Services
{
    public class InterestRateService : IInterestRateService
    {
        private readonly IUnitOfWork uow;
        private readonly IBaseRateService baseRateService;

        public InterestRateService(IUnitOfWork uow, IBaseRateService baseRateService)
        {
            this.uow = uow;
            this.baseRateService = baseRateService;
        }

        public async Task<ImpactOfBaseRateChange> CalculateImpactOfBaseRateChange
        (
            long userId,
            long agreementId,
            BaseRateCodeEnum newBaseRate
        )
        {
            var userRepo = this.uow.GetRepository<User>();
            var user = await userRepo.GetAsync(
                u => u.Id == userId && u.Agreements.Any(a => a.Id == agreementId),
                u => u.Include(x => x.Agreements)
            ) ?? throw new EntityNotFoundException($"User with Id: {userId}, which has an agreement with Id: {agreementId}, was not found.");

            var targetAgreement = user.Agreements.Single(a => a.Id == agreementId);

            var baseRatesCodes = new List<BaseRateCodeEnum> { targetAgreement.BaseRateCodeId, newBaseRate };
            var baseRateTasks = baseRatesCodes.Select(brc => this.baseRateService.GetBaseRate(brc));
            var baseRates = await Task.WhenAll(baseRateTasks);

            var currentInterestRate = targetAgreement.CalculateInterestRate(baseRates.First());
            var newInterestRate = targetAgreement.CalculateInterestRate(baseRates.Last());

            return new ImpactOfBaseRateChange
            {
                User = user.ToDto(),
                Agreement = targetAgreement.ToDto(),
                CurrentInterestRate = currentInterestRate,
                NewInterestRate = newInterestRate,
                InterestRateDifference = Math.Abs(newInterestRate - currentInterestRate)
            };
        }
    }
}
