using ServicesLibrary.Services.ProvinceSrv;
using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoProvince;
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
    public class ProvinceController : ControllerBase
    {
        private readonly IProvinceService _ProvinceService;
        private readonly IMapper _mapper;
        public ProvinceController(
            IProvinceService ProvinceService,
            IMapper mapper,
        UserManager<Account> userManager)
        {
            _ProvinceService = ProvinceService;
            _mapper = mapper;   
        }
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
        [HttpGet("Provinces")]
        public async Task<IActionResult> GetProvinces([FromQuery] PaginationParams @params)
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


    }
}
