using Domain.Contracts.DataAccess;
using Domain.Contracts.Services;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Services;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Test.Services
{
    [TestFixture]
    public class InterestRateServiceTests
    {
        private readonly Mock<IUnitOfWork> uowMock;
        private readonly Mock<IRepository<User>> userRepositoryMock;
        private readonly Mock<IBaseRateService> baseRateServiceMock;

        public InterestRateServiceTests()
        {
            this.uowMock = new Mock<IUnitOfWork>();
            this.userRepositoryMock = new Mock<IRepository<User>>();
            this.baseRateServiceMock = new Mock<IBaseRateService>();

            this.uowMock.Setup(x => x.GetRepository<User>()).Returns(userRepositoryMock.Object);
            this.baseRateServiceMock.Setup(x => x.GetBaseRate(BaseRateCodeEnum.VILIBOR1m)).ReturnsAsync(0.15m);
            this.baseRateServiceMock.Setup(x => x.GetBaseRate(BaseRateCodeEnum.VILIBOR1y)).ReturnsAsync(0.3m);
            this.baseRateServiceMock.Setup(x => x.GetBaseRate(BaseRateCodeEnum.VILIBOR3m)).ReturnsAsync(0.45m);
            this.baseRateServiceMock.Setup(x => x.GetBaseRate(BaseRateCodeEnum.VILIBOR6m)).ReturnsAsync(0.6m);
        }

        [SetUp]
        public void BeforeEveryTest()
        {
            this.userRepositoryMock.Reset();
        }

        private void SetupUserRepositoryGetToReturn(User returnedUser)
        {
            this.userRepositoryMock
                .Setup(ur => ur.GetAsync(
                    It.IsAny<Expression<Func<User, bool>>>(),
                    It.IsAny<Func<IQueryable<User>, IIncludableQueryable<User, object>>>()
                    ))
                .ReturnsAsync(returnedUser);
        }

        [Test]
        public void CalculateImpactOfBaseRateChange_WhenUserWithAgreementIsNotFound_ShoulThrowAnException()
        {
            this.SetupUserRepositoryGetToReturn(null);
            var service = new InterestRateService(this.uowMock.Object, this.baseRateServiceMock.Object);

            Assert.ThrowsAsync<EntityNotFoundException>(
                async () => await service.CalculateImpactOfBaseRateChange(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<BaseRateCodeEnum>())
            );
        }

        [Test]
        public async Task CalculateImpactOfBaseRateChange_BaseRateServiceMethodGetBaseRateShouldBeCalledTwice()
        {
            var user = new User
            {
                Id = 67812203006,
                Name = "Goras Trusevičius",
                Agreements = new List<Agreement>()
                {
                    new Agreement
                    {
                        Id = 1,
                        Amount = 12000,
                        BaseRateCodeId = BaseRateCodeEnum.VILIBOR3m,
                        Margin = 1.6M,
                        AgreementDuration = 60
                    }
                }
            };
            this.SetupUserRepositoryGetToReturn(user);
            var service = new InterestRateService(this.uowMock.Object, this.baseRateServiceMock.Object);

            await service.CalculateImpactOfBaseRateChange(67812203006, 1, BaseRateCodeEnum.VILIBOR1m);

            this.baseRateServiceMock.Verify(x => x.GetBaseRate(BaseRateCodeEnum.VILIBOR1m), Times.Once);
            this.baseRateServiceMock.Verify(x => x.GetBaseRate(BaseRateCodeEnum.VILIBOR3m), Times.Once);
        }

        [Test]
        public async Task CalculateImpactOfBaseRateChange_WhenBaseRateCodesAreTheSame_DifferenceShouldBe0()
        {
            var baseRateCode = BaseRateCodeEnum.VILIBOR3m;
            var user = new User
            {
                Id = 67812203006,
                Name = "Goras Trusevičius",
                Agreements = new List<Agreement>()
                {
                    new Agreement
                    {
                        Id = 1,
                        Amount = 12000,
                        BaseRateCodeId = baseRateCode,
                        Margin = 1.6M,
                        AgreementDuration = 60
                    }
                }
            };
            this.SetupUserRepositoryGetToReturn(user);
            var service = new InterestRateService(this.uowMock.Object, this.baseRateServiceMock.Object);

            var impact = await service.CalculateImpactOfBaseRateChange(67812203006, 1, baseRateCode);

            Assert.AreEqual(impact.InterestRateDifference, 0m);
        }

        [Test]
        public async Task CalculateImpactOfBaseRateChange_WhenBaseRateCodesAreDifferentAndBaseRatesAreDifferent_DifferenceShouldBeNot0()
        { 
            var user = new User
            {
                Id = 67812203006,
                Name = "Goras Trusevičius",
                Agreements = new List<Agreement>()
                {
                    new Agreement
                    {
                        Id = 1,
                        Amount = 12000,
                        BaseRateCodeId = BaseRateCodeEnum.VILIBOR1m,
                        Margin = 1.6M,
                        AgreementDuration = 60
                    }
                }
            };
            this.SetupUserRepositoryGetToReturn(user);
            var service = new InterestRateService(this.uowMock.Object, this.baseRateServiceMock.Object);

            var impact = await service.CalculateImpactOfBaseRateChange(67812203006, 1, BaseRateCodeEnum.VILIBOR1y);

            Assert.AreNotEqual(impact.InterestRateDifference, 0m);
        }
    }
}
