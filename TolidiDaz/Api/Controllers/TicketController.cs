using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoTicket;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.TicketSrv;
using ServicesLibrary.Services.DepartmentSrv;
using ServicesLibrary.Services.ViewCounter;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TicketController(
        ITicketService _TicketService,
        IDepartmentService _DepartmentService,
        IMapper _mapper,
        IViewCounterService _viewCounterService
        //UserManager<Account> userManager
        ) 
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Tickets")]
        public async Task<IActionResult> Add([FromBody] AddTicket model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddTicket>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var Ticket = _mapper.Map<AddTicket, Ticket>(model);
            int id = await _TicketService.AddAsync(Ticket);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddTicket>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddTicket>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Tickets")]
        public async Task<IActionResult> Update([FromBody] UpdateTicket model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateTicket>
                                                           (entity: new UpdateTicket(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));


            var Ticket = _mapper.Map<UpdateTicket, Ticket>(model);
            var upd = await _TicketService.UpdateAsync(Ticket);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateTicket>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }

            else
                return BadRequest(new ResponseApiEntity<UpdateTicket>
                                                           (entity: new UpdateTicket(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("Tickets/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _TicketService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultTicket>
                                                               (entity: new ResultTicket(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultTicket>
                                                           (entity: new ResultTicket(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName + "," + ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Tickets/{id}")]
        public async Task<IActionResult> GetTicketById([FromRoute] int id)
        {

            var data = await _TicketService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateTicket>
                                                           (entity: new UpdateTicket(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateTicket>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateTicket>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateTicket>
                                                           (entity: new UpdateTicket(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Tickets")]
        public async Task<IActionResult> GetTickets([FromQuery] PaginationParams @params, int? DepartmentID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultTicket>
                                                           (entity: new ResultTicket(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            Expression<Func<Ticket, bool>> predicate = x => true;
            if (!string.IsNullOrWhiteSpace(@params.SearchText))
            {
                var search = @params.SearchText;
                predicate = x =>
                    x.Title.Contains(search);
            }
            if (DepartmentID != null && DepartmentID > 0)
            {
                predicate = x =>
                    x.DepartmentID == DepartmentID;
            }
            var count = await _TicketService.GetCountAllAsync(predicate);
            var Tickets = await _TicketService
                                   .GetAllAsync(predicate
                                   , page: @params.Page, take: @params.Take);


            var mappedTickets = _mapper.Map<ICollection<ResultTicket>>(Tickets);

            return Ok(new ResponseApiEntities<ResultTicket>
                                                            (entities: mappedTickets,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [HttpGet("Tickets/Public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTicketsPublic([FromQuery] PaginationParams @params, int? DepartmentID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultTicket>
                                                           (entity: new ResultTicket(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            Expression<Func<Ticket, bool>> predicate = x => true;
            if (!string.IsNullOrWhiteSpace(@params.SearchText))
            {
                var search = @params.SearchText;
                predicate = x =>
                    x.Title.Contains(search);
            }
            if (DepartmentID != null && DepartmentID > 0)
            {
                predicate = x =>
                    x.DepartmentID == DepartmentID;
            }
            var count = await _TicketService.GetCountAllAsync(predicate);
            var Tickets = await _TicketService
                                   .GetAllAsync(predicate
                                   , page: @params.Page, take: @params.Take);

            var mappedTickets = _mapper.Map<ICollection<ResultTicket>>(Tickets);

            return Ok(new ResponseApiEntities<ResultTicket>
                                                            (entities: mappedTickets,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }

    }
}
