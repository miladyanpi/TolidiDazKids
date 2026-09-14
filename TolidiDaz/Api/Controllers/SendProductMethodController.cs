using ServicesLibrary.Services.SendProductMethodSrv;
using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoSendProductMethod;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SendProductMethodController(
        ISendProductMethodService _SendProductMethodService,
        IMapper _mapper
        //UserManager<Account> userManager
        )
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("SendProductMethods")]
        public async Task<IActionResult> Add([FromBody] AddSendProductMethod model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddSendProductMethod>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var SendProductMethod = _mapper.Map<AddSendProductMethod, SendProductMethod>(model);
            int id = await _SendProductMethodService.AddAsync(SendProductMethod);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddSendProductMethod>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddSendProductMethod>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("SendProductMethods")]
        public async Task<IActionResult> Update([FromBody] UpdateSendProductMethod model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateSendProductMethod>
                                                           (entity: new UpdateSendProductMethod(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var SendProductMethod = _mapper.Map<UpdateSendProductMethod, SendProductMethod>(model);
            var upd = await _SendProductMethodService.UpdateAsync(SendProductMethod);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateSendProductMethod>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateSendProductMethod>
                                                           (entity: new UpdateSendProductMethod(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("SendProductMethods/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _SendProductMethodService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultSendProductMethod>
                                                               (entity: new ResultSendProductMethod(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultSendProductMethod>
                                                           (entity: new ResultSendProductMethod(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("SendProductMethods")]
        public async Task<IActionResult> GetSendProductMethods([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultSendProductMethod>
                                                           (entity: new ResultSendProductMethod(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var count = await _SendProductMethodService.GetCountAllAsync(s=>s.Title.Contains(@params.SearchText) );
            var SendProductMethods = await _SendProductMethodService
                                .GetAllAsync(s =>s.Title.Contains(@params.SearchText) 
                                , page: @params.Page, take: @params.Take);

            var mappedSendProductMethods = _mapper.Map<ICollection<ResultSendProductMethod>>(SendProductMethods);
     
            return Ok(new ResponseApiEntities<ResultSendProductMethod>
                                                            (entities: mappedSendProductMethods,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [HttpGet("SendProductMethods/{id}")]
        public async Task<IActionResult> GetSendProductMethodById([FromRoute] int id)
        {

            var data = await _SendProductMethodService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateSendProductMethod>
                                                           (entity: new UpdateSendProductMethod(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateSendProductMethod>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateSendProductMethod>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateSendProductMethod>
                                                           (entity: new UpdateSendProductMethod(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName )]
        [HttpGet("SendProductMethods/Public/All")]
        public async Task<IActionResult> GetSendProductMethodsAll()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultSendProductMethod>
                                                           (entities: new List<ResultSendProductMethod>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var SendProductMethods = await _SendProductMethodService
                                .GetAllAsync();

            var mappedSendProductMethods = _mapper.Map<ICollection<ResultSendProductMethod>>(SendProductMethods);

            return Ok(new ResponseApiEntities<ResultSendProductMethod>
                                                            (entities: mappedSendProductMethods,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: SendProductMethods.Count()));

        }


    }
}
