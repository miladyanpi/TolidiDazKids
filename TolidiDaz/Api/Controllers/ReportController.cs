using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.OrderSrv;
using ServicesLibrary.Services.ProductSrv;
using AutoMapper;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.Report;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Utility;
using static Dto.Enum.EnumConstant;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ReportController : ControllerBase
    {
        private readonly IProductService _ProductService;
        private readonly ICustomerService _CustomerService;
        private readonly IOrderService _OrderService;
        private readonly IMapper _mapper;

        public ReportController(
            ICustomerService CustomerService,
            IOrderService OrderService,
            IProductService ProductService,
            IMapper mapper
            )
        {
            _OrderService= OrderService;
            _CustomerService =CustomerService;
            _ProductService = ProductService;
            _mapper = mapper;
        }
    
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Report/Count")]
        public async Task<IActionResult> GetProductsCountAll()
        {

            if (!ModelState.IsValid) return BadRequest();

            var productCount = await _ProductService
                           .GetCountAllAsync();

            var productCountToday = await _ProductService
                           .GetCountAllAsync(s => s.RegisterDate == DateFunctions.GetDateNow());

            var customerCount = await _CustomerService
                           .GetCountAllAsync();

            var orderCount = await _OrderService
                                       .GetCountAllAsync();

            var orderCountDelivered = await _OrderService
                                   .GetCountAllAsync(s=>s.OrderStatus== OrderStatus.delivered);

            var OrdersToday = await _OrderService
                                  .GetAllAsync(s => s.RegisterDate == DateFunctions.GetDateNow());
            var orderSumFinalPriceToday = OrdersToday.Sum(s => s.FinalAmount);
            
            var OrdersYesterday = await _OrderService
                               .GetAllAsync(s => s.RegisterDate == DateFunctions.GetYesterdayNow());
            var orderSumFinalPriceYesterday = OrdersYesterday.Sum(s => s.FinalAmount);

            ResultReportDashboard resultReportDashboard = new ResultReportDashboard
            {
                ProductCounAll = productCount,
                ProductCounAllToday= productCountToday,
                CustomerCount= customerCount,
                OrderCount= orderCount,
                OrdorderCountDelivereder= orderCountDelivered,
                OrderSumFinalPriceToday= orderSumFinalPriceToday,
                OrderSumFinalPriceYesterday= orderSumFinalPriceYesterday,

            };

            return Ok(new ResponseApiEntity<ResultReportDashboard>
                                                            (entity: resultReportDashboard,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk));



        }

    }
}
