namespace Domain
{
    /// <summary>
    /// انبار محصولات اولیه
    /// انبار محصولات خام
    /// </summary>
    public class RawProductStore:Base
    {
        public int? RawProductID { get; set; }
        /// <summary>
        /// نوع اندازه گیری
        /// enum MessurmentType
        /// </summary>
        public int MessurmentType { get; set; }
        /// <summary>
        /// مقدار یا تعداد به ازای هر کیلو یا متر
        /// </summary>
        public int Amount{ get; set; }
        public Int64 Price { get; set; }
        public int BuyDate { get; set; }
        #region RelationShip
        public virtual RawProduct? RawProduct { get; set; }
        public virtual ICollection<RawProductStore_Product>? RawProductStore_Products { get; set; }

        #endregion
    }
}
