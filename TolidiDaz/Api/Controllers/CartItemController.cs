using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoAccount;
using Dto.Models.DtoCart;
using Dto.Models.DtoCartItem;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.CartItemSrv;
using ServicesLibrary.Services.CartSrv;
using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.OrderSrv;
using ServicesLibrary.Services.ProductSrv;
using System.Linq.Expressions;
using System.Security.Claims;
using static Dto.Enum.EnumConstant;

namespace Api.Controllers
{
    /// <summary>
    /// این کنترلر برای مشتریان صفحه عمومی است
    /// </summary>
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]

    public class CartItemPublicController(
        ICustomerService _CustomerService,
        IOrderService _OrderService,
        ICartItemService _CartItemService,
        IProductService _ProductService,
        ICartService _CartService,
        IMapper _mapper
        )
        : ControllerBase
    {
        [HttpPost("CartItems/Public/Add")]
        public async Task<IActionResult> AddToCart2([FromBody] AddCartItem model)
        {

            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddCartItem>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddCartError));
            var CustomerID = User.Claims
                     .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<AddCartItem>
                                                                 (entity: new AddCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<AddCartItem>
                                                                 (entity: new AddCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));



            var cart = await _CartService.FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID) && s.CartStatus == (int)CartStatus.active);


            if (cart == null)
            {
                AddCart addCart = new AddCart
                {
                    CustomerID = int.Parse(CustomerID),
                    CartStatus = CartStatus.active,
                };
                var Cart = _mapper.Map<AddCart, Cart>(addCart);
                var id0 = await _CartService.AddAsync(Cart);
                cart = await _CartService.GetByIdAsync(id0);

            }
            //var orders = await _OrderService.GetAllAsync(s => s.CustomerID == int.Parse(CustomerID) && s.OrderStatus == OrderStatus.delivered);
            //foreach (var item in orders)
            //{
            //    var CheckOrderItem = item.OrderItems.Where(s => s.ProductID == model.ProductID).FirstOrDefault();
            //    if (CheckOrderItem != null)
            //        return BadRequest(new ResponseApiEntity<AddCartItem>
            //                                                   (entity: null,
            //                                                   statusCode: ResultMessageApi.ErrorCode,
            //                                                   status: ResultMessageApi.Error,
            //                                                   message: ResultMessageApi.AddCartErrorExistsInOrder));
            //}

            int id = 0;
            model.CartID = cart.ID;
            var cartItem = await _CartItemService.FirstOrDefaultAsync(s => s.CartID == cart.ID && s.ProductID == model.ProductID);
            if (cartItem != null)
            {
                cartItem.Quantity += 1;
                var product = await _ProductService.GetByIdAsync(model.ProductID);
                if (product == null)
                {
                    return BadRequest(new ResponseApiEntity<AddCartItem>
                                                         (entity: null,
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.AddCartError));
                }
                if (cartItem.Quantity > product.Count)
                    return BadRequest(new ResponseApiEntity<AddCartItem>
                                                      (entity: null,
                                                      statusCode: ResultMessageApi.ErrorCode,
                                                      status: ResultMessageApi.Error,
                                                      message: ResultMessageApi.AddCartNotExistsCountError));

                cartItem.Quantity = cartItem.Quantity <= 0 ? 1 : cartItem.Quantity;
                id = await _CartItemService.UpdateAsync(cartItem);
            }
            else
            {
                var CartItem = _mapper.Map<AddCartItem, CartItem>(model);
                id = await _CartItemService.AddAsync(CartItem);
            }


            if (id > 0)
                return Ok(new ResponseApiEntity<AddCartItem>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddCartOk));
            else
                return BadRequest(new ResponseApiEntity<AddCartItem>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddCartError));
        }

        [HttpPost("CartItems/Public/Sub")]
        public async Task<IActionResult> AddToCart3([FromBody] AddCartItem model)
        {

            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddCartItem>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddCartError));
            var CustomerID = User.Claims
                     .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<AddCartItem>
                                                                 (entity: new AddCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<AddCartItem>
                                                                 (entity: new AddCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var cart = await _CartService.FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID) && s.CartStatus == (int)CartStatus.active);
            if (cart == null)
            {
                AddCart addCart = new AddCart
                {
                    CustomerID = int.Parse(CustomerID),
                    CartStatus = CartStatus.active,
                };
                var Cart = _mapper.Map<AddCart, Cart>(addCart);
                var id0 = await _CartService.AddAsync(Cart);
                cart = await _CartService.GetByIdAsync(id0);

            }
            //var orders = await _OrderService.GetAllAsync(s => s.CustomerID == int.Parse(CustomerID) && s.OrderStatus == OrderStatus.delivered);
            //foreach (var item in orders)
            //{
            //    var CheckOrderItem = item.OrderItems.Where(s => s.ProductID == model.ProductID).FirstOrDefault();
            //    if (CheckOrderItem != null)
            //        return BadRequest(new ResponseApiEntity<AddCartItem>
            //                                                   (entity: null,
            //                                                   statusCode: ResultMessageApi.ErrorCode,
            //                                                   status: ResultMessageApi.Error,
            //                                                   message: ResultMessageApi.AddCartErrorExistsInOrder));
            //}

            int id = 0;
            model.CartID = cart.ID;
            var cartItem = await _CartItemService.FirstOrDefaultAsync(s => s.CartID == cart.ID && s.ProductID == model.ProductID);
            if (cartItem != null)
            {
                cartItem.Quantity -= 1;
                if (cartItem.Quantity <= 0)
                {
                    id = await _CartItemService.DeleteAsync(cartItem.ID);
                }
                else
                {
                    id = await _CartItemService.UpdateAsync(cartItem);
                }

            }
            else
            {
                var CartItem = _mapper.Map<AddCartItem, CartItem>(model);
                id = await _CartItemService.AddAsync(CartItem);
            }


            if (id > 0)
                return Ok(new ResponseApiEntity<AddCartItem>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.subCartOk));
            else
                return BadRequest(new ResponseApiEntity<AddCartItem>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddCartError));
        }
        [HttpPost("CartItems/Public")]
        public async Task<IActionResult> AddToCart1([FromBody] AddCartItem model)
        {

            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddCartItem>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddCartError));
            var CustomerID = User.Claims
                     .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<AddCartItem>
                                                                 (entity: new AddCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<AddCartItem>
                                                                 (entity: new AddCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));



            var cart = await _CartService.FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID) && s.CartStatus == (int)CartStatus.active);
            if (model.Quantity <= 0)
                model.Quantity = 1;

            if (cart == null)
            {
                AddCart addCart = new AddCart
                {
                    CustomerID = int.Parse(CustomerID),
                    CartStatus = CartStatus.active,
                };
                var Cart = _mapper.Map<AddCart, Cart>(addCart);
                var id0 = await _CartService.AddAsync(Cart);
                cart = await _CartService.GetByIdAsync(id0);

            }
            int id = 0;
            model.CartID = cart.ID;
            var cartItem = await _CartItemService.FirstOrDefaultAsync(s => s.CartID == cart.ID && s.ProductID == model.ProductID);
            if (cartItem != null)
            {
                cartItem.Quantity += model.Quantity;
                id = await _CartItemService.UpdateAsync(cartItem);
            }
            else
            {
                var CartItem = _mapper.Map<AddCartItem, CartItem>(model);
                id = await _CartItemService.AddAsync(CartItem);
            }


            if (id > 0)
                return Ok(new ResponseApiEntity<AddCartItem>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddCartOk));
            else
                return BadRequest(new ResponseApiEntity<AddCartItem>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddCartError));
        }

        [HttpPatch("CartItems/Public")]
        public async Task<IActionResult> Update([FromBody] UpdateCartItem model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateCartItem>
                                                                 (entity: new UpdateCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var CustomerID = User.Claims
                    .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<UpdateCartItem>
                                                                 (entity: new UpdateCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<UpdateCartItem>
                                                                 (entity: new UpdateCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var res = await _CartService.FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID) && s.ID == model.CartID);

            if (res == null)
                return BadRequest(new ResponseApiEntity<UpdateCartItem>
                                                                 (entity: new UpdateCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            if (model.Quantity <= 0)
                model.Quantity = 1;
            var cartItem = await _CartItemService.FirstOrDefaultAsync(s => s.CartID == model.CartID && s.ProductID == model.ProductID);
            int id = 0;
            if (cartItem != null)
            {
                cartItem.Quantity += model.Quantity;
                id = await _CartItemService.UpdateAsync(cartItem);
            }
            else
            {
                id = await _CartItemService.AddAsync(cartItem);
            }
            if (id > 0)
                return Ok(new ResponseApiEntity<UpdateCartItem>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateCartItem>
                                                           (entity: new UpdateCartItem(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }

        [HttpDelete("CartItems/Public/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var CustomerID = User.Claims
                    .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<ResultCartItem>
                                                                 (entity: new ResultCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<ResultCartItem>
                                                                 (entity: new ResultCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));




            var res = await _CartItemService.FirstOrDefaultAsync(s => s.Cart.CustomerID == int.Parse(CustomerID) && s.ID == id);

            if (res == null)
                return BadRequest(new ResponseApiEntity<ResultCartItem>
                                                                 (entity: new ResultCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));



            var del = await _CartItemService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultCartItem>
                                                               (entity: new ResultCartItem(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultCartItem>
                                                           (entity: new ResultCartItem(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [HttpGet("CartItems/Public")]
        public async Task<IActionResult> GetCartItems()
        {
            if (!ModelState.IsValid) return BadRequest();
            var CustomerID = User.Claims
                  .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntities<ResultCartItem>
                                                                 (entities: new List<ResultCartItem>(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntities<ResultCartItem>
                                                                 (entities: new List<ResultCartItem>(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var Cart = await _CartService
                              .FirstOrDefaultAsync(s => s.CustomerID == int.Parse(CustomerID) && s.CartStatus == (int)CartStatus.active);
            if (Cart == null)
                return NotFound(new ResponseApiEntities<ResultCartItem>
                                                               (entities: new List<ResultCartItem>(),
                                                               statusCode: ResultMessageApi.ErrorCode,
                                                               status: ResultMessageApi.Error,
                                                               message: ResultMessageApi.ErrorNotAllowRequest));


            var count = await _CartItemService
                           .GetCountAllAsync(s => s.CartID == Cart.ID);

            var CartItems = await _CartItemService
                                 .GetAllAsync(s => s.CartID == Cart.ID);


            var mappedCartItems = _mapper.Map<ICollection<ResultCartItem>>(CartItems);

            return Ok(new ResponseApiEntities<ResultCartItem>
                                                            (entities: mappedCartItems,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));
        }

        [HttpGet("CartItems/Public/{id}")]
        public async Task<IActionResult> GetCartItemById([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest();
            var CustomerID = User.Claims
                  .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<ResultCartItem>
                                                                 (entity: new ResultCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<ResultCartItem>
                                                                 (entity: new ResultCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));




            var res = await _CartService.GetByIdAsync(id);

            if (res == null)
                return BadRequest(new ResponseApiEntity<ResultCartItem>
                                                                 (entity: new ResultCartItem(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            if (res.CustomerID != int.Parse(CustomerID))
                return BadRequest(new ResponseApiEntity<ChangePasswordAccount>
                                                               (entity: new ChangePasswordAccount(),
                                                               statusCode: ResultMessageApi.ErrorCode,
                                                               status: ResultMessageApi.Error,
                                                               message: ResultMessageApi.ErrorNotAllowRequest));

            var CartItem = await _CartItemService.GetByIdAsync(id);
            var result = _mapper.Map<UpdateCartItem>(CartItem);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateCartItem>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateCartItem>
                                                           (entity: new UpdateCartItem(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
    }
    /// <summary>
    /// این کنترلر برای ادمین صفحه ادمین پنل است
    /// </summary>
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]

    public class CartItemController(
        ICartItemService _CartItemService,
        ICartService _CartService,
        IMapper _mapper
        ) 
        : ControllerBase
    {
        [HttpPost("CartItems")]
        public async Task<IActionResult> Add([FromBody] AddCartItem model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var CartItem = _mapper.Map<AddCartItem, CartItem>(model);
            int id = await _CartItemService.AddAsync(CartItem);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddCartItem>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddCartItem>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [HttpPatch("CartItems")]
        public async Task<IActionResult> Update([FromBody] UpdateCartItem model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var CartItem = _mapper.Map<UpdateCartItem, CartItem>(model);
            var upd = await _CartItemService.UpdateAsync(CartItem);
            if (upd > 0)
                return Ok(new ResponseApiEntity<UpdateCartItem>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateCartItem>
                                                           (entity: new UpdateCartItem(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [HttpDelete("CartItems/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _CartItemService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultCartItem>
                                                               (entity: new ResultCartItem(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultCartItem>
                                                           (entity: new ResultCartItem(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [HttpPost("CartItems/Data")]
        public async Task<IActionResult> GetCartItems([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultCartItem>
                                                                    (entities: new List<ResultCartItem>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<CartItem, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.CartID.ToString() ?? "").Contains(search);
                }

                var count = await _CartItemService.GetCountAllAsync(predicate);

                var CartItems = await _CartItemService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedCartItems = _mapper.Map<ICollection<ResultCartItem>>(CartItems);

                return Ok(new ResponseApiEntities<ResultCartItem>
                                                                (entities: mappedCartItems,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultCartItem>
                                                                    (entities: new List<ResultCartItem>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }
        [HttpGet("CartItems")]
        public async Task<IActionResult> GetCartItems([FromQuery] PaginationParams @params, string? IdentityCode)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultCartItem>
                                                           (entities: new List<ResultCartItem>(),
                                                           status: ResultMessageApi.Error,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           message: ResultMessageApi.GetError,
                                                           countAllRecordTable: 0));

            var cart = await _CartService.GetFirstOrDefaultAsync(s => s.IdentityCode.ToString() == IdentityCode);
            if (cart == null)  return BadRequest(new ResponseApiEntities<ResultCartItem>
                                                           (entities: new List<ResultCartItem>(),
                                                           status: ResultMessageApi.Error,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           message: ResultMessageApi.GetError,
                                                           countAllRecordTable: 0));
            var count = await _CartItemService
                       .GetCountAllAsync(s => s.CartID == cart.ID);

            var CartItems = await _CartItemService
                                 .GetAllAsync(s => s.CartID == cart.ID
                                 , page: @params.Page, take: @params.Take);



            var mappedCartItems = _mapper.Map<ICollection<ResultCartItem>>(CartItems);

            return Ok(new ResponseApiEntities<ResultCartItem>
                                                            (entities: mappedCartItems,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));
        }
        [HttpGet("CartItems/{id}")]
        public async Task<IActionResult> GetCartItemById([FromRoute] int id)
        {
            var CartItem = await _CartItemService.GetByIdAsync(id);
            var result = _mapper.Map<UpdateCartItem>(CartItem);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateCartItem>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateCartItem>
                                                           (entity: new UpdateCartItem(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("CartItems/ByGuid/{guid}")]
        public async Task<IActionResult> GetCartItemById([FromRoute] string guid)
        {
            try
            {
                var CartItem = await _CartItemService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (CartItem == null)
                    return BadRequest(new ResponseApiEntity<UpdateCartItem>
                                                                              (entity: new UpdateCartItem(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateCartItem>(CartItem);
                return Ok(new ResponseApiEntity<UpdateCartItem>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateCartItem>
                                                                             (entity: new UpdateCartItem(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }
    }
}
