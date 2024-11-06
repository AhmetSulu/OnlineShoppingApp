using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingApp.Data.Entities;

public class OrderProductEntity : BaseEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal TotalPrice => Price * Quantity;

    // Relational Property
    public OrderEntity? Order { get; set; }
    public ProductEntity Product { get; set; }
    public class OrderProductConfiguration : BaseConfiguration<OrderProductEntity>
    {
        public override void Configure(EntityTypeBuilder<OrderProductEntity> builder)
        {
            builder.Ignore(x => x.Id);
            builder.HasKey(op => new { op.OrderId, op.ProductId });
            builder.HasOne(op => op.Order)
                   .WithMany(o => o.OrderProducts)
                   .HasForeignKey(op => op.OrderId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.HasOne(op => op.Product)
                   .WithMany(p => p.OrderProducts)
                   .HasForeignKey(op => op.ProductId)
                   .OnDelete(DeleteBehavior.Cascade);
            builder.Property(op => op.Price)
                   .HasColumnType("decimal(18,2)");

            base.Configure(builder);
        }
    }
}
