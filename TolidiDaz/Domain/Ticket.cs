using static Dto.Enum.EnumConstant;

namespace Domain
{
    public class Ticket:Base
    {
        public int DepartmentID { get; set; }
        public string? Title { get; set; }
        public TicketPriority? TicketPriority { get; set; }
        public string? Description { get; set; }
        public string? JsonTicketFile { get; set; }
        #region RelationShip
        public virtual Department? Department { get; set; }
        #endregion
    }
}
