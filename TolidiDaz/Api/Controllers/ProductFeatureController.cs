using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoProductFeature;
using Dto.Models.ResponseApi;
using LinqKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.ProductFeatureSrv;
using ServicesLibrary.Services.ProductFeatureValueSrv;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductFeatureController(
        IProductFeatureService _ProductFeatureService,
        IProductFeatureValueService _ProductFeatureValueService,
        IMapper _mapper
        )
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("ProductFeatures")]
        public async Task<IActionResult> Add([FromBody] AddProductFeature model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddProductFeature>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
            if (model.CategoryID is null || model.CategoryID==0) return BadRequest(new ResponseApiEntity<AddProductFeature>
                                                          (entity: null,
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: "دسته بندی را انتخاب کنید"));
            var story = _mapper.Map<AddProductFeature, ProductFeature>(model);
            int id = await _ProductFeatureService.AddAsync(story);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddProductFeature>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddProductFeature>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("ProductFeatures")]
        public async Task<IActionResult> Update([FromBody] UpdateProductFeature model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateProductFeature>
                                                           (entity: new UpdateProductFeature(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var story = _mapper.Map<UpdateProductFeature, ProductFeature>(model);
            var upd = await _ProductFeatureService.UpdateAsync(story);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateProductFeature>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateProductFeature>
                                                           (entity: new UpdateProductFeature(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("ProductFeatures/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var q = await _ProductFeatureValueService.GetFirstOrDefaultAsync(s=>s.ProductFeatureID==id);
            if(q is not null)
                return BadRequest(new ResponseApiEntity<ResultProductFeature>
                                                          (entity: new ResultProductFeature(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.NotAllowDeleteError));

            var del = await _ProductFeatureService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultProductFeature>
                                                               (entity: new ResultProductFeature(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultProductFeature>
                                                           (entity: new ResultProductFeature(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("ProductFeatures/Data")]
        public async Task<IActionResult> GetProductFeatures([FromBody] PaginationParams @params,int? CategoryID=null)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProductFeature>
                                                                    (entities: new List<ResultProductFeature>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<ProductFeature, bool>> predicate = x => true;

                predicate= predicate.And(s=>s.CategoryID== CategoryID);
                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Title ?? "").Contains(search);
                }

                var count = await _ProductFeatureService.GetCountAllAsync(predicate);

                var ProductFeatures = await _ProductFeatureService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedProductFeatures = _mapper.Map<ICollection<ResultProductFeature>>(ProductFeatures);

                return Ok(new ResponseApiEntities<ResultProductFeature>
                                                                (entities: mappedProductFeatures,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultProductFeature>
                                                                    (entities: new List<ResultProductFeature>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductFeatures")]
        public async Task<IActionResult> GetProductFeatures2([FromQuery] PaginationParams @params,int? CategoryID=null)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _ProductFeatureService.GetCountAllAsync(s =>s.CategoryID==CategoryID &&
                                s.Title.Contains(@params.SearchText) );
            var ProductFeatures = await _ProductFeatureService
                                .GetAllAsync(s => s.CategoryID == CategoryID &&
                                s.Title.Contains(@params.SearchText) 
                                , page: @params.Page, take: @params.Take);

            var mappedProductFeatures = _mapper.Map<ICollection<ResultProductFeature>>(ProductFeatures);
     
            return Ok(new ResponseApiEntities<ResultProductFeature>
                                                            (entities: mappedProductFeatures,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductFeatures/{id}")]
        public async Task<IActionResult> GetProductFeatureById([FromRoute] int id)
        {

            var story = await _ProductFeatureService.GetByIdAsync(id);
            if(story==null)
                return BadRequest(new ResponseApiEntity<UpdateProductFeature>
                                                           (entity: new UpdateProductFeature(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateProductFeature>(story);
      
            if (result != null)
                return Ok(new ResponseApiEntity<UpdateProductFeature>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateProductFeature>
                                                           (entity: new UpdateProductFeature(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductFeatures/All/{CategoryID}")]
        public async Task<IActionResult> GetProductFeaturesAll([FromRoute] int? CategoryID = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseApiEntities<ResultProductFeature>
                                                           (entities: new List<ResultProductFeature>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var ProductFeatures = await _ProductFeatureService
                                .GetAllAsync(s =>s.CategoryID==CategoryID&& s.Visible == true);
            var mappedProductFeatures = _mapper.Map<ICollection<ResultProductFeature>>(ProductFeatures);

            return Ok(new ResponseApiEntities<ResultProductFeature>
                                                            (entities: mappedProductFeatures,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: ProductFeatures.Count()));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductFeatures/ByGuid/{guid}")]
        public async Task<IActionResult> GetProductFeatureById([FromRoute] string guid)
        {
            try
            {
                var ProductFeature = await _ProductFeatureService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (ProductFeature == null)
                    return BadRequest(new ResponseApiEntity<UpdateProductFeature>
                                                                              (entity: new UpdateProductFeature(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateProductFeature>(ProductFeature);
                return Ok(new ResponseApiEntity<UpdateProductFeature>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateProductFeature>
                                                                             (entity: new UpdateProductFeature(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }
    }
}
