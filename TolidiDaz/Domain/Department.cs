namespace Domain
{
    /// <summary>
    /// بخش مربوطه: پشتیبانی فنی-مالی و پرداخت ها-سفارشات و ارسال-پیشنهادات و انتقادات
    /// </summary>
    public class Department:Base
    {
        public string? Title { get; set; }
        #region RelationShip
        public virtual ICollection<Ticket>? Tickets { get; set; }
        #endregion
    }
}
