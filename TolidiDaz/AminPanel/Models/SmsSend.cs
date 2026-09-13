namespace AdminPanel.Models
{
    public class ParameterArray
    {
        public string Parameter { get; set; }
        public string ParameterValue { get; set; }
    }

    public class SmsSend
    {
        public List<ParameterArray> ParameterArray { get; set; }
        public string Mobile { get; set; }
        public string TemplateId { get; set; }
    }
}
