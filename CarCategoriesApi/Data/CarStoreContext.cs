
using Microsoft.EntityFrameworkCore;


namespace CarCategoriesApi.Data
{
    public class CarStoreContext : DbContext
    {
        IConfiguration _configuration;
        public CarStoreContext() 
        { 
            
        }

        public CarStoreContext(DbContextOptions<CarStoreContext> opt) : base(opt) 
        {
            
        }

        #region
        public DbSet<CarInfo> CarInfos { get; set; }
        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<BrandInfo> BrandInfos { get; set; }
        public DbSet<RefreshToken> refreshTokens { get; set; }
        #endregion



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserLogin>(entity =>
            {
                entity.HasIndex(e => e.Username).IsUnique();
            });
        }

    }
}
