using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoStory;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.StorySrv;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class StoryController(
        IStoryService _storyService,
        IMapper _mapper
        ) 
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Storys")]
        public async Task<IActionResult> Add([FromBody] AddStory model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddStory>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var story = _mapper.Map<AddStory, Story>(model);
            int id = await _storyService.AddAsync(story);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddStory>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddStory>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Storys")]
        public async Task<IActionResult> Update([FromBody] UpdateStory model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateStory>
                                                           (entity: new UpdateStory(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var story = _mapper.Map<UpdateStory, Story>(model);
            var upd = await _storyService.UpdateAsync(story);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateStory>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateStory>
                                                           (entity: new UpdateStory(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [HttpPatch("Storys/UpdateJsonFile")]
        public async Task<IActionResult> UpdateUpdateStoryJsonFile([FromBody] UpdateJsonFile model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var q = await _storyService.GetByIdAsync(model.ID);
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
            // var story = _mapper.Map<UpdateStory, Story>(q);
            var upd = await _storyService.UpdateAsync(q);
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
        [HttpDelete("Storys/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _storyService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultStory>
                                                               (entity: new ResultStory(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultStory>
                                                           (entity: new ResultStory(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Storys/Data")]
        public async Task<IActionResult> GetStorys([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultStory>
                                                                    (entities: new List<ResultStory>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<Story, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Title ?? "").Contains(search);
                }

                var count = await _storyService.GetCountAllAsync(predicate);

                var Storys = await _storyService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedStorys = _mapper.Map<ICollection<ResultStory>>(Storys);

                return Ok(new ResponseApiEntities<ResultStory>
                                                                (entities: mappedStorys,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultStory>
                                                                    (entities: new List<ResultStory>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Storys")]
        public async Task<IActionResult> GetStorys2([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _storyService.GetCountAllAsync(s =>
                                s.Title.Contains(@params.SearchText) );
            var Storys = await _storyService
                                .GetAllAsync(s =>
                                s.Title.Contains(@params.SearchText) 
                                , page: @params.Page, take: @params.Take);

            var mappedStorys = _mapper.Map<ICollection<ResultStory>>(Storys);
     
            return Ok(new ResponseApiEntities<ResultStory>
                                                            (entities: mappedStorys,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Storys/{id}")]
        public async Task<IActionResult> GetStoryById([FromRoute] int id)
        {

            var story = await _storyService.GetByIdAsync(id);
            if(story==null)
                return BadRequest(new ResponseApiEntity<UpdateStory>
                                                           (entity: new UpdateStory(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateStory>(story);
      
            if (result != null)
                return Ok(new ResponseApiEntity<UpdateStory>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateStory>
                                                           (entity: new UpdateStory(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("Storys/Public/All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStorysAll()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultStory>
                                                           (entities: new List<ResultStory>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var Storys = await _storyService
                                .GetAllAsync(s=>s.Visible==true);

            var mappedStorys = _mapper.Map<ICollection<ResultStory>>(Storys);

            return Ok(new ResponseApiEntities<ResultStory>
                                                            (entities: mappedStorys,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Storys.Count()));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Storys/ByGuid/{guid}")]
        public async Task<IActionResult> GetStoryById([FromRoute] string guid)
        {
            try
            {
                var Story = await _storyService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (Story == null)
                    return BadRequest(new ResponseApiEntity<UpdateStory>
                                                                              (entity: new UpdateStory(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateStory>(Story);
                return Ok(new ResponseApiEntity<UpdateStory>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateStory>
                                                                             (entity: new UpdateStory(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }

    }
}
