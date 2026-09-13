
namespace Domain
{
    public class Reminder:Base
    {
        public string? Title { get; set; }
        public int Date { get; set; }
        public TimeSpan Time { get; set; }
        public string? Description { get; set; }
        public bool SendSms { get; set; }

    }
}
