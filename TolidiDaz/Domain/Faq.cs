namespace Domain
{
    /// <summary>
    /// سوالات مشتریان
    /// </summary>
    public class Faq : Base
    {
        public string? FullName { get; set; }
        public string? Mobile { get; set; }
        public string? Subject { get; set; }
        public string? Message { get; set; }
        public string? IpAddress { get; set; }
        public bool IsRead{ get; set; }
    }
}
