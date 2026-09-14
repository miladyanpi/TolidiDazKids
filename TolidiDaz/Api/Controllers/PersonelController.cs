using ServicesLibrary.Services.PersonelSrv;
using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoPersonel;
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
    public class PersonelController(
        IPersonelService _PersonelService,
        IMapper _mapper,
        UserManager<Account> userManager
        ) 
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Personels")]
        public async Task<IActionResult> Add([FromBody] AddPersonel model)
        {
            if (!ModelState.IsValid)   return BadRequest(new ResponseApiEntity<AddPersonel>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var Personel = _mapper.Map<AddPersonel, Personel>(model);
            int id = await _PersonelService.AddAsync(Personel);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddPersonel>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddPersonel>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Personels")]
        public async Task<IActionResult> Update([FromBody] UpdatePersonel model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdatePersonel>
                                                           (entity: new UpdatePersonel(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            var Personel = _mapper.Map<UpdatePersonel, Personel>(model);
            Personel.IdentityCode = model.IdentityCode;
            var upd = await _PersonelService.UpdateAsync(Personel);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdatePersonel>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdatePersonel>
                                                           (entity: new UpdatePersonel(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }

        [HttpPatch("Personels/UpdateJsonFile")]
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        public async Task<IActionResult> UpdateUpdatePersonelJsonFile([FromBody] UpdateJsonFile model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResponseApiEntity<UpdateJsonFile>
                                                          (entity: new UpdateJsonFile(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.UpdateError));
            var q=await _PersonelService.GetByIdAsync(model.ID);
            if (q == null)
                return BadRequest(new ResponseApiEntity<UpdateJsonFile>
                                                         (entity: new UpdateJsonFile(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.UpdateError));
            q.JsonPicture=model.JsonPicture;    
           // var Personel = _mapper.Map<UpdatePersonel, Personel>(q);
            var upd = await _PersonelService.UpdateAsync(q);
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
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("Personels/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var user = await _PersonelService.GetByIdAsync(id);
            if(user==null)
                return BadRequest(new ResponseApiEntity<ResultPersonel>
                                                          (entity: new ResultPersonel(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.DeleteError));
            var del = await _PersonelService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultPersonel>
                                                               (entity: new ResultPersonel(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultPersonel>
                                                           (entity: new ResultPersonel(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Personels")]
        public async Task<IActionResult> GetPersonels([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _PersonelService.GetCountAllAsync(s =>
                                s.Name.Contains(@params.SearchText) ||
                                s.LastName.Contains(@params.SearchText));
            var Personels = await _PersonelService
                                .GetAllAsync(s =>
                                s.Name.Contains(@params.SearchText) ||
                                s.LastName.Contains(@params.SearchText)
                                , page: @params.Page, take: @params.Take);

            var mappedPersonels = _mapper.Map<ICollection<ResultPersonel>>(Personels);
            return Ok(new ResponseApiEntities<ResultPersonel>
                                                            (entities: mappedPersonels,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Personels/{id}")]
        public async Task<IActionResult> GetPersonelById([FromRoute] int id)
        {

            var Personel = await _PersonelService.GetByIdAsync(id);
            if(Personel==null)
                return BadRequest(new ResponseApiEntity<UpdatePersonel>
                                                           (entity: new UpdatePersonel(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdatePersonel>(Personel);
            if (result != null)
                return Ok(new ResponseApiEntity<UpdatePersonel>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdatePersonel>
                                                           (entity: new UpdatePersonel(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Personels/All")]
        public async Task<IActionResult> GetPersonels()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultPersonel>
                                                            (entities: new List<ResultPersonel>(),
                                                            status: ResultMessageApi.Error,
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            message: ResultMessageApi.ErrorBadRequest,
                                                            countAllRecordTable:0 ));


            var Personels = await _PersonelService
                                .GetAllAsync();

            var mappedPersonels = _mapper.Map<ICollection<ResultPersonel>>(Personels);
            return Ok(new ResponseApiEntities<ResultPersonel>
                                                            (entities: mappedPersonels,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Personels.Count()));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Personels/ByPositionID")]
        public async Task<IActionResult> GetPersonels(int? PositionID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultPersonel>
                                                            (entities: new List<ResultPersonel>(),
                                                            status: ResultMessageApi.Error,
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            message: ResultMessageApi.ErrorBadRequest,
                                                            countAllRecordTable: 0));


            var Personels = await _PersonelService
                                .GetAllAsync(s=>s.PositionID==PositionID);

            var mappedPersonels = _mapper.Map<ICollection<ResultPersonel>>(Personels);
            return Ok(new ResponseApiEntities<ResultPersonel>
                                                            (entities: mappedPersonels,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Personels.Count()));

        }


    }
}
