using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoProductVariant;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.ProductSrv;
using ServicesLibrary.Services.ProductVariantSrv;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductVariantController(
        IProductVariantService _ProductVariantService,
        IProductService _ProductService,
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
        [HttpPost("ProductVariants/AddRange")]
        public async Task<IActionResult> AddRange([FromBody] List<AddProductVariantRow> models)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<AddProductVariantRow>
                                                           (entities: new List<AddProductVariantRow>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError,
                                                           countAllRecordTable:0));


            var ProductVariants = _mapper.Map<List<ProductVariant>>(models);
            var ids = await _ProductVariantService.AddRangeAsync(ProductVariants);

            if (ids.Count() > 0)
                return Ok(new ResponseApiEntities<AddProductVariantRow>
                                                               (entities: models,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk,
                                                              countAllRecordTable: ids.Count()));
            else
                return BadRequest(new ResponseApiEntities<AddProductVariantRow>
                                                           (entities: new List<AddProductVariantRow>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError,
                                                            countAllRecordTable: ids.Count()));
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
        [HttpPatch("ProductVariants/JsonPatch/{ID}")]
        public async Task<IActionResult> UpdateJsonPatch([FromBody]  JsonPatchDocument<AddProductVariantRow> model,int ID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddProductVariantRow>
                                                           (entity: new AddProductVariantRow(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));

            var q = await _ProductVariantService.GetByIdAsync(ID);
            if (q == null)
                return BadRequest(new ResponseApiEntity<AddProductVariantRow>
                                                           (entity: new AddProductVariantRow(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));

            var ProductVariant = _mapper.Map<AddProductVariantRow>(q);
            model.ApplyTo(ProductVariant);
            _mapper.Map(ProductVariant, q);
            var upd = await _ProductVariantService.UpdateAsync(q);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<AddProductVariantRow>
                                                             (entity: new AddProductVariantRow(),
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }

            else
                return BadRequest(new ResponseApiEntity<AddProductVariantRow>
                                                           (entity: new AddProductVariantRow(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("ProductVariants/UpdateRange")]
        public async Task<IActionResult> UpdateRange([FromBody] List<UpdateProductVariant> models)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<UpdateProductVariant>
                                                           (entities: new List<UpdateProductVariant>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));


            var ProductVariants = _mapper.Map<List<ProductVariant>>(models);
            var c = await _ProductVariantService.UpdateRangeAsync(ProductVariants);
            if (c > 0)
            {
                return Ok(new ResponseApiEntities<UpdateProductVariant>
                                                             (entities:new List<UpdateProductVariant>(),
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }

            else
                return BadRequest(new ResponseApiEntities<UpdateProductVariant>
                                                           (entities: new List<UpdateProductVariant>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("ProductVariants/UpdateRange2")]
        public async Task<IActionResult> UpdateRange2([FromBody] List<AddProductVariantRow> models)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<AddProductVariantRow>
                                                           (entities: new List<AddProductVariantRow>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));


            var ProductVariants = _mapper.Map<List<ProductVariant>>(models);
            var c = await _ProductVariantService.UpdateRangeAsync(ProductVariants);
            if (c > 0)
            {
                return Ok(new ResponseApiEntities<AddProductVariantRow>
                                                             (entities: new List<AddProductVariantRow>(),
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }

            else
                return BadRequest(new ResponseApiEntities<AddProductVariantRow>
                                                           (entities: new List<AddProductVariantRow>(),
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
                        (x.Stock.ToString() ?? "").Contains(search);
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
        [HttpGet("ProductVariants/All")]
        public async Task<IActionResult> GetProductVariantsAll([FromQuery] string? ProductGuid)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProductVariant>
                                                           (entities: new List<ResultProductVariant>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            bool isValid = Guid.TryParse(ProductGuid, out Guid gid);
            if (!isValid)
                return BadRequest(new ResponseApiEntities<ResultProductVariant>
                                                            (entities: new List<ResultProductVariant>(),
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            status: ResultMessageApi.Error,
                                                            message: ResultMessageApi.GetError));

            var q = await _ProductService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == ProductGuid);
            if (q == null)
                return BadRequest(new ResponseApiEntities<ResultProductVariant>
                                                          (entities: new List<ResultProductVariant>(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.GetError));

            var ProductVariants = await _ProductVariantService
                                .GetAllAsync(s => s.ProductID == q.ID);

            var mappedProductVariants = _mapper.Map<ICollection<ResultProductVariant>>(ProductVariants);

            return Ok(new ResponseApiEntities<ResultProductVariant>
                                                            (entities: mappedProductVariants,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: ProductVariants.Count()));

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
