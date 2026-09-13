//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web;

//namespace Utility
//{
//    /// <summary>
//    /// کلاس بررسی صحت شماره همراه
//    /// </summary>
//    public static class MobileAuthentication
//    {
//        /// <summary>
//        /// تولید کد تصادفی 5 رقمی و ذخیره در کوکی
//        /// </summary>
//        /// <returns></returns>
//        public static int CreateCode(string MobileNumber)
//        {

//            // تولید کد 5 رقمی
//            Random RND = new Random();
//            var Code = RND.Next(minValue: 10000, maxValue: 99999);

//            // ذخیره در کوکی
//            HttpCookie cookie = HttpContext.Current.Request.Cookies["MobileAuthenticationCode"] ?? new HttpCookie("MobileAuthenticationCode");

//            cookie.Value = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{MobileNumber}:{Code}"));
//            cookie.Expires = DateTime.Now.AddMinutes(2);
//            cookie.HttpOnly = true;
//            HttpContext.Current.Response.Cookies.Add(cookie);
//            return Code;
//        }

//        /// <summary>
//        /// بررسی صحت کد ورودی کاربر با کد ارسال شده به شماره همراه
//        /// </summary>
//        /// <param name="Code"></param>
//        /// <returns></returns>
//        public static bool VerifyCode(string MobileNumber, string Code)
//        {

//            // ذخیره در کوکی
//            if (HttpContext.Current.Request.Cookies["MobileAuthenticationCode"] != null)
//            {
//                HttpCookie cookie = HttpContext.Current.Request.Cookies["MobileAuthenticationCode"];
//                string s = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{MobileNumber}:{Code}"));
//                if (s.Equals(cookie.Value))
//                    return true;
//            }
//            return false;
//        }
//    }
//}