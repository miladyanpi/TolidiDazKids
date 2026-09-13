using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;


namespace Domain
{
    /// <summary>
    /// مشخصات مشتری
    /// </summary>
    public class Personel : Base
    {
        public int? PositionID { get; set; }
        public string?  Name { get; set; }
        public string? LastName { get; set; }
        public int? Gender { get; set; }//enum Gender
        public int? BirthDate { get; set; }
        public string? Mobile { get; set; }
        public string? JsonPicture { get; set; }
        public string? Address { get; set; }
        public string? Description { get; set; }
        #region RelationShip
        public virtual ICollection<RegisterCostRawProductStore>? RegisterCostRawProductStores { get; set; }
        public virtual Position? Position { get; set; }

        #endregion
    }
}
