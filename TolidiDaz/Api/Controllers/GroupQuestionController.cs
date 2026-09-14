using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoGroupQuestion;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.GroupQuestionSrv;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GroupQuestionController(
        IGroupQuestionService _GroupQuestionService,
        IMapper _mapper,
        UserManager<Account> userManager
        )
        : ControllerBase
    {
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("GroupQuestions")]
        public async Task<IActionResult> Add([FromBody] AddGroupQuestion model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddGroupQuestion>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var data = _mapper.Map<AddGroupQuestion, GroupQuestion>(model);
            int id = await _GroupQuestionService.AddAsync(data);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddGroupQuestion>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddGroupQuestion>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("GroupQuestions")]
        public async Task<IActionResult> Update([FromBody] UpdateGroupQuestion model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateGroupQuestion>
                                                           (entity: new UpdateGroupQuestion(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));


            var data = _mapper.Map<UpdateGroupQuestion, GroupQuestion>(model);
            var upd = await _GroupQuestionService.UpdateAsync(data);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateGroupQuestion>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }

            else
                return BadRequest(new ResponseApiEntity<UpdateGroupQuestion>
                                                           (entity: new UpdateGroupQuestion(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("GroupQuestions/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _GroupQuestionService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultGroupQuestion>
                                                               (entity: new ResultGroupQuestion(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultGroupQuestion>
                                                           (entity: new ResultGroupQuestion(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [HttpPost("GroupQuestions/Data")]
        public async Task<IActionResult> GetGroupQuestions([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultGroupQuestion>
                                                                    (entities: new List<ResultGroupQuestion>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<GroupQuestion, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Title ?? "").Contains(search);
                }

                var count = await _GroupQuestionService.GetCountAllAsync(predicate);

                var GroupQuestions = await _GroupQuestionService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedGroupQuestions = _mapper.Map<ICollection<ResultGroupQuestion>>(GroupQuestions);

                return Ok(new ResponseApiEntities<ResultGroupQuestion>
                                                                (entities: mappedGroupQuestions,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultGroupQuestion>
                                                                    (entities: new List<ResultGroupQuestion>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("GroupQuestions")]
        public async Task<IActionResult> GetGroupQuestions2([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _GroupQuestionService.GetCountAllAsync(s =>
                                s.Title.Contains(@params.SearchText));
            var GroupQuestions = await _GroupQuestionService
                                .GetAllAsync(s =>
                                s.Title.Contains(@params.SearchText)
                                , page: @params.Page, take: @params.Take);

            var mappedGroupQuestions = _mapper.Map<ICollection<ResultGroupQuestion>>(GroupQuestions);

            return Ok(new ResponseApiEntities<ResultGroupQuestion>
                                                            (entities: mappedGroupQuestions,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("GroupQuestions/{id}")]
        public async Task<IActionResult> GetGroupQuestionById([FromRoute] int id)
        {

            var data = await _GroupQuestionService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateGroupQuestion>
                                                           (entity: new UpdateGroupQuestion(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateGroupQuestion>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateGroupQuestion>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateGroupQuestion>
                                                           (entity: new UpdateGroupQuestion(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("GroupQuestions/Public/IdentityCode/{Code}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGroupQuestionByIdentityCode([FromRoute] string? Code)
        {

            var data = await _GroupQuestionService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == Code);
            if (data == null)
                return BadRequest(new ResponseApiEntity<ResultGroupQuestion>
                                                           (entity: new ResultGroupQuestion(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<ResultGroupQuestion>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<ResultGroupQuestion>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<ResultGroupQuestion>
                                                           (entity: new ResultGroupQuestion(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("GroupQuestions/All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGroupQuestionsAll()
        {
            if (!ModelState.IsValid) return BadRequest();

            var GroupQuestions = await _GroupQuestionService
                                .GetAllAsync(s => s.Visible == true);
            var mappedGroupQuestions = _mapper.Map<ICollection<ResultGroupQuestion>>(GroupQuestions);

            return Ok(new ResponseApiEntities<ResultGroupQuestion>
                                                            (entities: mappedGroupQuestions,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: GroupQuestions.Count()));

        }

        [HttpGet("GroupQuestions/ByGuid/{guid}")]
        public async Task<IActionResult> GetGroupQuestionById([FromRoute] string guid)
        {
            try
            {
                var GroupQuestion = await _GroupQuestionService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (GroupQuestion == null)
                    return BadRequest(new ResponseApiEntity<UpdateGroupQuestion>
                                                                              (entity: new UpdateGroupQuestion(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateGroupQuestion>(GroupQuestion);
                return Ok(new ResponseApiEntity<UpdateGroupQuestion>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateGroupQuestion>
                                                                             (entity: new UpdateGroupQuestion(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }

    }
}
