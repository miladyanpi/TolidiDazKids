using System;
using System.Globalization;
using System.Text.Unicode;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Utility
{
    public class DateFunctions
    {
        public static int GetDateNow()
        {
            var date = PersianDateExtensionMethods.GetMiladiToPersianDate(DateTime.Now);
            var dt = int.Parse(date.Substring(0, 4) + date.Substring(5, 2) + date.Substring(8, 2));
            return dt;
        }
        public static string GetNewDate()
        {
            return PersianDateExtensionMethods.GetMiladiToPersianDate(DateTime.Now);
        }
        public static int GetYesterdayNow()
        {
            var date = PersianDateExtensionMethods
            .GetMiladiToPersianDate(DateTime.Now.AddDays(-1));

            var dt = int.Parse(date.Substring(0, 4) + date.Substring(5, 2) + date.Substring(8, 2));
            return dt;
        }
        public static int ConvertDateStringToInt(string date)
        {
            if (string.IsNullOrEmpty(date)) return 0;
            var day = ChangePersianNumbersToEnglish(date.Substring(0, 4));
            var month = ChangePersianNumbersToEnglish(date.Substring(5, 2));
            var year = ChangePersianNumbersToEnglish(date.Substring(8, 2));

            var dt = int.Parse(day + month + year);
            return dt;
        }
        //public static string ConvertDateIntToString(int? date)
        //{

        //    string sdate = date.ToString();
        //    var dt = $"{sdate.Substring(0, 4)}/{sdate.Substring(4, 2)}/{sdate.Substring(6, 2)}";
        //    return dt;
        //}
        public static string ConvertDateIntToString(int? date)
        {
            if (!date.HasValue || date == 0)
            {
                return "-"; // یا می‌تونی "" یا "-" یا "تاریخ نامشخص" برگردونی
            }

            string sdate = date.Value.ToString("00000000"); // اطمینان از ۸ رقم بودن (مثلاً 14021230)

            if (sdate.Length < 8)
            {
                return string.Empty;
            }

            var dt = $"{sdate.Substring(0, 4)}/{sdate.Substring(4, 2)}/{sdate.Substring(6, 2)}";
            return dt;
        }
        public static (int Year, int Month, int Day) ConvertDateToYearMonthDay(int date)
        {
            string sdate = date.ToString();
            var year = int.Parse(sdate.Substring(0, 4));
            var month = int.Parse(sdate.Substring(4, 2));
            var day = int.Parse(sdate.Substring(6, 2));
            return (year, month, day);
        }

        public static string AddMonthToDate(int date, int monthCount)
        {
            var result = ConvertDateToYearMonthDay(date);
            var count = monthCount + result.Month;
            var month = count % 12;
            var year = count / 12;
            string smonth = month < 10 ? $"0{month}" : month.ToString();
            string sday = result.Day < 10 ? $"0{result.Day}" : result.Day.ToString();
            return $"{result.Year + year}/{smonth}/{sday}";
        }
        //public static string AddDayToDate(int date, int dayCount)
        //{
        //    var result = ConvertDateToYearMonthDay(date);
        //    int day = 0, month=0;
        //    dayCount += result.Day;
        //    switch (result.Month)
        //    {
        //        case 1:
        //        case 2:
        //        case 3:
        //        case 4:
        //        case 5:
        //        case 6:
        //             day = dayCount % 31;
        //             month = dayCount / 31;
        //            break;
        //        case 7:
        //        case 8:
        //        case 9:
        //        case 10:
        //        case 11:
        //            day = dayCount % 30;
        //            month = dayCount / 30;
        //            break;
        //        case 12:
        //            day = dayCount % 29;
        //            month = dayCount / 29;
        //            break;
        //    }
        //    var count = month + result.Month;
        //     month = count % 12;
        //    var year = count / 12;
        //    string smonth = month < 10 ? $"0{month}" : month.ToString();
        //    string sday = day < 10 ? $"0{day}" : day.ToString();

        //    return $"{result.Year + year}/{smonth}/{sday}";

        //}


        public static string AddDayToDate(int date, int dayCount)
        {
            // تبدیل int به سال/ماه/روز
            int year = date / 10000;
            int month = (date % 10000) / 100;
            int day = date % 100;
            var pc = new PersianCalendar();
            // تبدیل به DateTime
            DateTime dt = pc.ToDateTime(year, month, day, 0, 0, 0, 0);
            // اضافه/کم کردن روز (dayCount منفی = گذشته)
            DateTime resultDate = dt.AddDays(dayCount);
            // برگرداندن به فرمت yyyyMMdd
            int rYear = pc.GetYear(resultDate);
            int rMonth = pc.GetMonth(resultDate);
            int rDay = pc.GetDayOfMonth(resultDate);

            return $"{rYear:0000}/{rMonth:00}/{rDay:00}";
        }
        public static string GetMonthNameOfYear(int month)
        {
            switch (month)
            {
                case 1: return "فروردین";
                case 2: return "اردیبهشت";
                case 3: return "خرداد";
                case 4: return "تیر";
                case 5: return "مرداد";
                case 6: return "شهریور";
                case 7: return "مهر";
                case 8: return "آبان";
                case 9: return "آذر";
                case 10: return "دی";
                case 11: return "بهمن";
                case 12: return "اسفند";
                default: return "نامشخص";
            }
        }
        public static string ChangePersianNumbersToEnglish(string input)
        {
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(persian[j], j.ToString());

            return input;
        }
        public static string ChangeEnglishNumbersToPersian(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };

            for (int j = 0; j < persian.Length; j++)
                input = input.Replace(j.ToString(), persian[j]);

            return input;
        }
    }
}