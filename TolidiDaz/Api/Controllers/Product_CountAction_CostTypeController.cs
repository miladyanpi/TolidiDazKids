using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoProduct_CountAction_CostType;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.Product_CountAction_CostTypeSrv;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class Product_CountAction_CostTypeController(
        IProduct_CountAction_CostTypeService _Product_CountAction_CostTypeService,
        IMapper _mapper
        )
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Product_CountAction_CostTypes")]
        public async Task<IActionResult> Add([FromBody] AddProduct_CountAction_CostType model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddProduct_CountAction_CostType>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

           var q=await _Product_CountAction_CostTypeService.FirstOrDefaultAsync(s => s.PositionID == model.PositionID && s.ProductID == model.ProductID);
            if(q != null)
                return BadRequest(new ResponseApiEntity<AddProduct_CountAction_CostType>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DataRepeatError));

            var Product_CountAction_CostType = _mapper.Map<AddProduct_CountAction_CostType, Product_CountAction_CostType>(model);
            int id = await _Product_CountAction_CostTypeService.AddAsync(Product_CountAction_CostType);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddProduct_CountAction_CostType>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddProduct_CountAction_CostType>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Product_CountAction_CostTypes")]
        public async Task<IActionResult> Update([FromBody] UpdateProduct_CountAction_CostType model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateProduct_CountAction_CostType>
                                                           (entity: new UpdateProduct_CountAction_CostType(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));


            var Product_CountAction_CostType = _mapper.Map<UpdateProduct_CountAction_CostType, Product_CountAction_CostType>(model);
            var upd = await _Product_CountAction_CostTypeService.UpdateAsync(Product_CountAction_CostType);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateProduct_CountAction_CostType>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }

            else
                return BadRequest(new ResponseApiEntity<UpdateProduct_CountAction_CostType>
                                                           (entity: new UpdateProduct_CountAction_CostType(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("Product_CountAction_CostTypes/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _Product_CountAction_CostTypeService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultProduct_CountAction_CostType>
                                                               (entity: new ResultProduct_CountAction_CostType(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultProduct_CountAction_CostType>
                                                           (entity: new ResultProduct_CountAction_CostType(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Product_CountAction_CostTypes/Data")]
        public async Task<IActionResult> GetProduct_CountAction_CostTypes([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProduct_CountAction_CostType>
                                                                    (entities: new List<ResultProduct_CountAction_CostType>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<Product_CountAction_CostType, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.CountAction.ToString() ?? "").Contains(search);
                }

                var count = await _Product_CountAction_CostTypeService.GetCountAllAsync(predicate);

                var Product_CountAction_CostTypes = await _Product_CountAction_CostTypeService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedProduct_CountAction_CostTypes = _mapper.Map<ICollection<ResultProduct_CountAction_CostType>>(Product_CountAction_CostTypes);

                return Ok(new ResponseApiEntities<ResultProduct_CountAction_CostType>
                                                                (entities: mappedProduct_CountAction_CostTypes,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultProduct_CountAction_CostType>
                                                                    (entities: new List<ResultProduct_CountAction_CostType>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Product_CountAction_CostTypes")]
        public async Task<IActionResult> GetProduct_CountAction_CostTypes([FromQuery] PaginationParams @params, int? PositionID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultProduct_CountAction_CostType>
                                                           (entity: new ResultProduct_CountAction_CostType(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            var count = await _Product_CountAction_CostTypeService
                                .GetCountAllAsync(s => s.PositionID == PositionID);
            var Product_CountAction_CostTypes = await _Product_CountAction_CostTypeService
                                .GetAllAsync(s => s.PositionID == PositionID
                                , page: @params.Page, take: @params.Take);

            var mappedProduct_CountAction_CostTypes = _mapper.Map<ICollection<ResultProduct_CountAction_CostType>>(Product_CountAction_CostTypes);

            return Ok(new ResponseApiEntities<ResultProduct_CountAction_CostType>
                                                            (entities: mappedProduct_CountAction_CostTypes,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]

        [HttpGet("Product_CountAction_CostTypes/{id}")]
        public async Task<IActionResult> GetProduct_CountAction_CostTypeById([FromRoute] int id)
        {

            var data = await _Product_CountAction_CostTypeService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateProduct_CountAction_CostType>
                                                           (entity: new UpdateProduct_CountAction_CostType(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateProduct_CountAction_CostType>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateProduct_CountAction_CostType>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateProduct_CountAction_CostType>
                                                           (entity: new UpdateProduct_CountAction_CostType(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]

        [HttpGet("Product_CountAction_CostTypes/By")]
        public async Task<IActionResult> GetProduct_CountAction_CostTypesAll([FromQuery] int? PositionID,int? ProductID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProduct_CountAction_CostType>
                                                           (entities: new List<ResultProduct_CountAction_CostType>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var Product_CountAction_CostType =
                await _Product_CountAction_CostTypeService
                                .FirstOrDefaultAsync(s => s.PositionID == PositionID&&s.ProductID== ProductID);

            var mappedProduct_CountAction_CostTypes = _mapper.Map<ResultProduct_CountAction_CostType>(Product_CountAction_CostType);

            return Ok(new ResponseApiEntity<ResultProduct_CountAction_CostType>
                                                            (entity: mappedProduct_CountAction_CostTypes,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Product_CountAction_CostTypes/ByGuid/{guid}")]
        public async Task<IActionResult> GetProduct_CountAction_CostTypeById([FromRoute] string guid)
        {
            try
            {
                var Product_CountAction_CostType = await _Product_CountAction_CostTypeService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (Product_CountAction_CostType == null)
                    return BadRequest(new ResponseApiEntity<UpdateProduct_CountAction_CostType>
                                                                              (entity: new UpdateProduct_CountAction_CostType(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateProduct_CountAction_CostType>(Product_CountAction_CostType);
                return Ok(new ResponseApiEntity<UpdateProduct_CountAction_CostType>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateProduct_CountAction_CostType>
                                                                             (entity: new UpdateProduct_CountAction_CostType(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }
    }
}
