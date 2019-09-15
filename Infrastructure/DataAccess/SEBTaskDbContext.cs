using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataAccess
{
    public class SEBTaskDbContext : DbContext
    {
        public SEBTaskDbContext(DbContextOptions<SEBTaskDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Agreement> Agreements { get; set; }
        public DbSet<BaseRateCode> BaseRateCodes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .ConfigureAgreement()
                .ConfigureBaseRateCode()
                .ConfigureUser();
        }
    }
}
