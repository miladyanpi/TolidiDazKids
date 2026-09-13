namespace Dto.Models.Constant
{
    public class ResultMessageApi
    {
        public static int SuccessCode= 200;
        public static int ErrorCode = -200;
        public static int SuccessAddCode = 300;
        public static int SuccessDeleteCode = 400;
        public static string AddOk = "اطلاعات با موفقیت ثبت شد";
        public static string AddError = "عملیات با خطا مواجه شد";

        public static string GetOk = "اطلاعات با موفقیت از سرور دریافت شد";
        public static string GetError = "عملیات دریافت اطلاعات از سرور با خطا مواجه شد";

        public static string UpdateOk = "اطلاعات با موفقیت ویرایش شد";
        public static string UpdateError = "عملیات ویرایش با خطا مواجه شد";
        public static string AddCartOk = "محصول مورد نظر به سبد خرید اضافه شد";
        public static string AddFavoritOk = "محصول مورد نظر به لیست علاقه مندی شما اضافه شد";
        public static string DeleteFavoritOk = "محصول مورد نظر از لیست علاقه مندی شما حذف شد";
        public static string subCartOk = "تعداد مورد نظر از سبد خرید کسر شد";
        public static string UserNameMobileReapetly = "شماره موبایل واد شده تکراری است.";
        public static string UserEmailReapetly = "پست الکترونیکی واد شده تکراری است.";
        public static string DeleteFilesErrorNotFound = "مجاز به حذف محصول نیستید";
        public static string DownloadFilesErrorNotPermission = "شما مجاز به دانلود این محصول نیستید";
        public static string DownloadFilesError = "دانلود محصول با خطا مواجه شد";
        public static string ErrorNotFoundOrderDetails = "جزئیات سفارشی یافت نشد";
        public static string DeleteOk = "اطلاعات با موفقیت حذف شد";
        public static string DeleteError = "عملیات حذف با خطا مواجه شد";
        public static string DeleteStatudeError = "بعد از وضعیت ویزیت شده مجاز به حذف نیستید و توسط کاربری پزشک قابل حذف می باشد";
        public static string EditStatudeError = "بعد از وضعیت ویزیت شده مجاز به ویرایش نیستید و توسط کاربری پزشک قابل ویرایش می باشد";
        public static string NotAllowDeleteError = "مجاز به حذف نیستید";
        public static string NotAllowDeleteErrorDocument = "مجاز به حذف نیستید مدارک در این پرونده بارگذاری شده است";
        public static string NotAllowDeleteErrorVisit = "مجاز به حذف نیستید ویزیت برای این پرونده ثبت شده است";
        public static string DateFormatError = "فرمت تاریخ درست وارد نشده است";
        public static string ErrorCartExists = "مجاز به حذف نیستید. ابتدا سبد خرید این کاربر را حذف کنید";
        public static string AddCartErrorExistsInOrder = "این محصول قبلا خریداری شده است";
        public static string ErrorNotAllowRequestAmountISNotValid = "مبلغ حداقل باید 10000 تومان باشد";
        public static string RedirectToZarinPall = "به درگاه زرین پال انتقال یافت";
        public static string RedirectToSepPay = "به درگاه پرداخت انتقال یافت";
        public static string ErrorInRedirectToSepPay = "در دریافت توکن جهت انتقال به درگاه با خطا مواجه شد";
        public static string GetErrorPaymentStatuse = "پرداخت با شکست مواجه شده است";
        public static string GetErrorPaymentExistsTransaction = "این تراکنش قبلا ثبت شده است";
        public static string GetOkPaymentStatuse = "پرداخت به درستی انجام شده است";
        public static string ErrorNotOpenCart = "درخواست شما نامعتبر است. سبد خرید باز وجود ندارد";
        public static string ErrorBadRequest = "درخواست شما نامعتبر است. ";
        public static string ErrorTokenExpired = "توکن شما منقضی شده است";
        public static string GetErrorBalanceWallIsLessAmount = "موجودی کیف شما کمتر از مبلغ سفارش است";
        public static string RedirectToWall = "از طریق کیف پول پرداخت شد";
        public static string ErrorNotAllowRequestOrderExists = "درخواست شما نامعتبر است. این سفارش قبلا ثبت شده است";

        public static string NotAllowError = "به دلیل استفاده این اطلاعات در جداول دیگر مجاز به حذف نیستید";
        public static string NotAllowDeleteSubMenu = "به دلیل وجود زیر گزینه مجاز به حذف نیستید";
        public static string DataRepeatError = "اطلاعات تکراری است قبلا ثبت شده است";
        public static string McodeRepeatError = "کدملی وارد شده تکراری است قبلا ثبت شده است";
        public static string BadRequestError = "ورودی نامعتبر است";
        public static string ManualIdRepeatError = "شماره پرونده وارد شده تکراری است قبلا ثبت شده است";
        public static string DataRepeatOk = "اطلاعات تکراری نیست و می توانید ثبت کنید";
        public static string CountOk = "تعداد رکورد با موفقیت دریافت شد";

        public static string SaveYeaAfterSendYearOk = "سالهای بعد از سال انتخابی آزاد و اطلاعاتی ثبت نشده است می توانید ثبت کنید";
        public static string SaveYeaAfterSendYearError = "سال انتخابی شما قبلا ثبت شده است.رکورد مربوط در آن سالها را ابتدا حذف کنید یا سال انتخابی را تغییر دهید";
        
        public static string ErrorEmptyData = "هیچ اطلاعاتی ثبت نشده است";
        public static string ErrorNotRegisterYear = "اطلاعات سال یافت نشد";
        public static string InvalidModelState = "ورودی نامعتبر است";
        public static string SuccessFishWasPaid = "فیش مورد نظر پرداخت شد";

        public static string LoginOk = "ورود با موفقیت انجام شد";
        public static string LoginError = "نام کاربری یا رمز عبور اشتباه است";

        public static string NotFoundUserError = "چنین کاربری یافت نشده";
        public static string NotFoundRoleError = "نقشی یافت نشده";

        public static string UserNameExistsError = "نام کاربری وارد شده قبلا ثبت شده است";
        public static string UserNotFound = "کاربر مورد نظر یافت نشد";
        public static string ChangePasswordOk = "رمز با موفقیت تغییر یافت";
        public static string ResetPasswordOk = "رمز پیش فرض به 123 تغییر یافت";
        public static string WrongPasswordError = "رمز عبور فعلی وارد شده اشتباه است";
        public static string OneRoleExistsError = "حداقل یک کاربر مدیر باید در سیستم فعال باشد. نمی تواند حذف کنید";
        public static string ErrorExpiredToken = "اعتبار توکن به پایان رسیده است";
        public static string OKExpiredToken = "توکن اعتبار دارد";
        public static string OKExpiredTokenRefresh = "توکن به روز شد و اعتبار دارد";
        public static string UserNameMobileExistsError = "شماره موبایل واد شده تکراری است.";
        public static string DeleteFilesError = "محصولی برای حذف وجود نداشت";
        public static string DeleteFilesSuccess = "محصول با موفقیت حذف شد";
        public static string UploadFilesSuccess = "محصول ها با موفقیت آپلود شد";
        public static string SetDefualError = "اعمال پیش فرض با خطا مواجه شد";
        public static string SetDefualOK = "اعمال پیش فرض با موفقیت انجام شد";
        public static string RoleIsExistsInUser = "این نقش قبلا برای کاربر مورد نظر ثبت شده است";
        public static string RoleAddForUser = "نقش انتخاب شده برای این کاربر  ثبت شد";
        public static string UploadFilesError = "هیچ محصولی آپلود نشد";
        public static string PaymentError = "جمع مبلغ پرداختی از مقدار شهریه بیشتر می باشد. مجاز به ثبت نیستید";
        public static string SendMessageSms = "پیامک با موفقیت ارسال شد";
        public static string ErrorSendMessageSms = "ارسال پیامک با خطا مواجه شد";
        public static string ErrorSetSetting = "پیام ارسال نشد. دربخش تنظیمات اطلاعات مربوط به وب سرویس پیامک را تنظیم کنید";
        public static string ErrorReminderType = "این نوع از پیام قبلا ثبت شده است";
        public static string ErrorSqlConnection = "ارتباط با بانک اطلاعاتی قطع می باشد";
        public static string ErrorSmsCodeNotExists = "کد وارد شده اشتباه است";
        public static string ErrorSmsCodeExpired = "اعتبار کد یکبارمصرف به اتمام رسیده است.  مجددا کد دریافت کنید";
        public static string ErrorSmsCodeSendAfter = "کد یکبارمصرف قبلا برای شما ارسال شده است 2 دقیقه دیگه دوباره تلاش کنید";

        public static string SuccessUploadFile = "محصول آپلود شد";
        public static string ErrorUploadFile = "محصول آپلود شد";
        public static string ErrorVisitDateExists = "در این تاریخ ویزیت ثبت شده است";
        public static string ErrorNotAllowRequest = "درخواست شما نامعتبر است";
        public static string ErrorNotAllowDeleteAddress = "این آدرس قبلا در سفارشهای دیگری استفاده شده است. نمی توانید حذف کنید. میتوانید ویرایش کنید";
        public static string AddCartError = "عملیات افزودن به سبد با خطا مواجه شد";
        public static string AddCartNotExistsCountError = "بیشتر از این تعداد محصول در انبار موجود نیست";
        public static string ErrorDisconnectApi = "ارتباط با وب سرویس قطع می باشد";

        public static string Success = "success";
        public static string Error = "error";
        public static string ErrorShowBoxCode = "ErrorShowBoxCode";
        
        public static string GetCustomDataRepeatErrorMeesageError(string word)
        {
            return $" {word} تکراری است قبلا ثبت شده است";
        }
    }

}
