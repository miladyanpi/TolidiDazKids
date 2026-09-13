using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Enum
{
    public class PaymentSepStatuseResponse
    {
      public enum EnumPaymentSepStatuseResponse
        {
            CanceledByUser=1,
            OK=2,
            Failed=3,
            SessionIsNull=4,
            InvalidParameters=5,
            MerchantIpAddressIsInvalid=8,
            TokenNotFound=10,
            TokenRequired= 11,
            TerminalNotFound=12,
            MultisettlePolicyErrors=21,
        }
        public static string GetMessagePaymentSepStatuseResponse(EnumPaymentSepStatuseResponse enumPaymentSepStatuseResponse)
        {
            switch(enumPaymentSepStatuseResponse)
            {
                case EnumPaymentSepStatuseResponse.CanceledByUser:
                    return "کاربر انصراف داده است";
                case EnumPaymentSepStatuseResponse.OK:
                    return "پرداخت با موفقیت انجام شد";
                case EnumPaymentSepStatuseResponse.Failed:
                    return "پرداخت انجام نشد";
                case EnumPaymentSepStatuseResponse.SessionIsNull:
                    return "کاربر در بازه زمانی تعیین شده پاسخی ارسال نکرده است";
                case EnumPaymentSepStatuseResponse.InvalidParameters:
                    return "پارامترهای ارسالی نامعتبر است";
                case EnumPaymentSepStatuseResponse.MerchantIpAddressIsInvalid:
                    return "آدرس سرور پذیرنده نامعتبر است (در پرداخت های بر پایه\r\nتوکن)";
                case EnumPaymentSepStatuseResponse.TokenNotFound:
                    return "توکن ارسال شده یافت نشد";
                case EnumPaymentSepStatuseResponse.TokenRequired:
                    return "با این شماره ترمینال فقط تراکنش های توکنی قابل پرداخت\r\nهستند";
                case EnumPaymentSepStatuseResponse.TerminalNotFound:
                    return "شماره ترمینال ارسال شده یافت نشد";
                case EnumPaymentSepStatuseResponse.MultisettlePolicyErrors:
                    return "محدودیت های مدل چند حسابی رعایت نشده";
                default:
                    return "پاسخ نامشخص";

            }
        }
    }
}
