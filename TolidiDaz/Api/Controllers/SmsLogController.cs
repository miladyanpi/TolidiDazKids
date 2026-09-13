using AutoMapper;
using DAL.Paginagion;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Api.Models.DtoSmsLog;
using ServicesLibrary.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using DAL.Context;
using ServicesLibrary.Services.SettingSrv;
using Dto.Models.DtoSmsLog;
using Dto.Models.ResponseApi;
using Dto.Models.Constant;
using Dto.Services.SmsLogSrv;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SmsLogController : ControllerBase
    {
        private readonly ISmsLogService _SmsLogService;
        private readonly IMapper _mapper;
        private readonly UserManager<Account> _userManager;
        private readonly ISettingService _settingService;

        public SmsLogController(
            ISmsLogService SmsLogService,
            IMapper mapper,
            UserManager<Account> userManager,
            ISettingService settingService
            )
        {
            _SmsLogService = SmsLogService;
            _mapper = mapper;
            _userManager = userManager;
            _settingService = settingService;
        }
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
        [HttpGet("SmsLogs")]
        public async Task<IActionResult> GetSmsLogs([FromQuery] PaginationParams @params)
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


    }
}