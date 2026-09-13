namespace Domain
{
    public class SmsLog:Base
    {
        public int? CustomerID { get; set; }
        public string? Title{ get; set; }
        public string? Mobile{ get; set; }
        public string? Message { get; set; }
        public string? StatusText { get; set; }
        public int? SendDate { get; set; }
        public TimeSpan? Time { get; set; }
        #region RelationShip
        public virtual Customer? Customer { get; set; }
        #endregion
    }
}
