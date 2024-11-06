using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShoppingApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.Data.Entities
{
    public class ProductEntity : BaseEntity
    {
        public string ProductName { get; set; }
        public decimal Price { get; set; }        
        public int StockQuantity { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public ProductCategory ProductCategory { get; set; }

        // Relational Property
        public ICollection<OrderProductEntity> OrderProducts { get; set; }
        public class ProductConfiguration : BaseConfiguration<ProductEntity>
        {
            public override void Configure(EntityTypeBuilder<ProductEntity> builder)
            {
                builder.Property(p => p.Price)
                       .HasColumnType("decimal(18,2)");

                base.Configure(builder);
            }
        }
    }
}
