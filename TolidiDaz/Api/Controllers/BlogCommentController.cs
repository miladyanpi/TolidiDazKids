using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoBlogComment;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.BlogCommentSrv;
using ServicesLibrary.Services.BlogSrv;
using ServicesLibrary.Services.ViewCounter;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BlogCommentController(
        IBlogCommentService _BlogCommentService,
        IBlogService _BlogService,
        //ICustomerService _CustomerService,
        IMapper _mapper,
        IViewCounterService _viewCounterService
        )
        : ControllerBase
    {
        [HttpPost("BlogComments")]
        [AllowAnonymous]
        public async Task<IActionResult> Add([FromBody] AddBlogComment model, string Key)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddBlogComment>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
            if (Keys.CustomerSatisfactionKey != Key)
                return BadRequest(new ResponseApiEntity<AddBlogComment>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var BlogComment = _mapper.Map<BlogComment>(model);
            int id = await _BlogCommentService.AddAsync(BlogComment);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddBlogComment>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddBlogComment>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        //[Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        //[HttpPatch("BlogComments")]
        //public async Task<IActionResult> Update([FromBody] UpdateBlogComment model)
        //{
        //    if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateBlogComment>
        //                                                   (entity: new UpdateBlogComment(),
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.UpdateError));


        //    var BlogComment = _mapper.Map<UpdateBlogComment, BlogComment>(model);
        //    var upd = await _BlogCommentService.UpdateAsync(BlogComment);
        //    if (upd > 0)
        //    {
        //        return Ok(new ResponseApiEntity<UpdateBlogComment>
        //                                                     (entity: model,
        //                                                     statusCode: ResultMessageApi.SuccessCode,
        //                                                     status: ResultMessageApi.Success,
        //                                                     message: ResultMessageApi.UpdateOk));
        //    }

        //    else
        //        return BadRequest(new ResponseApiEntity<UpdateBlogComment>
        //                                                   (entity: new UpdateBlogComment(),
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.UpdateError));
        //}
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("BlogComments/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _BlogCommentService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultBlogComment>
                                                               (entity: new ResultBlogComment(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultBlogComment>
                                                           (entity: new ResultBlogComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [HttpPost("BlogComments/Data")]
        public async Task<IActionResult> GetBlogComments([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultBlogComment>
                                                                    (entities: new List<ResultBlogComment>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<BlogComment, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.BlogID.ToString() ?? "").Contains(search);
                }

                var count = await _BlogCommentService.GetCountAllAsync(predicate);

                var BlogComments = await _BlogCommentService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedBlogComments = _mapper.Map<ICollection<ResultBlogComment>>(BlogComments);

                return Ok(new ResponseApiEntities<ResultBlogComment>
                                                                (entities: mappedBlogComments,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultBlogComment>
                                                                    (entities: new List<ResultBlogComment>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("BlogComments/SetStatus")]
        public async Task<IActionResult> SetStatus([FromQuery] int Id, EnumConstant.CommentStatus commentStatus)
        {

            var data = await _BlogCommentService.GetByIdAsync(Id);
            if (data is null)
                return BadRequest(new ResponseApiEntity<ResultBlogComment>
                                                          (entity: new ResultBlogComment(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.ErrorBadRequest));
            data.Status = commentStatus;
            string message = string.Empty;
            switch(commentStatus)
            {
                case EnumConstant.CommentStatus.Approved:
                    message = "پیام ارسالی کاربر تایید شد";
                    break;
                case EnumConstant.CommentStatus.Rejected:
                    message = "پیام ارسالی کاربر رد شد";
                    break;
            }
            var upd = await _BlogCommentService.UpdateAsync(data);
            return Ok(new ResponseApiEntity<ResultBlogComment>
                                                               (entity: new ResultBlogComment(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: message));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("BlogComments/{id}")]
        public async Task<IActionResult> GetBlogCommentById([FromRoute] int id)
        {

            var data = await _BlogCommentService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateBlogComment>
                                                           (entity: new UpdateBlogComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateBlogComment>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateBlogComment>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateBlogComment>
                                                           (entity: new UpdateBlogComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("BlogComments/ByGuid/{guid}")]
        public async Task<IActionResult> GetBlogCommentByGuid([FromRoute] string guid)
        {
            bool isValid = Guid.TryParse(guid, out Guid gid);
            if (!isValid)
                return BadRequest(new ResponseApiEntity<ResultBlogComment>
                                                            (entity: new ResultBlogComment(),
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            status: ResultMessageApi.Error,
                                                            message: ResultMessageApi.GetError));


            var data = await _BlogCommentService.FirstOrDefaultAsync(s => s.IdentityCode == gid);
            if (data == null)
                return BadRequest(new ResponseApiEntity<ResultBlogComment>
                                                           (entity: new ResultBlogComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<ResultBlogComment>(data);
            if (result != null)
                return Ok(new ResponseApiEntity<ResultBlogComment>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<ResultBlogComment>
                                                           (entity: new ResultBlogComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("BlogComments")]
        public async Task<IActionResult> GetBlogComments([FromQuery] PaginationParams @params, int? BlogID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultBlogComment>
                                                           (entity: new ResultBlogComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            Expression<Func<BlogComment, bool>> predicate = x => true;
            if (!string.IsNullOrWhiteSpace(@params.SearchText))
            {
                var search = @params.SearchText;
                predicate = x =>
                    x.AuthorName.Contains(search);
            }
            if (BlogID != null && BlogID > 0)
            {
                predicate = x =>
                    x.BlogID == BlogID;
            }
            var count = await _BlogCommentService.GetCountAllAsync(predicate);
            var BlogComments = await _BlogCommentService
                                   .GetAllAsync(predicate
                                   , page: @params.Page, take: @params.Take);


            var mappedBlogComments = _mapper.Map<ICollection<ResultBlogComment>>(BlogComments);

            return Ok(new ResponseApiEntities<ResultBlogComment>
                                                            (entities: mappedBlogComments,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [HttpGet("BlogComments/Public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlogCommentsPublic([FromQuery] PaginationParams @params, int? BlogID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultBlogComment>
                                                           (entity: new ResultBlogComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            Expression<Func<BlogComment, bool>> predicate = x => true;
            if (!string.IsNullOrWhiteSpace(@params.SearchText))
            {
                var search = @params.SearchText;
                predicate = x =>

                    x.AuthorName.Contains(search);
            }
            if (BlogID != null && BlogID > 0)
            {
                predicate = x =>
                    x.BlogID == BlogID;
            }
            var count = await _BlogCommentService.GetCountAllAsync(predicate);
            var BlogComments = await _BlogCommentService
                                   .GetAllAsync(predicate
                                   , page: @params.Page, take: @params.Take);

            var mappedBlogComments = _mapper.Map<ICollection<ResultBlogComment>>(BlogComments);

            return Ok(new ResponseApiEntities<ResultBlogComment>
                                                            (entities: mappedBlogComments,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [HttpGet("BlogComments/Public/All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlogCommentsPublicAll([FromQuery] int? BlogID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultBlogComment>
                                                           (entity: new ResultBlogComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            Expression<Func<BlogComment, bool>> predicate = x => true;

            predicate = x => x.BlogID == BlogID;
            if (BlogID != null && BlogID > 0)
            {
                predicate = x =>
                    x.BlogID == BlogID && x.Status==EnumConstant.CommentStatus.Approved;
            }
            var count = await _BlogCommentService.GetCountAllAsync(predicate);
            var BlogComments = await _BlogCommentService
                                   .GetAllAsync(predicate);

            var mappedBlogComments = _mapper.Map<ICollection<ResultBlogComment>>(BlogComments);

            return Ok(new ResponseApiEntities<ResultBlogComment>
                                                            (entities: mappedBlogComments,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [HttpGet("BlogComments/ByGuid/{guid}")]
        public async Task<IActionResult> GetBlogCommentById([FromRoute] string guid)
        {
            try
            {
                var BlogComment = await _BlogCommentService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (BlogComment == null)
                    return BadRequest(new ResponseApiEntity<UpdateBlogComment>
                                                                              (entity: new UpdateBlogComment(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateBlogComment>(BlogComment);
                return Ok(new ResponseApiEntity<UpdateBlogComment>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateBlogComment>
                                                                             (entity: new UpdateBlogComment(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }

    }
}
