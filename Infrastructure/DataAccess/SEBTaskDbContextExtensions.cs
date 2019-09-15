using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.DataAccess
{
    public static class SEBTaskDbContextExtensions
    {
        public static ModelBuilder ConfigureAgreement(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agreement>()
                .HasOne(a => a.BaseRateCode)
                .WithMany(brc => brc.Agreements)
                .HasForeignKey(a => a.BaseRateCodeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Agreement>()
                .HasOne(a => a.User)
                .WithMany(u => u.Agreements)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            var data = new List<Agreement>
            {
                new Agreement
                {
                    Id = 1,
                    Amount = 12000,
                    BaseRateCodeId = BaseRateCodeEnum.VILIBOR3m,
                    Margin = 1.6M,
                    AgreementDuration = 60,
                    UserId = 67812203006
                },

                new Agreement
                {
                    Id = 2,
                    Amount = 8000,
                    BaseRateCodeId = BaseRateCodeEnum.VILIBOR1y,
                    Margin = 2.2M,
                    AgreementDuration = 36,
                    UserId = 78706151287
                },

                new Agreement
                {
                    Id = 3,
                    Amount = 1000,
                    BaseRateCodeId = BaseRateCodeEnum.VILIBOR6m,
                    Margin = 1.85M,
                    AgreementDuration = 24,
                    UserId = 78706151287
                },
            };

            modelBuilder.Entity<Agreement>()
                .HasData(data);

            return modelBuilder;
        }

        public static ModelBuilder ConfigureBaseRateCode(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseRateCode>()
                .Property(brc => brc.Id)
                .ValueGeneratedNever();

            var data = Enum.GetValues(typeof(BaseRateCodeEnum))
                .OfType<BaseRateCodeEnum>()
                .Select(brce => new BaseRateCode() { Id = brce, Name = brce.ToString() })
                .ToArray();

            modelBuilder.Entity<BaseRateCode>()
                .HasData(data);

            return modelBuilder;
        }

        public static ModelBuilder ConfigureUser(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Name)
                .IsRequired();

            var data = new List<User>
            {
                new User
                {
                    Id = 67812203006,
                    Name = "Goras Trusevičius"
                },
                new User
                {
                    Id = 78706151287,
                    Name = "Dange Kulkavičiutė"
                }
            };

            modelBuilder.Entity<User>()
                .HasData(data);

            return modelBuilder;
        }
    }
}
