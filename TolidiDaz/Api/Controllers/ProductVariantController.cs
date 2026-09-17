using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoProductVariant;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.ProductVariantSrv;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductVariantController(
        IProductVariantService _ProductVariantService,
        IMapper _mapper
        )
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("ProductVariants")]
        public async Task<IActionResult> Add([FromBody] AddProductVariant model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddProductVariant>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

           var q=await _ProductVariantService.FirstOrDefaultAsync(s => s.ProductID == model.ProductID && s.ProductID == model.ProductID);
            if(q != null)
                return BadRequest(new ResponseApiEntity<AddProductVariant>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DataRepeatError));

            var ProductVariant = _mapper.Map<AddProductVariant, ProductVariant>(model);
            int id = await _ProductVariantService.AddAsync(ProductVariant);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddProductVariant>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddProductVariant>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("ProductVariants")]
        public async Task<IActionResult> Update([FromBody] UpdateProductVariant model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateProductVariant>
                                                           (entity: new UpdateProductVariant(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));


            var ProductVariant = _mapper.Map<UpdateProductVariant, ProductVariant>(model);
            var upd = await _ProductVariantService.UpdateAsync(ProductVariant);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateProductVariant>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }

            else
                return BadRequest(new ResponseApiEntity<UpdateProductVariant>
                                                           (entity: new UpdateProductVariant(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("ProductVariants/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _ProductVariantService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultProductVariant>
                                                               (entity: new ResultProductVariant(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultProductVariant>
                                                           (entity: new ResultProductVariant(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("ProductVariants/Data")]
        public async Task<IActionResult> GetProductVariants([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProductVariant>
                                                                    (entities: new List<ResultProductVariant>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<ProductVariant, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Count.ToString() ?? "").Contains(search);
                }

                var count = await _ProductVariantService.GetCountAllAsync(predicate);

                var ProductVariants = await _ProductVariantService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedProductVariants = _mapper.Map<ICollection<ResultProductVariant>>(ProductVariants);

                return Ok(new ResponseApiEntities<ResultProductVariant>
                                                                (entities: mappedProductVariants,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultProductVariant>
                                                                    (entities: new List<ResultProductVariant>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductVariants")]
        public async Task<IActionResult> GetProductVariants([FromQuery] PaginationParams @params, int? ProductID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultProductVariant>
                                                           (entity: new ResultProductVariant(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            var count = await _ProductVariantService
                                .GetCountAllAsync(s => s.ProductID == ProductID);
            var ProductVariants = await _ProductVariantService
                                .GetAllAsync(s => s.ProductID == ProductID
                                , page: @params.Page, take: @params.Take);

            var mappedProductVariants = _mapper.Map<ICollection<ResultProductVariant>>(ProductVariants);

            return Ok(new ResponseApiEntities<ResultProductVariant>
                                                            (entities: mappedProductVariants,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]

        [HttpGet("ProductVariants/{id}")]
        public async Task<IActionResult> GetProductVariantById([FromRoute] int id)
        {

            var data = await _ProductVariantService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateProductVariant>
                                                           (entity: new UpdateProductVariant(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateProductVariant>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateProductVariant>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateProductVariant>
                                                           (entity: new UpdateProductVariant(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]

        [HttpGet("ProductVariants/By")]
        public async Task<IActionResult> GetProductVariantsAll([FromQuery] int? ProductID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProductVariant>
                                                           (entities: new List<ResultProductVariant>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var ProductVariant =
                await _ProductVariantService
                                .FirstOrDefaultAsync(s => s.ProductID == ProductID&&s.ProductID== ProductID);

            var mappedProductVariants = _mapper.Map<ResultProductVariant>(ProductVariant);

            return Ok(new ResponseApiEntity<ResultProductVariant>
                                                            (entity: mappedProductVariants,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductVariants/ByGuid/{guid}")]
        public async Task<IActionResult> GetProductVariantById([FromRoute] string guid)
        {
            try
            {
                var ProductVariant = await _ProductVariantService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (ProductVariant == null)
                    return BadRequest(new ResponseApiEntity<UpdateProductVariant>
                                                                              (entity: new UpdateProductVariant(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateProductVariant>(ProductVariant);
                return Ok(new ResponseApiEntity<UpdateProductVariant>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateProductVariant>
                                                                             (entity: new UpdateProductVariant(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }
    }
}
