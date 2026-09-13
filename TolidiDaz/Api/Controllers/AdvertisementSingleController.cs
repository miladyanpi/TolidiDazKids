using ServicesLibrary.Services.AdvertisementSingleSrv;
using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoAdvertisementSingle;
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
    public class AdvertisementSingleController : ControllerBase
    {
        private readonly IAdvertisementSingleService _AdvertisementSingleService;
        private readonly IMapper _mapper;
        public AdvertisementSingleController(
            IAdvertisementSingleService advertisementSingleService,
            IMapper mapper,
        UserManager<Account> userManager)
        {
            _AdvertisementSingleService = advertisementSingleService;
            _mapper = mapper;   
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("AdvertisementSingles")]
        public async Task<IActionResult> Add([FromBody] AddAdvertisementSingle model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddAdvertisementSingle>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var advertisementSingle = _mapper.Map<AddAdvertisementSingle, AdvertisementSingle>(model);
            int id = await _AdvertisementSingleService.AddAsync(advertisementSingle);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddAdvertisementSingle>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddAdvertisementSingle>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("AdvertisementSingles")]
        public async Task<IActionResult> Update([FromBody] UpdateAdvertisementSingle model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateAdvertisementSingle>
                                                           (entity: new UpdateAdvertisementSingle(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var advertisementSingle = _mapper.Map<UpdateAdvertisementSingle, AdvertisementSingle>(model);
            var upd = await _AdvertisementSingleService.UpdateAsync(advertisementSingle);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateAdvertisementSingle>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateAdvertisementSingle>
                                                           (entity: new UpdateAdvertisementSingle(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [HttpPatch("AdvertisementSingles/UpdateJsonFile")]
        public async Task<IActionResult> UpdateUpdateAdvertisementSingleJsonFile([FromBody] UpdateJsonFile model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var q = await _AdvertisementSingleService.GetByIdAsync(model.ID);
            if (q == null)
                return BadRequest(new ResponseApiEntity<UpdateJsonFile>
                                                         (entity: new UpdateJsonFile(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.UpdateError));
            switch(model.EnumJsonImageFileVideo)
            {
                case EnumConstant.EnumJsonImageFileVideo.Image:
                    q.JsonPicture = model.JsonPicture;
                    break;
              
            }
            // var advertisementSingle = _mapper.Map<UpdateAdvertisementSingle, AdvertisementSingle>(q);
            var upd = await _AdvertisementSingleService.UpdateAsync(q);
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
        [HttpDelete("AdvertisementSingles/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _AdvertisementSingleService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultAdvertisementSingle>
                                                               (entity: new ResultAdvertisementSingle(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultAdvertisementSingle>
                                                           (entity: new ResultAdvertisementSingle(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("AdvertisementSingles")]
        public async Task<IActionResult> GetAdvertisementSingles([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _AdvertisementSingleService.GetCountAllAsync(s =>
                                s.Title.Contains(@params.SearchText) );
            var AdvertisementSingles = await _AdvertisementSingleService
                                .GetAllAsync(s =>
                                s.Title.Contains(@params.SearchText) 
                                , page: @params.Page, take: @params.Take);

            var mappedAdvertisementSingles = _mapper.Map<ICollection<ResultAdvertisementSingle>>(AdvertisementSingles);
     
            return Ok(new ResponseApiEntities<ResultAdvertisementSingle>
                                                            (entities: mappedAdvertisementSingles,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("AdvertisementSingles/{id}")]
        public async Task<IActionResult> GetAdvertisementSingleById([FromRoute] int id)
        {

            var advertisementSingle = await _AdvertisementSingleService.GetByIdAsync(id);
            if(advertisementSingle==null)
                return BadRequest(new ResponseApiEntity<UpdateAdvertisementSingle>
                                                           (entity: new UpdateAdvertisementSingle(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateAdvertisementSingle>(advertisementSingle);
      
            if (result != null)
                return Ok(new ResponseApiEntity<UpdateAdvertisementSingle>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateAdvertisementSingle>
                                                           (entity: new UpdateAdvertisementSingle(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("AdvertisementSingles/Public/All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAdvertisementSinglesAll()
        {
            if (!ModelState.IsValid) return BadRequest();

            var AdvertisementSingles = await _AdvertisementSingleService
                                .GetAllAsync(s=>s.Visible==true);
            //if(AdvertisementSingles!=null && AdvertisementSingles.Count()>0)
            //{
            //    var random = new Random();
            //    AdvertisementSingles = AdvertisementSingles
            //        .OrderBy(x => random.Next())
            //        .Take(1) 
            //        .ToList();
            //}
    
            var mappedAdvertisementSingles = _mapper.Map<ICollection<ResultAdvertisementSingle>>(AdvertisementSingles);

            return Ok(new ResponseApiEntities<ResultAdvertisementSingle>
                                                            (entities: mappedAdvertisementSingles,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: AdvertisementSingles.Count()));

        }

    }
}
