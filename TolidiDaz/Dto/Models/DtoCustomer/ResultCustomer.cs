using Dto.Models.Base;
using Dto.Models.DtoCustomerAddress;
using Dto.Models.DtoProvince;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoCustomer
{
    public class ResultCustomer:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("کدملی")]
        public string? Mcode { get; set; }
        [DisplayName("نام")]
        public string? Name { get; set; }
        [DisplayName("نام خانوادگی")]
        public string? LastName { get; set; }
        [DisplayName("جنسیت")]
        public string? Gender { get; set; }
        [DisplayName("تاریخ تولد")]
        public string? BirthDate { get; set; }

        [DisplayName("موبایل")]
        public string? Mobile { get; set; }

        [DisplayName("پست الکترونیکی")]
        public string? Email { get; set; }
        [DisplayName("عکس")]
        public string? JsonPicture { get; set; }

        public string? UserID { get; set; }
        [DisplayName("نقش ها")]
        public List<(string RoleName,string RoleTitle)>? Roles { get; set; }
        public string? ImageUIrl { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
        [DisplayName("سبد")]
        public int CartCount { get; set; }
        [DisplayName("سفارش")]
        public int OrderCount { get; set; }
        public ResultCustomerUserInfo? ResultCustomerUserInfo { get; set; }=new ResultCustomerUserInfo();
        public List<ResultCustomerAddress>? ResultCustomerAddresss { get; set; }= new List<ResultCustomerAddress>();
    }
}
