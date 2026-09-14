using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.ProductFeatureValueSrv;
using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoProductFeature;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductFeatureValueController(
        IProductFeatureValueService _ProductFeatureValueService,
        IMapper _mapper
        //UserManager<Account> userManager
        ) 
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("ProductFeatureValues")]
        public async Task<IActionResult> Add([FromBody] AddProductFeatureValue model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddProductFeatureValue>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var ProductFeatureValue = _mapper.Map<AddProductFeatureValue, ProductFeatureValue>(model);
            int id = await _ProductFeatureValueService.AddAsync(ProductFeatureValue);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddProductFeatureValue>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddProductFeatureValue>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("ProductFeatureValues/List")]
        public async Task<IActionResult> Add([FromBody] List<AddProductFeatureValue> models)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<AddProductFeatureValue>
                                                           (entities: new List<AddProductFeatureValue>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var mapModels = _mapper.Map<List<AddProductFeatureValue>, List<ProductFeatureValue>>(models);
            var ids = await _ProductFeatureValueService.AddRangeAsync(mapModels);

            if (ids.Count > 0)
                return Ok(new ResponseApiEntities<AddProductFeatureValue>
                                                               (entities: models,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntities<AddProductFeatureValue>
                                                           (entities: new List<AddProductFeatureValue>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("ProductFeatureValues")]
        public async Task<IActionResult> Update([FromBody] UpdateProductFeatureValue model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateProductFeatureValue>
                                                           (entity: new UpdateProductFeatureValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var ProductFeatureValue = _mapper.Map<UpdateProductFeatureValue, ProductFeatureValue>(model);
            var upd = await _ProductFeatureValueService.UpdateAsync(ProductFeatureValue);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateProductFeatureValue>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateProductFeatureValue>
                                                           (entity: new UpdateProductFeatureValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("ProductFeatureValues/list")]
        public async Task<IActionResult> UpdateList([FromBody] List<UpdateProductFeatureValue> models)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<UpdateProductFeatureValue>
                                                           (entities: new List<UpdateProductFeatureValue>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));


            var ProductFeatureValue = _mapper.Map<List<UpdateProductFeatureValue>, List<ProductFeatureValue>>(models);
            var upd = await _ProductFeatureValueService.UpdateRangeAsync(ProductFeatureValue);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntities<UpdateProductFeatureValue>
                                                             (entities: models,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }

            else
                return BadRequest(new ResponseApiEntities<UpdateProductFeatureValue>
                                                           (entities: new List<UpdateProductFeatureValue>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("ProductFeatureValues/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _ProductFeatureValueService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultProductFeatureValue>
                                                               (entity: new ResultProductFeatureValue(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultProductFeatureValue>
                                                           (entity: new ResultProductFeatureValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductFeatureValues")]
        public async Task<IActionResult> GetProductFeatureValues([FromQuery] PaginationParams @params,int? ProductID=null)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _ProductFeatureValueService.GetCountAllAsync(s =>s.ProductID == ProductID &&
                                s.Value.Contains(@params.SearchText) );
            var ProductFeatureValues = await _ProductFeatureValueService
                                .GetAllAsync(s => s.ProductID == ProductID &&
                                s.Value.Contains(@params.SearchText) 
                                , page: @params.Page, take: @params.Take);

            var mappedProductFeatureValues = _mapper.Map<ICollection<ResultProductFeatureValue>>(ProductFeatureValues);
     
            return Ok(new ResponseApiEntities<ResultProductFeatureValue>
                                                            (entities: mappedProductFeatureValues,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductFeatureValues/{id}")]
        public async Task<IActionResult> GetProductFeatureValueById([FromRoute] int id)
        {

            var ProductFeatureValue = await _ProductFeatureValueService.GetByIdAsync(id);
            if(ProductFeatureValue==null)
                return BadRequest(new ResponseApiEntity<UpdateProductFeatureValue>
                                                           (entity: new UpdateProductFeatureValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateProductFeatureValue>(ProductFeatureValue);
      
            if (result != null)
                return Ok(new ResponseApiEntity<UpdateProductFeatureValue>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateProductFeatureValue>
                                                           (entity: new UpdateProductFeatureValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductFeatureValues/All/{ProductID}")]
        public async Task<IActionResult> GetProductFeatureValuesAll([FromRoute] int? ProductID=null)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseApiEntity<UpdateProductFeatureValue>
                                                           (entity: new UpdateProductFeatureValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var ProductFeatureValues = await _ProductFeatureValueService
                                .GetAllAsync(s =>s.ProductID==ProductID );
            var mappedProductFeatureValues = _mapper.Map<ICollection<UpdateProductFeatureValue>>(ProductFeatureValues);

            return Ok(new ResponseApiEntities<UpdateProductFeatureValue>
                                                            (entities: mappedProductFeatureValues,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: ProductFeatureValues.Count()));

        }
        [HttpGet("ProductFeatureValues/Public/All/{ProductID}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductFeatureValuesAllPublic([FromRoute] int? ProductID = null)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseApiEntities<ResultProductFeatureValue>
                                                           (entities: new List<ResultProductFeatureValue>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
          
            var ProductFeatureValues = await _ProductFeatureValueService
                                .GetAllAsync(s => s.ProductID == ProductID);
            var mappedProductFeatureValues = _mapper.Map<ICollection<ResultProductFeatureValue>>(ProductFeatureValues);

            return Ok(new ResponseApiEntities<ResultProductFeatureValue>
                                                            (entities: mappedProductFeatureValues,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: ProductFeatureValues.Count()));

        }
    }
}
