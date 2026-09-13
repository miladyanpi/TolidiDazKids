
using static Dto.Enum.EnumConstant;

namespace Domain
{
    public class ReminderEvent:Base
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool SendSms { get; set; }
        public TimeSpan Time { get; set; }
        public ReminderType ReminderType { get; set; }

    }
}
