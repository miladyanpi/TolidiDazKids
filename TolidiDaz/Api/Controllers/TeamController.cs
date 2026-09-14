using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoTeam;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.TeamSrv;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TeamController(
        ITeamService _TeamService,
        IMapper _mapper
        )
        : ControllerBase
    {
        [HttpPost("Teams")]
        public async Task<IActionResult> Add([FromBody] AddTeam model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var Team = _mapper.Map<AddTeam, Team>(model);
            int id = await _TeamService.AddAsync(Team);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddTeam> (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddTeam>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }

        [HttpPatch("Teams")]
        public async Task<IActionResult> Update([FromBody] UpdateTeam model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var Team = _mapper.Map<UpdateTeam, Team>(model);
            var upd = await _TeamService.UpdateAsync(Team);
            if (upd > 0)
                return Ok(new ResponseApiEntity<UpdateTeam>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateTeam>
                                                           (entity: new UpdateTeam(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [HttpDelete("Teams/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

  
            var del = await _TeamService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultTeam>
                                                               (entity: new ResultTeam(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultTeam>
                                                           (entity: new ResultTeam(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [HttpGet("Teams")]
        public async Task<IActionResult> GetTeams([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultTeam>
                                                           (entity: new ResultTeam(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));

            var count = await _TeamService.GetCountAllAsync(s =>
                                 s.Name.Contains(@params.SearchText) ||
                                   s.Title.Contains(@params.SearchText));
            var Teams = await _TeamService
                                .GetAllAsync(s =>
                                s.Name.Contains(@params.SearchText) ||
                                   s.Title.Contains(@params.SearchText)
                                , take: @params.Take, page:@params.Page);
            var mappedTeams = _mapper.Map<ICollection<ResultTeam>>(Teams);

            return Ok(new ResponseApiEntities<ResultTeam>
                                                            (entities: mappedTeams,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }


        [HttpGet("Teams/{id}")]
        public async Task<IActionResult> GetTeamById([FromRoute] int id)
        {

            var Team = await _TeamService.GetByIdAsync(id);
            var result = _mapper.Map<UpdateTeam>(Team);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateTeam>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateTeam>
                                                           (entity: new UpdateTeam(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Teams/UpdateJsonFile")]
        public async Task<IActionResult> UpdateUpdateCustomerJsonFile([FromBody] UpdateJsonFile model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var q = await _TeamService.GetByIdAsync(model.ID);
            if (q == null)
                return BadRequest(new ResponseApiEntity<UpdateJsonFile>
                                                         (entity: new UpdateJsonFile(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.UpdateError));
            q.JsonPictures = model.JsonPicture;
            // var customer = _mapper.Map<UpdateCustomer, Customer>(q);
            var upd = await _TeamService.UpdateAsync(q);
            if (upd > 0)
                return Ok(new ResponseApiEntity<UpdateJsonFile>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateJsonFile>
                                                           (entity: new UpdateJsonFile(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }

        [HttpGet("Teams/All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTeamsAll()
        {
            if (!ModelState.IsValid) return BadRequest();
            var Teams = await _TeamService
                                .GetAllAsync(s =>s.ShowInAbout==true&&
                                s.Visible==true);
            var mappedTeams = _mapper.Map<ICollection<ResultTeam>>(Teams);

            return Ok(new ResponseApiEntities<ResultTeam>
                                                            (entities: mappedTeams,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Teams.Count()));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Teams/GetForBlogPage")]
        public async Task<IActionResult> GetTeamsAllGetForBlogPage()
        {
            if (!ModelState.IsValid) return BadRequest();
            var Teams = await _TeamService
                                .GetAllAsync(s =>
                                s.Visible == true);
            var mappedTeams = _mapper.Map<ICollection<ResultTeam>>(Teams);

            return Ok(new ResponseApiEntities<ResultTeam>
                                                            (entities: mappedTeams,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Teams.Count()));

        }
    }
}
