using Dto.Models.Base;
using Dto.Models.DtoPosition;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using static Dto.Enum.EnumConstant;

namespace  Dto.Models.DtoPersonel
{
    public class ResultPersonel:BaseModel
    {
        public int ID { get; set; }
        [DisplayName("نام")]
        public string? Name { get; set; }
        [DisplayName("نام خانوادگی")]
        public string? LastName { get; set; }
        [DisplayName("پرسنل")]
        public string? FullName { get; set; }
        [DisplayName("جنسیت")]
        public string? Gender { get; set; }
        [DisplayName("تاریخ تولد")]
        public string? BirthDate { get; set; }
        [DisplayName("موبایل")]
        public string? Mobile { get; set; }
        [DisplayName("جایگاه شغلی(سمت)")]
        public int? PositionID { get; set; }
        [DisplayName("عکس")]
        public string? JsonPicture { get; set; }
        [DisplayName("آدرس")]
        public string? Address { get; set; }
        [DisplayName("توضیحات")]
        public string? Description { get; set; }
        public string? ImageUIrl { get; set; }

        public ResultPosition? ResultPosition { get; set; }

    }
}
