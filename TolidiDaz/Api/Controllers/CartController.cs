using AutoMapper;
using Castle.Core.Resource;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoCart;
using Dto.Models.DtoOrder;
using Dto.Models.DtoOrderItem;
using Dto.Models.DtoSendProductMethod;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.CartItemSrv;
using ServicesLibrary.Services.CartSrv;
using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.SettingSrv;
using System.Security.Claims;
using static Dto.Enum.EnumConstant;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]

    public class CartPublicController : ControllerBase
    {

        private readonly ICustomerService _CustomerService;
        private readonly ICartService _CartService;
        private readonly IMapper _mapper;

        public CartPublicController(
            ICustomerService CustomerService,
            ICartService CartService,
            IMapper mapper

            )
        {
            _CustomerService=CustomerService;
            _CartService = CartService;
            _mapper = mapper;


        }
 
        [HttpGet("Carts/Public")]
        public async Task<IActionResult> GetCarts()
        {
            if (!ModelState.IsValid) return BadRequest();
            var CustomerID = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

            var UniqCode = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<ResultCart>
                                                                 (entity: new ResultCart(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if(customer==null)
                return BadRequest(new ResponseApiEntity<ResultCart>
                                                                 (entity: new ResultCart(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var Cart = await _CartService
                                 .FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID) && s.CartStatus == (int)CartStatus.active);

            if(Cart==null)
                return BadRequest(new ResponseApiEntity<ResultCart>
                                                               (entity: new ResultCart(),
                                                               statusCode: ResultMessageApi.ErrorCode,
                                                               status: ResultMessageApi.Error,
                                                               message: ResultMessageApi.ErrorNotAllowRequest));
            var mappedCart = _mapper.Map<ResultCart>(Cart);

            return Ok(new ResponseApiEntity<ResultCart>
                                                            (entity: mappedCart,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk));
        }
        [HttpPatch("Carts/Public/Update/SetSendProductMethodID/{SendProductMethodID}")]
        public async Task<IActionResult> GetCarts([FromRoute] int SendProductMethodID)
        {
            if (!ModelState.IsValid) return BadRequest();
            var CustomerID = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

            var UniqCode = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<UpdateCart>
                                                                 (entity: new UpdateCart(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<UpdateCart>
                                                                 (entity: new UpdateCart(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var Cart = await _CartService
                                 .FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID) && s.CartStatus == (int)CartStatus.active);
            if(Cart == null)
                return BadRequest(new ResponseApiEntity<UpdateCart>
                                                                (entity: new UpdateCart(),
                                                                statusCode: ResultMessageApi.ErrorCode,
                                                                status: ResultMessageApi.Error,
                                                                message: ResultMessageApi.ErrorNotAllowRequest));
            Cart.SendProductMethodID = SendProductMethodID;
            var res=await _CartService.UpdateAsync(Cart);
            if(res>0)
            return Ok(new ResponseApiEntity<UpdateCart>
                                                            (entity: new UpdateCart(),
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateCart>
                                                              (entity: new UpdateCart(),
                                                              statusCode: ResultMessageApi.ErrorCode,
                                                              status: ResultMessageApi.Error,
                                                              message: ResultMessageApi.ErrorNotAllowRequest));
        }
        [HttpGet("Carts/Public/{id}")]
        public async Task<IActionResult> GetCartById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateCart>
                                                           (entity: new UpdateCart(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var CustomerID = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<UpdateCart>
                                                                 (entity: new UpdateCart(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<UpdateCart>
                                                                 (entity: new UpdateCart(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var Cart = await _CartService.GetByIdAsync(id);
            if (Cart == null || Cart.CustomerID != int.Parse(CustomerID))
                return BadRequest(new ResponseApiEntity<UpdateCart>
                                                                 (entity: new UpdateCart(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var result = _mapper.Map<UpdateCart>(Cart);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateCart>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateCart>
                                                           (entity: new UpdateCart(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

    }

    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]

    public class CartController : ControllerBase
    {
        private readonly ICartService _CartService;
        private readonly ICustomerService _CustomerService;
        private readonly IMapper _mapper;

        private readonly ICartItemService _CartItemService;

        public CartController(
            ICartService CartService,
             ICustomerService CustomerService,
        IMapper mapper,

            ISettingService settingService,
            ICartItemService CartItemService

            )
        {
            _CartService = CartService;
            _CustomerService= CustomerService;
            _mapper = mapper;
            _CartItemService = CartItemService;

        }
        [HttpPost("Carts")]
        public async Task<IActionResult> Add([FromBody] AddCart model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddCart>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
            var Cart = _mapper.Map<AddCart, Cart>(model);
            int id = await _CartService.AddAsync(Cart);
            if (id > 0)
                return Ok(new ResponseApiEntity<AddCart>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddCart>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [HttpDelete("Carts/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            //var count=await _CartItemService.GetCountAllAsync(s=>s.CartID == id);
            //if (count > 0)
            //    return BadRequest(new ResponseApiEntity<ResultCart>
            //                                               (entity: new ResultCart(),
            //                                               statusCode: ResultMessageApi.ErrorCode,
            //                                               status: ResultMessageApi.Error,
            //                                               message:"مجاز به حذف نیستید ابتدا اقلام سبد خرید را حذف کنید"));
            var count2 = await _CartService.GetCountAllAsync(s => s.ID == id && s.CartStatus == (int)CartStatus.checked_out);
            if (count2 > 0)
                return BadRequest(new ResponseApiEntity<ResultCart>
                                                          (entity: new ResultCart(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: "مجاز به حذف نیستید این سبد خرید تبدیل به سفارش شده و بسته شده است"));
            var del = await _CartService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultCart>
                                                               (entity: new ResultCart(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultCart>
                                                           (entity: new ResultCart(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [HttpGet("Carts")]
        public async Task<IActionResult> GetCarts([FromQuery] PaginationParams @params, string? IdentityCode,CartStatus? cartStatus=CartStatus.All)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseApiEntities<ResultCart>
                                                           (entities: new List<ResultCart>(),
                                                           status: ResultMessageApi.Error,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           message: ResultMessageApi.GetError,
                                                           countAllRecordTable: 0));
            Customer customer = new Customer();
            if (!string.IsNullOrEmpty(IdentityCode))
            {
                customer = await _CustomerService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == IdentityCode);
                if (customer == null) return BadRequest(new ResponseApiEntity<ResultOrder>
                                                           (entity: new ResultOrder(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            }
            int Count = 0;
            IEnumerable<Cart> Carts = new List<Cart>();
            if (string.IsNullOrEmpty(IdentityCode))
            {
                if (cartStatus == EnumConstant.CartStatus.All)
                {
                     Count = await _CartService
                         .GetCountAllAsync();

                     Carts = await _CartService
                                         .GetAllAsync(page: @params.Page, take: @params.Take);

                }
                else
                {
                    Count = await _CartService
                          .GetCountAllAsync(s=>s.CartStatus==(int)cartStatus);

                    Carts = await _CartService
                                        .GetAllAsync(s => s.CartStatus == (int)cartStatus,
                                                    page: @params.Page, take: @params.Take);
                }

            }
            else
            {
                if (cartStatus == EnumConstant.CartStatus.All)
                {
                    Count = await _CartService
                        .GetCountAllAsync(s=>s.CustomerID==customer.ID);

                    Carts = await _CartService
                                        .GetAllAsync(s => s.CustomerID == customer.ID,page: @params.Page, take: @params.Take);

                }
                else
                {
                    Count = await _CartService
                          .GetCountAllAsync(s => 
                                            s.CustomerID == customer.ID&&
                                            s.CartStatus == (int)cartStatus);

                    Carts = await _CartService
                                        .GetAllAsync(s => 
                                                     s.CustomerID == customer.ID &&
                                                     s.CartStatus == (int)cartStatus,
                                                     page: @params.Page, take: @params.Take);
                }
            }

            var mappedCarts = _mapper.Map<ICollection<ResultCart>>(Carts);

            return Ok(new ResponseApiEntities<ResultCart>
                                                            (entities: mappedCarts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Count));
        }
        [HttpGet("Carts/{id}")]
        public async Task<IActionResult> GetCartById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest();

            var Cart = await _CartService.GetByIdAsync(id);

            var result = _mapper.Map<UpdateCart>(Cart);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateCart>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateCart>
                                                           (entity: new UpdateCart(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        
    }

    
}
