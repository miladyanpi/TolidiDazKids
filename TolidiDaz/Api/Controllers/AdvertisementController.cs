using ServicesLibrary.Services.AdvertisementSrv;
using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoAdvertisement;
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
    public class AdvertisementController : ControllerBase
    {
        private readonly IAdvertisementService _AdvertisementService;
        private readonly IMapper _mapper;
        public AdvertisementController(
            IAdvertisementService advertisementService,
            IMapper mapper,
        UserManager<Account> userManager)
        {
            _AdvertisementService = advertisementService;
            _mapper = mapper;   
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Advertisements")]
        public async Task<IActionResult> Add([FromBody] AddAdvertisement model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddAdvertisement>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var advertisement = _mapper.Map<AddAdvertisement, Advertisement>(model);
            int id = await _AdvertisementService.AddAsync(advertisement);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddAdvertisement>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddAdvertisement>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Advertisements")]
        public async Task<IActionResult> Update([FromBody] UpdateAdvertisement model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateAdvertisement>
                                                           (entity: new UpdateAdvertisement(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var advertisement = _mapper.Map<UpdateAdvertisement, Advertisement>(model);
            var upd = await _AdvertisementService.UpdateAsync(advertisement);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateAdvertisement>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateAdvertisement>
                                                           (entity: new UpdateAdvertisement(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [HttpPatch("Advertisements/UpdateJsonFile")]
        public async Task<IActionResult> UpdateUpdateAdvertisementJsonFile([FromBody] UpdateJsonFile model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var q = await _AdvertisementService.GetByIdAsync(model.ID);
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
            // var advertisement = _mapper.Map<UpdateAdvertisement, Advertisement>(q);
            var upd = await _AdvertisementService.UpdateAsync(q);
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
        [HttpDelete("Advertisements/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _AdvertisementService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultAdvertisement>
                                                               (entity: new ResultAdvertisement(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultAdvertisement>
                                                           (entity: new ResultAdvertisement(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Advertisements")]
        public async Task<IActionResult> GetAdvertisements([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _AdvertisementService.GetCountAllAsync(s =>
                                s.Title.Contains(@params.SearchText) );
            var Advertisements = await _AdvertisementService
                                .GetAllAsync(s =>
                                s.Title.Contains(@params.SearchText) 
                                , page: @params.Page, take: @params.Take);

            var mappedAdvertisements = _mapper.Map<ICollection<ResultAdvertisement>>(Advertisements);
     
            return Ok(new ResponseApiEntities<ResultAdvertisement>
                                                            (entities: mappedAdvertisements,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Advertisements/{id}")]
        public async Task<IActionResult> GetAdvertisementById([FromRoute] int id)
        {

            var advertisement = await _AdvertisementService.GetByIdAsync(id);
            if(advertisement==null)
                return BadRequest(new ResponseApiEntity<UpdateAdvertisement>
                                                           (entity: new UpdateAdvertisement(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateAdvertisement>(advertisement);
      
            if (result != null)
                return Ok(new ResponseApiEntity<UpdateAdvertisement>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateAdvertisement>
                                                           (entity: new UpdateAdvertisement(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("Advertisements/Public/All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAdvertisementsAll()
        {
            if (!ModelState.IsValid) return BadRequest();

            var Advertisements = await _AdvertisementService
                                .GetAllAsync(s=>s.Visible==true);
            //if(Advertisements!=null && Advertisements.Count()>0)
            //{
            //    var random = new Random();
            //    Advertisements = Advertisements
            //        .OrderBy(x => random.Next())
            //        .Take(2) 
            //        .ToList();
            //}
    
            var mappedAdvertisements = _mapper.Map<ICollection<ResultAdvertisement>>(Advertisements);

            return Ok(new ResponseApiEntities<ResultAdvertisement>
                                                            (entities: mappedAdvertisements,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Advertisements.Count()));

        }

    }
}
