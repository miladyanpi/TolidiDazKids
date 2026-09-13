using ServicesLibrary.Services.CartItemSrv;
using ServicesLibrary.Services.CartSrv;
using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.OrderItemSrv;
using ServicesLibrary.Services.OrderSrv;
using ServicesLibrary.Services.WalletSrv;
using ServicesLibrary.Services.WalletTransactionSrv;
using AutoMapper;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoWallet;
using Dto.Models.DtoWalletTransaction;
using Dto.Models.DtpPayment;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

    public class PaymentZarinPallController : ControllerBase
    {

        private readonly IWalletService _WalletService;
        private readonly IWalletTransactionService _WalletTransactionSService;
        private readonly IOrderService _OrderService;
        private readonly IOrderItemService _OrderItemService;
        private readonly ICustomerService _CustomerService;
        private readonly ICartService _CartService;
        private readonly ICartItemService _CartItemService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;

        public PaymentZarinPallController(
            IWalletService WalletService,
            IWalletTransactionService WalletTransactionSService,
            IOrderService OrderService,
            IOrderItemService OrderItemService,
            ICustomerService CustomerService,
            ICartItemService CartItemService,
            ICartService CartService,
            IMapper mapper,
            IHttpClientFactory httpClientFactory,
            UserManager<Account> userManager)
        {
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
        }
        [HttpGet("Payments/Zarinpal/GetAuthorityForAddAmountToWallet/Public")]
        public async Task<IActionResult> PaymentsAddAmountToWallet([FromQuery] Int64 Amount)
        {
            if (!ModelState.IsValid) return NotFound(new ResponseApiEntity<ZarinpalResponse>
                                                               (entity: new ZarinpalResponse(),
                                                               statusCode: ResultMessageApi.ErrorCode,
                                                               status: ResultMessageApi.Error,
                                                               message: ResultMessageApi.ErrorNotAllowRequest));
            if (Amount < 1000)
                return NotFound(new ResponseApiEntity<ZarinpalResponse>
                                                               (entity: new ZarinpalResponse(),
                                                               statusCode: ResultMessageApi.ErrorCode,
                                                               status: ResultMessageApi.Error,
                                                               message: ResultMessageApi.ErrorNotAllowRequestAmountISNotValid));
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


            var info = await _userManager.Users.Where(s => s.CustomerID == int.Parse(CustomerID)).FirstOrDefaultAsync();
            if (info == null)
                return BadRequest(new ResponseApiEntity<ZarinpalResponse>
                                                            (entity: new ZarinpalResponse(),
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            status: ResultMessageApi.Error,
                                                            message: ResultMessageApi.ErrorNotAllowRequest));
            
            var client = _httpClientFactory.CreateClient();
           // var maxOrderCode = await _OrderService.GetMaxAsync<long>(s => s.OrderCode);



            var url = "https://payment.zarinpal.com/pg/v4/payment/request.json";
            var requestBody = new
            {
                merchant_id = PaymentConstant.merchant_id,
                amount = Amount.ToString(),
                currency = "IRT",
                description = $"واریز به کیف پول توسط {customer.Name} {customer.LastName} ",
                callback_url = "http://biafile.ir/Payments/",
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
        [HttpGet("Payments/Zarinpal/AddAmountToWallet/Statuse")]
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


            var info = await _userManager.Users.Where(s => s.CustomerID == int.Parse(CustomerID)).FirstOrDefaultAsync();
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

        [HttpGet("Payments/Zarinpal/Public")]
        public async Task<IActionResult> Payments([FromQuery] PaymentMethod paymentMethod)
        {
            if (!ModelState.IsValid) return NotFound(new ResponseApiEntity<ZarinpalResponse>
                                                               (entity: new ZarinpalResponse(),
                                                               statusCode: ResultMessageApi.ErrorCode,
                                                               status: ResultMessageApi.Error,
                                                               message: ResultMessageApi.ErrorNotAllowRequest));
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

            var info = await _userManager.Users.Where(s => s.CustomerID == int.Parse(CustomerID)).FirstOrDefaultAsync();
            if (info == null)
                return BadRequest(new ResponseApiEntity<ZarinpalResponse>
                                                            (entity: new ZarinpalResponse(),
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            status: ResultMessageApi.Error,
                                                            message: ResultMessageApi.ErrorNotAllowRequest));

            var Cart = await _CartService
                              .FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID) && s.CartStatus == (int)CartStatus.active);
            if (Cart == null)
                return NotFound(new ResponseApiEntity<ZarinpalResponse>
                                                               (entity: new ZarinpalResponse(),
                                                               statusCode: ResultMessageApi.ErrorCode,
                                                               status: ResultMessageApi.Error,
                                                               message: ResultMessageApi.ErrorNotOpenCart));


            var CartItems = await _CartItemService
                                 .GetAllAsync(s => s.CartID == Cart.ID);

            if (CartItems.Count() == 0)
                return BadRequest(new ResponseApiEntity<ZarinpalResponse>
                                                             (entity: new ZarinpalResponse(),
                                                             statusCode: ResultMessageApi.ErrorCode,
                                                             status: ResultMessageApi.Error,
                                                             message: ResultMessageApi.ErrorNotAllowRequest));

           
          
            var Price = CartItems.Sum(s => s.Product.Price * s.Quantity);
            var maxOrderCode = await _OrderService.GetMaxAsync<long>(s => s.OrderCode);

            if(paymentMethod==PaymentMethod.ZarrinPalPaymentGateway)
            {
                var client = _httpClientFactory.CreateClient();
                var url = "https://payment.zarinpal.com/pg/v4/payment/request.json";
                var requestBody = new
                {
                    merchant_id = PaymentConstant.merchant_id,
                    amount = Price.ToString(),
                    currency = "IRT",
                    description = $"خرید فایل توسط {customer.Name} {customer.LastName}",
                    callback_url = "http://biafile.ir/Payments/",
                    metadata = new
                    {
                        mobile = info.PhoneNumber,
                        email = info.Email,
                        order_id = maxOrderCode == null ? "128211" : (maxOrderCode + 1).ToString(),
                    },
                    mobile = info.PhoneNumber,
                    email = info.Email,
                    order_id = maxOrderCode == null ? "128211" : (maxOrderCode + 1).ToString(),
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

                ZarinpalResponse zarinpalResponse = new ZarinpalResponse();
                if (Price> Wallet.Balance)
                    return BadRequest(new ResponseApiEntity<ZarinpalResponse>
                                                           (entity: zarinpalResponse,
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
                    OrderCode = (int)(maxOrderCode == null ? 128211 : (maxOrderCode + 1)),
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
                    Amount = Price,
                    AfterBalance = Wallet.Balance - Price,
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
                    Wallet.Balance -= Price;
                    _ = await _WalletService.UpdateAsync(Wallet);
                }
          
                return Ok(new ResponseApiEntity<ZarinpalResponse>
                                                              (entity: zarinpalResponse,
                                                              status: ResultMessageApi.Success,
                                                              statusCode: ResultMessageApi.SuccessCode,
                                                              message: ResultMessageApi.RedirectToWall));
            }
            
            //var authority = result.Data.Authority;
            //var redirectUrl = $"https://payment.zarinpal.com/pg/StartPay/{authority}";
            //return Redirect(redirectUrl);
        }
        [HttpGet("Payments/Zarinpal/Public/Statuse")]
        public async Task<IActionResult> GetStatusePayments(string Authority)
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

            var info = await _userManager.Users.Where(s => s.CustomerID == int.Parse(CustomerID)).FirstOrDefaultAsync();
            if (info == null)
                return BadRequest(new ResponseApiEntity<VerifyPayment>
                                                            (entity: new VerifyPayment(),
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            status: ResultMessageApi.Error,
                                                            message: ResultMessageApi.ErrorNotAllowRequest));

            var Cart = await _CartService
                              .FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID) && s.CartStatus == (int)CartStatus.active);
            if (Cart == null)
                return NotFound(new ResponseApiEntity<VerifyPayment>
                                                               (entity: new VerifyPayment(),
                                                               statusCode: ResultMessageApi.ErrorCode,
                                                               status: ResultMessageApi.Error,
                                                               message: ResultMessageApi.ErrorNotAllowRequest));


            var CartItems = await _CartItemService
                                 .GetAllAsync(s => s.CartID == Cart.ID);

            if (CartItems.Count() == 0)
                return BadRequest(new ResponseApiEntity<VerifyPayment>
                                                             (entity: new VerifyPayment(),
                                                             statusCode: ResultMessageApi.ErrorCode,
                                                             status: ResultMessageApi.Error,
                                                             message: ResultMessageApi.ErrorNotAllowRequest));

           
            
            var Price = CartItems.Sum(s => s.Product.Price * s.Quantity);
            var client = _httpClientFactory.CreateClient();

            var url = "https://payment.zarinpal.com/pg/v4/payment/verify.json";
            var requestBody = new
            {
                merchant_id = PaymentConstant.merchant_id,
                authority = Authority,
                amount = Price.ToString(),
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
            var resultCheckOrder = await _OrderService.FirstOrDefaultAsync(s => s.RefId == result.data.ref_id.ToString());
            if (resultCheckOrder != null)
            {
                return BadRequest(new ResponseApiEntity<VerifyPayment>
                                                           (entity: new VerifyPayment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.ErrorNotAllowRequestOrderExists));
            }
            var resultWalletTransaction = await _WalletTransactionSService.FirstOrDefaultAsync(s => s.RefID == result.data.ref_id.ToString());
            if (resultWalletTransaction != null)
                return NotFound(new ResponseApiEntity<VerifyPayment>
                                                         (entity: result,
                                                         status: ResultMessageApi.Error,
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         message: ResultMessageApi.GetErrorPaymentExistsTransaction));




            Cart.CartStatus = (int)CartStatus.checked_out;
            await _CartService.UpdateAsync(Cart);
            var price = Cart.CartItems.Sum(s => s.Product.Price * s.Quantity);
            var order = new Order
            {
                CustomerID = Cart.CustomerID,
                CardPen = result.data.card_pan,
                Discount = 0,
                FinalAmount = price,
                OrderCode = int.Parse(result.data.order_id),
                Visible = true,
                PaymentStatus = PaymentStatus.paid,
                RefId = result.data.ref_id.ToString(),
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


            var Wallet = await _WalletService
                 .FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID));

            AddWalletTransaction addWalletTransaction = new AddWalletTransaction
            {
                Amount = Price,
                AfterBalance = Wallet!=null? Wallet.Balance:0,
                Kind = TransactionKind.Purchase,
                Status = TransactionStatus.Completed,
                Description = "خرید از طریق درگاه پرداخت",
                RefID = result.data.ref_id.ToString(),
                CardPen = result.data.card_pan,
                WalletID = Wallet.ID,
            };
            var mapedWalletTransaction = _mapper.Map<WalletTransaction>(addWalletTransaction);
            var rescount = await _WalletTransactionSService.AddAsync(mapedWalletTransaction);
            return Ok(new ResponseApiEntity<VerifyPayment>
                                                            (entity: result,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOkPaymentStatuse));
        }

    }




}
