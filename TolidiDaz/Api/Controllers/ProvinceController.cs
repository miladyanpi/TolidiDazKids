using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoProvince;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.ProvinceSrv;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProvinceController(
        IProvinceService _ProvinceService,
        IMapper _mapper
        )
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Provinces")]
        public async Task<IActionResult> Add([FromBody] AddProvince model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddProvince>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var Province = _mapper.Map<AddProvince, Province>(model);
            int id = await _ProvinceService.AddAsync(Province);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddProvince>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddProvince>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Provinces")]
        public async Task<IActionResult> Update([FromBody] UpdateProvince model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateProvince>
                                                           (entity: new UpdateProvince(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var Province = _mapper.Map<UpdateProvince, Province>(model);
            var upd = await _ProvinceService.UpdateAsync(Province);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateProvince>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateProvince>
                                                           (entity: new UpdateProvince(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("Provinces/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _ProvinceService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultProvince>
                                                               (entity: new ResultProvince(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultProvince>
                                                           (entity: new ResultProvince(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Provinces/Data")]
        public async Task<IActionResult> GetProvinces([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProvince>
                                                                    (entities: new List<ResultProvince>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<Province, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Title ?? "").Contains(search);
                }

                var count = await _ProvinceService.GetCountAllAsync(predicate);

                var Provinces = await _ProvinceService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedProvinces = _mapper.Map<ICollection<ResultProvince>>(Provinces);

                return Ok(new ResponseApiEntities<ResultProvince>
                                                                (entities: mappedProvinces,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultProvince>
                                                                    (entities: new List<ResultProvince>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Provinces")]
        public async Task<IActionResult> GetProvinces2([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultProvince>
                                                           (entity: new ResultProvince(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var count = await _ProvinceService.GetCountAllAsync(s=>s.Title.Contains(@params.SearchText) );
            var Provinces = await _ProvinceService
                                .GetAllAsync(s =>s.Title.Contains(@params.SearchText) 
                                , page: @params.Page, take: @params.Take);

            var mappedProvinces = _mapper.Map<ICollection<ResultProvince>>(Provinces);
     
            return Ok(new ResponseApiEntities<ResultProvince>
                                                            (entities: mappedProvinces,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Provinces/{id}")]
        public async Task<IActionResult> GetProvinceById([FromRoute] int id)
        {

            var data = await _ProvinceService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateProvince>
                                                           (entity: new UpdateProvince(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateProvince>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateProvince>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateProvince>
                                                           (entity: new UpdateProvince(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("Provinces/Public/All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProvincesAll()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultProvince>
                                                           (entities: new List<ResultProvince>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var Provinces = await _ProvinceService
                                .GetAllAsync();

            var mappedProvinces = _mapper.Map<ICollection<ResultProvince>>(Provinces);

            return Ok(new ResponseApiEntities<ResultProvince>
                                                            (entities: mappedProvinces,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Provinces.Count()));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Provinces/ByGuid/{guid}")]
        public async Task<IActionResult> GetProvinceById([FromRoute] string guid)
        {
            try
            {
                var Province = await _ProvinceService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (Province == null)
                    return BadRequest(new ResponseApiEntity<UpdateProvince>
                                                                              (entity: new UpdateProvince(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateProvince>(Province);
                return Ok(new ResponseApiEntity<UpdateProvince>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateProvince>
                                                                             (entity: new UpdateProvince(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }
    }
}
