﻿// <auto-generated />
using Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(SEBTaskDbContext))]
    [Migration("20190915172706_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Domain.Entities.Agreement", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AgreementDuration");

                    b.Property<decimal>("Amount");

                    b.Property<int>("BaseRateCodeId");

                    b.Property<decimal>("Margin");

                    b.Property<long>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("BaseRateCodeId");

                    b.HasIndex("UserId");

                    b.ToTable("Agreements");

                    b.HasData(
                        new
                        {
                            Id = 1L,
                            AgreementDuration = 60,
                            Amount = 12000m,
                            BaseRateCodeId = 2,
                            Margin = 1.6m,
                            UserId = 67812203006L
                        },
                        new
                        {
                            Id = 2L,
                            AgreementDuration = 36,
                            Amount = 8000m,
                            BaseRateCodeId = 4,
                            Margin = 2.2m,
                            UserId = 78706151287L
                        },
                        new
                        {
                            Id = 3L,
                            AgreementDuration = 24,
                            Amount = 1000m,
                            BaseRateCodeId = 3,
                            Margin = 1.85m,
                            UserId = 78706151287L
                        });
                });

            modelBuilder.Entity("Domain.Entities.BaseRateCode", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("BaseRateCodes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "VILIBOR1m"
                        },
                        new
                        {
                            Id = 2,
                            Name = "VILIBOR3m"
                        },
                        new
                        {
                            Id = 3,
                            Name = "VILIBOR6m"
                        },
                        new
                        {
                            Id = 4,
                            Name = "VILIBOR1y"
                        });
                });

            modelBuilder.Entity("Domain.Entities.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 67812203006L,
                            Name = "Goras Trusevičius"
                        },
                        new
                        {
                            Id = 78706151287L,
                            Name = "Dange Kulkavičiutė"
                        });
                });

            modelBuilder.Entity("Domain.Entities.Agreement", b =>
                {
                    b.HasOne("Domain.Entities.BaseRateCode", "BaseRateCode")
                        .WithMany("Agreements")
                        .HasForeignKey("BaseRateCodeId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Domain.Entities.User", "User")
                        .WithMany("Agreements")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
