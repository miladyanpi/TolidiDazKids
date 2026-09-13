using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoQuestion;
using Dto.Models.DtoQuestion;
using Dto.Models.DtoProduct;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using ServicesLibrary.Services.QuestionSrv;
using ServicesLibrary.Services.GroupQuestionSrv;
using ServicesLibrary.Services.ViewCounter;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _QuestionService;
        private readonly IGroupQuestionService _GroupQuestionService;
        private readonly IMapper _mapper;
        private readonly IViewCounterService _viewCounterService;
        public QuestionController(
            IQuestionService QuestionService,
            IGroupQuestionService GroupQuestionService,
            IViewCounterService viewCounterService,
            IMapper mapper,
        UserManager<Account> userManager)
        {
            _QuestionService = QuestionService;
            _GroupQuestionService = GroupQuestionService;
            _viewCounterService = viewCounterService;
            _mapper = mapper;
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Questions")]
        public async Task<IActionResult> Add([FromBody] AddQuestion model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddQuestion>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var Question = _mapper.Map<AddQuestion, Question>(model);
            int id = await _QuestionService.AddAsync(Question);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddQuestion>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddQuestion>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Questions")]
        public async Task<IActionResult> Update([FromBody] UpdateQuestion model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateQuestion>
                                                           (entity: new UpdateQuestion(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));


            var Question = _mapper.Map<UpdateQuestion, Question>(model);
            var upd = await _QuestionService.UpdateAsync(Question);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateQuestion>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }

            else
                return BadRequest(new ResponseApiEntity<UpdateQuestion>
                                                           (entity: new UpdateQuestion(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("Questions/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _QuestionService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultQuestion>
                                                               (entity: new ResultQuestion(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultQuestion>
                                                           (entity: new ResultQuestion(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName + "," + ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Questions/{id}")]
        public async Task<IActionResult> GetQuestionById([FromRoute] int id)
        {

            var data = await _QuestionService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateQuestion>
                                                           (entity: new UpdateQuestion(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateQuestion>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateQuestion>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateQuestion>
                                                           (entity: new UpdateQuestion(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Questions")]
        public async Task<IActionResult> GetQuestions([FromQuery] PaginationParams @params, int? GroupQuestionID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultQuestion>
                                                           (entity: new ResultQuestion(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            Expression<Func<Question, bool>> predicate = x => true;
            if (!string.IsNullOrWhiteSpace(@params.SearchText))
            {
                var search = @params.SearchText;
                predicate = x =>
                    x.Title.Contains(search);
            }
            if (GroupQuestionID != null && GroupQuestionID > 0)
            {
                predicate = x =>
                    x.GroupQuestionID == GroupQuestionID;
            }
            var count = await _QuestionService.GetCountAllAsync(predicate);
            var Questions = await _QuestionService
                                   .GetAllAsync(predicate
                                   , page: @params.Page, take: @params.Take);


            var mappedQuestions = _mapper.Map<ICollection<ResultQuestion>>(Questions);

            return Ok(new ResponseApiEntities<ResultQuestion>
                                                            (entities: mappedQuestions,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [HttpGet("Questions/Public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetQuestionsPublic([FromQuery] PaginationParams @params, int? GroupQuestionID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultQuestion>
                                                           (entity: new ResultQuestion(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            Expression<Func<Question, bool>> predicate = x => true;
            if (!string.IsNullOrWhiteSpace(@params.SearchText))
            {
                var search = @params.SearchText;
                predicate = x =>
                    x.Title.Contains(search);
            }
            if (GroupQuestionID != null && GroupQuestionID > 0)
            {
                predicate = x =>
                    x.GroupQuestionID == GroupQuestionID;
            }
            var count = await _QuestionService.GetCountAllAsync(predicate);
            var Questions = await _QuestionService
                                   .GetAllAsync(predicate
                                   , page: @params.Page, take: @params.Take);

            var mappedQuestions = _mapper.Map<ICollection<ResultQuestion>>(Questions);

            return Ok(new ResponseApiEntities<ResultQuestion>
                                                            (entities: mappedQuestions,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        
    }
}
