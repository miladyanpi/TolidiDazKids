using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Models
{
    public class AddsmsModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? FromNumber { get; set; }
        public string[]? ToNumbers { get; set; }
        public string? MessageContent { get; set; }
        public bool IsFlash { get; set; } = false;
        public long[]? RecId { get; set; } = null;
        public byte[]? Status { get; set; } = null;
    }
    public class SmsViewModel
    {
        public static string GetSmsResponseMessage(SendSmsReturnType sendSmsReturnType)
        {
            switch (sendSmsReturnType)
            {
                case SendSmsReturnType.None:
                    return "نامشخص";
                case SendSmsReturnType.SendWasSuccessful:
                    return "ارسال با موفقیت انجام شد";
                case SendSmsReturnType.InvalidUserNameOrPassword:
                    return "نام کاربر یا کلمه عبور نامعتبر می باشد";
                case SendSmsReturnType.UserBlocked:
                    return "کاربر مسدود شده است";
                case SendSmsReturnType.InvalidSenderNumber:
                    return "شماره فرستنده نامعتبر است";
                case SendSmsReturnType.LimitationInDailySend:
                    return "محدودیت در ارسال روزانه";
                case SendSmsReturnType.LimitationInRecieverCount:
                    return "تعداد گیرندگان حداکثر 100 شماره می باشد";
                case SendSmsReturnType.SenderLineIsInactive:
                    return "خط فرسنتده غیرفعال است";
                case SendSmsReturnType.SmsContentFilteredWordsIsIncluded:
                    return "متن پیامک شامل کلمات فیلتر شده است";
                case SendSmsReturnType.NoCredit:
                    return "اعتبار کافی نیست";
                case SendSmsReturnType.SystemBeingUpdated:
                    return "سامانه در حال بروز رسانی است";
                case SendSmsReturnType.NotImplemented:
                    return "پیاده سازی نشده است";
                default:
                    return "نامشخص";
            }
        }
        public enum SendSmsReturnType
        {
            [Description("")]
            None = -10,

            [Description("ارسال با موفقیت انجام شد")]
            SendWasSuccessful = 0,

            [Description("نام کاربر یا کلمه عبور نامعتبر می باشد")]
            InvalidUserNameOrPassword = 1,

            [Description("کاربر مسدود شده است")]
            UserBlocked = 2,

            [Description("شماره فرستنده نامعتبر است")]
            InvalidSenderNumber = 3,

            [Description("محدودیت در ارسال روزانه")]
            LimitationInDailySend = 4,

            [Description("تعداد گیرندگان حداکثر 100 شماره می باشد")]
            LimitationInRecieverCount = 5,

            [Description("خط فرسنتده غیرفعال است")]
            SenderLineIsInactive = 6,

            [Description("متن پیامک شامل کلمات فیلتر شده است")]
            SmsContentFilteredWordsIsIncluded = 7,

            [Description("اعتبار کافی نیست")]
            NoCredit = 8,

            [Description("سامانه در حال بروز رسانی است")]
            SystemBeingUpdated = 9,

            [Description("پیاده سازی نشده است")]
            NotImplemented = 10
        }
        //public SmsViewModel(string message, string[] toNumbers)
        //{
        //    AddsmsModel.MessageContent = message;
        //    AddsmsModel.ToNumbers = toNumbers;
        //    AddsmsModel.IsFlash = false;
        //    AddsmsModel.RecId = null;
        //    AddsmsModel.Status = null;
        //}
        //public AddsmsModel AddsmsModel { get; set; }


    }
}
