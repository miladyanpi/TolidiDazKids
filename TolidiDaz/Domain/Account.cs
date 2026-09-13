using Microsoft.AspNetCore.Identity;

namespace Domain
{

    public class Account : IdentityUser
    {
        public int? CustomerID { get; set; } = null;

        #region Relation
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<RefreshTokenEntity>? RefreshTokenEntitys { get; set; }
        
        #endregion
    }
}
