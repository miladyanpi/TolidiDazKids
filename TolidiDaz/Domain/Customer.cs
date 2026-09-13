using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain
{
    /// <summary>
    /// مشخصات مشتری
    /// </summary>
    [Index(nameof(Mcode), IsUnique = true)]
    public class Customer : Base
    {
        public string? Mcode { get; set; }
        public string?  Name { get; set; }
        public string? LastName { get; set; }
        public int? Gender { get; set; }//enum Gender
        public int? BirthDate { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? JsonPicture { get; set; }
        public string? Address { get; set; }
        public string? PostalCode { get; set; }
        public string? Description { get; set; }

        #region RelationShip
        public virtual Account? Account { get; set; }
        public virtual ICollection<SmsLog>? SmsLogs { get; set; }
        public virtual ICollection<CustomerAddress>? CustomerAddresss { get; set; }
        public virtual ICollection<Cart>? Carts { get; set; }
        public virtual ICollection<Order>? Orders { get; set; }
        public virtual Wallet? Wallet { get; set; }
        public virtual ICollection<FavoritUserProduct>? FavoritUserProducts { get; set; }
        public virtual ICollection<ProductComment>? ProductComments { get; set; }
        public virtual ICollection<BlogComment>? BlogComments { get; set; }
        #endregion
    }
}
