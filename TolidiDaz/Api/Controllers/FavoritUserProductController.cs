using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoCart;
using Dto.Models.DtoFavoritUserProduct;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.FavoritUserProductSrv;
using System;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FavoritUserProductController : ControllerBase
    {
        private readonly IFavoritUserProductService _FavoritUserProductService;
        private readonly ICustomerService _CustomerService;
        private readonly IMapper _mapper;
        public FavoritUserProductController(
            IFavoritUserProductService FavoritUserProductService,
            ICustomerService CustomerService,
            IMapper mapper,
        UserManager<Account> userManager)
        {
            _FavoritUserProductService = FavoritUserProductService;
            _CustomerService = CustomerService;
            _mapper = mapper;   
        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpPost("FavoritUserProducts")]
        public async Task<IActionResult> Add([FromQuery] int? ProductID)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddFavoritUserProduct>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
            var CustomerID = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

            var UniqCode = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<AddFavoritUserProduct>
                                                                 (entity: new AddFavoritUserProduct(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<AddFavoritUserProduct>
                                                                 (entity: new AddFavoritUserProduct(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var CheckExists = await _FavoritUserProductService
                            .FirstOrDefaultAsync(s=>s.CustomerID== customer.ID&&s.ProductID==ProductID);
            int id = 0;
            bool f = true;
            var model = new AddFavoritUserProduct
            {
                ProductID = ProductID,
                CustomerID = customer.ID,
            };
            var FavoritUserProduct = _mapper.Map<AddFavoritUserProduct,FavoritUserProduct>(model);

            if (CheckExists==null)
            {

                id = await _FavoritUserProductService.AddAsync(FavoritUserProduct);
                f = true;
            }
            else
            {
                id = await _FavoritUserProductService.DeleteAsync(CheckExists.ID);
                f = false;
            }

            if (id > 0)
                return Ok(new ResponseApiEntity<AddFavoritUserProduct>
                                                               (entity: new AddFavoritUserProduct() { ProductID = ProductID },
                                                               id: id,
                                                               statusCode: f ? ResultMessageApi.SuccessAddCode : ResultMessageApi.SuccessDeleteCode,
                                                               status: ResultMessageApi.Success,
                                                               message:f? ResultMessageApi.AddFavoritOk: ResultMessageApi.DeleteFavoritOk));
            else
                return BadRequest(new ResponseApiEntity<AddFavoritUserProduct>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        //[Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        //[HttpPatch("FavoritUserProducts")]
        //public async Task<IActionResult> Update([FromBody] UpdateFavoritUserProduct model)
        //{
        //    if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateFavoritUserProduct>
        //                                                   (entity: new UpdateFavoritUserProduct(),
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.UpdateError));
            
       
        //    var FavoritUserProduct = _mapper.Map<UpdateFavoritUserProduct, FavoritUserProduct>(model);
        //    var upd = await _FavoritUserProductService.UpdateAsync(FavoritUserProduct);
        //    if (upd > 0)
        //    {
        //        return Ok(new ResponseApiEntity<UpdateFavoritUserProduct>
        //                                                     (entity: model,
        //                                                     statusCode: ResultMessageApi.SuccessCode,
        //                                                     status: ResultMessageApi.Success,
        //                                                     message: ResultMessageApi.UpdateOk));
        //    }
              
        //    else
        //        return BadRequest(new ResponseApiEntity<UpdateFavoritUserProduct>
        //                                                   (entity: new UpdateFavoritUserProduct(),
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.UpdateError));
        //}
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("FavoritUserProducts/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _FavoritUserProductService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultFavoritUserProduct>
                                                               (entity: new ResultFavoritUserProduct(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultFavoritUserProduct>
                                                           (entity: new ResultFavoritUserProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("FavoritUserProducts")]
        public async Task<IActionResult> GetFavoritUserProducts([FromQuery] PaginationParams @params,int? CustomerID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultFavoritUserProduct>
                                                           (entity: new ResultFavoritUserProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var FavoritUserProducts = await _FavoritUserProductService
                                .GetAllAsync(page: @params.Page, take: @params.Take);

            var mappedFavoritUserProducts = _mapper.Map<ICollection<ResultFavoritUserProduct>>(FavoritUserProducts);

            return Ok(new ResponseApiEntities<ResultFavoritUserProduct>
                                                            (entities: mappedFavoritUserProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: FavoritUserProducts.Count()));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("FavoritUserProducts/AllDataForCurrentUser")]
        public async Task<IActionResult> GetFavoritUserProducts()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultFavoritUserProduct>
                                                           (entity: new ResultFavoritUserProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            var CustomerID = User.Claims
                    .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

            var UniqCode = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<AddFavoritUserProduct>
                                                                 (entity: new AddFavoritUserProduct(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<AddFavoritUserProduct>
                                                                 (entity: new AddFavoritUserProduct(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var FavoritUserProducts = await _FavoritUserProductService
                                .GetAllAsync(s=>s.CustomerID==customer.ID);

            var mappedFavoritUserProducts = _mapper.Map<ICollection<ResultFavoritUserProduct>>(FavoritUserProducts);

            return Ok(new ResponseApiEntities<ResultFavoritUserProduct>
                                                            (entities: mappedFavoritUserProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: FavoritUserProducts.Count()));

        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpGet("FavoritUserProducts/Public")]
        public async Task<IActionResult> GetFavoritUserProducts([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultFavoritUserProduct>
                                                           (entity: new ResultFavoritUserProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            var CustomerID = User.Claims
                        .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<AddFavoritUserProduct>
                                                                 (entity: new AddFavoritUserProduct(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<AddFavoritUserProduct>
                                                                 (entity: new AddFavoritUserProduct(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var count = await _FavoritUserProductService
                               .GetCountAllAsync(s => s.CustomerID == int.Parse(CustomerID));
            var FavoritUserProducts = await _FavoritUserProductService
                                .GetAllAsync(s=>s.CustomerID== int.Parse(CustomerID),page: @params.Page, take: @params.Take);

            var mappedFavoritUserProducts = _mapper.Map<ICollection<ResultFavoritUserProduct>>(FavoritUserProducts);

            return Ok(new ResponseApiEntities<ResultFavoritUserProduct>
                                                            (entities: mappedFavoritUserProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("FavoritUserProducts/{id}")]
        public async Task<IActionResult> GetFavoritUserProductById([FromRoute] int id)
        {

            var data = await _FavoritUserProductService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateFavoritUserProduct>
                                                           (entity: new UpdateFavoritUserProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateFavoritUserProduct>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateFavoritUserProduct>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateFavoritUserProduct>
                                                           (entity: new UpdateFavoritUserProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

    }
}
