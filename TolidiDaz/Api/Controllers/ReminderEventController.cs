using Api.Models.DtoReminderEvent;
using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Models.Constant;
using Dto.Models.ResponseApi;
using Dto.Services.ReminderEventSrv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.SettingSrv;
using System.Linq.Expressions;
using static Dto.Enum.EnumConstant;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReminderEventController(
        IReminderEventService _ReminderEventService,
        IMapper _mapper
        )
        : ControllerBase
    {
        [HttpPost("ReminderEvents")]
        public async Task<IActionResult> Add([FromBody] AddReminderEvent model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var ReminderEvent = _mapper.Map<AddReminderEvent, ReminderEvent>(model);
            int id = await _ReminderEventService.AddAsync(ReminderEvent);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddReminderEvent>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddReminderEvent>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }

        [HttpPatch("ReminderEvents")]
        public async Task<IActionResult> Update([FromBody] UpdateReminderEvent model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var ReminderEvent = _mapper.Map<UpdateReminderEvent, ReminderEvent>(model);
            var upd = await _ReminderEventService.UpdateAsync(ReminderEvent);
            if (upd > 0)
                return Ok(new ResponseApiEntity<UpdateReminderEvent>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateReminderEvent>
                                                           (entity: new UpdateReminderEvent(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [HttpDelete("ReminderEvents/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _ReminderEventService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultReminderEvent>
                                                               (entity: new ResultReminderEvent(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultReminderEvent>
                                                           (entity: new ResultReminderEvent(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [HttpPost("ReminderEvents/Data")]
        public async Task<IActionResult> GetReminderEvents([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultReminderEvent>
                                                                    (entities: new List<ResultReminderEvent>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<ReminderEvent, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Title ?? "").Contains(search);
                }

                var count = await _ReminderEventService.GetCountAllAsync(predicate);

                var ReminderEvents = await _ReminderEventService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedReminderEvents = _mapper.Map<ICollection<ResultReminderEvent>>(ReminderEvents);

                return Ok(new ResponseApiEntities<ResultReminderEvent>
                                                                (entities: mappedReminderEvents,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultReminderEvent>
                                                                    (entities: new List<ResultReminderEvent>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [HttpGet("ReminderEvents")]
        public async Task<IActionResult> GetReminderEvents2([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _ReminderEventService
                           .GetCountAllAsync();
            var ReminderEvents = await _ReminderEventService
                                 .GetAllAsync(page: @params.Page, take: @params.Take);
            var mappedReminderEvents = _mapper.Map<ICollection<ResultReminderEvent>>(ReminderEvents);

            return Ok(new ResponseApiEntities<ResultReminderEvent>
                                                            (entities: mappedReminderEvents,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));
        }
        [HttpGet("ReminderEventsByReminderType")]
        public async Task<IActionResult> GetReminderEvents([FromQuery] ReminderType reminderType)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _ReminderEventService
                           .GetCountAllAsync(s => s.ReminderType == reminderType);
            var ReminderEvents = await _ReminderEventService
                                 .GetAllAsync(s => s.ReminderType == reminderType);
            var mappedReminderEvents = _mapper.Map<ICollection<ResultReminderEvent>>(ReminderEvents);

            return BadRequest(new ResponseApiEntities<ResultReminderEvent>
                                                            (entities: mappedReminderEvents,
                                                            status: ResultMessageApi.Error,
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            message: ResultMessageApi.ErrorReminderType,
                                                            countAllRecordTable: count));
        }
        [HttpGet("ReminderEvents/{id}")]
        public async Task<IActionResult> GetReminderEventById([FromRoute] int id)
        {
            var ReminderEvent = await _ReminderEventService.GetByIdAsync(id);
            var result = _mapper.Map<UpdateReminderEvent>(ReminderEvent);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateReminderEvent>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateReminderEvent>
                                                           (entity: new UpdateReminderEvent(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

        [HttpGet("ReminderEvents/ByGuid/{guid}")]
        public async Task<IActionResult> GetReminderEventById([FromRoute] string guid)
        {
            try
            {
                var ReminderEvent = await _ReminderEventService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (ReminderEvent == null)
                    return BadRequest(new ResponseApiEntity<UpdateReminderEvent>
                                                                              (entity: new UpdateReminderEvent(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateReminderEvent>(ReminderEvent);
                return Ok(new ResponseApiEntity<UpdateReminderEvent>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateReminderEvent>
                                                                             (entity: new UpdateReminderEvent(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }

    }
}