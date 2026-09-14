using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoAbout;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ServicesLibrary.Services.AboutSrv;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
    public class AboutController(
        IAboutService _AboutService,
        IMapper _mapper,
        UserManager<Account> _userManager
        )
        : ControllerBase
    {
        [HttpPost("Abouts")]
        public async Task<IActionResult> Add([FromBody] AddAbout model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var About = _mapper.Map<AddAbout, About>(model);
            int id = await _AboutService.AddAsync(About);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddAbout>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddAbout>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [HttpPatch("Abouts")]
        public async Task<IActionResult> Update([FromBody] UpdateAbout model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var About = _mapper.Map<UpdateAbout, About>(model);
            var upd = await _AboutService.UpdateAsync(About);
            if (upd > 0)
                return Ok(new ResponseApiEntity<UpdateAbout>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateAbout>
                                                           (entity: new UpdateAbout(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [HttpDelete("Abouts/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
        
            var del = await _AboutService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultAbout>
                                                               (entity: new ResultAbout(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultAbout>
                                                           (entity: new ResultAbout(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [HttpGet("Abouts")]
        public async Task<IActionResult> GetAbouts([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();
            var count = await _AboutService
                           .GetCountAllAsync(s => 
                           s.ShortDescription.Contains(@params.SearchText) );

            var Abouts = await _AboutService
                                 .GetAllAsync(s =>
                                 (s.ShortDescription.Contains(@params.SearchText))
                                 , page: @params.Page, take: @params.Take);


            var mappedAbouts = _mapper.Map<ICollection<ResultAbout>>(Abouts);

            return Ok(new ResponseApiEntities<ResultAbout>
                                                            (entities: mappedAbouts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));
        }
        [HttpGet("Abouts/LastRecord")]
        public async Task<IActionResult> GetAboutsLastRecord()
        {
            if (!ModelState.IsValid) return BadRequest();

            var About = await _AboutService
                                 .LastOrDefaultAsync();

            var mappedAbouts = _mapper.Map<UpdateAbout>(About);
            return Ok(new ResponseApiEntity<UpdateAbout>
                                                            (entity: mappedAbouts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk));
        }
        [HttpGet("Abouts/{id}")]
        public async Task<IActionResult> GetAboutById([FromRoute] int id)
        {
            var About = await _AboutService.GetByIdAsync(id);
            var result = _mapper.Map<UpdateAbout>(About);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateAbout>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateAbout>
                                                           (entity: new UpdateAbout(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpPatch("Abouts/UpdateJsonFile")]
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        public async Task<IActionResult> UpdateAboutJsonFile([FromBody] UpdateJsonFile model)
        {
            if (!ModelState.IsValid) BadRequest(new ResponseApiEntity<UpdateJsonFile>
                                                           (entity: new UpdateJsonFile(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            var q = await _AboutService.GetByIdAsync(model.ID);
            if (q == null)
                return BadRequest(new ResponseApiEntity<UpdateJsonFile>
                                                         (entity: new UpdateJsonFile(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.UpdateError));
            q.JsonPicture = model.JsonPicture;
            // var About = _mapper.Map<UpdateAbout, About>(q);
            var upd = await _AboutService.UpdateAsync(q);
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
        [HttpGet("Abouts/All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAboutsAll()
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _AboutService
                           .GetCountAllAsync();

            var Abouts = await _AboutService
                                 .GetAllAsync();

            var mappedAbouts = _mapper.Map<ICollection<UpdateAbout>>(Abouts);

            return Ok(new ResponseApiEntities<UpdateAbout>
                                                            (entities: mappedAbouts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));
        }
        [HttpGet("Abouts/ByGuid/{guid}")]
        public async Task<IActionResult> GetAboutById([FromRoute] string guid)
        {
            try
            {
                var About = await _AboutService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (About == null)
                    return BadRequest(new ResponseApiEntity<UpdateAbout>
                                                                              (entity: new UpdateAbout(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateAbout>(About);
                return Ok(new ResponseApiEntity<UpdateAbout>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateAbout>
                                                                              (entity: new UpdateAbout(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ex.Message));
            }

        }
    }
}
