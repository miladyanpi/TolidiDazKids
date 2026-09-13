//using RestSharp;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Net;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;


//namespace Utility
//{
//    public class SmsParameterValue
//    {
//        public SmsParameterValue(string to, string text)
//        {
//            SmsWebServiceScheme = "Http";
//            SmsWebServiceHost = "webone-sms.ir";
//            SmsWebServicePort = 8080;
//            SmsWebServicePath = "smsinoutbox/sendsms";
//            UserName = "09118791383";
//            PassWord = "42923";
//            From = "10002147";
//            To = to;
//            Text = text;
//        }
//        public string SmsWebServiceScheme { get; set; }
//        public string SmsWebServiceHost { get; set; }
//        public int SmsWebServicePort { get; set; }
//        public string SmsWebServicePath { get; set; }
//        public string UserName { get; set; }
//        public string PassWord { get; set; }
//        public string From { get; set; }
//        public string To { get; set; }
//        public string Text { get; set; }
//    }
//    public class SMS
//    {
//        public enum SendSmsReturnType
//        {
//            [Description("")]
//            None = -10,

//            [Description("ارسال با موفقیت انجام شد")]
//            SendWasSuccessful = 0,

//            [Description("نام کاربر یا کلمه عبور نامعتبر می باشد")]
//            InvalidUserNameOrPassword = 1,

//            [Description("کاربر مسدود شده است")]
//            UserBlocked = 2,

//            [Description("شماره فرستنده نامعتبر است")]
//            InvalidSenderNumber = 3,

//            [Description("محدودیت در ارسال روزانه")]
//            LimitationInDailySend = 4,

//            [Description("تعداد گیرندگان حداکثر 100 شماره می باشد")]
//            LimitationInRecieverCount = 5,

//            [Description("خط فرسنتده غیرفعال است")]
//            SenderLineIsInactive = 6,

//            [Description("متن پیامک شامل کلمات فیلتر شده است")]
//            SmsContentFilteredWordsIsIncluded = 7,

//            [Description("اعتبار کافی نیست")]
//            NoCredit = 8,

//            [Description("سامانه در حال بروز رسانی است")]
//            SystemBeingUpdated = 9,

//            [Description("پیاده سازی نشده است")]
//            NotImplemented = 10
//        }
//        //public string SendAsync(string SmsWebServiceScheme, string SmsWebServiceHost, int SmsWebServicePort, string SmsWebServicePath, string UserName, string PassWord, string From, string To, string Text)
//        //{
//        //    //SendSMS consms = new SendSMS();
//        //    //string consmsSend = consms.Send_Sms1("username", "password", "09121111111", "test sms");
//        //    //Label1.Text = (consmsSend);

//        //    var sendServiceClient = new SendServiceClient();
//        //    long[] recId = null;
//        //    byte[] status = null;
//        //    var result = sendServiceClient.SendSMS(UserName, PassWord,From, new[] { To },Text, false, ref recId, ref status);

//        //    if (result == (int)SendSmsReturnType.SendWasSuccessful)
//        //    {
//        //        return "ارسال با موفقیت انجام شد";
//        //    }
//        //    else
//        //    {
//        //        return string.Format("ارسال انجام نشد، لطفا کد برگشتی ({0}) وب سرویس را بررسی نمایید", result);
//        //    }
//        //}

//        [Obsolete]
//        public async Task<string> SendAsync(string SmsWebServiceScheme, string SmsWebServiceHost, int SmsWebServicePort, string SmsWebServicePath, string UserName, string PassWord, string From, string To, string Text)
//        {
//            UriBuilder uriBuilder = new UriBuilder
//            {
//                Scheme = SmsWebServiceScheme,
//                Host = SmsWebServiceHost,
//                Path = SmsWebServicePath,
//                Query = $"UserName={UserName}&PassWord={PassWord}&From={From}&To={To}&Text={Text}"
//            };
//            var client = new RestClient(baseUrl: $"{uriBuilder.Uri.Scheme}://{uriBuilder.Uri.Host}");
//            var request = new RestRequest(uriBuilder.Uri.PathAndQuery)
//            {
//                Timeout = TimeSpan.FromSeconds(10).Milliseconds
//            };
//            var response = await client.ExecuteTaskAsync<string>(request);
//            Console.WriteLine($"Status Code :{response.StatusCode}");
//            Console.WriteLine(response.Content);
//            if (response.StatusCode == HttpStatusCode.OK)
//                return response.Content;
//            else
//                return default;
//        }

//        public string Send(string SmsWebServiceScheme, string SmsWebServiceHost, int SmsWebServicePort, string SmsWebServicePath, string UserName, string PassWord, string From, string To, string Text)
//        {
//            UriBuilder uriBuilder = new UriBuilder
//            {
//                Scheme = SmsWebServiceScheme,
//                Host = SmsWebServiceHost,
//                Path = SmsWebServicePath,
//                Query = $"UserName={UserName}&PassWord={PassWord}&From={From}&To={To}&Text={Text}"
//            };
//            var client = new RestClient(baseUrl: $"{uriBuilder.Uri.Scheme}://{uriBuilder.Uri.Host}");
//            var request = new RestRequest(uriBuilder.Uri.PathAndQuery)
//            {
//                Timeout = TimeSpan.FromSeconds(10).Milliseconds
//            };
//            var response = client.Execute(request);
//            Console.WriteLine($"Status Code :{response.StatusCode}");
//            Console.WriteLine(response.Content);
//            if (response.StatusCode == HttpStatusCode.OK)
//                return response.Content;
//            else
//                return string.Empty;
//        }

//        public static bool IsValidPhoneNumber(string number)
//        {
//            return Regex.Match(number, @"(\+98|0)?9\d{9}").Success;
//        }
//    }

//    public class SmsServiceModel
//    {
//        public string UserName { get; set; }
//        public string PassWord { get; set; }
//        public string From { get; set; }
//        public string To { get; set; }
//        public string Text { get; set; }
//    }
//}