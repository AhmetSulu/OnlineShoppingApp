using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.Data.Entities
{
    public class OrderEntity : BaseEntity
    {
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public int CustomerId { get; set; }

        // Relational Property
        public ICollection<OrderProductEntity> OrderProducts { get; set; }

        // Navigation Property
        public UserEntity Customer { get; set; }

        public OrderEntity()
        {
            OrderProducts = new List<OrderProductEntity>();
        }

        public class OrderConfiguration : BaseConfiguration<OrderEntity>
        {
            public override void Configure(EntityTypeBuilder<OrderEntity> builder)
            {
                builder.Property(o => o.TotalAmount)
                       .HasColumnType("decimal(18,2)");
                builder.HasOne(o => o.Customer)
                       .WithMany(u => u.Orders)
                       .HasForeignKey(o => o.CustomerId);

                base.Configure(builder);
            }
        }
    }
}
