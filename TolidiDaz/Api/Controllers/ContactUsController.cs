using ServicesLibrary.Services.ContactUsSrv;
using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoContactUs;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ContactUsController : ControllerBase
    {
        private readonly IContactUsService _ContactUsService;
        private readonly IMapper _mapper;
        public ContactUsController(
            IContactUsService storyService,
            IMapper mapper,
        UserManager<Account> userManager)
        {
            _ContactUsService = storyService;
            _mapper = mapper;   
        }
        [HttpPost("ContactUss")]
        [AllowAnonymous]
        public async Task<IActionResult> Add([FromBody] AddContactUs model, string Key)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddContactUs>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            if (Keys.CustomerSatisfactionKey != Key)
                return BadRequest(new ResponseApiEntity<AddContactUs>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
            var add = _mapper.Map<AddContactUs, ContactUs>(model);
            int id = await _ContactUsService.AddAsync(add);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddContactUs>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddContactUs>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("ContactUss")]
        public async Task<IActionResult> Update([FromBody] UpdateContactUs model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateContactUs>
                                                           (entity: new UpdateContactUs(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var story = _mapper.Map<UpdateContactUs, ContactUs>(model);
            var upd = await _ContactUsService.UpdateAsync(story);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateContactUs>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateContactUs>
                                                           (entity: new UpdateContactUs(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("ContactUss/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _ContactUsService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultContactUs>
                                                               (entity: new ResultContactUs(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultContactUs>
                                                           (entity: new ResultContactUs(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ContactUss")]
        public async Task<IActionResult> GetContactUss([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _ContactUsService.GetCountAllAsync(s =>
                                s.FullName.Contains(@params.SearchText) || s.Subject.Contains(@params.SearchText));
            var ContactUss = await _ContactUsService
                                .GetAllAsync(s =>
                                s.FullName.Contains(@params.SearchText) || s.Subject.Contains(@params.SearchText)
                                , page: @params.Page, take: @params.Take);

            var mappedContactUss = _mapper.Map<ICollection<ResultContactUs>>(ContactUss);
     
            return Ok(new ResponseApiEntities<ResultContactUs>
                                                            (entities: mappedContactUss,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ContactUss/{id}")]
        public async Task<IActionResult> GetContactUsById([FromRoute] int id)
        {

            var story = await _ContactUsService.GetByIdAsync(id);
            if(story==null)
                return BadRequest(new ResponseApiEntity<UpdateContactUs>
                                                           (entity: new UpdateContactUs(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateContactUs>(story);
      
            if (result != null)
                return Ok(new ResponseApiEntity<UpdateContactUs>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateContactUs>
                                                           (entity: new UpdateContactUs(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ContactUss/All")]
        public async Task<IActionResult> GetContactUssAll()
        {
            if (!ModelState.IsValid) return BadRequest();

            var ContactUss = await _ContactUsService
                                .GetAllAsync(s=>s.Visible==true);
            var mappedContactUss = _mapper.Map<ICollection<ResultContactUs>>(ContactUss);

            return Ok(new ResponseApiEntities<ResultContactUs>
                                                            (entities: mappedContactUss,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: ContactUss.Count()));

        }




    }
}
