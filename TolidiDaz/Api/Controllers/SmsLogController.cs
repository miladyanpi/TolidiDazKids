using Api.Models.DtoSmsLog;
using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Models.Constant;
using Dto.Models.DtoSmsLog;
using Dto.Models.ResponseApi;
using Dto.Services.SmsLogSrv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.SettingSrv;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SmsLogController(
        ISmsLogService _SmsLogService,
        IMapper _mapper
        ) 
        : ControllerBase
    {
        [HttpPost("SmsLogs")]
        public async Task<IActionResult> Add([FromBody] AddSmsLog model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var SmsLog = _mapper.Map<AddSmsLog, SmsLog>(model);
            int id = await _SmsLogService.AddAsync(SmsLog);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddSmsLog>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddSmsLog>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }

        [HttpPatch("SmsLogs")]
        public async Task<IActionResult> Update([FromBody] UpdateSmsLog model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var SmsLog = _mapper.Map<UpdateSmsLog, SmsLog>(model);
            var upd = await _SmsLogService.UpdateAsync(SmsLog);
            if (upd > 0)
                return Ok(new ResponseApiEntity<UpdateSmsLog>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateSmsLog>
                                                           (entity: new UpdateSmsLog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [HttpDelete("SmsLogs/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _SmsLogService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultSmsLog>
                                                               (entity: new ResultSmsLog(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultSmsLog>
                                                           (entity: new ResultSmsLog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [HttpGet("SmsLogsByCustomerID")]
        public async Task<IActionResult> GetSmsLogs([FromQuery] PaginationParams @params, int CustomerID)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _SmsLogService
                           .GetCountAllAsync(s => s.CustomerID == CustomerID);
            var SmsLogs = await _SmsLogService
                                 .GetAllAsync(s => s.CustomerID == CustomerID, page: @params.Page,take: @params.Take);
            var mappedSmsLogs = _mapper.Map<ICollection<ResultSmsLog>>(SmsLogs);

            return Ok(new ResponseApiEntities<ResultSmsLog>
                                                            (entities: mappedSmsLogs,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));
        }

        [HttpPost("SmsLogs/Data")]
        public async Task<IActionResult> GetSmsLogs([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultSmsLog>
                                                                    (entities: new List<ResultSmsLog>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<SmsLog, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Title ?? "").Contains(search);
                }

                var count = await _SmsLogService.GetCountAllAsync(predicate);

                var SmsLogs = await _SmsLogService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedSmsLogs = _mapper.Map<ICollection<ResultSmsLog>>(SmsLogs);

                return Ok(new ResponseApiEntities<ResultSmsLog>
                                                                (entities: mappedSmsLogs,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultSmsLog>
                                                                    (entities: new List<ResultSmsLog>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [HttpGet("SmsLogs")]
        public async Task<IActionResult> GetSmsLogs2([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _SmsLogService
                           .GetCountAllAsync();
            var SmsLogs = await _SmsLogService
                                 .GetAllAsync(page: @params.Page, take: @params.Take);
            var mappedSmsLogs = _mapper.Map<ICollection<ResultSmsLog>>(SmsLogs);

            return Ok(new ResponseApiEntities<ResultSmsLog>
                                                            (entities: mappedSmsLogs,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));
        }
        [HttpGet("SmsLogs/{id}")]
        public async Task<IActionResult> GetSmsLogById([FromRoute] int id)
        {
            var SmsLog = await _SmsLogService.GetByIdAsync(id);
            var result = _mapper.Map<UpdateSmsLog>(SmsLog);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateSmsLog>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateSmsLog>
                                                           (entity: new UpdateSmsLog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

        [HttpGet("SmsLogs/ByGuid/{guid}")]
        public async Task<IActionResult> GetSmsLogById([FromRoute] string guid)
        {
            try
            {
                var SmsLog = await _SmsLogService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (SmsLog == null)
                    return BadRequest(new ResponseApiEntity<UpdateSmsLog>
                                                                              (entity: new UpdateSmsLog(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateSmsLog>(SmsLog);
                return Ok(new ResponseApiEntity<UpdateSmsLog>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateSmsLog>
                                                                             (entity: new UpdateSmsLog(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }
    }
}