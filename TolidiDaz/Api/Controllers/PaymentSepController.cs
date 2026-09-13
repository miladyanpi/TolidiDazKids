using AutoMapper;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoProduct;
using Dto.Models.DtoWallet;
using Dto.Models.DtoWalletTransaction;
using Dto.Models.DtpPayment;
using Dto.Models.ResponseApi;
using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ServicesLibrary.Services;
using ServicesLibrary.Services.CartItemSrv;
using ServicesLibrary.Services.CartSrv;
using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.OrderItemSrv;
using ServicesLibrary.Services.OrderPaymentTempSrv;
using ServicesLibrary.Services.OrderSrv;
using ServicesLibrary.Services.PricingRuleSrv;
using ServicesLibrary.Services.ProductSrv;
using ServicesLibrary.Services.WalletSrv;
using ServicesLibrary.Services.WalletTransactionSrv;
using System.Security.Claims;
using System.Text;
using Utility;
using static Dto.Enum.EnumConstant;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]

    public class PaymentSepController : ControllerBase
    {
        private readonly IOrderPaymentTempService _OrderPaymentTempService;
        private readonly IRecurringJobManager _recurringJobManager;

        private readonly IWalletService _WalletService;
        private readonly IWalletTransactionService _WalletTransactionSService;
        private readonly IOrderService _OrderService;
        private readonly IOrderItemService _OrderItemService;
        private readonly ICustomerService _CustomerService;
        private readonly ICartService _CartService;
        private readonly ICartItemService _CartItemService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly IPricingRuleService _pricingRuleService;
        private readonly IProductService _productService;
        private readonly UserManager<Account> _userManager;
        public PaymentSepController(
            IRecurringJobManager recurringJobManager,
            IWalletService WalletService,
            IWalletTransactionService WalletTransactionSService,
            IOrderService OrderService,
            IOrderItemService OrderItemService,
            ICustomerService CustomerService,
            ICartItemService CartItemService,
            ICartService CartService,
            IMapper mapper,
            IHttpClientFactory httpClientFactory,
            IOrderPaymentTempService OrderPaymentTempService,
            IPricingRuleService pricingRuleService,
            IProductService productService,
            UserManager<Account> userManager)
        {
            _recurringJobManager = recurringJobManager;
            _WalletService = WalletService;
            _WalletTransactionSService = WalletTransactionSService;
            _CustomerService = CustomerService;
            _OrderService = OrderService;
            _CartItemService = CartItemService;
            _CartService = CartService;
            _OrderItemService = OrderItemService;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _userManager = userManager;
            _OrderPaymentTempService = OrderPaymentTempService;
            _pricingRuleService = pricingRuleService;
            _productService = productService;

        }
        [HttpGet("Payments/Sep/GetAuthorityForAddAmountToWallet/Public")]
        public async Task<IActionResult> PaymentsAddAmountToWallet([FromQuery] Int64 Amount)
        {
            if (!ModelState.IsValid) return NotFound(new ResponseApiEntity<ZarinpalResponse>
                                                               (entity: new ZarinpalResponse(),
                                                               statusCode: ResultMessageApi.ErrorCode,
                                                               status: ResultMessageApi.Error,
                                                               message: ResultMessageApi.ErrorNotAllowRequest));
            //if (Amount < 1000)
            //    return NotFound(new ResponseApiEntity<ZarinpalResponse>
            //                                                   (entity: new ZarinpalResponse(),
            //                                                   statusCode: ResultMessageApi.ErrorCode,
            //                                                   status: ResultMessageApi.Error,
            //                                                   message: ResultMessageApi.ErrorNotAllowRequestAmountISNotValid));
            var CustomerID = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

            var UniqCode = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<ZarinpalResponse>
                                                                 (entity: new ZarinpalResponse(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

           
            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<ZarinpalResponse>
                                                                 (entity: new ZarinpalResponse(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var info =  _userManager.Users.Where(s => s.CustomerID == int.Parse(CustomerID)).FirstOrDefault();
            if (info == null)
                return BadRequest(new ResponseApiEntity<ZarinpalResponse>
                                                            (entity: new ZarinpalResponse(),
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            status: ResultMessageApi.Error,
                                                            message: ResultMessageApi.ErrorNotAllowRequest));

            var client = _httpClientFactory.CreateClient();
            var maxOrderCode = await _OrderService.GetMaxAsync<long>(s => s.OrderCode);



            var url = "https://payment.zarinpal.com/pg/v4/payment/request.json";
            var requestBody = new
            {
                merchant_id = PaymentConstant.merchant_id,
                amount = Amount.ToString(),
                currency = "IRT",
                description = $"واریز به کیف پول توسط {customer.Name} {customer.LastName} - {customer.Mcode}",
                callback_url = "https://api.tolidyahan.ir/Payments/",
                metadata = new
                {
                    mobile = info.PhoneNumber,
                    email = info.Email,
                    order_id = $"Wallet-{CustomerID}".ToString(),
                },
                mobile = info.PhoneNumber,
                email = info.Email,
                order_id = $"Wallet-{CustomerID}".ToString(),
            };
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            var responseText = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ZarinpalResponse>(responseText);

            return Ok(new ResponseApiEntity<ZarinpalResponse>
                                                            (entity: result,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.RedirectToZarinPall));

            //var authority = result.Data.Authority;
            //var redirectUrl = $"https://payment.zarinpal.com/pg/StartPay/{authority}";
            //return Redirect(redirectUrl);
        }
        [HttpGet("Payments/Sep/AddAmountToWallet/Statuse")]
        public async Task<IActionResult> GetStatusePaymentsAddAmountToWallet(string Authority, Int64 Amount)
        {
            if (!ModelState.IsValid) return NotFound(new ResponseApiEntity<VerifyPayment>
                                                               (entity: new VerifyPayment(),
                                                               statusCode: ResultMessageApi.ErrorCode,
                                                               status: ResultMessageApi.Error,
                                                               message: ResultMessageApi.ErrorNotAllowRequest));
            var CustomerID = User.Claims
                     .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

            var UniqCode = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<VerifyPayment>
                                                                 (entity: new VerifyPayment(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<VerifyPayment>
                                                                 (entity: new VerifyPayment(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var info =  _userManager.Users.Where(s => s.CustomerID == int.Parse(CustomerID)).FirstOrDefault();
            if (info == null)
                return BadRequest(new ResponseApiEntity<VerifyPayment>
                                                            (entity: new VerifyPayment(),
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            status: ResultMessageApi.Error,
                                                            message: ResultMessageApi.ErrorNotAllowRequest));




            var client = _httpClientFactory.CreateClient();

            var url = "https://payment.zarinpal.com/pg/v4/payment/verify.json";
            var requestBody = new
            {
                merchant_id = PaymentConstant.merchant_id,
                authority = Authority,
                amount = Amount.ToString(),
            };
            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            var responseText = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<VerifyPayment>(responseText);

            if (result.errors is JObject errObj)
            {
                //var message = errObj["message"]?.ToString();
                //var code = errObj["code"]?.ToString();
                return NotFound(new ResponseApiEntity<VerifyPayment>
                                                           (entity: result,
                                                           status: ResultMessageApi.Error,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           message: ResultMessageApi.GetErrorPaymentStatuse));
            }

            var Wallet = await _WalletService
                   .FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID));
            if (Wallet == null)
            {
                AddWallet addWallet = new AddWallet
                {
                    Balance = Amount,
                    CustomerID = int.Parse(CustomerID),
                    GiftCredit = 0,
                };
                Wallet = _mapper.Map<Wallet>(addWallet);
                await _WalletService.AddAsync(Wallet);
            }
            var resultWalletTransaction = await _WalletTransactionSService.FirstOrDefaultAsync(s => s.RefID == result.data.ref_id.ToString());
            if (resultWalletTransaction != null)
                return NotFound(new ResponseApiEntity<VerifyPayment>
                                                         (entity: result,
                                                         status: ResultMessageApi.Error,
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         message: ResultMessageApi.GetErrorPaymentExistsTransaction));


            AddWalletTransaction addWalletTransaction = new AddWalletTransaction
            {
                Amount = Amount,
                AfterBalance = Wallet.Balance + Amount,
                Kind = TransactionKind.Deposit,
                Status = TransactionStatus.Completed,
                Description = "واریز به کیف پول",
                RefID = result.data.ref_id.ToString(),
                CardPen = result.data.card_pan,
                WalletID = Wallet.ID,

            };
            var mapedWalletTransaction = _mapper.Map<WalletTransaction>(addWalletTransaction);
            var rescount = await _WalletTransactionSService.AddAsync(mapedWalletTransaction);
            if (rescount > 0)
            {
                Wallet.Balance += Amount;
                _ = await _WalletService.UpdateAsync(Wallet);
            }
            return Ok(new ResponseApiEntity<VerifyPayment>
                                                       (entity: result,
                                                       status: ResultMessageApi.Success,
                                                       statusCode: ResultMessageApi.SuccessCode,
                                                       message: ResultMessageApi.GetOkPaymentStatuse));


        }

        public async Task ClearOrderExpiredPayment(string JobId)
        {
            var orders = await _OrderService.GetAllAsync(s =>
                                                         s.PaymentStatus == PaymentStatus.PendingPayment &&
                                                         s.OrderStatus == OrderStatus.PendingPayment &&
                                                         s.ExpireAt < DateTime.Now);
            foreach(var order in orders)
            {
                var Cart = await _CartService
                                   .FirstOrDefaultAsync(s => s.CustomerID == order.CustomerID && s.CartStatus == (int)CartStatus.active);
                if (Cart != null)
                {
                    var CartItems = await _CartItemService
                                   .GetAllAsync(s => s.CartID == Cart.ID);
                    foreach (var item in CartItems)
                    {
                        item.Product.Count += item.Quantity;
                        await _productService.UpdateAsync(item.Product);
                    }
                }
                var id = await _OrderService.DeleteAsync(order.ID);

            }
            _recurringJobManager.RemoveIfExists(JobId);


        }
        //[HttpGet("BackJob")]
        //[AllowAnonymous]
        //public async Task<IActionResult> BackJob()
        //{
        //    var dateTime = DateTime.Now.AddMinutes(1);
        //    var univDateTime = dateTime.ToUniversalTime();
        //    var id = Guid.NewGuid().ToString();
        //    _recurringJobManager.AddOrUpdate(id, () => ClearOrderExpiredPayment(id), $"{univDateTime.Minute} * * * *");
        //    return Ok(new ResponseApiEntity<ResponseTokenSep>
        //                                                               (entity: new ResponseTokenSep(),
        //                                                               statusCode: ResultMessageApi.SuccessCode,
        //                                                               status: ResultMessageApi.Success,
        //                                                               message: "Running!"));
        //}
        [HttpGet("Payments/Sep/Public")]
        public async Task<IActionResult> Payments([FromQuery] PaymentMethod paymentMethod)
        {
            try
            {
                if (!ModelState.IsValid) return NotFound(new ResponseApiEntity<ResponseTokenSep>
                                                                   (entity: new ResponseTokenSep(),
                                                                   statusCode: ResultMessageApi.ErrorCode,
                                                                   status: ResultMessageApi.Error,
                                                                   message: ResultMessageApi.ErrorNotAllowRequest));
                var CustomerID = User.Claims
                         .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

                var UniqCode = User.Claims
                           .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
                if (!Guid.TryParse(UniqCode, out Guid guid))
                    return BadRequest(new ResponseApiEntity<ResponseTokenSep>
                                                                     (entity: new ResponseTokenSep(),
                                                                     statusCode: ResultMessageApi.ErrorCode,
                                                                     status: ResultMessageApi.Error,
                                                                     message: ResultMessageApi.ErrorNotAllowRequest));


                var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
                if (customer == null)
                    return BadRequest(new ResponseApiEntity<ResponseTokenSep>
                                                                     (entity: new ResponseTokenSep(),
                                                                     statusCode: ResultMessageApi.ErrorCode,
                                                                     status: ResultMessageApi.Error,
                                                                     message: ResultMessageApi.ErrorNotAllowRequest));

                var info =  _userManager.Users.Where(s => s.CustomerID == int.Parse(CustomerID)).FirstOrDefault();
                if (info == null)
                    return BadRequest(new ResponseApiEntity<ResponseTokenSep>
                                                                (entity: new ResponseTokenSep(),
                                                                statusCode: ResultMessageApi.ErrorCode,
                                                                status: ResultMessageApi.Error,
                                                                message: ResultMessageApi.ErrorNotAllowRequest));

                var Cart = await _CartService
                                  .FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID) && s.CartStatus == (int)CartStatus.active);
                if (Cart == null)
                    return NotFound(new ResponseApiEntity<ResponseTokenSep>
                                                                   (entity: new ResponseTokenSep(),
                                                                   statusCode: ResultMessageApi.ErrorCode,
                                                                   status: ResultMessageApi.Error,
                                                                   message: ResultMessageApi.ErrorNotOpenCart));


                var CartItems = await _CartItemService
                                     .GetAllAsync(s => s.CartID == Cart.ID);
                string CountMessage = string.Empty;
                bool flag = false;
                foreach (var item in CartItems)
                {
                    if (item.Quantity > item.Product.Count)
                    {
                        CountMessage += $"<h6>  {item.Product.Title}=>تعداد سبد خرید({item.Quantity}) بیش از موجودی انبار ({item.Product.Count})</h6><br>";
                        flag = true;
                    }
                }
                if (flag)
                {
                    CountMessage += "<h6 style='text-align:center'>تعداد سبد خرید را اصلاح کنید</h6>";
                    return BadRequest(new ResponseApiEntity<ResponseTokenSep>
                                                               (entity: new ResponseTokenSep(),
                                                               statusCode: ResultMessageApi.ErrorCode,
                                                               status: ResultMessageApi.Error,
                                                               message: CountMessage));
                }

                if (CartItems.Count() == 0)
                    return BadRequest(new ResponseApiEntity<ResponseTokenSep>
                                                                 (entity: new ResponseTokenSep(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


                var listCartItems = CartItems.ToList();
                var roles = await _userManager.GetRolesAsync(info);
                Int64 SumPrice = 0;
                foreach (var item in listCartItems)
                {
                    var resultProduct = _mapper.Map<ResultProduct>(item.Product);

                    var Price = _pricingRuleService.CalculatePrice(resultProduct, item.Quantity, roles.ToList(), true);
                    SumPrice += Price * item.Quantity;
                }
                //var SumPrice = listCartItems.Count > 0 ? listCartItems.Sum(s => s.Product.Price * s.Quantity) : 0;
                var SumDscount = listCartItems.Count > 0 ? listCartItems.Sum(s => s.Product.Discount * s.Quantity) : 0;
                var SumPriceFinish = SumPrice - SumDscount;
                //if (SumPriceFinish < 1000)
                //    return NotFound(new ResponseApiEntity<ResponseTokenSep>
                //                                                   (entity: new ResponseTokenSep(),
                //                                                   statusCode: ResultMessageApi.ErrorCode,
                //                                                   status: ResultMessageApi.Error,
                //                                                   message: ResultMessageApi.ErrorNotAllowRequestAmountISNotValid));


                if (paymentMethod == PaymentMethod.Sep)
                {
                    var client = _httpClientFactory.CreateClient();
                    var url = "https://sep.shaparak.ir/onlinepg/onlinepg";

                    var maxOrderCode = (new Random()).Next(11111111, 99999999);
                    var OrderCode = long.Parse($"{Cart.CustomerID}{(long)maxOrderCode + 1}");

                    var guidResNum = $"{customer.IdentityCode}@{OrderCode}";
                 
                    var requestBody = new
                    {
                        action = "token",
                        TerminalId = PaymentConstant.TerminalId,
                        RedirectUrl = $"{ApiLink.PublicSiteBaseUrl}/Authority?Res={guidResNum}",
                        ResNum = guidResNum,
                        Amount = SumPriceFinish * 10,
                        CellNumber = info.PhoneNumber,
                    };
                    var json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, content);
                    var responseText = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeObject<ResponseTokenSep>(responseText);


                    if (result.status == 1)
                    {
                        foreach (var item in listCartItems)
                        {
                            item.Product.Count = item.Product.Count - item.Quantity;
                            await _productService.UpdateAsync(item.Product);
                        }
                        var order = new Order
                        {
                            CustomerID = Cart.CustomerID,
                            CardPen = null,
                            Discount = SumDscount,
                            FinalAmount = SumPriceFinish,
                            OrderCode = OrderCode,
                            Visible = true,
                            PaymentStatus = PaymentStatus.PendingPayment,
                            RefId = null,
                            ResNum= guidResNum,
                            OrderStatus = OrderStatus.PendingPayment,
                            TotalAmount = SumPrice,
                            SendProductMethodID = Cart.SendProductMethodID,
                            IdentityCode = Guid.NewGuid(),
                            ExpireAt = DateTime.Now.AddMinutes(10),//چون زمان درگاه پرداخت 10 دقیقه است

                            RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                            EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                            RegisterDate = DateFunctions.GetDateNow(),
                            EditDate = DateFunctions.GetDateNow(),

                        };
                        var id = await _OrderService.AddAsync(order);
                        var dateTime = DateTime.Now.AddMinutes(11);
                        var univDateTime = dateTime.ToUniversalTime();
                        _recurringJobManager.AddOrUpdate($"Order_{id.ToString()}", () => ClearOrderExpiredPayment($"Order_{id.ToString()}"), $"{univDateTime.Minute} * * * *");

                        //string authHeader = HttpContext.Request.Headers["Authorization"].ToString();
                        //string token = string.Empty;
                        //if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                        //{
                        //    token = authHeader.Substring("Bearer ".Length).Trim();
                        //}
                        //var addOrderPaymentTemp = new AddOrderPaymentTemp
                        //{
                        //    Amount = requestBody.Amount.ToString(),
                        //    ResNum = requestBody.ResNum,
                        //    Token = token,
                        //};
                        //  var model = _mapper.Map<AddOrderPaymentTemp, OrderPaymentTemp>(addOrderPaymentTemp);
                        //  await _OrderPaymentTempService.AddAsync(model);
                        return Ok(new ResponseApiEntity<ResponseTokenSep>
                                                                   (entity: result,
                                                                   status: ResultMessageApi.Success,
                                                                   statusCode: ResultMessageApi.SuccessCode,
                                                                   message: ResultMessageApi.RedirectToSepPay));
                    }
                    else
                    {
                        return BadRequest(new ResponseApiEntity<ResponseTokenSep>
                                                                  (entity: result,
                                                                  status: ResultMessageApi.Error,
                                                                  statusCode: ResultMessageApi.ErrorCode,
                                                                  message: ResultMessageApi.ErrorInRedirectToSepPay));
                    }

                }
                else
                {
                    var Wallet = await _WalletService.FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID));
                    if (Wallet == null)
                    {
                        AddWallet addWallet = new AddWallet
                        {
                            Balance = 0,
                            CustomerID = int.Parse(CustomerID),
                            GiftCredit = 0,
                        };
                        Wallet = _mapper.Map<Wallet>(addWallet);
                        await _WalletService.AddAsync(Wallet);
                    }

                    ResponseTokenSep ResponseTokenSep = new ResponseTokenSep();
                    if (SumPriceFinish > Wallet.Balance)
                        return BadRequest(new ResponseApiEntity<ResponseTokenSep>
                                                               (entity: ResponseTokenSep,
                                                               status: ResultMessageApi.Error,
                                                               statusCode: ResultMessageApi.ErrorCode,
                                                               message: ResultMessageApi.GetErrorBalanceWallIsLessAmount));
                    #region Order
                    Cart.CartStatus = (int)CartStatus.checked_out;
                    await _CartService.UpdateAsync(Cart);
                    var price = Cart.CartItems.Sum(s => s.Product.Price * s.Quantity);
                    var order = new Order
                    {
                        CustomerID = Cart.CustomerID,
                        CardPen = "کیف پول",
                        Discount = 0,
                        FinalAmount = price,
                        OrderCode = long.Parse(DateTime.UtcNow.ToString("yyyyMMddHHmmssfff")),
                        Visible = true,
                        PaymentStatus = PaymentStatus.paid,
                        RefId = string.Empty,
                        OrderStatus = OrderStatus.delivered,
                        TotalAmount = price,
                        IdentityCode = Guid.NewGuid(),
                        RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                        EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                        RegisterDate = DateFunctions.GetDateNow(),
                        EditDate = DateFunctions.GetDateNow(),
                    };
                    var id = await _OrderService.AddAsync(order);
                    var now = DateTime.Now;
                    var date = DateFunctions.GetDateNow();
                    var time = new TimeSpan(now.Hour, now.Minute, now.Second);
                    var orderItems = Cart.CartItems.Select(item => new OrderItem
                    {
                        OrderID = id,
                        Visible = true,
                        ProductID = item.ProductID,
                        PriceAtOrder = item.Product.Price,
                        Quantity = item.Quantity,
                        RegisterDate = date,
                        EditDate = date,
                        RegisterTime = time,
                        EditTime = time,
                        IdentityCode = Guid.NewGuid()
                    }).ToList();

                    _ = await _OrderItemService.AddRangeAsync(orderItems);

                    #endregion
                    AddWalletTransaction addWalletTransaction = new AddWalletTransaction
                    {
                        Amount = SumPriceFinish,
                        AfterBalance = Wallet.Balance - SumPriceFinish,
                        Kind = TransactionKind.Withdrawal,
                        Status = TransactionStatus.Completed,
                        Description = "خرید از کیف پول",
                        RefID = string.Empty,
                        CardPen = "کیف پول",
                        WalletID = Wallet.ID,

                    };
                    var mapedWalletTransaction = _mapper.Map<WalletTransaction>(addWalletTransaction);
                    var rescount = await _WalletTransactionSService.AddAsync(mapedWalletTransaction);
                    if (rescount > 0)
                    {
                        Wallet.Balance -= SumPriceFinish;
                        _ = await _WalletService.UpdateAsync(Wallet);
                    }

                    return Ok(new ResponseApiEntity<ResponseTokenSep>
                                                                  (entity: ResponseTokenSep,
                                                                  status: ResultMessageApi.Success,
                                                                  statusCode: ResultMessageApi.SuccessCode,
                                                                  message: ResultMessageApi.RedirectToWall));
                }

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<ResponseTokenSep>
                                                            (entity: null,
                                                            status: ResultMessageApi.Error,
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            message: ex.Message));
            }
        }

        //[HttpPost("Payments/Sep/Public/Verify")]
        //public async Task<IActionResult> GetPaymentsVerify([FromBody] PaymentResponse paymentResponse)
        //{

        //    if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<PaymentSepResult>
        //                                                       (entity: new PaymentSepResult(),
        //                                                       statusCode: ResultMessageApi.ErrorCode,
        //                                                       status: ResultMessageApi.Error,
        //                                                       message: ResultMessageApi.ErrorNotAllowRequest));


        //    var CustomerID = User.Claims
        //             .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

        //    var UniqCode = User.Claims
        //               .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
        //    if (!Guid.TryParse(UniqCode, out Guid guid))
        //        return BadRequest(new ResponseApiEntity<PaymentSepResult>
        //                                                         (entity: new PaymentSepResult(),
        //                                                         statusCode: ResultMessageApi.ErrorCode,
        //                                                         status: ResultMessageApi.Error,
        //                                                         message: ResultMessageApi.ErrorNotAllowRequest));



        //    var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
        //    if (customer == null)
        //        return NotFound(new ResponseApiEntity<PaymentSepResult>
        //                                                         (entity: new PaymentSepResult(),
        //                                                         statusCode: ResultMessageApi.ErrorCode,
        //                                                         status: ResultMessageApi.Error,
        //                                                         message: ResultMessageApi.ErrorNotAllowRequest));


        //    var info = await _userManager.Users.Where(s => s.CustomerID == int.Parse(CustomerID)).FirstOrDefaultAsync();
        //    if (info == null)
        //        return NotFound(new ResponseApiEntity<PaymentSepResult>
        //                                                    (entity: new PaymentSepResult(),
        //                                                    statusCode: ResultMessageApi.ErrorCode,
        //                                                    status: ResultMessageApi.Error,
        //                                                    message: ResultMessageApi.ErrorNotAllowRequest));


        //    var Cart = await _CartService
        //                      .FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID) && s.CartStatus == (int)CartStatus.active);
        //    if (Cart == null)
        //        return NotFound(new ResponseApiEntity<PaymentSepResult>
        //                                                       (entity: new PaymentSepResult(),
        //                                                       statusCode: ResultMessageApi.ErrorCode,
        //                                                       status: ResultMessageApi.Error,
        //                                                       message: ResultMessageApi.ErrorNotAllowRequest));



        //    var CartItems = await _CartItemService
        //                         .GetAllAsync(s => s.CartID == Cart.ID);

        //    if (CartItems.Count() == 0)
        //        return NotFound(new ResponseApiEntity<PaymentSepResult>
        //                                                     (entity: new PaymentSepResult(),
        //                                                     statusCode: ResultMessageApi.ErrorCode,
        //                                                     status: ResultMessageApi.Error,
        //                                                     message: ResultMessageApi.ErrorNotAllowRequest));

        //    var client = _httpClientFactory.CreateClient();

        //    var url = "https://sep.shaparak.ir/verifyTxnRandomSessionkey/ipg/VerifyTransaction";
        //    var requestBody = new
        //    {
        //        RefNum = paymentResponse.RefNum,
        //        TerminalNumber = long.Parse(paymentResponse.TerminalId),
        //    };
        //    var json = JsonConvert.SerializeObject(requestBody);
        //    var content = new StringContent(json, Encoding.UTF8, "application/json");


        //    var response = await client.PostAsync(url, content);
        //    var responseText = await response.Content.ReadAsStringAsync();

        //    var result = JsonConvert.DeserializeObject<PaymentSepResult>(responseText);


        //    if (result.Success is false)
        //    {
        //        //var message = errObj["message"]?.ToString();
        //        //var code = errObj["code"]?.ToString();
        //        return NotFound(new ResponseApiEntity<PaymentSepResult>
        //                                                   (entity: result,
        //                                                   status: ResultMessageApi.Error,
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   message: result.ResultDescription));
        //    }


        //    var resultCheckOrder = await _OrderService.FirstOrDefaultAsync(s => s.RefId == result.TransactionDetail.RefNum);
        //    if (resultCheckOrder != null)
        //    {
        //        return NotFound(new ResponseApiEntity<PaymentSepResult>
        //                                                   (entity: result,
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.ErrorNotAllowRequestOrderExists));
        //    }


        //    var resultWalletTransaction = await _WalletTransactionSService.FirstOrDefaultAsync(s => s.RefID == result.TransactionDetail.RefNum);
        //    if (resultWalletTransaction != null)
        //        return NotFound(new ResponseApiEntity<PaymentSepResult>
        //                                                 (entity: result,
        //                                                 status: ResultMessageApi.Error,
        //                                                 statusCode: ResultMessageApi.ErrorCode,
        //                                                 message: ResultMessageApi.GetErrorPaymentExistsTransaction));





        //    Cart.CartStatus = (int)CartStatus.checked_out;
        //    await _CartService.UpdateAsync(Cart);
        //    long SumPrice = Cart.CartItems.Sum(x => x.Product.Price * x.Quantity);
        //    long SumDiscount = Cart.CartItems.Sum(x => x.Product.Discount * x.Quantity);
        //    long SumFinal = SumPrice - SumDiscount;
        //    var order = new Order
        //    {
        //        CustomerID = Cart.CustomerID,
        //        CardPen = result.TransactionDetail.MaskedPan,
        //        Discount = SumDiscount,
        //        FinalAmount = SumFinal,
        //        OrderCode = long.Parse(result.TransactionDetail.RRN),
        //        Visible = true,
        //        PaymentStatus = PaymentStatus.paid,
        //        RefId = result.TransactionDetail.RRN,
        //        OrderStatus = OrderStatus.pending,
        //        TotalAmount = SumPrice,
        //        SendProductMethodID = Cart.SendProductMethodID,
        //        IdentityCode = Guid.NewGuid(),
        //        RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
        //        EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
        //        RegisterDate = DateFunctions.GetDateNow(),
        //        EditDate = DateFunctions.GetDateNow(),

        //    };
        //    var address = await _CustomerAddressService.FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID) && s.Default == true);

        //    if (address != null)
        //    {
        //        var model = _mapper.Map<CustomerAddress, ResultCustomerAddress>(address);
        //        order.JsonAddress = JsonConvert.SerializeObject(model);
        //    }

        //    var id = await _OrderService.AddAsync(order);
        //    var now = DateTime.Now;
        //    var date = DateFunctions.GetDateNow();
        //    var time = new TimeSpan(now.Hour, now.Minute, now.Second);
        //    var orderItems = Cart.CartItems.Select(item => new OrderItem
        //    {
        //        OrderID = id,
        //        Visible = true,
        //        ProductID = item.ProductID,
        //        PriceAtOrder = item.Product.Price,
        //        Quantity = item.Quantity,
        //        RegisterDate = date,
        //        EditDate = date,
        //        RegisterTime = time,
        //        EditTime = time,
        //        IdentityCode = Guid.NewGuid(),
        //    }).ToList();


        //    _ = await _OrderItemService.AddRangeAsync(orderItems);


        //    var Wallet = await _WalletService
        //         .FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID));
        //    if (Wallet == null)
        //    {
        //        AddWallet addWallet = new AddWallet
        //        {
        //            Balance = 0,
        //            CustomerID = int.Parse(CustomerID),
        //            GiftCredit = 0,
        //        };
        //        Wallet = _mapper.Map<Wallet>(addWallet);
        //        await _WalletService.AddAsync(Wallet);
        //    }


        //    AddWalletTransaction addWalletTransaction = new AddWalletTransaction
        //    {
        //        Amount = SumFinal,
        //        AfterBalance = Wallet != null ? Wallet.Balance : 0,
        //        Kind = TransactionKind.Purchase,
        //        Status = TransactionStatus.Completed,
        //        Description = "خرید از طریق درگاه پرداخت",
        //        RefID = result.TransactionDetail.RefNum,
        //        CardPen = result.TransactionDetail.MaskedPan,
        //        WalletID = Wallet.ID,
        //    };


        //    var mapedWalletTransaction = _mapper.Map<WalletTransaction>(addWalletTransaction);
        //    var rescount = await _WalletTransactionSService.AddAsync(mapedWalletTransaction);
        //    /// بخش ارسالاطلاع رسانی به فروشنده محصول از طریق پیامک
        //    List<string> phonNumbers = new List<string>();
        //    VerifySendSmsOtpCode smsModel = new();
        //    var set = await _SettingService.GetAllAsync();
        //    if (set.Count() > 0)
        //    {
        //        var settings = set.ToList();
        //        phonNumbers.Add("09039637334");
        //        SendSMSRequest smsRequest =
        //        new(settings[0].UserName,
        //        settings[0].Password,
        //        settings[0].PhoneSender,
        //        phonNumbers.ToArray(),
        //        $"محصولی با کد سفارش {result.TransactionDetail.RRN} برای مشتری {customer.Name} {customer.LastName} ثبت سفارش شد. \r\n TolidiAyhan.ir",
        //        smsModel.IsFlash,
        //        smsModel.RecId,
        //        smsModel.Status);
        //        SendServiceClient client2 = new SendServiceClient();
        //        var sendSMSResponse = await client2.SendSMSAsync(smsRequest);

        //        var SmsResponseMessage = SmsViewModel.GetSmsResponseMessage((SendSmsReturnType)sendSMSResponse.SendSMSResult);

        //    }

        //    var OrderPaymentTemps = await _OrderPaymentTempService
        //                       .FirstOrDefaultAsync(s => s.Amount == SumFinal.ToString() && s.ResNum == paymentResponse.ResNum);

        //    if (OrderPaymentTemps != null)
        //    {
        //        await _OrderPaymentTempService.DeleteAsync(OrderPaymentTemps.ID);
        //    }
        //    return NotFound(new ResponseApiEntity<PaymentSepResult>
        //                                                    (entity: result,
        //                                                    status: ResultMessageApi.Success,
        //                                                    statusCode: ResultMessageApi.SuccessCode,
        //                                                    message: result.ResultDescription));
        //}

    }




}
