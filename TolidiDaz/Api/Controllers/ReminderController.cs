using AutoMapper;
using DAL.Paginagion;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Dto.Services.ReminderSrv;
using ServicesLibrary.Services.SettingSrv;
using Dto.Models.DtoReminderSrv;
using Dto.Models.ResponseApi;
using Dto.Models.Constant;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ReminderController(
        IReminderService _ReminderService,
        IMapper _mapper,
        ISettingService _settingService,
        UserManager<Account> _userManager
        )
        : ControllerBase
    {
        [HttpPost("Reminders")]
        public async Task<IActionResult> Add([FromBody] AddReminder model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var Reminder = _mapper.Map<AddReminder, Reminder>(model);
            int id = await _ReminderService.AddAsync(Reminder);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddReminder>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddReminder>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }

        [HttpPatch("Reminders")]
        public async Task<IActionResult> Update([FromBody] UpdateReminder model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var Reminder = _mapper.Map<UpdateReminder, Reminder>(model);
            var upd = await _ReminderService.UpdateAsync(Reminder);
            if (upd > 0)
                return Ok(new ResponseApiEntity<UpdateReminder>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateReminder>
                                                           (entity: new UpdateReminder(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [HttpDelete("Reminders/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _ReminderService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultReminder>
                                                               (entity: new ResultReminder(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultReminder>
                                                           (entity: new ResultReminder(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [HttpGet("Reminders")]
        public async Task<IActionResult> GetReminders([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _ReminderService
                           .GetCountAllAsync();
            var Reminders = await _ReminderService
                                 .GetAllAsync(page:@params.Page,take: @params.Take);
            var mappedReminders = _mapper.Map<ICollection<ResultReminder>>(Reminders);

            return Ok(new ResponseApiEntities<ResultReminder>
                                                            (entities: mappedReminders,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));
        }
        [HttpGet("Reminders/{id}")]
        public async Task<IActionResult> GetReminderById([FromRoute] int id)
        {
            var Reminder = await _ReminderService.GetByIdAsync(id);
            var result = _mapper.Map<UpdateReminder>(Reminder);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateReminder>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateReminder>
                                                           (entity: new UpdateReminder(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }


    }
}