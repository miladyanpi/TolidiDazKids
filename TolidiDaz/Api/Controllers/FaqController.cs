using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoFaq;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.FaqSrv;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FaqController(
        IFaqService _FaqService,
        IMapper _mapper,
        UserManager<Account> userManager
        ) 
        : ControllerBase
    {
        [HttpPost("Faqs")]
        [AllowAnonymous]
        public async Task<IActionResult> Add([FromBody] AddFaq model, string Key)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddFaq>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            if (Keys.CustomerSatisfactionKey != Key)
                return BadRequest(new ResponseApiEntity<AddFaq>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
            var add = _mapper.Map<AddFaq, Faq>(model);
            int id = await _FaqService.AddAsync(add);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddFaq>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddFaq>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Faqs")]
        public async Task<IActionResult> Update([FromBody] UpdateFaq model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateFaq>
                                                           (entity: new UpdateFaq(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var story = _mapper.Map<UpdateFaq, Faq>(model);
            var upd = await _FaqService.UpdateAsync(story);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateFaq>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateFaq>
                                                           (entity: new UpdateFaq(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("Faqs/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _FaqService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultFaq>
                                                               (entity: new ResultFaq(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultFaq>
                                                           (entity: new ResultFaq(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [HttpPost("Faqs/Data")]
        public async Task<IActionResult> GetFaqs([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultFaq>
                                                                    (entities: new List<ResultFaq>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<Faq, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.FullName ?? "").Contains(search);
                }

                var count = await _FaqService.GetCountAllAsync(predicate);

                var Faqs = await _FaqService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedFaqs = _mapper.Map<ICollection<ResultFaq>>(Faqs);

                return Ok(new ResponseApiEntities<ResultFaq>
                                                                (entities: mappedFaqs,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultFaq>
                                                                    (entities: new List<ResultFaq>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Faqs2")]
        public async Task<IActionResult> GetFaqs2([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _FaqService.GetCountAllAsync(s =>
                                s.FullName.Contains(@params.SearchText) || s.Subject.Contains(@params.SearchText));
            var Faqs = await _FaqService
                                .GetAllAsync(s =>
                                s.FullName.Contains(@params.SearchText) || s.Subject.Contains(@params.SearchText)
                                , page: @params.Page, take: @params.Take);

            var mappedFaqs = _mapper.Map<ICollection<ResultFaq>>(Faqs);
     
            return Ok(new ResponseApiEntities<ResultFaq>
                                                            (entities: mappedFaqs,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Faqs/{id}")]
        public async Task<IActionResult> GetFaqById([FromRoute] int id)
        {

            var story = await _FaqService.GetByIdAsync(id);
            if(story==null)
                return BadRequest(new ResponseApiEntity<UpdateFaq>
                                                           (entity: new UpdateFaq(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateFaq>(story);
      
            if (result != null)
                return Ok(new ResponseApiEntity<UpdateFaq>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateFaq>
                                                           (entity: new UpdateFaq(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Faqs/All")]
        public async Task<IActionResult> GetFaqsAll()
        {
            if (!ModelState.IsValid) return BadRequest();

            var Faqs = await _FaqService
                                .GetAllAsync(s=>s.Visible==true);
            var mappedFaqs = _mapper.Map<ICollection<ResultFaq>>(Faqs);

            return Ok(new ResponseApiEntities<ResultFaq>
                                                            (entities: mappedFaqs,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Faqs.Count()));

        }
        [HttpGet("Faqs/ByGuid/{guid}")]
        public async Task<IActionResult> GetFaqById([FromRoute] string guid)
        {
            try
            {
                var Faq = await _FaqService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (Faq == null)
                    return BadRequest(new ResponseApiEntity<UpdateFaq>
                                                                              (entity: new UpdateFaq(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateFaq>(Faq);
                return Ok(new ResponseApiEntity<UpdateFaq>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateFaq>
                                                                             (entity: new UpdateFaq(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }
    }
}
