using ServicesLibrary.Services.CitySrv;
using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoCity;
using Dto.Models.DtoCity;
using Dto.Models.DtoProvince;
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
    public class CityController : ControllerBase
    {
        private readonly ICityService _CityService;
        private readonly IMapper _mapper;
        public CityController(
            ICityService CityService,
            IMapper mapper,
        UserManager<Account> userManager)
        {
            _CityService = CityService;
            _mapper = mapper;   
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Citys")]
        public async Task<IActionResult> Add([FromBody] AddCity model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddCity>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var City = _mapper.Map<AddCity, City>(model);
            int id = await _CityService.AddAsync(City);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddCity>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddCity>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Citys")]
        public async Task<IActionResult> Update([FromBody] UpdateCity model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateCity>
                                                           (entity: new UpdateCity(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var City = _mapper.Map<UpdateCity, City>(model);
            var upd = await _CityService.UpdateAsync(City);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateCity>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateCity>
                                                           (entity: new UpdateCity(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("Citys/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _CityService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultCity>
                                                               (entity: new ResultCity(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultCity>
                                                           (entity: new ResultCity(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Citys")]
        public async Task<IActionResult> GetCitys([FromQuery] PaginationParams @params,int? ProvinceID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultCity>
                                                           (entity: new ResultCity(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            int count = 0;
            IEnumerable<City> Citys =new List<City>();
            if (ProvinceID == null)
            {
                 count = await _CityService.GetCountAllAsync(s => 
                               s.Title.Contains(@params.SearchText));
                 Citys = await _CityService
                                    .GetAllAsync(s => 
                                    s.Title.Contains(@params.SearchText)
                                    , page: @params.Page, take: @params.Take);
            }
            else
            {
               count = await _CityService.GetCountAllAsync(s => s.ProvinceID == ProvinceID &&
                              s.Title.Contains(@params.SearchText));
                Citys = await _CityService
                                    .GetAllAsync(s => s.ProvinceID == ProvinceID &&
                                    s.Title.Contains(@params.SearchText)
                                    , page: @params.Page, take: @params.Take);
            }


            var mappedCitys = _mapper.Map<ICollection<ResultCity>>(Citys);
            return Ok(new ResponseApiEntities<ResultCity>
                                                            (entities: mappedCitys,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName + "," + ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Citys/{id}")]
        public async Task<IActionResult> GetCityById([FromRoute] int id)
        {

            var data = await _CityService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateCity>
                                                           (entity: new UpdateCity(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateCity>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateCity>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateCity>
                                                           (entity: new UpdateCity(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("Citys/All")]
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName +  "," + ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        public async Task<IActionResult> GetCitysAll([FromQuery]int? ProvinceID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultCity>
                                                           (entity: new ResultCity(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var Citys = await _CityService
                                .GetAllAsync(s=>s.ProvinceID== ProvinceID);

            var mappedCitys = _mapper.Map<ICollection<ResultCity>>(Citys);

            return Ok(new ResponseApiEntities<ResultCity>
                                                            (entities: mappedCitys,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Citys.Count()));

        }


    }
}
