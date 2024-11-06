using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OnlineShoppingApp.Data.Enums;
using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingApp.Data.Entities
{
    public class UserEntity : BaseEntity
    {       
        public string FirstName { get; set; }        
        public string LastName { get; set; }        
        public string Email { get; set; }     
        public string PhoneNumber { get; set; }       
        public string Password { get; set; }      
        public string ConfirmPassword { get; set; }     
        public DateTime BirthDate { get; set; }       
        public string Address { get; set; }
        public UserType UserType { get; set; }

        // Relational Property
        public ICollection<OrderEntity> Orders { get; set; }
        public class UserConfiguration : BaseConfiguration<UserEntity>
        {
            public override void Configure(EntityTypeBuilder<UserEntity> builder)
            {
                base.Configure(builder);
            }
        }
    }
}
