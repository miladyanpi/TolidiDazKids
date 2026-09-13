using static Dto.Enum.EnumConstant;

namespace Domain
{

    public class Order:Base
    {
        public int? CustomerID { get; set; }
        public int? SendProductMethodID { get; set; }
        public long OrderCode { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public Int64 TotalAmount { get; set; }
        public Int64 Discount { get; set; }
        public Int64 FinalAmount { get; set; }
        public string? CardPen { get; set; }
    /// <summary>
    /// RefNum Or Refrence Number
    /// </summary>
        public string? RefId { get; set; }
        public string? ResNum { get; set; }
        public string? JsonAddress { get; set; }
        public DateTime? ExpireAt { get; set; }
        public string? StatusDescription { get; set; }
        #region RelationShip
        public virtual Customer? Customer { get; set; }
        public virtual ICollection<OrderItem>? OrderItems { get; set; }
        public virtual SendProductMethod? SendProductMethod { get; set; }

        #endregion


    }
}
