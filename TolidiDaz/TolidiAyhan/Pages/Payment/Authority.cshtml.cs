using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoOrderPaymentTemp;
using Dto.Models.DtpPayment;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RestSharp;
using Utility;
using static Dto.Enum.PaymentSepStatuseResponse;


namespace TolidiAyhan.Pages.Payment
{
    [IgnoreAntiforgeryToken]
    //[Authorize(Roles = ConstantRoles.CustomerName)]
    public class AuthorityModel : PageModel
    {
        private readonly RootApi<ResponseApiEntity<PaymentSepResult>> _RootApiResponseTokenSep;
        private readonly RootApi<ResponseApiEntity<SearchOrderPaymentTemp>> _RootApiSearchOrderPaymentTemp;

        public AuthorityModel(TokenServiceServer tokenService,RootApi<ResponseApiEntity<PaymentSepResult>> RootApiResponseTokenSep,
            RootApi<ResponseApiEntity<SearchOrderPaymentTemp>> RootApiSearchOrderPaymentTemp)
        {
            _RootApiResponseTokenSep = RootApiResponseTokenSep;
            _RootApiSearchOrderPaymentTemp=RootApiSearchOrderPaymentTemp;
        }
        public PaymentResponse? paymentResponse { get; set; } = new PaymentResponse();

        public async Task<IActionResult> OnPost()
        {
            paymentResponse.MID = Request.Form["MID"];
            paymentResponse.State = Request.Form["State"];
            paymentResponse.Status = int.Parse(Request.Form["Status"]);
            paymentResponse.RRN = Request.Form["RRN"] == string.Empty ? "-" : Request.Form["RRN"];
            paymentResponse.RefNum = Request.Form["RefNum"];
            paymentResponse.ResNum = Request.Form["ResNum"];
            paymentResponse.TerminalId = Request.Form["TerminalId"];
            paymentResponse.TraceNo = Request.Form["TraceNo"];
            paymentResponse.Amount = Request.Form["Amount"];
            paymentResponse.Wage = Request.Form["Wage"];
            paymentResponse.SecurePan = Request.Form["SecurePan"];
            paymentResponse.HashedCardNumber = Request.Form["HashedCardNumber"];
            var date = PersianDateExtensionMethods.GetMiladiToPersianDate(DateTime.Now);
            date = date.Replace("/", "-");
            var Time = (new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second)).ToString();
            var dt = $"{date}   {Time}";
            if (paymentResponse.Status == (int)EnumPaymentSepStatuseResponse.OK)
            {
                try
                {
                    var searchOrderPaymentTemp = new SearchOrderPaymentTemp
                    {
                        ResNum = paymentResponse.ResNum,
                        Amount = paymentResponse.Amount,    
                    };
                    var (resdataOrderPaymentTemp, authorized) = await _RootApiSearchOrderPaymentTemp.RunMethodApi($"OrderPaymentTemps", searchOrderPaymentTemp, method: Method.Post);
                    if(resdataOrderPaymentTemp.Status == ResultMessageApi.Success)
                    {
                        var  (resdata, authorized2) = await _RootApiResponseTokenSep.RunMethodApi($"Payments/Sep/Public/Verify", paymentResponse, method: Method.Post, Token: resdataOrderPaymentTemp.Entity.Token);//
                        if (resdata.Status == ResultMessageApi.Success)
                        {

                            var Message = GetMessagePaymentSepStatuseResponse((EnumPaymentSepStatuseResponse)paymentResponse.Status);
                            return Redirect($"/Payment-Success?Amount={paymentResponse.Amount}&OrderCode={paymentResponse.RRN}&DateOrder={dt}");
                        }
                        else
                        {
                            return Redirect($"/Payment-Faild?Amount={paymentResponse.Amount}&OrderCode={paymentResponse.RRN}&Message={resdata.Message}&DateOrder={dt}");
                        }
                    }
                    else
                    {
                        return Redirect($"/Payment-Faild?Amount={paymentResponse.Amount}&OrderCode={paymentResponse.RRN}&Message=خطایی زخ داده است&DateOrder={dt}");
                    }

                }
                catch (Exception ex)
                {
                    return Redirect($"/Payment-Faild?Amount={paymentResponse.Amount}&OrderCode={paymentResponse.RRN}&Message={ex.Message}&DateOrder={dt}");
                }

            }
            else
            {
                var Message = PaymentSepStatuseResponse.GetMessagePaymentSepStatuseResponse((EnumPaymentSepStatuseResponse)paymentResponse.Status);
                return Redirect($"/Payment-Faild?Amount={paymentResponse.Amount}&OrderCode={paymentResponse.RRN}&Message={Message}&DateOrder={dt}");

            }




        }
        public async Task<IActionResult> OnGet()
        {
           return Redirect($"/Cart");

        }
    }
}
