using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoTraitValue;
using Dto.Models.ResponseApi;
using LinqKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.TraitSrv;
using ServicesLibrary.Services.TraitValueSrv;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TraitValueController(
        ITraitService _TraitService,
        ITraitValueService _TraitValueService,
        IMapper _mapper
        )
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("TraitValues")]
        public async Task<IActionResult> Add([FromBody] AddTraitValue model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddTraitValue>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var TraitValue = _mapper.Map<AddTraitValue, TraitValue>(model);
            int id = await _TraitValueService.AddAsync(TraitValue);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddTraitValue>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddTraitValue>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("TraitValues")]
        public async Task<IActionResult> Update([FromBody] UpdateTraitValue model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateTraitValue>
                                                           (entity: new UpdateTraitValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var TraitValue = _mapper.Map<UpdateTraitValue, TraitValue>(model);
            var upd = await _TraitValueService.UpdateAsync(TraitValue);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateTraitValue>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateTraitValue>
                                                           (entity: new UpdateTraitValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("TraitValues/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _TraitValueService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultTraitValue>
                                                               (entity: new ResultTraitValue(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultTraitValue>
                                                           (entity: new ResultTraitValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("TraitValues/Data")]
        public async Task<IActionResult> GetTraitValues([FromBody] PaginationParams @params,string guid)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultTraitValue>
                                                                    (entities: new List<ResultTraitValue>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));
                bool isValid = Guid.TryParse(guid, out Guid gid);
                if (!isValid)
                    return BadRequest(new ResponseApiEntities<ResultTraitValue>
                                                                    (entities: new List<ResultTraitValue>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                var q = await _TraitService.FirstOrDefaultAsync(s => s.IdentityCode == gid);
                if (q is null)
                    return BadRequest(new ResponseApiEntities<ResultTraitValue>
                                                                    (entities: new List<ResultTraitValue>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<TraitValue, bool>> predicate = x => true;
                predicate = predicate.And(s => s.TraitID == q.ID);
                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Value ?? "").Contains(search);
                }

                var count = await _TraitValueService.GetCountAllAsync(predicate);

                var TraitValues = await _TraitValueService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedTraitValues = _mapper.Map<ICollection<ResultTraitValue>>(TraitValues);

                return Ok(new ResponseApiEntities<ResultTraitValue>
                                                                (entities: mappedTraitValues,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultTraitValue>
                                                                    (entities: new List<ResultTraitValue>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("TraitValues")]
        public async Task<IActionResult> GetTraitValues([FromQuery] PaginationParams @params,int? TraitID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultTraitValue>
                                                           (entity: new ResultTraitValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            int count = 0;
            IEnumerable<TraitValue> TraitValues =new List<TraitValue>();
            if (TraitID == null)
            {
                 count = await _TraitValueService.GetCountAllAsync(s => 
                               s.Value.Contains(@params.SearchText));
                 TraitValues = await _TraitValueService
                                    .GetAllAsync(s => 
                                    s.Value.Contains(@params.SearchText)
                                    , page: @params.Page, take: @params.Take);
            }
            else
            {
               count = await _TraitValueService.GetCountAllAsync(s => s.TraitID == TraitID &&
                              s.Value.Contains(@params.SearchText));
                TraitValues = await _TraitValueService
                                    .GetAllAsync(s => s.TraitID == TraitID &&
                                    s.Value.Contains(@params.SearchText)
                                    , page: @params.Page, take: @params.Take);
            }


            var mappedTraitValues = _mapper.Map<ICollection<ResultTraitValue>>(TraitValues);
            return Ok(new ResponseApiEntities<ResultTraitValue>
                                                            (entities: mappedTraitValues,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName + "," + ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("TraitValues/{id}")]
        public async Task<IActionResult> GetTraitValueById([FromRoute] int id)
        {

            var data = await _TraitValueService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateTraitValue>
                                                           (entity: new UpdateTraitValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateTraitValue>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateTraitValue>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateTraitValue>
                                                           (entity: new UpdateTraitValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("TraitValues/All")]
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName +  "," + ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        public async Task<IActionResult> GetTraitValuesAll([FromQuery]int? TraitID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultTraitValue>
                                                           (entity: new ResultTraitValue(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var TraitValues = await _TraitValueService
                                .GetAllAsync(s=>s.TraitID== TraitID);

            var mappedTraitValues = _mapper.Map<ICollection<ResultTraitValue>>(TraitValues);

            return Ok(new ResponseApiEntities<ResultTraitValue>
                                                            (entities: mappedTraitValues,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: TraitValues.Count()));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("TraitValues/ByGuid/{guid}")]
        public async Task<IActionResult> GetTraitValueById([FromRoute] string guid)
        {
            try
            {
                var TraitValue = await _TraitValueService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (TraitValue == null)
                    return BadRequest(new ResponseApiEntity<UpdateTraitValue>
                                                                              (entity: new UpdateTraitValue(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateTraitValue>(TraitValue);
                return Ok(new ResponseApiEntity<UpdateTraitValue>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateTraitValue>
                                                                             (entity: new UpdateTraitValue(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }
    }
}
