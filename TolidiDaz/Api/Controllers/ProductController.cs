using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoPagination;
using Dto.Models.DtoProduct;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using LinqKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.CartItemSrv;
using ServicesLibrary.Services.CategorySrv;
using ServicesLibrary.Services.OrderItemSrv;
using ServicesLibrary.Services.ProductSrv;
using ServicesLibrary.Services.ViewCounter;
using System.Linq.Expressions;
using System.Security.Claims;
using Utility;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ProductController(
        IProductService _ProductService,
        ICartItemService _CartItemService,
        IOrderItemService _OrderItemService,
        ICategoryService _CategoryService,
        IMapper _mapper,
        IViewCounterService _viewCounterService
        ) 
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Products")]
        public async Task<IActionResult> Add([FromBody] AddProduct model)
        {
            if (!ModelState.IsValid) return BadRequest();
            string ProductCode = string.Empty;
            while (true)
            {
                ProductCode = new Random().Next(11111111, 99999999).ToString();
                var q = await _ProductService.FirstOrDefaultAsync(s => s.ProductCode == ProductCode);
                if (q == null)
                    break;
            }
            var Product = _mapper.Map<AddProduct, Product>(model);
            Product.ProductCode = ProductCode;
            int id = await _ProductService.AddAsync(Product);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddProduct>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddProduct>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Products")]
        public async Task<IActionResult> Update([FromBody] UpdateProduct model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var Product = _mapper.Map<UpdateProduct, Product>(model);
            var upd = await _ProductService.UpdateAsync(Product);
            if (upd > 0)
                return Ok(new ResponseApiEntity<UpdateProduct>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateProduct>
                                                           (entity: new UpdateProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Products/UpdateJsonFile")]
        public async Task<IActionResult> UpdateUpdateCustomerJsonFile([FromBody] UpdateJsonFile model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var q = await _ProductService.GetByIdAsync(model.ID);
            if (q == null)
                return BadRequest(new ResponseApiEntity<UpdateJsonFile>
                                                         (entity: new UpdateJsonFile(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.UpdateError));
            q.JsonPicture = model.JsonPicture;
            // var customer = _mapper.Map<UpdateCustomer, Customer>(q);
            var upd = await _ProductService.UpdateAsync(q);
            if (upd > 0)
                return Ok(new ResponseApiEntity<UpdateJsonFile>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateJsonFile>
                                                           (entity: new UpdateJsonFile(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("Products/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var cartitemsCount = await _CartItemService.GetCountAllAsync(s => s.ProductID == id);
            if (cartitemsCount > 0)
                return BadRequest(new ResponseApiEntity<ResultProduct>
                                                           (entity: new ResultProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.NotAllowDeleteError));
            var OrderItemCount = await _OrderItemService.GetCountAllAsync(s => s.ProductID == id);
            if (OrderItemCount > 0)
                return BadRequest(new ResponseApiEntity<ResultProduct>
                                                           (entity: new ResultProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.NotAllowDeleteError));


            var del = await _ProductService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultProduct>
                                                               (entity: new ResultProduct(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultProduct>
                                                           (entity: new ResultProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Products/Data")]
        public async Task<IActionResult> GetProducts([FromBody] PaginationParams @params,int? CategoryID=null)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProduct>
                                                                    (entities: new List<ResultProduct>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<Product, bool>> predicate = x => true;
                if(CategoryID != null)
                    predicate= predicate.And(s=>s.CategoryID== CategoryID);

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Title ?? "").Contains(search);
                }

                var count = await _ProductService.GetCountAllAsync(predicate);

                var Products = await _ProductService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedProducts = _mapper.Map<ICollection<ResultProduct>>(Products);

                return Ok(new ResponseApiEntities<ResultProduct>
                                                                (entities: mappedProducts,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultProduct>
                                                                    (entities: new List<ResultProduct>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Products")]
        public async Task<IActionResult> GetProducts2([FromQuery] PaginationParams @params, int? CategoryID = null)
        {

            if (!ModelState.IsValid) return BadRequest();

            var count = CategoryID == null ? await _ProductService
                           .GetCountAllAsync(s => s.Title.Contains(@params.SearchText) || s.ProductCode.Contains(@params.SearchText))
                           :
                           await _ProductService
                           .GetCountAllAsync(s => s.CategoryID == CategoryID &&
                           (s.Title.Contains(@params.SearchText) || s.ProductCode.Contains(@params.SearchText)));

            var Products = CategoryID == null ?
                                             await _ProductService
                                                             .GetAllAsync(s => s.Title.Contains(@params.SearchText) || s.ProductCode.Contains(@params.SearchText)
                                                             , page: @params.Page, take: @params.Take)
                                            :
                                            await _ProductService
                                                             .GetAllAsync(s => s.CategoryID == CategoryID &&
                                                             (s.Title.Contains(@params.SearchText) || s.ProductCode.Contains(@params.SearchText))
                                                             , page: @params.Page, take: @params.Take);


            var mappedProducts = _mapper.Map<ICollection<ResultProduct>>(Products);

            return Ok(new ResponseApiEntities<ResultProduct>
                                                            (entities: mappedProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));



        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Products/ByCategoryID")]
        public async Task<IActionResult> GetProductsByCategoryID([FromQuery] int? CategoryID = null)
        {

            if (!ModelState.IsValid) return BadRequest();

            var Products = await _ProductService
                                 .GetAllAsync(s => s.CategoryID == CategoryID);


            var mappedProducts = _mapper.Map<ICollection<ResultProduct>>(Products);

            return Ok(new ResponseApiEntities<ResultProduct>
                                                            (entities: mappedProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: mappedProducts.Count()));



        }

        [HttpGet("Products/Discount")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsDiscount()
        {

            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProduct>
                                                            (entities: new List<ResultProduct>(),
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: 0));
            var Products = await _ProductService
                                 .GetAllAsync(s => s.Discount > 0);


            var mappedProducts = _mapper.Map<ICollection<ResultProduct>>(Products);

            return Ok(new ResponseApiEntities<ResultProduct>
                                                            (entities: mappedProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Products.Count()));



        }
        [HttpGet("Products/ByGuidCategory")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts([FromQuery] PaginationParams @params, string? UniqCode = null)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultProduct>
                                                         (entity: new ResultProduct(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.GetError));
            Guid code;
            int Count = 0;
            IEnumerable<Product> Products = new List<Product>();
            if (UniqCode != null)
            {
                var isValid = Guid.TryParse(UniqCode, out code);
                if (!isValid)
                {
                    return BadRequest(new ResponseApiEntity<ResultProduct>
                                                             (entity: new ResultProduct(),
                                                             statusCode: ResultMessageApi.ErrorCode,
                                                             status: ResultMessageApi.Error,
                                                             message: ResultMessageApi.GetError));
                }
                var q = await _CategoryService.FirstOrDefaultAsync(s => s.IdentityCode == code);
                if (q == null)
                {
                    return BadRequest(new ResponseApiEntity<ResultProduct>
                                                            (entity: new ResultProduct(),
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            status: ResultMessageApi.Error,
                                                            message: ResultMessageApi.GetError));
                }
                Count = await _ProductService
                         .GetCountAllAsync(s => s.CategoryID == q.ID &&
                        (s.Title.Contains(@params.SearchText) || s.ProductCode.Contains(@params.SearchText)));

                Products = await _ProductService
                                    .GetAllAsync(s => s.CategoryID == q.ID &&
                                    (s.Title.Contains(@params.SearchText) || s.ProductCode.Contains(@params.SearchText))
                                    , page: @params.Page, take: @params.Take);
            }

            else
            {
                Count = await _ProductService
                         .GetCountAllAsync(s =>
                         (s.Title.Contains(@params.SearchText) || s.ProductCode.Contains(@params.SearchText)));

                Products = await _ProductService
                                    .GetAllAsync(s =>
                                    (s.Title.Contains(@params.SearchText) || s.ProductCode.Contains(@params.SearchText))
                                    , page: @params.Page, take: @params.Take);
            }





            var mappedProducts = _mapper.Map<ICollection<ResultProduct>>(Products);

            return Ok(new ResponseApiEntities<ResultProduct>
                                                            (entities: mappedProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Count));
        }
        [HttpPost("Products/CategoryIDS")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsCategoryIDS([FromBody] PaginationParamsIDs @params)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultProduct>
                                                         (entity: new ResultProduct(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.GetError));



            var Count = await _ProductService
                     .GetCountAllAsync(s => @params.IDs.Contains((int)s.CategoryID) &&
                     s.Title.Contains(@params.Pagination.SearchText));

            var Products = await _ProductService
                                .GetAllAsync(s => @params.IDs.Contains((int)s.CategoryID) &&
                                (s.Title.Contains(@params.Pagination.SearchText))
                                , page: @params.Pagination.Page, take: @params.Pagination.Take);


            var mappedProducts = _mapper.Map<ICollection<ResultProduct>>(Products);

            return Ok(new ResponseApiEntities<ResultProduct>
                                                            (entities: mappedProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Count));
        }
        [HttpGet("Products/ByGuidProduct/{UniqCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts([FromRoute] string? UniqCode = null)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultProduct>
                                                        (entity: new ResultProduct(),
                                                        statusCode: ResultMessageApi.ErrorCode,
                                                        status: ResultMessageApi.Error,
                                                        message: ResultMessageApi.GetError));
            Guid code;
            var isValid = Guid.TryParse(UniqCode, out code);
            if (!isValid)
            {
                return BadRequest(new ResponseApiEntity<ResultProduct>
                                                         (entity: new ResultProduct(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.GetError));
            }
            var q = await _ProductService.FirstOrDefaultAsync(s => s.IdentityCode == code);
            if (q == null)
            {
                return BadRequest(new ResponseApiEntity<ResultProduct>
                                                        (entity: new ResultProduct(),
                                                        statusCode: ResultMessageApi.ErrorCode,
                                                        status: ResultMessageApi.Error,
                                                        message: ResultMessageApi.GetError));
            }
            string userIdentifier = GetUserIdentifier();
            // ثبت بازدید
            _viewCounterService.RecordViewProduct(q.ID, userIdentifier);
            var modelProduct = _mapper.Map<ResultProduct>(q);
            return Ok(new ResponseApiEntity<ResultProduct>
                                                            (entity: modelProduct,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk));
        }
        private string GetUserIdentifier()
        {
            // اگر کاربر لاگین کرده باشد
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return $"user:{userId}";
            }

            // کاربر مهمان: ترکیبی از IP + User-Agent
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userAgent = Request.Headers["User-Agent"].ToString();

            // هش ساده برای کوتاه کردن
            int hash = (ip + userAgent).GetHashCode();
            return $"guest:{ip}:{hash}";
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Products/{id}")]
        public async Task<IActionResult> GetProductById([FromRoute] int id)
        {
            var Product = await _ProductService.GetByIdAsync(id);

            var result = _mapper.Map<UpdateProduct>(Product);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateProduct>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateProduct>
                                                           (entity: new UpdateProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        /// <summary>
        /// محصولاتی که بیشترین بازدید را داشته اند
        /// </summary>
        /// <returns></returns>
        [HttpGet("Products/MostVisitedProduct")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsAllNewProduct()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProduct>
                                                            (entities: new List<ResultProduct>(),
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: 0));
            var Products = await _ProductService.GetTopAsync(p => p.ViewCount, 30);
            var mappedProducts = _mapper.Map<ICollection<ResultProduct>>(Products);

            return Ok(new ResponseApiEntities<ResultProduct>
                                                            (entities: mappedProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: mappedProducts.Count()));
        }
        /// <summary>
        /// جدیدترین محصولات ثبت شده
        /// </summary>
        /// <returns></returns>
        [HttpGet("Products/NewProduct")]
        [AllowAnonymous]
        public async Task<IActionResult> GetNewProductAllPublicPage()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProduct>
                                                            (entities: new List<ResultProduct>(),
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: 0));
            var Products = await _ProductService.GetTopAsync(p => p.ID, 30);

            var mappedProducts = _mapper.Map<ICollection<ResultProduct>>(Products);

            return Ok(new ResponseApiEntities<ResultProduct>
                                                            (entities: mappedProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: mappedProducts.Count()));
        }
        /// <summary>
        /// پربازدیدترین محصولات هقته
        /// </summary>
        /// <returns></returns>
        [HttpGet("Products/LatestMostViewedProductsOfWeeks")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLatestMostViewedProductsOfWeeksAllPublicPage()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProduct>
                                                            (entities: new List<ResultProduct>(),
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: 0));
            var TodayDate = DateFunctions.GetDateNow();
            //var Last7Day = DateFunctions.ConvertDateStringToInt(DateFunctions.AddDayToDate(TodayDate, -7));
            IEnumerable<Product> Products = new List<Product>();

            Products = await _ProductService.GetTopAsync(p => p.ViewCount, s => s.RegisterDate <= TodayDate, 30);


            var mappedProducts = _mapper.Map<ICollection<ResultProduct>>(Products);

            return Ok(new ResponseApiEntities<ResultProduct>
                                                            (entities: mappedProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: mappedProducts.Count()));
        }
        [HttpGet("Products/NewProductTop50")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductsAllPublicPage()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProduct>
                                                            (entities: new List<ResultProduct>(),
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: 0));
            var Products = await _ProductService
                                 .GetAllAsync(s => s.Visible == true, page: 1, take: 50);
            var mappedProducts = _mapper.Map<ICollection<ResultProduct>>(Products);

            return Ok(new ResponseApiEntities<ResultProduct>
                                                            (entities: mappedProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: mappedProducts.Count()));
        }


        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Products/ByGuid/{guid}")]
        public async Task<IActionResult> GetProductById([FromRoute] string guid)
        {
            try
            {
                var Product = await _ProductService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (Product == null)
                    return BadRequest(new ResponseApiEntity<UpdateProduct>
                                                                              (entity: new UpdateProduct(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateProduct>(Product);
                return Ok(new ResponseApiEntity<UpdateProduct>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateProduct>
                                                                             (entity: new UpdateProduct(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }
    }

}
