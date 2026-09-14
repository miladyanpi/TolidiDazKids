using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoRegisterCostRawProductStore;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.RegisterCostRawProductStoreSrv;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RegisterCostRawProductStoreController(
        IRegisterCostRawProductStoreService _RegisterCostRawProductStoreService,
        IMapper _mapper
        ) 
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("RegisterCostRawProductStores")]
        public async Task<IActionResult> Add([FromBody] AddRegisterCostRawProductStore model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddRegisterCostRawProductStore>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var RegisterCostRawProductStore = _mapper.Map<AddRegisterCostRawProductStore, RegisterCostRawProductStore>(model);
            int id = await _RegisterCostRawProductStoreService.AddAsync(RegisterCostRawProductStore);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddRegisterCostRawProductStore>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddRegisterCostRawProductStore>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("RegisterCostRawProductStores")]
        public async Task<IActionResult> Update([FromBody] UpdateRegisterCostRawProductStore model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateRegisterCostRawProductStore>
                                                           (entity: new UpdateRegisterCostRawProductStore(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var RegisterCostRawProductStore = _mapper.Map<UpdateRegisterCostRawProductStore, RegisterCostRawProductStore>(model);
            var upd = await _RegisterCostRawProductStoreService.UpdateAsync(RegisterCostRawProductStore);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateRegisterCostRawProductStore>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateRegisterCostRawProductStore>
                                                           (entity: new UpdateRegisterCostRawProductStore(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("RegisterCostRawProductStores/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _RegisterCostRawProductStoreService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultRegisterCostRawProductStore>
                                                               (entity: new ResultRegisterCostRawProductStore(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultRegisterCostRawProductStore>
                                                           (entity: new ResultRegisterCostRawProductStore(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("RegisterCostRawProductStores/Data")]
        public async Task<IActionResult> GetRegisterCostRawProductStores([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultRegisterCostRawProductStore>
                                                                    (entities: new List<ResultRegisterCostRawProductStore>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                //Expression<Func<RegisterCostRawProductStore, bool>> predicate = x => true;

                //if (!string.IsNullOrWhiteSpace(@params.SearchText))
                //{
                //    var search = @params.SearchText;

                //    predicate = x =>
                //        (x.Name ?? "").Contains(search);
                //}

                var count = await _RegisterCostRawProductStoreService.GetCountAllAsync();

                var RegisterCostRawProductStores = await _RegisterCostRawProductStoreService
                                    .GetAllAsync( page: @params.Page, take: @params.Take);


                var mappedRegisterCostRawProductStores = _mapper.Map<ICollection<ResultRegisterCostRawProductStore>>(RegisterCostRawProductStores);

                return Ok(new ResponseApiEntities<ResultRegisterCostRawProductStore>
                                                                (entities: mappedRegisterCostRawProductStores,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultRegisterCostRawProductStore>
                                                                    (entities: new List<ResultRegisterCostRawProductStore>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("RegisterCostRawProductStores")]
        public async Task<IActionResult> GetRegisterCostRawProductStores2([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultRegisterCostRawProductStore>
                                                           (entity: new ResultRegisterCostRawProductStore(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var RegisterCostRawProductStores = await _RegisterCostRawProductStoreService
                                .GetAllAsync(page: @params.Page, take: @params.Take);

            var mappedRegisterCostRawProductStores = _mapper.Map<ICollection<ResultRegisterCostRawProductStore>>(RegisterCostRawProductStores);
     
            return Ok(new ResponseApiEntities<ResultRegisterCostRawProductStore>
                                                            (entities: mappedRegisterCostRawProductStores,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: RegisterCostRawProductStores.Count()));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("RegisterCostRawProductStores/{id}")]
        public async Task<IActionResult> GetRegisterCostRawProductStoreById([FromRoute] int id)
        {

            var story = await _RegisterCostRawProductStoreService.GetByIdAsync(id);
            if (story == null)
                return BadRequest(new ResponseApiEntity<UpdateRegisterCostRawProductStore>
                                                           (entity: new UpdateRegisterCostRawProductStore(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateRegisterCostRawProductStore>(story);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateRegisterCostRawProductStore>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateRegisterCostRawProductStore>
                                                           (entity: new UpdateRegisterCostRawProductStore(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("RegisterCostRawProductStores/ByGuid/{guid}")]
        public async Task<IActionResult> GetRegisterCostRawProductStoreById([FromRoute] string guid)
        {
            try
            {
                var RegisterCostRawProductStore = await _RegisterCostRawProductStoreService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (RegisterCostRawProductStore == null)
                    return BadRequest(new ResponseApiEntity<UpdateRegisterCostRawProductStore>
                                                                              (entity: new UpdateRegisterCostRawProductStore(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateRegisterCostRawProductStore>(RegisterCostRawProductStore);
                return Ok(new ResponseApiEntity<UpdateRegisterCostRawProductStore>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateRegisterCostRawProductStore>
                                                                             (entity: new UpdateRegisterCostRawProductStore(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }
    }
}
