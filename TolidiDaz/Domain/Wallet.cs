using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain
{
    public class Wallet:Base
    {
        public int CustomerID { get; set; }
        public string Currency { get; set; } = "IRR"; // ISO currency code
        public Int64 Balance { get; set; }
        public Int64 GiftCredit { get; set; }
        #region RelationShip
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<WalletTransaction>? WalletTransactions { get; set; }
        #endregion
    }

   
}
