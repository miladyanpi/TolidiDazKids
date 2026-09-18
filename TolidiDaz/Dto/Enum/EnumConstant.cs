using System.Transactions;
using static Dto.Enum.EnumConstant;

namespace Dto.Enum
{

    public class EnumConstant
    {
        #region Gender
        public enum Gender
        {
            NotDefine = 0,
            Man = 1,
            Woman = 2,
        }
        public static List<(int? Id, string Title)> GetListGender()
        {
            List<(int? ID, string Title)> list = new();

            list.Add(((int?)Gender.NotDefine, "نامشخص"));
            list.Add(((int?)Gender.Man, "مرد"));
            list.Add(((int?)Gender.Woman, "زن"));

            return list;
        }
        public static string GetTitleGender(int? value)
        {
            if (!value.HasValue)
                return "نامشخص";

            switch (value)
            {
                case (int)Gender.NotDefine:
                    return "نامشخص";
                case (int)Gender.Man:
                    return "مرد";
                case (int)Gender.Woman:
                    return "زن";
                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region ReminderType
        public enum ReminderType
        {
            /// <summary>
            /// تولد
            /// </summary>
            Birthday = 1,
            /// <summary>
            /// تمدید عضویت
            /// </summary>
            join = 2,
        }
        public static List<(int? Id, string Title)> GetListReminderType()
        {
            List<(int? ID, string Title)> list = new();

            list.Add(((int?)ReminderType.Birthday, "تولد"));
            list.Add(((int?)ReminderType.join, "تمدید عضویت"));

            return list;
        }
        public static string GetTitleReminderType(int? value)
        {
            if (!value.HasValue)
                return "نامشخص";

            switch (value)
            {
                case (int)ReminderType.Birthday:
                    return "تولد";
                case (int)ReminderType.join:
                    return "تمدید عضویت";
                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region MessageType
        public enum MessageType
        {
            success = 1,
            danger = 2,
            info = 3,
            warning = 4,
        }
        public static List<(int? Id, string Title)> GetListMessageType()
        {
            List<(int? ID, string Title)> list = new();

            list.Add(((int?)MessageType.success, "موفق"));
            list.Add(((int?)MessageType.danger, "خطا"));
            list.Add(((int?)MessageType.info, "اطلاعات"));
            list.Add(((int?)MessageType.warning, "هشدار"));

            return list;
        }
        public static string GetTitleMessageType(int? value)
        {
            if (!value.HasValue)
                return "نامشخص";

            switch (value)
            {
                case (int)MessageType.success:
                    return "موفق";
                case (int)MessageType.danger:
                    return "خطا";
                case (int)MessageType.info:
                    return "اطلاعات";
                case (int)MessageType.warning:
                    return "هشدار";
                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region CurrencyUnit
        public enum CurrencyUnit
        {
            /// <summary>
            /// ریال
            /// </summary>
            IRR = 1,
            /// <summary>
            /// تومان
            /// </summary>
            IRT = 2,

        }
        public static List<(int? Id, string Title)> GetListCurrencyUnit()
        {
            List<(int? ID, string Title)> list = new();

            list.Add(((int?)CurrencyUnit.IRR, "ریال"));
            list.Add(((int?)CurrencyUnit.IRT, "تومان"));

            return list;
        }
        public static string GetTitleCurrencyUnit(int? value)
        {
            if (!value.HasValue)
                return "نامشخص";

            switch (value)
            {
                case (int)CurrencyUnit.IRR:
                    return "ریال (IRR)";
                case (int)CurrencyUnit.IRT:
                    return "تومان (IRT)";
                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region ProductExistStatus
        public enum ProductExistStatus
        {
            /// <summary>
            /// موجود
            /// </summary>
            Existent = 1,
            /// <summary>
            /// نا موجود
            /// </summary>
            Nonexistent = 2
        }
        public static List<(int? Id, string Title)> GetListProductExistStatus()
        {
            List<(int? ID, string Title)> list = new();

            list.Add(((int?)ProductExistStatus.Existent, "موجود"));
            list.Add(((int?)ProductExistStatus.Nonexistent, "ناموجود"));

            return list;
        }
        public static string GetTitleProductExistStatus(int? value)
        {
            if (!value.HasValue)
                return "نامشخص";

            switch (value)
            {
                case (int)ProductExistStatus.Existent:
                    return "موجود";
                case (int)ProductExistStatus.Nonexistent:
                    return "ناموجود";
                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region OrderStatus
        public enum OrderStatus
        {
            /// <summary>
            /// همه
            /// </summary>
            All = 0,
            /// <summary>
            /// درحال بررسی
            /// </summary>
            pending = 1,
            /// <summary>
            /// ارسال شده
            /// </summary>
            shipped = 2,
            /// <summary>
            /// تحویل داده شده
            /// </summary>
            delivered = 3,
            /// <summary>
            /// لغو شده
            /// </summary>
            canceled = 4,
            /// <summary>
            /// رزرو شده-در انتظار پرداخت
            /// </summary>
            PendingPayment = 5,

        }
        public static List<(int? Id, string Title)> GetListOrderStatus()
        {
            List<(int? ID, string Title)> list = new();

            list.Add(((int?)OrderStatus.All, "همه"));
            list.Add(((int?)OrderStatus.pending, "درحال بررسی"));
            list.Add(((int?)OrderStatus.shipped, "ارسال شده"));
            list.Add(((int?)OrderStatus.delivered, "تحویل داده شده"));
            list.Add(((int?)OrderStatus.canceled, "لغو شده"));
            list.Add(((int?)OrderStatus.PendingPayment, "رزرو شده - در انتظار پرداخت"));

            return list;
        }
        public static string GetTitleOrderStatus(int? value)
        {
            if (!value.HasValue)
                return "نامشخص";

            switch (value)
            {
                case (int)OrderStatus.All:
                    return "همه";
                case (int)OrderStatus.pending:
                    return "درحال بررسی";
                case (int)OrderStatus.shipped:
                    return "ارسال شده";
                case (int)OrderStatus.delivered:
                    return "تحویل داده شده";
                case (int)OrderStatus.canceled:
                    return "لغو شده";
                case (int)OrderStatus.PendingPayment:
                    return "رزرو شده - در انتظار پرداخت";
                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region PaymentStatus
        public enum PaymentStatus
        {

            /// <summary>
            /// پرداخت شده
            /// </summary>
            paid = 1,
            /// <summary>
            /// ناموفق
            /// </summary>
            failed = 2,
            /// <summary>
            /// رزرو شده، در انتظار پرداخت
            /// </summary>
            PendingPayment = 3,
            /// <summary>
            /// مهلت تمام شده
            /// </summary>
            Expired = 4,
            /// <summary>
            /// بازگشت وجه
            /// </summary>
            Refunded = 5

        }
        public static List<(int? Id, string Title)> GetListPaymentStatus()
        {
            List<(int? ID, string Title)> list = new();

            list.Add(((int?)PaymentStatus.paid, "پرداخت شده"));
            list.Add(((int?)PaymentStatus.failed, "ناموفق"));
            list.Add(((int?)PaymentStatus.PendingPayment, "رزرو شده - در انتظار پرداخت"));
            list.Add(((int?)PaymentStatus.Expired, "مهلت تمام شده"));
            list.Add(((int?)PaymentStatus.Refunded, "بازگشت وجه"));

            return list;
        }
        public static string GetTitlePaymentStatus(int? value)
        {
            if (!value.HasValue)
                return "نامشخص";

            switch (value)
            {
                case (int)PaymentStatus.paid:
                    return "پرداخت شده";
                case (int)PaymentStatus.failed:
                    return "ناموفق";
                case (int)PaymentStatus.PendingPayment:
                    return "رزرو شده - در انتظار پرداخت";
                case (int)PaymentStatus.Expired:
                    return "مهلت تمام شده";
                case (int)PaymentStatus.Refunded:
                    return "بازگشت وجه";
                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region CartStatus
        public enum CartStatus
        {
            /// <summary>
            /// همه
            /// </summary>
            All = 0,

            /// <summary>
            /// فعال
            /// سبد خرید باز امکان حذف و اضافه
            /// </summary>
            active = 1,
            /// <summary>
            /// سبد خرید پرداخت شده و بسته میشه
            /// </summary>
            checked_out = 2,
            /// <summary>
            /// رها شده
            /// یعنی مدت زیادی گذشته
            /// </summary>
            abandoned = 3
        }
        public static List<(int? Id, string Title)> GetListCartStatus()
        {
            List<(int? ID, string Title)> list = new();

            list.Add(((int?)CartStatus.All, "همه"));
            list.Add(((int?)CartStatus.active, "باز"));
            list.Add(((int?)CartStatus.checked_out, "پرداخت شده"));
            list.Add(((int?)CartStatus.abandoned, "رها شده"));

            return list;
        }
        public static string GetTitleCartStatus(int? value)
        {
            if (!value.HasValue)
                return "نامشخص";

            switch (value)
            {
                case (int)CartStatus.All:
                    return "همه";
                case (int)CartStatus.active:
                    return "باز";
                case (int)CartStatus.checked_out:
                    return "پرداخت شده";
                case (int)CartStatus.abandoned:
                    return "رها شده";
                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region EnumJsonImageFileVideo
        public enum EnumJsonImageFileVideo
        {
            Image = 1,
            File = 2,
            Video = 3

        }
        public static List<(int? Id, string Title)> GetListEnumJsonImageFileVideo()
        {
            List<(int? ID, string Title)> list = new();

            list.Add(((int?)EnumJsonImageFileVideo.Image, "تصویر"));
            list.Add(((int?)EnumJsonImageFileVideo.File, "فایل"));
            list.Add(((int?)EnumJsonImageFileVideo.Video, "ویدیو"));

            return list;
        }
        public static string GetTitleEnumJsonImageFileVideo(int? value)
        {
            if (!value.HasValue)
                return "نامشخص";

            switch (value)
            {
                case (int)EnumJsonImageFileVideo.Image:
                    return "تصویر";
                case (int)EnumJsonImageFileVideo.File:
                    return "فایل";
                case (int)EnumJsonImageFileVideo.Video:
                    return "ویدیو";
                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region TransactionKind
        public enum TransactionKind
        {
            /// <summary>
            ///  واریز به کیف پول (مثلاً از شارژ مستقیم)
            /// </summary>
            Deposit = 1,
            /// <summary>
            ///  برداشت از کیف پول (برداشت به حساب بانکی)
            /// </summary>
            Withdrawal = 2,
            /// <summary>
            ///  پرداخت برای خرید (از طریق درگاه یا کیف پول)
            /// </summary>
            Purchase = 3,
            /// <summary>
            ///  برگشت وجه
            /// </summary>
            Refund = 4,
            /// <summary>
            /// کارمزد
            /// </summary>
            Fee = 5,
            /// <summary>
            ///   اصلاح دستی/ادمین
            /// </summary>
            Adjustment = 6
        }
        public static List<(int? Id, string Title)> GetListTransactionKind()
        {
            List<(int? ID, string Title)> list = new();

            list.Add(((int?)TransactionKind.Deposit, "واریز"));
            list.Add(((int?)TransactionKind.Withdrawal, "برداشت"));
            list.Add(((int?)TransactionKind.Purchase, "خرید"));
            list.Add(((int?)TransactionKind.Refund, "برگشت وجه"));
            list.Add(((int?)TransactionKind.Fee, "کارمزد"));
            list.Add(((int?)TransactionKind.Adjustment, "اصلاح دستی"));

            return list;
        }
        public static string GetTitleTransactionKind(int? value)
        {
            if (!value.HasValue)
                return "نامشخص";

            switch (value)
            {
                case (int)TransactionKind.Deposit:
                    return "واریز";
                case (int)TransactionKind.Withdrawal:
                    return "برداشت";
                case (int)TransactionKind.Purchase:
                    return "خرید";
                case (int)TransactionKind.Refund:
                    return "برگشت وجه";
                case (int)TransactionKind.Fee:
                    return "کارمزد";
                case (int)TransactionKind.Adjustment:
                    return "اصلاح دستی";
                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region TransactionStatus
        public enum TransactionStatus
        {
            /// <summary>
            /// در انتظار
            /// </summary>
            Pending = 0,
            /// <summary>
            /// تکمیل شده
            /// </summary>
            Completed = 1,
            /// <summary>
            /// ناموفق
            /// </summary>
            Failed = 2,
            /// <summary>
            /// لغو شده
            /// </summary>
            Cancelled = 3,
            /// <summary>
            /// برگشت داده شده
            /// </summary>
            Reversed = 4
        }
        public static List<(int? Id, string Title)> GetListTransactionStatus()
        {
            List<(int? ID, string Title)> list = new();

            list.Add(((int?)TransactionStatus.Pending, "در انتظار"));
            list.Add(((int?)TransactionStatus.Completed, "تکمیل شده"));
            list.Add(((int?)TransactionStatus.Failed, "ناموفق"));
            list.Add(((int?)TransactionStatus.Cancelled, "لغو شده"));
            list.Add(((int?)TransactionStatus.Reversed, "برگشت داده شده"));

            return list;
        }
        public static string GetTitleTransactionStatus(int? value)
        {
            if (!value.HasValue)
                return "نامشخص";

            switch (value)
            {
                case (int)TransactionStatus.Pending:
                    return "در انتظار";
                case (int)TransactionStatus.Completed:
                    return "تکمیل شده";
                case (int)TransactionStatus.Failed:
                    return "ناموفق";
                case (int)TransactionStatus.Cancelled:
                    return "لغو شده";
                case (int)TransactionStatus.Reversed:
                    return "برگشت داده شده";
                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region PaymentMethod
        public enum PaymentMethod
        {
            /// <summary>
            /// کیف پول
            /// </summary>
            Wallet = 1,
            /// <summary>
            /// درگاه زرین پال
            /// </summary>
            ZarrinPalPaymentGateway = 2,
            Sep = 3,
        }
        public static List<(int? Id, string Title)> GetListPaymentMethod()
        {
            List<(int? ID, string Title)> list = new();

            list.Add(((int?)PaymentMethod.Wallet, "کیف پول"));
            list.Add(((int?)PaymentMethod.ZarrinPalPaymentGateway, "زرین پال"));
            list.Add(((int?)PaymentMethod.Sep, "درگاه SEP"));

            return list;
        }
        public static string GetTitlePaymentMethod(int? value)
        {
            if (!value.HasValue)
                return "نامشخص";

            switch (value)
            {
                case (int)PaymentMethod.Wallet:
                    return "کیف پول";
                case (int)PaymentMethod.ZarrinPalPaymentGateway:
                    return "زرین پال";
                case (int)PaymentMethod.Sep:
                    return "درگاه SEP";
                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region CommentStatus
        public enum CommentStatus
        {
            Pending = 0,
            Approved = 1,
            Rejected = 2
        }
        public static List<(int? Id, string Title)> GetListCommentStatus()
        {
            List<(int? ID, string Title)> list = new();

            list.Add(((int?)CommentStatus.Pending, "در انتظار تایید"));
            list.Add(((int?)CommentStatus.Approved, "تایید شده"));
            list.Add(((int?)CommentStatus.Rejected, "رد شده"));

            return list;
        }
        public static string GetTitleCommentStatus(CommentStatus? value)
        {
            if (!value.HasValue)
                return "<span class='px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-200'>نامشخص</span>";


            switch (value)
            {
                case CommentStatus.Pending:
                    return "<span class='px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-yellow-100 text-yellow-800 dark:bg-yellow-900 dark:text-yellow-200'>در انتظار تایید</span>";

                case CommentStatus.Approved:
                    return "<span class='px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-green-100 text-green-800 dark:bg-green-900 dark:text-green-200'>تایید شده</span>";

                case CommentStatus.Rejected:
                    return "<span class='px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-red-100 text-red-800 dark:bg-red-900 dark:text-red-200'>رد شده</span>";

                default:
                    return "<span class='px-2 inline-flex text-xs leading-5 font-semibold rounded-full bg-gray-100 text-gray-800 dark:bg-gray-700 dark:text-gray-200'>نامشخص</span>";

            }
        }

        #endregion
        #region RuleType
        public enum RuleType
        {
            Quantity = 1,           // فقط بر اساس تعداد خرید
            Group = 2,      // فقط بر اساس نوع مشتری
            QuantityAndGroup = 3,   // ترکیبی: تعداد خرید + نوع مشتری
            Date = 4    // بین تاریخ خاص //جشنواره- تخفیف مناسبتی-فروش فوری
        }
        public static List<(int? Id, string Title)> GetListRuleType()
        {
            List<(int? ID, string Title)> list = new();

            list.Add(((int?)RuleType.Quantity, "براساس تعداد خرید"));
            list.Add(((int?)RuleType.Group, "براساس نوع مشتری"));
            list.Add(((int?)RuleType.QuantityAndGroup, "تعداد خرید + نوع مشتری"));
            list.Add(((int?)RuleType.Date, "بین بازه تاریخی"));

            return list;
        }
        public static string GetTitleRuleType(int? value)
        {
            if (!value.HasValue)
                return "نامشخص";

            switch (value)
            {
                case (int)RuleType.Quantity:
                    return "براساس تعداد خرید";
                case (int)RuleType.Group:
                    return "براساس نوع مشتری";
                case (int)RuleType.QuantityAndGroup:
                    return "تعداد خرید + نوع مشتری";
                case (int)RuleType.Date:
                    return "بین بازه تاریخی";
                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region CostType
        public enum CostType
        {
            /// <summary>
            ///  براساس محصول و تعداد عمل خاصی قیمت گذاری می شود
            /// </summary>
            Product_CountAction = 1,
            /// <summary>
            /// براساس واحد
            /// </summary>
            Unit = 2,
        }
        public static List<(int? Id, string Title)> GetListCostType()
        {
            List<(int? ID, string Title)> costTypes = new List<(int?, string)>();

            costTypes.Add(((int?)CostType.Product_CountAction, "براساس محصول و تعداد کار خاص"));
            costTypes.Add(((int?)CostType.Unit, "براساس واحد"));
            return costTypes;
        }
        public static string GetTitleCostType(int? costType)
        {
            if (!costType.HasValue)
                return "نامشخص";
            switch (costType)
            {
                case (int)CostType.Product_CountAction:
                    return "براساس محصول و تعداد کار خاص";
                case (int)CostType.Unit:
                    return "براساس واحد";

                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region MessurmentType
        public enum MessurmentType
        {
            /// <summary>
            /// کیلوگرم
            /// </summary>
            KG = 1,
            /// <summary>
            /// تعداد
            /// </summary>
            Count = 2,
            /// <summary>
            /// متر
            /// </summary>
            Meter = 3
        }
        public static List<(int ID, string Title)> GetListMessurmentType()
        {
            List<(int, string)> list = new List<(int, string)>();

            list.Add(new((int)MessurmentType.KG, "کیلوگرم"));
            list.Add(new((int)MessurmentType.Count, "تعداد"));
            list.Add(new((int)MessurmentType.Meter, "متر"));
            return list;
        }
        public static string GetTitleMessurmentType(MessurmentType vahenAndazegiri)
        {
            switch (vahenAndazegiri)
            {
                case MessurmentType.KG:
                    return "کیلوگرم";
                case MessurmentType.Count:
                    return "تعداد";
                case MessurmentType.Meter:
                    return "متر";
                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region TicketPriority
        public enum TicketPriority
        {
            Normal = 1,
            Medium = 2,
            Urgent = 3
        }
        public static List<(int? Id, string Title)> GetListTicketPriority()
        {
            return new List<(int?, string)>
            {
                ((int?)TicketPriority.Normal, "عادی"),
                ((int?)TicketPriority.Medium, "متوسط"),
                ((int?)TicketPriority.Urgent, "فوری")
            };
        }
        public static string GetTitleTicketPriority(TicketPriority? priority)
        {
            switch (priority)
            {
                case TicketPriority.Normal:
                    return "عادی";

                case TicketPriority.Medium:
                    return "متوسط";

                case TicketPriority.Urgent:
                    return "فوری";

                default:
                    return "نامشخص";
            }
        }
        #endregion
        #region TraitDisplayType
        public enum TraitDisplayType
        {
            Text = 1,       // متن معمولی (مثل: مدل، حافظه، عیار طلا، جنس)
            Color = 2,      // پالت رنگ (دارای کد هگز)
            Size = 3,       // سایز و ابعاد
            Image = 4       // انتخابی دارای پترن/تصویر (مثل طرح پارچه)
        }

        public static List<(int? Id, string Title)> GetListTraitDisplayType()
        {
            return new List<(int?, string)>
    {
        ((int?)TraitDisplayType.Text, "متن معمولی"),
        ((int?)TraitDisplayType.Color, "پالت رنگ"),
        ((int?)TraitDisplayType.Size, "سایز"),
        ((int?)TraitDisplayType.Image, "تصویر/طرح")
    };
        }

        public static string GetTitleTraitDisplayType(TraitDisplayType? displayType)
        {
            switch (displayType)
            {
                case TraitDisplayType.Text:
                    return "متن معمولی";

                case TraitDisplayType.Color:
                    return "پالت رنگ";

                case TraitDisplayType.Size:
                    return "سایز";

                case TraitDisplayType.Image:
                    return "تصویر / طرح";

                default:
                    return "نامشخص";
            }
        }
        #endregion

    }
}
