using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;
namespace  Dto.Models.DtoCustomer
{
    public class ResultCustomerSelect
    {
        public bool Select { get; set; }
        [DisplayName("کدملی")]
        [MinLength(10, ErrorMessage = "| کد ملی 10 رقمی می باشد ")]
        [MaxLength(10, ErrorMessage = "| کد ملی 10 رقمی می باشد ")]
        [Required(ErrorMessage = "| الزامی است")]
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
        public int? BirthDate { get; set; }

        [RegularExpression(@"^0([0-9]{10})$", ErrorMessage = "| شماره همراه را با فرمت درست وارد نمایید")]
        [StringLength(11, ErrorMessage = "| شماره همراه 11 رقمی می باشد ")]
        [DisplayName("موبایل")]
        public string? Mobile { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "| الزامی است")]
        [DisplayName("پست الکترونیکی")]
        [RegularExpression(@"^\w+[\w-\.]*\@\w+((-\w+)|(\w*))\.[a-z]{2,3}$", ErrorMessage = "ایمیل را به درستی وارد نمایید")]
        public string? Email { get; set; }
        [DisplayName("عکس")]
        public string? JsonPicture { get; set; }
        public string? Description { get; set; }

    }
}
