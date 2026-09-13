using ServicesLibrary.Services.SliderSrv;
using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoSlider;
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
    public class SliderController : ControllerBase
    {
        private readonly ISliderService _SliderService;
        private readonly IMapper _mapper;
        public SliderController(
            ISliderService storyService,
            IMapper mapper,
        UserManager<Account> userManager)
        {
            _SliderService = storyService;
            _mapper = mapper;   
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Sliders")]
        public async Task<IActionResult> Add([FromBody] AddSlider model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddSlider>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var story = _mapper.Map<AddSlider, Slider>(model);
            int id = await _SliderService.AddAsync(story);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddSlider>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddSlider>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Sliders")]
        public async Task<IActionResult> Update([FromBody] UpdateSlider model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateSlider>
                                                           (entity: new UpdateSlider(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var story = _mapper.Map<UpdateSlider, Slider>(model);
            var upd = await _SliderService.UpdateAsync(story);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateSlider>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateSlider>
                                                           (entity: new UpdateSlider(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [HttpPatch("Sliders/UpdateJsonFile")]
        public async Task<IActionResult> UpdateUpdateSliderJsonFile([FromBody] UpdateJsonFile model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var q = await _SliderService.GetByIdAsync(model.ID);
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
                case EnumConstant.EnumJsonImageFileVideo.Video:
                    q.JsonVideo = model.JsonVideo;
                    break;
                default:
                    q.JsonPicture = model.JsonPicture;
                    break;
            }
            // var story = _mapper.Map<UpdateSlider, Slider>(q);
            var upd = await _SliderService.UpdateAsync(q);
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
        [HttpDelete("Sliders/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _SliderService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultSlider>
                                                               (entity: new ResultSlider(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultSlider>
                                                           (entity: new ResultSlider(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Sliders")]
        public async Task<IActionResult> GetSliders([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _SliderService.GetCountAllAsync(s =>
                                s.Title.Contains(@params.SearchText) );
            var Sliders = await _SliderService
                                .GetAllAsync(s =>
                                s.Title.Contains(@params.SearchText) 
                                , page: @params.Page, take: @params.Take);

            var mappedSliders = _mapper.Map<ICollection<ResultSlider>>(Sliders);
     
            return Ok(new ResponseApiEntities<ResultSlider>
                                                            (entities: mappedSliders,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Sliders/{id}")]
        public async Task<IActionResult> GetSliderById([FromRoute] int id)
        {

            var story = await _SliderService.GetByIdAsync(id);
            if(story==null)
                return BadRequest(new ResponseApiEntity<UpdateSlider>
                                                           (entity: new UpdateSlider(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateSlider>(story);
      
            if (result != null)
                return Ok(new ResponseApiEntity<UpdateSlider>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateSlider>
                                                           (entity: new UpdateSlider(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("Sliders/Public/All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSlidersAll()
        {
            if (!ModelState.IsValid) return BadRequest();

            var Sliders = await _SliderService
                                .GetAllAsync(s=>s.Visible==true);

            var mappedSliders = _mapper.Map<ICollection<ResultSlider>>(Sliders);

            return Ok(new ResponseApiEntities<ResultSlider>
                                                            (entities: mappedSliders,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Sliders.Count()));

        }

    }
}
