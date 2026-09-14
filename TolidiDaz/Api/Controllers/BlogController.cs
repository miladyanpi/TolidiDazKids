using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoBlog;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.BlogSrv;
using ServicesLibrary.Services.GroupBlogSrv;
using ServicesLibrary.Services.ViewCounter;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BlogController(
        IBlogService _BlogService,
        IGroupBlogService _GroupBlogService,
        IMapper _mapper,
        IViewCounterService _viewCounterService
        )
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Blogs")]
        public async Task<IActionResult> Add([FromBody] AddBlog model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddBlog>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var Blog = _mapper.Map<AddBlog, Blog>(model);
            int id = await _BlogService.AddAsync(Blog);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddBlog>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddBlog>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Blogs")]
        public async Task<IActionResult> Update([FromBody] UpdateBlog model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateBlog>
                                                           (entity: new UpdateBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));


            var Blog = _mapper.Map<UpdateBlog, Blog>(model);
            var upd = await _BlogService.UpdateAsync(Blog);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateBlog>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }

            else
                return BadRequest(new ResponseApiEntity<UpdateBlog>
                                                           (entity: new UpdateBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [HttpPatch("Blogs/UpdateJsonFile")]
        public async Task<IActionResult> UpdateUpdateBlogJsonFile([FromBody] UpdateJsonFile model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var q = await _BlogService.GetByIdAsync(model.ID);
            if (q == null)
                return BadRequest(new ResponseApiEntity<UpdateJsonFile>
                                                         (entity: new UpdateJsonFile(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.UpdateError));
            switch (model.EnumJsonImageFileVideo)
            {
                case EnumConstant.EnumJsonImageFileVideo.Image:
                    q.JsonPicture = model.JsonPicture;
                    break;

            }
            // var Blog = _mapper.Map<UpdateBlog, Blog>(q);
            var upd = await _BlogService.UpdateAsync(q);
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
        [HttpDelete("Blogs/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _BlogService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultBlog>
                                                               (entity: new ResultBlog(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultBlog>
                                                           (entity: new ResultBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName + "," + ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Blogs/{id}")]
        public async Task<IActionResult> GetBlogById([FromRoute] int id)
        {

            var data = await _BlogService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateBlog>
                                                           (entity: new UpdateBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateBlog>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateBlog>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateBlog>
                                                           (entity: new UpdateBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("Blogs/Public/{identityCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlogByidentityCode([FromRoute] string identityCode)
        {

            var data = await _BlogService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == identityCode);
            if (data == null)
                return BadRequest(new ResponseApiEntity<ResultBlog>
                                                           (entity: new ResultBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            string userIdentifier = GetUserIdentifier();
            // ثبت بازدید
            _viewCounterService.RecordViewBlog(data.ID, userIdentifier);
            var result = _mapper.Map<ResultBlog>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<ResultBlog>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<ResultBlog>
                                                           (entity: new ResultBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        private string GetUserIdentifier()
        {
            // اگر کاربر لاگین کرده باشد
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                return $"user:{userId}";
            }

            // کاربر مهمان: ترکیبی از IP + User-Agent
            string ip = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            string userAgent = Request.Headers["User-Agent"].ToString();

            // هش ساده برای کوتاه کردن
            int hash = (ip + userAgent).GetHashCode();
            return $"guest:{ip}:{hash}";
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Blogs/Data")]
        public async Task<IActionResult> GetBlogs([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultBlog>
                                                                    (entities: new List<ResultBlog>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<Blog, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Title ?? "").Contains(search);
                }

                var count = await _BlogService.GetCountAllAsync(predicate);

                var Blogs = await _BlogService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedBlogs = _mapper.Map<ICollection<ResultBlog>>(Blogs);

                return Ok(new ResponseApiEntities<ResultBlog>
                                                                (entities: mappedBlogs,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultBlog>
                                                                    (entities: new List<ResultBlog>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Blogs")]
        public async Task<IActionResult> GetBlogs([FromQuery] PaginationParams @params, int? GroupBlogID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultBlog>
                                                           (entity: new ResultBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            Expression<Func<Blog, bool>> predicate = x => true;
            if (!string.IsNullOrWhiteSpace(@params.SearchText))
            {
                var search = @params.SearchText;
                predicate = x =>
                    x.Title.Contains(search);
            }
            if (GroupBlogID != null && GroupBlogID > 0)
            {
                predicate = x =>
                    x.GroupBlogID == GroupBlogID;
            }
            var count = await _BlogService.GetCountAllAsync(predicate);
            var Blogs = await _BlogService
                                   .GetAllAsync(predicate
                                   , page: @params.Page, take: @params.Take);


            var mappedBlogs = _mapper.Map<ICollection<ResultBlog>>(Blogs);

            return Ok(new ResponseApiEntities<ResultBlog>
                                                            (entities: mappedBlogs,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [HttpGet("Blogs/Public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlogsPublic([FromQuery] PaginationParams @params, int? GroupBlogID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultBlog>
                                                           (entity: new ResultBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            Expression<Func<Blog, bool>> predicate = x => true;
            if (!string.IsNullOrWhiteSpace(@params.SearchText))
            {
                var search = @params.SearchText;
                predicate = x =>
                    x.Title.Contains(search);
            }
            if (GroupBlogID != null && GroupBlogID > 0)
            {
                predicate = x =>
                    x.GroupBlogID == GroupBlogID;
            }
            var count = await _BlogService.GetCountAllAsync(predicate);
            var Blogs = await _BlogService
                                   .GetAllAsync(predicate
                                   , page: @params.Page, take: @params.Take);

            var mappedBlogs = _mapper.Map<ICollection<ResultBlog>>(Blogs);

            return Ok(new ResponseApiEntities<ResultBlog>
                                                            (entities: mappedBlogs,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [HttpGet("Blogs/Public/IdentityCode")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlogsPublic([FromQuery] PaginationParams @params, string? Code)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultBlog>
                                                           (entity: new ResultBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            int GroupBlogId = 0;
            var data = await _GroupBlogService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == Code);
            if (data != null)
                GroupBlogId = data.ID;

            Expression<Func<Blog, bool>> predicate = x => true;
            if (!string.IsNullOrWhiteSpace(@params.SearchText))
            {
                var search = @params.SearchText;
                predicate = x =>
                    x.Title.Contains(search);
            }
            if(GroupBlogId>0)
                predicate = x =>
                    x.GroupBlogID == GroupBlogId;

            var count = await _BlogService.GetCountAllAsync(predicate);
            var Blogs = await _BlogService
                                   .GetAllAsync(predicate
                                   , page: @params.Page, take: @params.Take);

            var mappedBlogs = _mapper.Map<ICollection<ResultBlog>>(Blogs);

            return Ok(new ResponseApiEntities<ResultBlog>
                                                            (entities: mappedBlogs,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        ///
        [HttpGet("Blogs/Public/RelatedBlogsCount/{GroupBlogID}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlogsPublicRelatedBlogsCount([FromQuery] int GroupBlogID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultBlog>
                                                           (entity: new ResultBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
    

            Expression<Func<Blog, bool>> predicate = x => true;
            if (GroupBlogID > 0)
                predicate = x =>
                    x.GroupBlogID == GroupBlogID;

            var count = await _BlogService.GetCountAllAsync(predicate);
            var Blogs = await _BlogService
                                   .GetAllAsync(predicate
                                   , page: 1, take: 8);

            var mappedBlogs = _mapper.Map<ICollection<ResultBlog>>(Blogs);

            return Ok(new ResponseApiEntities<ResultBlog>
                                                            (entities: mappedBlogs,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [HttpGet("Blogs/Public/All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlogsAll()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultBlog>
                                                           (entity: new ResultBlog(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var Blogs = await _BlogService
                                .GetAllAsync(page: 1, take: 15);

            var mappedBlogs = _mapper.Map<ICollection<ResultBlog>>(Blogs);

            return Ok(new ResponseApiEntities<ResultBlog>
                                                            (entities: mappedBlogs,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Blogs.Count()));

        }
        /// <summary>
        /// بلاگی که بیشترین بازدید را داشته اند
        /// </summary>
        /// <returns></returns>
        [HttpGet("Blogs/MostVisitedBlog")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBlogsAllNewProduct()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultBlog>
                                                            (entities: new List<ResultBlog>(),
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: 0));
            var blogs = await _BlogService.GetTopAsync(p => p.NumberOfVisits, 5);
            var mappedProducts = _mapper.Map<ICollection<ResultBlog>>(blogs);

            return Ok(new ResponseApiEntities<ResultBlog>
                                                            (entities: mappedProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: mappedProducts.Count()));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Blogs/ByGuid/{guid}")]
        public async Task<IActionResult> GetBlogById([FromRoute] string guid)
        {
            try
            {
                var Blog = await _BlogService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (Blog == null)
                    return BadRequest(new ResponseApiEntity<UpdateBlog>
                                                                              (entity: new UpdateBlog(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateBlog>(Blog);
                return Ok(new ResponseApiEntity<UpdateBlog>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateBlog>
                                                                             (entity: new UpdateBlog(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }
    }
}
