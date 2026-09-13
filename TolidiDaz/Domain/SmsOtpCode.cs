namespace Domain
{
    public class SmsOtpCode:Base
    {
        public string? PhoneNumber { get; set; }
        public string? Code { get; set; }
        public int DateExpired { get; set; }
        public TimeSpan TimeExpired { get; set; }
    }
}
