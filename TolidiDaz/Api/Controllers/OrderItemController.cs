using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoOrder;
using Dto.Models.DtoOrderItem;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.OrderItemSrv;
using ServicesLibrary.Services.ProductSrv;
using ServicesLibrary.Services.SettingSrv;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]

    public class OrderItemPublicController(
        IProductService _ProductService,
        ICustomerService _CustomerService,
        IOrderItemService _OrderItemService,
        IMapper _mapper
        )
        : ControllerBase
    {
       
        //[HttpGet("OrderItems/Public/CheckProductExistInOrderItem/{ProductUniqCode}")]
        //public async Task<IActionResult> CheckProductExistInOrderItem([FromRoute] string ProductUniqCode)
        //{
        //    if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultOrderItem>
        //                                                         (entity: new ResultOrderItem(),
        //                                                         statusCode: ResultMessageApi.ErrorCode,
        //                                                         status: ResultMessageApi.Error,
        //                                                         message: ResultMessageApi.ErrorNotAllowRequest));
        //    if (!Guid.TryParse(ProductUniqCode, out Guid guid))
        //    {
        //        return BadRequest(new ResponseApiEntity<ResultOrderItem>
        //                                                         (entity: new ResultOrderItem(),
        //                                                         statusCode: ResultMessageApi.ErrorCode,
        //                                                         status: ResultMessageApi.Error,
        //                                                         message: ResultMessageApi.ErrorNotAllowRequest));
        //    }
        //    var CustomerID = User.Claims
        //                 .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

        //    var UniqCode = User.Claims
        //               .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
        //    if (!Guid.TryParse(UniqCode, out Guid guidCustimer))
        //        return BadRequest(new ResponseApiEntity<ResultOrder>
        //                                                         (entity: new ResultOrder(),
        //                                                         statusCode: ResultMessageApi.ErrorCode,
        //                                                         status: ResultMessageApi.Error,
        //                                                         message: ResultMessageApi.ErrorNotAllowRequest));


        //    var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guidCustimer);
        //    if (customer == null)
        //        return BadRequest(new ResponseApiEntity<ResultOrder>
        //                                                         (entity: new ResultOrder(),
        //                                                         statusCode: ResultMessageApi.ErrorCode,
        //                                                         status: ResultMessageApi.Error,
        //                                                         message: ResultMessageApi.ErrorNotAllowRequest));





        //    var Product = await _ProductService.FirstOrDefaultAsync(s => s.IdentityCode == guid);
        //    if (Product == null)
        //        return BadRequest(new ResponseApiEntity<ResultOrderItem>
        //                                                         (entity: new ResultOrderItem(),
        //                                                         statusCode: ResultMessageApi.ErrorCode,
        //                                                         status: ResultMessageApi.Error,
        //                                                         message: ResultMessageApi.ErrorNotAllowRequest));

        //    var OrderItem = await _OrderItemService.FirstOrDefaultAsync(s => s.Order.CustomerID == int.Parse(CustomerID) && s.ProductID == Product.ID);
        //    if (OrderItem != null)
        //        return Ok(new ResponseApiEntity<ResultOrderItem>
        //                                                         (entity: new ResultOrderItem(),
        //                                                         statusCode: ResultMessageApi.SuccessCode,
        //                                                         status: ResultMessageApi.Success,
        //                                                         message: ResultMessageApi.ErrorNotAllowRequestExistsInOrderItemt));

        //     return BadRequest(new ResponseApiEntity<ResultOrderItem>
        //                                                         (entity: new ResultOrderItem(),
        //                                                         statusCode: ResultMessageApi.ErrorCode,
        //                                                         status: ResultMessageApi.Error,
        //                                                         message: ResultMessageApi.ErrorNotAllowRequestNotExistsInOrderItemt));

        //}
        [HttpGet("OrderItems/Public/{OrderUniqCode}")]
        public async Task<IActionResult> GetOrderItemByOrderUniqCode([FromRoute] string OrderUniqCode)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultOrderItem>
                                                                 (entity: new ResultOrderItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            if (!Guid.TryParse(OrderUniqCode, out Guid orderGuid))
            {
                return BadRequest(new ResponseApiEntity<ResultOrderItem>
                                                                 (entity: new ResultOrderItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            }


            var CustomerID = User.Claims
                        .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

            var UniqCode = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guidCustimer))
                return BadRequest(new ResponseApiEntity<ResultOrder>
                                                                 (entity: new ResultOrder(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guidCustimer);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<ResultOrder>
                                                                 (entity: new ResultOrder(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));




            var OrderItem = await _OrderItemService.GetAllAsync(s => s.Order.CustomerID == int.Parse(CustomerID) && s.Order.IdentityCode== orderGuid);

            if (OrderItem == null || OrderItem.Count()==0)
                return BadRequest(new ResponseApiEntity<ResultOrderItem>
                                                                 (entity: new ResultOrderItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotFoundOrderDetails));

         
            var results = _mapper.Map<ICollection<ResultOrderItem>>(OrderItem);

            if (results != null)
                return Ok(new ResponseApiEntities<ResultOrderItem>
                                                               (entities: results,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk,
                                                               countAllRecordTable: results.Count));
            else
                return BadRequest(new ResponseApiEntities<ResultOrderItem>
                                                           (entities: new List<ResultOrderItem>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.ErrorNotFoundOrderDetails));

        }
        [HttpGet("OrderItems/Public/Paging")]
        public async Task<IActionResult> GetOrderItemByOrderUniqCode([FromQuery] PaginationParams @params, string Code)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultOrderItem>
                                                                 (entity: new ResultOrderItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            if (!Guid.TryParse(Code, out Guid orderGuid))
            {
                return BadRequest(new ResponseApiEntity<ResultOrderItem>
                                                                 (entity: new ResultOrderItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            }


            var CustomerID = User.Claims
                        .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

            var UniqCode = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guidCustimer))
                return BadRequest(new ResponseApiEntity<ResultOrder>
                                                                 (entity: new ResultOrder(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guidCustimer);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<ResultOrder>
                                                                 (entity: new ResultOrder(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var Count = await _OrderItemService
                .GetCountAllAsync(s => s.Order.CustomerID == int.Parse(CustomerID) &&
                                s.Order.IdentityCode == orderGuid);

            var OrderItem = await _OrderItemService
                .GetAllAsync(s => s.Order.CustomerID == int.Parse(CustomerID) &&
                                s.Order.IdentityCode == orderGuid
                                , page: @params.Page, take: @params.Take);

            if (OrderItem == null || OrderItem.Count() == 0)
                return BadRequest(new ResponseApiEntity<ResultOrderItem>
                                                                 (entity: new ResultOrderItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotFoundOrderDetails));


            var results = _mapper.Map<ICollection<ResultOrderItem>>(OrderItem);

            if (results != null)
                return Ok(new ResponseApiEntities<ResultOrderItem>
                                                               (entities: results,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk,
                                                               countAllRecordTable: Count));
            else
                return BadRequest(new ResponseApiEntities<ResultOrderItem>
                                                           (entities: new List<ResultOrderItem>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.ErrorNotFoundOrderDetails));

        }

    }

    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]

    public class OrderItemController(
        IOrderItemService _OrderItemService,
        IMapper _mapper
        ) 
        : ControllerBase
    {
        [HttpPost("OrderItems")]
        public async Task<IActionResult> Add([FromBody] AddOrderItem model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var OrderItem = _mapper.Map<AddOrderItem, OrderItem>(model);
            int id = await _OrderItemService.AddAsync(OrderItem);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddOrderItem>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddOrderItem>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }

        [HttpPatch("OrderItems")]
        public async Task<IActionResult> Update([FromBody] UpdateOrderItem model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var OrderItem = _mapper.Map<UpdateOrderItem, OrderItem>(model);
            var upd = await _OrderItemService.UpdateAsync(OrderItem);
            if (upd > 0)
                return Ok(new ResponseApiEntity<UpdateOrderItem>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateOrderItem>
                                                           (entity: new UpdateOrderItem(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [HttpDelete("OrderItems/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _OrderItemService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultOrderItem>
                                                               (entity: new ResultOrderItem(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultOrderItem>
                                                           (entity: new ResultOrderItem(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [HttpPost("OrderItems/Data")]
        public async Task<IActionResult> GetOrderItems([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultOrderItem>
                                                                    (entities: new List<ResultOrderItem>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<OrderItem, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.OrderID.ToString() ?? "").Contains(search);
                }

                var count = await _OrderItemService.GetCountAllAsync(predicate);

                var OrderItems = await _OrderItemService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedOrderItems = _mapper.Map<ICollection<ResultOrderItem>>(OrderItems);

                return Ok(new ResponseApiEntities<ResultOrderItem>
                                                                (entities: mappedOrderItems,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultOrderItem>
                                                                    (entities: new List<ResultOrderItem>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [HttpGet("OrderItems")]
        public async Task<IActionResult> GetOrderItems([FromQuery] PaginationParams @params, int? OrderID)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _OrderItemService
                           .GetCountAllAsync(s => s.OrderID == OrderID);

            var OrderItems = await _OrderItemService
                                 .GetAllAsync(s => s.OrderID == OrderID
                                 , page: @params.Page, take: @params.Take);


            var mappedOrderItems = _mapper.Map<ICollection<ResultOrderItem>>(OrderItems);

            return Ok(new ResponseApiEntities<ResultOrderItem>
                                                            (entities: mappedOrderItems,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));
        }
        [HttpGet("OrderItems/{id}")]
        public async Task<IActionResult> GetOrderItemById([FromRoute] int id)
        {
            var OrderItem = await _OrderItemService.GetByIdAsync(id);
            var result = _mapper.Map<UpdateOrderItem>(OrderItem);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateOrderItem>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateOrderItem>
                                                           (entity: new UpdateOrderItem(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

        [HttpGet("OrderItems/ByGuid/{guid}")]
        public async Task<IActionResult> GetOrderItemById([FromRoute] string guid)
        {
            try
            {
                var OrderItem = await _OrderItemService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (OrderItem == null)
                    return BadRequest(new ResponseApiEntity<UpdateOrderItem>
                                                                              (entity: new UpdateOrderItem(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateOrderItem>(OrderItem);
                return Ok(new ResponseApiEntity<UpdateOrderItem>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateOrderItem>
                                                                             (entity: new UpdateOrderItem(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }

    }
}
