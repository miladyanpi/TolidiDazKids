using Dto.Models.Base;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace  Dto.Models.DtoSetting
{
    public class UpdateSetting:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("نام فروشگاه")]
        [Required(ErrorMessage = "| الزامی است")]
        public string? Name { get; set; }
        [DisplayName("تعداد رکورد قابل نمایش در جداول")]
        [Required(ErrorMessage = "| الزامی است")]
        public int CountShowRecord { get; set; }
        [DisplayName("تصاویر")]
        public string? JsonPicture { get; set; }
        [DisplayName("شماره فرستنده پیامک")]
        public string? PhoneSender { get; set; }
        [DisplayName("نام کاربری وب سرویس پیامک")]
        public string? UserName { get; set; }
        [DisplayName("رمز عبور وب سرویس پیامک")]
        public string? Password { get; set; }
        [DisplayName("واحد پول")]
        public int? CurrencyUnit { get; set; }
        [DisplayName("آدرس ایمیل")]
        public string? Email { get; set; }
        [DisplayName("تلفن ثابت")]
 
        public string? JsonTel { get; set; }
        [DisplayName("موبایل")]

        public string? JsonMobile { get; set; }
        [DisplayName("آدرس فروشگاه")]
        public string? Address { get; set; }
    }
}
