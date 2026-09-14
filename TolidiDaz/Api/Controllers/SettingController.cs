using AutoMapper;
using DAL.Paginagion;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Dto.Models.ResponseApi;
using Dto.Models.Constant;
using Dto.Models.DtoSetting;
using Dto.Enum;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ServicesLibrary.Services.SettingSrv;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]

    public class SettingController(
        ISettingService _SettingService,
        IMapper _mapper
        ) 
        : ControllerBase
    {
        [HttpPost("Settings")]
        public async Task<IActionResult> Add([FromBody] AddSetting model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var Setting = _mapper.Map<AddSetting, Setting>(model);
            var value = await _SettingService.AddAsync(Setting);

            if (value > 0)
                return Ok(new ResponseApiEntity<AddSetting>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddSetting>
                                                           (entity: new AddSetting(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [HttpPatch("Settings")]
        public async Task<IActionResult> Update([FromBody] UpdateSetting model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var Setting = _mapper.Map<Setting>(model);
            var upd = await _SettingService.UpdateAsync(Setting);
            if (upd > 0)
                return Ok(new ResponseApiEntity<UpdateSetting>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateSetting>
                                                           (entity: new UpdateSetting(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
       
        [HttpGet("Settings")]
        public async Task<IActionResult> GetSettings([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _SettingService.GetCountAllAsync();
            var Settings = await _SettingService.GetAllAsync (page:@params.Page,take:@params.Take);
            //if (Settings is null)
            //    return NotFound();
            var mappedSettings = _mapper.Map<ICollection<ResultSetting>>(Settings);

            return Ok(new ResponseApiEntities<ResultSetting>
                                                             (entities: mappedSettings,
                                                             status: ResultMessageApi.Success,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             message: ResultMessageApi.GetOk,
                                                             countAllRecordTable: count));
        }
      
  
        [HttpGet("Settings/{id}")]
        public async Task<IActionResult> GetSettingById([FromRoute] int id)
        {
            var Setting = await _SettingService.GetByIdAsync(id);
            var result = _mapper.Map<UpdateSetting>(Setting);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateSetting>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateSetting>
                                                           (entity: new UpdateSetting(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
        }

        [HttpGet("Settings/All")]
        public async Task<IActionResult> GetSettingsAll()
        {

            var data = await _SettingService.GetAllAsync();
            var mappedSettings = _mapper.Map<ICollection<ResultSetting>>(data);


            return Ok(new ResponseApiEntities<ResultSetting>
                         (entities: mappedSettings,
                         status: ResultMessageApi.Success,
                         statusCode: ResultMessageApi.SuccessCode,
                         message: ResultMessageApi.GetOk,
                         countAllRecordTable: mappedSettings.Count));
        }
        [HttpGet("Settings/Public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSettingsPublicAll()
        {

            var data = await _SettingService.GetAllAsync();
            var mappedSettings = _mapper.Map<ICollection<ResultPublicSetting>>(data);


            return Ok(new ResponseApiEntities<ResultPublicSetting>
                         (entities: mappedSettings,
                         status: ResultMessageApi.Success,
                         statusCode: ResultMessageApi.SuccessCode,
                         message: ResultMessageApi.GetOk,
                         countAllRecordTable: mappedSettings.Count));
        }

    }
}
