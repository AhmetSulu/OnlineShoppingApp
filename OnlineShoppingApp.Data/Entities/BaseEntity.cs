using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OnlineShoppingApp.Data.Entities
{
    public class BaseEntity
    {
        public BaseEntity()
        {
              CreatedDate = DateTime.Now;
        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.ModifiedDate).IsRequired(false);
            builder.HasQueryFilter(x => x.IsDeleted == false);
        }
    }
}
 