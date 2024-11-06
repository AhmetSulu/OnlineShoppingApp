using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Data.Entities;
using static OnlineShoppingApp.Data.Entities.OrderEntity;
using static OnlineShoppingApp.Data.Entities.ProductEntity;
using static OnlineShoppingApp.Data.Entities.UserEntity;
using static OrderProductEntity;

namespace OnlineShoppingApp.Data.Context
{
    public class OnlineShoppingAppDbContext : DbContext
    {
        public OnlineShoppingAppDbContext(DbContextOptions<OnlineShoppingAppDbContext> options) : base(options)
        {
        }
        public DbSet<UserEntity> Users => Set<UserEntity>();
        public DbSet<ProductEntity> Products => Set<ProductEntity>();
        public DbSet<OrderEntity> Orders => Set<OrderEntity>();
        public DbSet<OrderProductEntity> OrderProducts => Set<OrderProductEntity>();
        public DbSet<SettingEntity> Settings => Set<SettingEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderEntity>().HasQueryFilter(o => !o.IsDeleted);
            modelBuilder.Entity<OrderProductEntity>().HasQueryFilter(op => !op.IsDeleted);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new ProductConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new OrderProductConfiguration());
            modelBuilder.Entity<SettingEntity>().HasData(new SettingEntity()
            {
                Id=1,
                MaintenanceMode = false
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
