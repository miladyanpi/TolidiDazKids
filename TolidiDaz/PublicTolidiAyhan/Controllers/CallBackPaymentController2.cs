//using AutoMapper;
//using Domain;
//using Dto.Enum;
//using Dto.Models;
//using Dto.Models.Constant;
//using Dto.Models.DtoCustomerAddress;
//using Dto.Models.DtoSmsModel;
//using Dto.Models.DtoWallet;
//using Dto.Models.DtoWalletTransaction;
//using Dto.Models.DtpPayment;
//using Dto.Models.ResponseApi;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using PublicTolidiAyhan.Services;
//using ServiceReference1;
//using ServicesLibrary.Services.CartItemSrv;
//using ServicesLibrary.Services.CartSrv;
//using ServicesLibrary.Services.CustomerAddressSrv;
//using ServicesLibrary.Services.CustomerSrv;
//using ServicesLibrary.Services.OrderItemSrv;
//using ServicesLibrary.Services.OrderSrv;
//using ServicesLibrary.Services.PaymentSepService;
//using ServicesLibrary.Services.ProductSrv;
//using ServicesLibrary.Services.SettingSrv;
//using ServicesLibrary.Services.WalletSrv;
//using ServicesLibrary.Services.WalletTransactionSrv;
//using Utility;
//using static Dto.Enum.EnumConstant;
//using static Dto.Enum.PaymentSepStatuseResponse;
//using static Dto.Models.SmsViewModel;

//namespace PublicTolidiAyhan.Controllers
//{
//    [Route("api/")]
//    [ApiController]
//    public class CallBackPaymentController2(
//    IMapper _mapper,
//    ICustomerService _CustomerService,
//    ICartService _CartService,
//    ICartItemService _CartItemService,
//    IOrderService _OrderService,
//    IWalletService _WalletService,
//    IWalletTransactionService _WalletTransactionSService,
//    ICustomerAddressService _CustomerAddressService,
//    IOrderItemService _OrderItemService,
//    ISettingService _SettingService,
//    IProductService _ProductService,
//    VerifyTransactionService _verifyTransactionService

//        ) : ControllerBase
//    {
//        string OrderCode = "-";

//        [HttpPost("Authority")]
//        [HttpGet("Authority")]
//        [IgnoreAntiforgeryToken]
//        [AllowAnonymous]
//        public async Task<IActionResult> Authority(string Res = "")
//        {

//            PaymentResponse paymentResponse = new PaymentResponse();
//            var date = PersianDateExtensionMethods.GetMiladiToPersianDate(DateTime.Now).Replace("/", "-");
//            var time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second).ToString();
//            var dt = $"{date} {time}";
//            var successUrl = $"{ApiLink.PublicSiteBaseUrl}/Payment-Success";
//            var failUrlBase = $"{ApiLink.PublicSiteBaseUrl}/Payment-Faild";
//            try
//            {
//                if (Request.HasFormContentType)
//                {

//                    successUrl = $"{ApiLink.PublicSiteBaseUrl}/Payment-Success?Amount={paymentResponse.Amount}&OrderCode={OrderCode}&DateOrder={dt}";
//                     failUrlBase = $"{ApiLink.PublicSiteBaseUrl}/Payment-Faild?Amount={paymentResponse.Amount}&OrderCode={OrderCode}&DateOrder={dt}";

//                    paymentResponse.State ??= Request.Form["State"];
//                    if (int.TryParse(Request.Form["Status"], out var status))
//                        paymentResponse.Status = status;
//                    paymentResponse.RRN = string.IsNullOrEmpty(Request.Form["RRN"]) ? "-" : Request.Form["RRN"];
//                    paymentResponse.RefNum ??= Request.Form["RefNum"];
//                    paymentResponse.ResNum ??= Request.Form["ResNum"];
//                    paymentResponse.TerminalId ??= Request.Form["TerminalId"];
//                    paymentResponse.TraceNo ??= Request.Form["TraceNo"];
//                    paymentResponse.Amount ??= Request.Form["Amount"];
//                    paymentResponse.Wage ??= Request.Form["Wage"];
//                    paymentResponse.SecurePan ??= Request.Form["SecurePan"];
//                    paymentResponse.HashedCardNumber ??= Request.Form["HashedCardNumber"];
//            }
//                else
//                {
//                    var encodedMessage1 = Uri.EscapeDataString("مراحل پرداخت لغو شد" ?? "پرداخت ناموفق");
//                    var endUrl = $"{failUrlBase}&Message={encodedMessage1}";
//                    return Redirect(endUrl);
//                }


//            }
//            catch (Exception ex)
//            {
//                var errorMsg = Uri.EscapeDataString(ex.Message);
//                return Redirect($"{failUrlBase}&Message={errorMsg}");
//            }
//            if (paymentResponse.Status == (int)EnumPaymentSepStatuseResponse.OK)
//            {
//                try
//                {

//                    var resdata = await GetPaymentsVerify(paymentResponse);
//                    if (resdata != null && resdata.Status == ResultMessageApi.Success)
//                    {
//                        return Redirect(successUrl);
//                    }
//                    var res = await CheckExpiredPayments(paymentResponse);
//                    var encodedMessage1 = Uri.EscapeDataString(resdata.Message ?? "پرداخت ناموفق");
//                    return Redirect($"{failUrlBase}&Message={encodedMessage1}");

//                }
//                catch (Exception ex)
//                {
//                    var res = await CheckExpiredPayments(paymentResponse);
//                    var errorMsg = Uri.EscapeDataString(ex.Message);
//                    return Redirect($"{failUrlBase}&Message={errorMsg}");
//                }
//            }
//            else
//            {
//                var res = await CheckExpiredPayments(paymentResponse);
//                if (res != null)
//                {
//                    var encodedMessage = Uri.EscapeDataString(res.Message ?? "پرداخت ناموفق");
//                    return Redirect($"{failUrlBase}&Message={encodedMessage}");
//                }

//                var message2 = PaymentSepStatuseResponse.GetMessagePaymentSepStatuseResponse(
//                     (EnumPaymentSepStatuseResponse)paymentResponse.Status);

//                var encodedMessage2 = Uri.EscapeDataString(message2 ?? "پرداخت ناموفق");
//                return Redirect($"{failUrlBase}&Message={encodedMessage2}");
//            }
//        }

//        //public async Task<ResponseApiEntity<SearchOrderPaymentTemp>> GetOrderPaymentTemps([FromBody] SearchOrderPaymentTemp @params)
//        //{
//        //    if (!ModelState.IsValid) return new ResponseApiEntity<SearchOrderPaymentTemp>
//        //                                                    (entity: @params,
//        //                                                    status: ResultMessageApi.Error,
//        //                                                    statusCode: ResultMessageApi.ErrorCode,
//        //                                                    message: ResultMessageApi.GetError);

//        //    var OrderPaymentTemps = await _OrderPaymentTempService
//        //                        .FirstOrDefaultAsync(s => s.Amount == @params.Amount && s.ResNum == @params.ResNum);

//        //    if (OrderPaymentTemps == null)
//        //        return new ResponseApiEntity<SearchOrderPaymentTemp>
//        //                                                    (entity: @params,
//        //                                                    status: ResultMessageApi.Error,
//        //                                                    statusCode: ResultMessageApi.ErrorCode,
//        //                                                    message: ResultMessageApi.GetError);
//        //    var mappedOrderPaymentTemps = _mapper.Map<SearchOrderPaymentTemp>(OrderPaymentTemps);

//        //    return new ResponseApiEntity<SearchOrderPaymentTemp>
//        //                                                    (entity: mappedOrderPaymentTemps,
//        //                                                    status: ResultMessageApi.Success,
//        //                                                    statusCode: ResultMessageApi.SuccessCode,
//        //                                                    message: ResultMessageApi.GetOk);

//        //}

//        public async Task<ResponseApiEntity<PaymentSepResult>> CheckExpiredPayments([FromBody] PaymentResponse paymentResponse)
//        {

//            if (!ModelState.IsValid) return new ResponseApiEntity<PaymentSepResult>
//                                                               (entity: new PaymentSepResult(),
//                                                               statusCode: ResultMessageApi.ErrorCode,
//                                                               status: ResultMessageApi.Error,
//                                                               message: ResultMessageApi.ErrorNotAllowRequest);

//            var index = paymentResponse.ResNum.IndexOf("@");
//            var CustomerIdentityCode = paymentResponse.ResNum.Substring(0, index);
//            if (!Guid.TryParse(CustomerIdentityCode, out Guid guid))
//                return new ResponseApiEntity<PaymentSepResult>
//                                                                 (entity: new PaymentSepResult(),
//                                                                 statusCode: ResultMessageApi.ErrorCode,
//                                                                 status: ResultMessageApi.Error,
//                                                                 message: ResultMessageApi.ErrorNotAllowRequest);



//            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.IdentityCode == guid);
//            if (customer == null)
//                return new ResponseApiEntity<PaymentSepResult>
//                                                                 (entity: new PaymentSepResult(),
//                                                                 statusCode: ResultMessageApi.ErrorCode,
//                                                                 status: ResultMessageApi.Error,
//                                                                 message: ResultMessageApi.ErrorNotAllowRequest);
//            var Cart = await _CartService
//                             .FirstOrDefaultAsync(s => s.CustomerID == customer.ID && s.CartStatus == (int)CartStatus.active);
//            if (Cart == null)
//                return new ResponseApiEntity<PaymentSepResult>
//                                                               (entity: new PaymentSepResult(),
//                                                               statusCode: ResultMessageApi.ErrorCode,
//                                                               status: ResultMessageApi.Error,
//                                                               message: ResultMessageApi.ErrorNotAllowRequest);



//            var CartItems = await _CartItemService
//                                 .GetAllAsync(s => s.CartID == Cart.ID);

//            if (CartItems.Count() == 0)
//                return new ResponseApiEntity<PaymentSepResult>
//                                                             (entity: new PaymentSepResult(),
//                                                             statusCode: ResultMessageApi.ErrorCode,
//                                                             status: ResultMessageApi.Error,
//                                                             message: ResultMessageApi.ErrorNotAllowRequest);


//            Int64 SumPrice = Int64.Parse(paymentResponse.Amount) / 10;
//            var order = await _OrderService.FirstOrDefaultAsync(s =>
//                                                              s.CustomerID == customer.ID &&
//                                                              s.TotalAmount == SumPrice &&
//                                                              s.PaymentStatus == PaymentStatus.PendingPayment &&
//                                                              s.OrderStatus == OrderStatus.PendingPayment &&
//                                                              s.ResNum == paymentResponse.ResNum);

//            if (order == null)
//                return new ResponseApiEntity<PaymentSepResult>
//                                                           (entity: new PaymentSepResult(),
//                                                           status: ResultMessageApi.Error,
//                                                           statusCode: ResultMessageApi.ErrorCode,
//                                                           message: "هیچ سفارشی ثبت نشده است. درصورت کسر مبلغ تا 72 ساعت آینده به حساب شما برگشت خواهد شد");
//            OrderCode = order.OrderCode.ToString();
//            var result = await _verifyTransactionService.VerifyTransaction(paymentResponse);
//            if (result.Success is false)
//            {
//                foreach (var item in CartItems)
//                {
//                    item.Product.Count += item.Quantity;
//                    await _ProductService.UpdateAsync(item.Product);
//                }
//                var message = PaymentSepStatuseResponse.GetMessagePaymentSepStatuseResponse(
//                        (EnumPaymentSepStatuseResponse)paymentResponse.Status);

//                order.CardPen = result.TransactionDetail.MaskedPan;
//                order.PaymentStatus = PaymentStatus.failed;
//                order.RefId = result.TransactionDetail.RefNum;
//                order.OrderStatus = OrderStatus.canceled;
//                order.JsonAddress = null;
//                order.StatusDescription = message;
//                var resid = await _OrderService.UpdateAsync(order);
//            }
//            return new ResponseApiEntity<PaymentSepResult>
//                                                            (entity: new PaymentSepResult(),
//                                                            status: ResultMessageApi.Success,
//                                                            statusCode: ResultMessageApi.SuccessCode,
//                                                            message: "کاربر انصراف داده است");
//        }
//        public async Task<ResponseApiEntity<PaymentSepResult>> GetPaymentsVerify([FromBody] PaymentResponse paymentResponse)
//        {

//            if (!ModelState.IsValid) return new ResponseApiEntity<PaymentSepResult>
//                                                               (entity: new PaymentSepResult(),
//                                                               statusCode: ResultMessageApi.ErrorCode,
//                                                               status: ResultMessageApi.Error,
//                                                               message: ResultMessageApi.ErrorNotAllowRequest);


//            var index = paymentResponse.ResNum.IndexOf("@");
//            var CustomerIdentityCode = paymentResponse.ResNum.Substring(0, index);
//            if (!Guid.TryParse(CustomerIdentityCode, out Guid guid))
//                return new ResponseApiEntity<PaymentSepResult>
//                                                                 (entity: new PaymentSepResult(),
//                                                                 statusCode: ResultMessageApi.ErrorCode,
//                                                                 status: ResultMessageApi.Error,
//                                                                 message: ResultMessageApi.ErrorNotAllowRequest);

//            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.IdentityCode == guid);
//            if (customer == null)
//                return new ResponseApiEntity<PaymentSepResult>
//                                                                 (entity: new PaymentSepResult(),
//                                                                 statusCode: ResultMessageApi.ErrorCode,
//                                                                 status: ResultMessageApi.Error,
//                                                                 message: ResultMessageApi.ErrorNotAllowRequest);


//            var Cart = await _CartService
//                              .FirstOrDefaultAsync(s => s.CustomerID == customer.ID && s.CartStatus == (int)CartStatus.active);
//            if (Cart == null)
//                return new ResponseApiEntity<PaymentSepResult>
//                                                               (entity: new PaymentSepResult(),
//                                                               statusCode: ResultMessageApi.ErrorCode,
//                                                               status: ResultMessageApi.Error,
//                                                               message: ResultMessageApi.ErrorNotAllowRequest);
//            var CartItems = await _CartItemService
//                                 .GetAllAsync(s => s.CartID == Cart.ID);

//            if (CartItems.Count() == 0)
//                return new ResponseApiEntity<PaymentSepResult>
//                                                             (entity: new PaymentSepResult(),
//                                                             statusCode: ResultMessageApi.ErrorCode,
//                                                             status: ResultMessageApi.Error,
//                                                             message: ResultMessageApi.ErrorNotAllowRequest);

//            var resultCheckOrder = await _OrderService.FirstOrDefaultAsync(s => s.RefId == paymentResponse.RefNum);
//            if (resultCheckOrder != null)
//            {
//                return new ResponseApiEntity<PaymentSepResult>
//                                                           (entity: new PaymentSepResult(),
//                                                           statusCode: ResultMessageApi.ErrorCode,
//                                                           status: ResultMessageApi.Error,
//                                                           message: ResultMessageApi.ErrorNotAllowRequestOrderExists);
//            }
//            var resultWalletTransaction = await _WalletTransactionSService.FirstOrDefaultAsync(s => s.RefID == paymentResponse.RefNum);
//            if (resultWalletTransaction != null)
//                return new ResponseApiEntity<PaymentSepResult>
//                                                         (entity: new PaymentSepResult(),
//                                                         status: ResultMessageApi.Error,
//                                                         statusCode: ResultMessageApi.ErrorCode,
//                                                         message: ResultMessageApi.GetErrorPaymentExistsTransaction);
//            Int64 SumPrice = Int64.Parse(paymentResponse.Amount) / 10;
//            var order = await _OrderService.FirstOrDefaultAsync(s =>
//                                                              s.CustomerID == customer.ID &&
//                                                              s.TotalAmount == SumPrice &&
//                                                              s.PaymentStatus == PaymentStatus.PendingPayment &&
//                                                              s.OrderStatus == OrderStatus.PendingPayment &&
//                                                              s.ResNum == paymentResponse.ResNum &&
//                                                              s.ExpireAt > DateTime.Now.AddMinutes(-1));
//            if (order == null)
//                return new ResponseApiEntity<PaymentSepResult>
//                                                           (entity: new PaymentSepResult(),
//                                                           status: ResultMessageApi.Error,
//                                                           statusCode: ResultMessageApi.ErrorCode,
//                                                           message: " سفارش شما ثبت نشد. درصورت کسر مبلغ تا 72 ساعت آینده به حساب شما برگشت خواهد شد");

//            OrderCode = order.OrderCode.ToString();

//            var result = await _verifyTransactionService.VerifyTransaction(paymentResponse);

//            if (result.Success is false)
//            {
//                order.CardPen = result.TransactionDetail.MaskedPan;
//                order.PaymentStatus = PaymentStatus.failed;
//                order.RefId = result.TransactionDetail.RefNum;
//                order.OrderStatus = OrderStatus.canceled;
//                order.StatusDescription = result.ResultDescription;
//                var resid = await _OrderService.UpdateAsync(order);

//                return new ResponseApiEntity<PaymentSepResult>
//                                                           (entity: result,
//                                                           status: ResultMessageApi.Error,
//                                                           statusCode: ResultMessageApi.ErrorCode,
//                                                           message: result.ResultDescription);//
//            }


//            long SumFinal = SumPrice - order.Discount;
//            order.CardPen = result.TransactionDetail.MaskedPan;
//            order.PaymentStatus = PaymentStatus.paid;
//            order.RefId = result.TransactionDetail.RefNum;
//            order.OrderStatus = OrderStatus.pending;
//            order.StatusDescription = result.ResultDescription;

//            var address = await _CustomerAddressService.FirstOrDefaultAsync(s => s.CustomerID == customer.ID && s.Default == true);

//            if (address != null)
//            {
//                var model = _mapper.Map<CustomerAddress, ResultCustomerAddress>(address);
//                order.JsonAddress = JsonConvert.SerializeObject(model);
//            }

//            var id = await _OrderService.UpdateAsync(order);
//            if (id == 0)
//            {
//                return new ResponseApiEntity<PaymentSepResult>
//                                                       (entity: result,
//                                                       status: ResultMessageApi.Error,
//                                                       statusCode: ResultMessageApi.ErrorCode,
//                                                       message: " سفارش شما ثبت نشد. درصورت کسر مبلغ تا 72 ساعت آینده به حساب شما برگشت خواهد شد");
//            }
//            Cart.CartStatus = (int)CartStatus.checked_out;
//            await _CartService.UpdateAsync(Cart);

//            var now = DateTime.Now;
//            var date = DateFunctions.GetDateNow();
//            var time = new TimeSpan(now.Hour, now.Minute, now.Second);

//            var orderItems = Cart.CartItems.Select(item => new OrderItem
//            {
//                OrderID = order.ID,
//                Visible = true,
//                ProductID = item.ProductID,
//                PriceAtOrder = item.Product == null ? 0 : item.Product.Price,
//                Quantity = item.Quantity,
//                RegisterDate = date,
//                EditDate = date,
//                RegisterTime = time,
//                EditTime = time,
//                IdentityCode = Guid.NewGuid(),
//            }).ToList();


//            var ResultOrderItem = await _OrderItemService.AddRangeAsync(orderItems);


//            var Wallet = await _WalletService
//                 .FirstOrDefaultAsync(s => s.CustomerID == customer.ID);
//            if (Wallet == null)
//            {
//                AddWallet addWallet = new AddWallet
//                {
//                    Balance = 0,
//                    CustomerID = customer.ID,
//                    GiftCredit = 0,
//                };
//                Wallet = _mapper.Map<Wallet>(addWallet);
//                await _WalletService.AddAsync(Wallet);
//            }
//            AddWalletTransaction addWalletTransaction = new AddWalletTransaction
//            {
//                Amount = SumFinal,
//                AfterBalance = Wallet != null ? Wallet.Balance : 0,
//                Kind = TransactionKind.Purchase,
//                Status = TransactionStatus.Completed,
//                Description = "خرید از طریق درگاه پرداخت",
//                RefID = result.TransactionDetail.RefNum,
//                CardPen = result.TransactionDetail.MaskedPan,
//                WalletID = Wallet.ID,
//            };


//            var mapedWalletTransaction = _mapper.Map<WalletTransaction>(addWalletTransaction);
//            var rescount = await _WalletTransactionSService.AddAsync(mapedWalletTransaction);
//            /// بخش ارسالاطلاع رسانی به فروشنده محصول از طریق پیامک
//            List<string> phonNumbers = new List<string>();
//            VerifySendSmsOtpCode smsModel = new();
//            var set = await _SettingService.GetAllAsync();
//            if (set.Count() > 0)
//            {
//                var settings = set.ToList();
//                phonNumbers.Add("09039637334");
//                SendSMSRequest smsRequest =
//                new(settings[0].UserName,
//                settings[0].Password,
//                settings[0].PhoneSender,
//                phonNumbers.ToArray(),
//                $"محصولی با کد سفارش {result.TransactionDetail.RRN} برای مشتری {customer.Name} {customer.LastName} ثبت سفارش شد. \r\n TolidiAyhan.ir",
//                smsModel.IsFlash,
//                smsModel.RecId,
//                smsModel.Status);
//                SendServiceClient client2 = new SendServiceClient();
//                var sendSMSResponse = await client2.SendSMSAsync(smsRequest);

//                var SmsResponseMessage = SmsViewModel.GetSmsResponseMessage((SendSmsReturnType)sendSMSResponse.SendSMSResult);

//            }
//            return new ResponseApiEntity<PaymentSepResult>
//                                                            (entity: result,
//                                                            status: ResultMessageApi.Success,
//                                                            statusCode: ResultMessageApi.SuccessCode,
//                                                            message: result.ResultDescription);//
//        }
//    }
//}
