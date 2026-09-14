using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoGroupBlog;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.GroupBlogSrv;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GroupBlogController(
        IGroupBlogService _GroupBlogService,
        IMapper _mapper,
        UserManager<Account> userManager
        )
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("GroupBlogs")]
        public async Task<IActionResult> Add([FromBody] AddGroupBlog model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddGroupBlog>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var data = _mapper.Map<AddGroupBlog, GroupBlog>(model);
            int id = await _GroupBlogService.AddAsync(data);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddGroupBlog>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddGroupBlog>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("GroupBlogs")]
        public async Task<IActionResult> Update([FromBody] UpdateGroupBlog model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateGroupBlog>
                                                           (entity: new UpdateGroupBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var data = _mapper.Map<UpdateGroupBlog, GroupBlog>(model);
            var upd = await _GroupBlogService.UpdateAsync(data);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateGroupBlog>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateGroupBlog>
                                                           (entity: new UpdateGroupBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("GroupBlogs/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _GroupBlogService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultGroupBlog>
                                                               (entity: new ResultGroupBlog(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultGroupBlog>
                                                           (entity: new ResultGroupBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("GroupBlogs/Data")]
        public async Task<IActionResult> GetGroupBlogs([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultGroupBlog>
                                                                    (entities: new List<ResultGroupBlog>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<GroupBlog, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Title ?? "").Contains(search);
                }

                var count = await _GroupBlogService.GetCountAllAsync(predicate);

                var GroupBlogs = await _GroupBlogService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedGroupBlogs = _mapper.Map<ICollection<ResultGroupBlog>>(GroupBlogs);

                return Ok(new ResponseApiEntities<ResultGroupBlog>
                                                                (entities: mappedGroupBlogs,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultGroupBlog>
                                                                    (entities: new List<ResultGroupBlog>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("GroupBlogs")]
        public async Task<IActionResult> GetGroupBlogs2([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _GroupBlogService.GetCountAllAsync(s =>
                                s.Title.Contains(@params.SearchText) );
            var GroupBlogs = await _GroupBlogService
                                .GetAllAsync(s =>
                                s.Title.Contains(@params.SearchText) 
                                , page: @params.Page, take: @params.Take);

            var mappedGroupBlogs = _mapper.Map<ICollection<ResultGroupBlog>>(GroupBlogs);
     
            return Ok(new ResponseApiEntities<ResultGroupBlog>
                                                            (entities: mappedGroupBlogs,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("GroupBlogs/{id}")]
        public async Task<IActionResult> GetGroupBlogById([FromRoute] int id)
        {

            var data = await _GroupBlogService.GetByIdAsync(id);
            if(data==null)
                return BadRequest(new ResponseApiEntity<UpdateGroupBlog>
                                                           (entity: new UpdateGroupBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateGroupBlog>(data);
      
            if (result != null)
                return Ok(new ResponseApiEntity<UpdateGroupBlog>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateGroupBlog>
                                                           (entity: new UpdateGroupBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("GroupBlogs/Public/IdentityCode/{Code}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGroupBlogByIdentityCode([FromRoute] string? Code)
        {

            var data = await _GroupBlogService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == Code);
            if (data == null)
                return BadRequest(new ResponseApiEntity<ResultGroupBlog>
                                                           (entity: new ResultGroupBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<ResultGroupBlog>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<ResultGroupBlog>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<ResultGroupBlog>
                                                           (entity: new ResultGroupBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("GroupBlogs/All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGroupBlogsAll()
        {
            if (!ModelState.IsValid) return BadRequest();

            var GroupBlogs = await _GroupBlogService
                                .GetAllAsync(s=>s.Visible==true);
            var mappedGroupBlogs = _mapper.Map<ICollection<ResultGroupBlog>>(GroupBlogs);

            return Ok(new ResponseApiEntities<ResultGroupBlog>
                                                            (entities: mappedGroupBlogs,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: GroupBlogs.Count()));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("GroupBlogs/ByGuid/{guid}")]
        public async Task<IActionResult> GetGroupBlogById([FromRoute] string guid)
        {
            try
            {
                var GroupBlog = await _GroupBlogService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (GroupBlog == null)
                    return BadRequest(new ResponseApiEntity<UpdateGroupBlog>
                                                                              (entity: new UpdateGroupBlog(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateGroupBlog>(GroupBlog);
                return Ok(new ResponseApiEntity<UpdateGroupBlog>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateGroupBlog>
                                                                             (entity: new UpdateGroupBlog(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }

    }
}
