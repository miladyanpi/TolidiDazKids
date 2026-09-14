using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.OrderItemSrv;
using ServicesLibrary.Services.OrderSrv;
using ServicesLibrary.Services.SettingSrv;
using AutoMapper;
using Castle.Core.Resource;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoCart;
using Dto.Models.DtoOrder;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Dto.Enum.EnumConstant;
using ZstdSharp.Unsafe;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]

    public class OrderPublicController : ControllerBase
    {
        private readonly IOrderService _OrderService;
        private readonly ICustomerService _CustomerService;

        private readonly IMapper _mapper;
        public OrderPublicController(
            ICustomerService CustomerService,
            IOrderService OrderService,
            IMapper mapper,
            UserManager<Account> userManager,
            ISettingService settingService
            
            )
        {
            _CustomerService= CustomerService;
            _OrderService = OrderService;
            _mapper = mapper;
        }

        [HttpGet("Orders/Public")]
        public async Task<IActionResult> GetOrders([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return  BadRequest(new ResponseApiEntity<ResultOrder>
                                                                 (entity: new ResultOrder(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var CustomerID = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

            var UniqCode = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<ResultOrder>
                                                                 (entity: new ResultOrder(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<ResultOrder>
                                                                 (entity: new ResultOrder(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var count = await _OrderService
                           .GetCountAllAsync(s => s.CustomerID == int.Parse(CustomerID));

            var Orders = await _OrderService
                                 .GetAllAsync(s => s.CustomerID == int.Parse(CustomerID)
                                 , page: @params.Page, take: @params.Take);


            var mappedOrders = _mapper.Map<ICollection<ResultOrder>>(Orders);

            return Ok(new ResponseApiEntities<ResultOrder>
                                                            (entities: mappedOrders,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));
        }
        //[HttpGet("Orders/Public/{identityCode}")]
        //public async Task<IActionResult> GetOrderById([FromRoute] string identityCode)
        //{
        //    var CustomerID = User.Claims
        //              .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

        //    var UniqCode = User.Claims
        //               .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
        //    if (!Guid.TryParse(UniqCode, out Guid guid))
        //        return BadRequest(new ResponseApiEntity<UpdateOrder>
        //                                                         (entity: new UpdateOrder(),
        //                                                         statusCode: ResultMessageApi.ErrorCode,
        //                                                         status: ResultMessageApi.Error,
        //                                                         message: ResultMessageApi.ErrorNotAllowRequest));


        //    var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
        //    if (customer == null)
        //        return BadRequest(new ResponseApiEntity<UpdateOrder>
        //                                                         (entity: new UpdateOrder(),
        //                                                         statusCode: ResultMessageApi.ErrorCode,
        //                                                         status: ResultMessageApi.Error,
        //                                                         message: ResultMessageApi.ErrorNotAllowRequest));

        //    if (!Guid.TryParse(identityCode, out Guid ConvidentityCode))
        //        return BadRequest(new ResponseApiEntity<UpdateOrder>
        //                                                         (entity: new UpdateOrder(),
        //                                                         statusCode: ResultMessageApi.ErrorCode,
        //                                                         status: ResultMessageApi.Error,
        //                                                         message: ResultMessageApi.ErrorNotAllowRequest));
        //    var Order = await _OrderService.FirstOrDefaultAsync(s=>s.IdentityCode==ConvidentityCode);
        //    if(Order == null)
        //        return BadRequest(new ResponseApiEntity<UpdateOrder>
        //                                                   (entity: new UpdateOrder(),
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.GetError));
        //    var result = _mapper.Map<UpdateOrder>(Order);

        //    if (result != null)
        //        return Ok(new ResponseApiEntity<UpdateOrder>
        //                                                       (entity: result,
        //                                                       statusCode: ResultMessageApi.SuccessCode,
        //                                                       status: ResultMessageApi.Success,
        //                                                       message: ResultMessageApi.GetOk));
        //    else
        //        return BadRequest(new ResponseApiEntity<UpdateOrder>
        //                                                   (entity: new UpdateOrder(),
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.GetError));

        //}
    }

    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]

    public class OrderController(
        IOrderService _OrderService,
        ICustomerService _CustomerService,
        IMapper _mapper,
        IOrderItemService _OrderItemService
        )
        : ControllerBase
    {
        [HttpPost("Orders")]
        public async Task<IActionResult> Add([FromBody] AddOrder model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var Order = _mapper.Map<AddOrder, Order>(model);
            int id = await _OrderService.AddAsync(Order);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddOrder>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddOrder>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [HttpPatch("Orders")]
        public async Task<IActionResult> Update([FromBody] UpdateOrder model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var Order = _mapper.Map<UpdateOrder, Order>(model);
            var upd = await _OrderService.UpdateAsync(Order);
            if (upd > 0)
                return Ok(new ResponseApiEntity<UpdateOrder>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateOrder>
                                                           (entity: new UpdateOrder(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [HttpDelete("Orders/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var count=await _OrderItemService.GetCountAllAsync(s=>s.OrderID==id);
            if (count > 0)
                return BadRequest(new ResponseApiEntity<ResultOrder>
                                                          (entity: new ResultOrder(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.NotAllowDeleteError));
            
            var del = await _OrderService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultOrder>
                                                               (entity: new ResultOrder(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultOrder>
                                                           (entity: new ResultOrder(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [HttpGet("Orders")]
        public async Task<IActionResult> GetOrders([FromQuery] PaginationParams @params, string? IdentityCode, OrderStatus? OrderStatus=EnumConstant.OrderStatus.All)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultOrder>
                                                           (entity: new ResultOrder(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            Customer customer=new Customer();
            if(!string.IsNullOrEmpty(IdentityCode))
            {
                 customer = await _CustomerService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == IdentityCode);
                if (customer == null) return BadRequest(new ResponseApiEntity<ResultOrder>
                                                           (entity: new ResultOrder(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            }
           
            int Count = 0;
            IEnumerable<Order> Orders=new List<Order>();    
            if (string.IsNullOrEmpty(IdentityCode))
            {
                if(OrderStatus== EnumConstant.OrderStatus.All)
                {
                    Count = await _OrderService
                     .GetCountAllAsync();

                    Orders = await _OrderService
                                         .GetAllAsync(page: @params.Page, take: @params.Take);
                }
                else
                {
                    Count = await _OrderService
                    .GetCountAllAsync(s=>s.OrderStatus== OrderStatus);

                    Orders = await _OrderService
                                         .GetAllAsync(s => s.OrderStatus == OrderStatus
                                         ,page: @params.Page, take: @params.Take);
                }
             
            }
            else
            {
                if (OrderStatus == EnumConstant.OrderStatus.All)
                {
                    Count = await _OrderService
                       .GetCountAllAsync(s => s.CustomerID == customer.ID);

                    Orders = await _OrderService
                                         .GetAllAsync(s => s.CustomerID == customer.ID
                                         , page: @params.Page, take: @params.Take);
                }
                else
                {
                    Count = await _OrderService
                       .GetCountAllAsync(s =>s.OrderStatus== OrderStatus &&
                                             s.CustomerID == customer.ID);

                    Orders = await _OrderService
                                         .GetAllAsync(s => 
                                         s.OrderStatus == OrderStatus &&
                                         s.CustomerID == customer.ID
                                         , page: @params.Page, take: @params.Take);
                }
            }


                var mappedOrders = _mapper.Map<ICollection<ResultOrder>>(Orders);

            return Ok(new ResponseApiEntities<ResultOrder>
                                                            (entities: mappedOrders,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Count));
        }
        [HttpGet("Orders/{id}")]
        public async Task<IActionResult> GetOrderById([FromRoute] int id)
        {
            var Order = await _OrderService.GetByIdAsync(id);
            var result = _mapper.Map<UpdateOrder>(Order);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateOrder>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateOrder>
                                                           (entity: new UpdateOrder(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("Orders/ChangeStatus/{id}")]
        public async Task<IActionResult> GetOrderChangeStatusById([FromRoute] int id)
        {
            var Order = await _OrderService.GetByIdAsync(id);
            if(Order==null)
                return BadRequest(new ResponseApiEntity<ResultOrder>
                                                           (entity: new ResultOrder(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            string Message=string.Empty;
            switch(Order.OrderStatus)
            {
                case OrderStatus.pending:
                    Order.OrderStatus= OrderStatus.shipped;
                    Message = "ارسال شده";
                    break;
                case OrderStatus.shipped:
                    Order.OrderStatus = OrderStatus.delivered;
                    Message = "تحویل داده شده";

                    break;
                //case OrderStatus.delivered:
                //    Order.OrderStatus = OrderStatus.canceled;
                //    Message = "لغو شده";

                //    break;
                default:
                    //Order.OrderStatus= OrderStatus.pending;
                    //Message = "در حال بررسی";
                    break;

            }
            var createdCount = await _OrderService.UpdateAsync(Order);

            var result = _mapper.Map<ResultOrder>(Order);

            if (createdCount > 0)
                return Ok(new ResponseApiEntity<ResultOrder>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: $"وضعیت به حالت {Message} تغییر بافت"));
            else
                return BadRequest(new ResponseApiEntity<ResultOrder>
                                                           (entity: new ResultOrder(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("Orders/Result/{id}")]
        public async Task<IActionResult> GetResultOrderById([FromRoute] int id)
        {
            var Order = await _OrderService.GetByIdAsync(id);
            var result = _mapper.Map<ResultOrder>(Order);

            if (result != null)
                return Ok(new ResponseApiEntity<ResultOrder>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<ResultOrder>
                                                           (entity: new ResultOrder(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
    }
}
