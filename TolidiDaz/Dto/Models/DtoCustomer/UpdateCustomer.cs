using Dto.Models.Base;
using Dto.Models.DtoCity;
using Dto.Models.DtoCustomerAddress;
using Dto.Models.DtoProvince;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoCustomer
{
    public class UpdateCustomer:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("کدملی(اختیاری)")]
        public string? Mcode { get; set; }
        [DisplayName("نام")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Name { get; set; }
        [DisplayName("نام خانوادگی")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? LastName { get; set; }
        [DisplayName("جنسیت")]
        [Required(ErrorMessage = "| الزامی است")]
        public Gender Gender { get; set; }
        [DisplayName("تاریخ تولد")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? BirthDate { get; set; }

        [RegularExpression(@"^0([0-9]{10})$", ErrorMessage = "| شماره همراه را با فرمت درست وارد نمایید")]
        [StringLength(11, ErrorMessage = "| شماره همراه 11 رقمی می باشد ")]
        [DisplayName("موبایل")]
        public string? Mobile { get; set; }
        [DisplayName("نام کاربری")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? UserName { get; set; }
        [RegularExpression(@"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$", ErrorMessage = "ایمیل را به درستی وارد نمایید")]
        [DisplayName("پست الکترونیکی")]
        public string? Email { get; set; }
        [DisplayName("عکس")]
        public string? JsonPicture { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }

        public ResultCustomerUserInfo? ResultCustomerUserInfo { get; set; } = new ResultCustomerUserInfo();

        public List<UpdateCustomerAddress>? UpdateCustomerAddresss { get; set; } = new List<UpdateCustomerAddress>();

    }
}
