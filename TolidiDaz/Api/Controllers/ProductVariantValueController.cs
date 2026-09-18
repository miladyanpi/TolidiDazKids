using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoProductVariantValue;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.ProductVariantValueSrv;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductVariantValueController(
        IProductVariantValueService _ProductVariantValueService,
        ICustomerService _CustomerService,
        IMapper _mapper,
         UserManager<Account> userManager
        )
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpPost("ProductVariantValues")]
        public async Task<IActionResult> Add([FromQuery] int? ProductVariantID)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddProductVariantValue>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
            var TraitValueID = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

            var UniqCode = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<AddProductVariantValue>
                                                                 (entity: new AddProductVariantValue(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(TraitValueID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<AddProductVariantValue>
                                                                 (entity: new AddProductVariantValue(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var CheckExists = await _ProductVariantValueService
                            .FirstOrDefaultAsync(s=>s.TraitValueID== customer.ID&&s.ProductVariantID==ProductVariantID);
            int id = 0;
            bool f = true;
            var model = new AddProductVariantValue
            {
                ProductVariantID = ProductVariantID,
                TraitValueID = customer.ID,
            };
            var ProductVariantValue = _mapper.Map<AddProductVariantValue,ProductVariantValue>(model);

            if (CheckExists==null)
            {

                id = await _ProductVariantValueService.AddAsync(ProductVariantValue);
                f = true;
            }
            else
            {
                id = await _ProductVariantValueService.DeleteAsync(CheckExists.ID);
                f = false;
            }

            if (id > 0)
                return Ok(new ResponseApiEntity<AddProductVariantValue>
                                                               (entity: new AddProductVariantValue() { ProductVariantID = ProductVariantID },
                                                               id: id,
                                                               statusCode: f ? ResultMessageApi.SuccessAddCode : ResultMessageApi.SuccessDeleteCode,
                                                               status: ResultMessageApi.Success,
                                                               message:f? ResultMessageApi.AddFavoritOk: ResultMessageApi.DeleteFavoritOk));
            else
                return BadRequest(new ResponseApiEntity<AddProductVariantValue>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        //[Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        //[HttpPatch("ProductVariantValues")]
        //public async Task<IActionResult> Update([FromBody] UpdateProductVariantValue model)
        //{
        //    if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateProductVariantValue>
        //                                                   (entity: new UpdateProductVariantValue(),
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.UpdateError));
            
       
        //    var ProductVariantValue = _mapper.Map<UpdateProductVariantValue, ProductVariantValue>(model);
        //    var upd = await _ProductVariantValueService.UpdateAsync(ProductVariantValue);
        //    if (upd > 0)
        //    {
        //        return Ok(new ResponseApiEntity<UpdateProductVariantValue>
        //                                                     (entity: model,
        //                                                     statusCode: ResultMessageApi.SuccessCode,
        //                                                     status: ResultMessageApi.Success,
        //                                                     message: ResultMessageApi.UpdateOk));
        //    }
              
        //    else
        //        return BadRequest(new ResponseApiEntity<UpdateProductVariantValue>
        //                                                   (entity: new UpdateProductVariantValue(),
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.UpdateError));
        //}
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("ProductVariantValues/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _ProductVariantValueService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultProductVariantValue>
                                                               (entity: new ResultProductVariantValue(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultProductVariantValue>
                                                           (entity: new ResultProductVariantValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("ProductVariantValues/Data")]
        public async Task<IActionResult> GetProductVariantValues([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProductVariantValue>
                                                                    (entities: new List<ResultProductVariantValue>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                //Expression<Func<ProductVariantValue, bool>> predicate = x => true;

                //if (!string.IsNullOrWhiteSpace(@params.SearchText))
                //{
                //    var search = @params.SearchText;

                //    predicate = x =>
                //        (x.Name ?? "").Contains(search);
                //}

                var count = await _ProductVariantValueService.GetCountAllAsync();

                var ProductVariantValues = await _ProductVariantValueService
                                    .GetAllAsync( page: @params.Page, take: @params.Take);


                var mappedProductVariantValues = _mapper.Map<ICollection<ResultProductVariantValue>>(ProductVariantValues);

                return Ok(new ResponseApiEntities<ResultProductVariantValue>
                                                                (entities: mappedProductVariantValues,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultProductVariantValue>
                                                                    (entities: new List<ResultProductVariantValue>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductVariantValues")]
        public async Task<IActionResult> GetProductVariantValues([FromQuery] PaginationParams @params,int? TraitValueID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultProductVariantValue>
                                                           (entity: new ResultProductVariantValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var ProductVariantValues = await _ProductVariantValueService
                                .GetAllAsync(page: @params.Page, take: @params.Take);

            var mappedProductVariantValues = _mapper.Map<ICollection<ResultProductVariantValue>>(ProductVariantValues);

            return Ok(new ResponseApiEntities<ResultProductVariantValue>
                                                            (entities: mappedProductVariantValues,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: ProductVariantValues.Count()));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductVariantValues/AllDataForCurrentUser")]
        public async Task<IActionResult> GetProductVariantValues()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultProductVariantValue>
                                                           (entity: new ResultProductVariantValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            var TraitValueID = User.Claims
                    .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

            var UniqCode = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<AddProductVariantValue>
                                                                 (entity: new AddProductVariantValue(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(TraitValueID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<AddProductVariantValue>
                                                                 (entity: new AddProductVariantValue(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var ProductVariantValues = await _ProductVariantValueService
                                .GetAllAsync(s=>s.TraitValueID==customer.ID);

            var mappedProductVariantValues = _mapper.Map<ICollection<ResultProductVariantValue>>(ProductVariantValues);

            return Ok(new ResponseApiEntities<ResultProductVariantValue>
                                                            (entities: mappedProductVariantValues,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: ProductVariantValues.Count()));

        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpGet("ProductVariantValues2/Public")]
        public async Task<IActionResult> GetProductVariantValues2([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultProductVariantValue>
                                                           (entity: new ResultProductVariantValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            var TraitValueID = User.Claims
                        .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<AddProductVariantValue>
                                                                 (entity: new AddProductVariantValue(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(TraitValueID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<AddProductVariantValue>
                                                                 (entity: new AddProductVariantValue(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var count = await _ProductVariantValueService
                               .GetCountAllAsync(s => s.TraitValueID == int.Parse(TraitValueID));
            var ProductVariantValues = await _ProductVariantValueService
                                .GetAllAsync(s=>s.TraitValueID== int.Parse(TraitValueID),page: @params.Page, take: @params.Take);

            var mappedProductVariantValues = _mapper.Map<ICollection<ResultProductVariantValue>>(ProductVariantValues);

            return Ok(new ResponseApiEntities<ResultProductVariantValue>
                                                            (entities: mappedProductVariantValues,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductVariantValues/{id}")]
        public async Task<IActionResult> GetProductVariantValueById([FromRoute] int id)
        {

            var data = await _ProductVariantValueService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateProductVariantValue>
                                                           (entity: new UpdateProductVariantValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateProductVariantValue>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateProductVariantValue>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateProductVariantValue>
                                                           (entity: new UpdateProductVariantValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductVariantValues/ByGuid/{guid}")]
        public async Task<IActionResult> GetProductVariantValueById([FromRoute] string guid)
        {
            try
            {
                var ProductVariantValue = await _ProductVariantValueService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (ProductVariantValue == null)
                    return BadRequest(new ResponseApiEntity<UpdateProductVariantValue>
                                                                              (entity: new UpdateProductVariantValue(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateProductVariantValue>(ProductVariantValue);
                return Ok(new ResponseApiEntity<UpdateProductVariantValue>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateProductVariantValue>
                                                                             (entity: new UpdateProductVariantValue(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }

    }
}
