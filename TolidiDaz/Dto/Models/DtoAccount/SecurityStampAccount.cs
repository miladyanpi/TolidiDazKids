namespace Dto.Models.DtoAccount
{
    public class SecurityStampAccount
    {
        public SecurityStampAccount()
        {
            SecurityStamp=string.Empty;
            CustomerID = 0;
        }
        public string? SecurityStamp { get; set; }
        public int? CustomerID { get; set; }
    }
}
