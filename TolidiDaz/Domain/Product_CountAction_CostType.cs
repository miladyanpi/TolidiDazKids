namespace Domain
{
    /// <summary>
    /// براساس محصول و تعداد عمل خاصی قیمت گذاری می شود
    /// مثال دوخت پارچه
    /// </summary>
    public class Product_CountAction_CostType:Base
    {
        public int? PositionID { get; set; }
        public int? ProductID { get; set; }
        /// <summary>
        /// تعدا کار خاص
        /// </summary>
        public int CountAction { get; set; }
        /// <summary>
        /// قیمت هر عدد
        /// </summary>
        public Int64 Price { get; set; }
        public string? Description { get; set; }

        #region RelationShip
        public virtual Position? Position { get; set; }
        public virtual Product? Product { get; set; }

        #endregion
    }
}
