using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoProductComment;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.OrderSrv;
using ServicesLibrary.Services.ProductCommentSrv;
using ServicesLibrary.Services.ViewCounter;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductCommentController(
        IProductCommentService _ProductCommentService,
        IOrderService _OrderService,
        ICustomerService _CustomerService,
        IMapper _mapper,
        IViewCounterService _viewCounterService
        //UserManager<Account> userManager
        ) 
        : ControllerBase
    {
        [HttpPost("ProductComments")]
        [AllowAnonymous]
        public async Task<IActionResult> Add([FromBody] AddProductComment model, string Key)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddProductComment>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
            if (Keys.CustomerSatisfactionKey != Key)
                return BadRequest(new ResponseApiEntity<AddProductComment>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var CustomerID = User.Claims
                    .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(UniqCode, out Guid guid))
            {
                var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
                if (customer != null)
                {
                    if ((string.IsNullOrEmpty(customer.Name) && string.IsNullOrEmpty(customer.LastName)))
                        model.AuthorName = $"{model.AuthorName}(کاربر سایت)";
                    else
                        model.AuthorName = $"{customer.Name} {customer.LastName}(کاربر سایت)";
                    model.CustomerID = customer.ID;
                    var order = await _OrderService.FirstOrDefaultAsync(s => s.CustomerID == customer.ID);
                    if(order != null)
                    {
                        model.IsVerifiedBuyer = true;
                    }
                }
            }

            var ProductComment = _mapper.Map<ProductComment>(model);
            int id = await _ProductCommentService.AddAsync(ProductComment);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddProductComment>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddProductComment>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        //[Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        //[HttpPatch("ProductComments")]
        //public async Task<IActionResult> Update([FromBody] UpdateProductComment model)
        //{
        //    if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateProductComment>
        //                                                   (entity: new UpdateProductComment(),
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.UpdateError));


        //    var ProductComment = _mapper.Map<UpdateProductComment, ProductComment>(model);
        //    var upd = await _ProductCommentService.UpdateAsync(ProductComment);
        //    if (upd > 0)
        //    {
        //        return Ok(new ResponseApiEntity<UpdateProductComment>
        //                                                     (entity: model,
        //                                                     statusCode: ResultMessageApi.SuccessCode,
        //                                                     status: ResultMessageApi.Success,
        //                                                     message: ResultMessageApi.UpdateOk));
        //    }

        //    else
        //        return BadRequest(new ResponseApiEntity<UpdateProductComment>
        //                                                   (entity: new UpdateProductComment(),
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.UpdateError));
        //}

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("ProductComments/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _ProductCommentService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultProductComment>
                                                               (entity: new ResultProductComment(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultProductComment>
                                                           (entity: new ResultProductComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductComments/SetStatus")]
        public async Task<IActionResult> SetStatus([FromQuery] int Id, EnumConstant.CommentStatus commentStatus)
        {

            var data = await _ProductCommentService.GetByIdAsync(Id);
            if (data is null)
                return BadRequest(new ResponseApiEntity<ResultProductComment>
                                                          (entity: new ResultProductComment(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.ErrorBadRequest));
            data.Status = commentStatus;
            string message = string.Empty;
            switch (commentStatus)
            {
                case EnumConstant.CommentStatus.Approved:
                    message = "پیام ارسالی کاربر تایید شد";
                    break;
                case EnumConstant.CommentStatus.Rejected:
                    message = "پیام ارسالی کاربر رد شد";
                    break;
            }
            var upd = await _ProductCommentService.UpdateAsync(data);
            return Ok(new ResponseApiEntity<ResultProductComment>
                                                               (entity: new ResultProductComment(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: message));

        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName + "," + ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductComments/{id}")]
        public async Task<IActionResult> GetProductCommentById([FromRoute] int id)
        {

            var data = await _ProductCommentService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateProductComment>
                                                           (entity: new UpdateProductComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateProductComment>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateProductComment>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateProductComment>
                                                           (entity: new UpdateProductComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductComments/ByGuid/{guid}")]
        public async Task<IActionResult> GetProductCommentByGuid([FromRoute] string guid)
        {
            bool isValid = Guid.TryParse(guid, out Guid gid);
            if (!isValid)
                return BadRequest(new ResponseApiEntity<ResultProductComment>
                                                            (entity: new ResultProductComment(),
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            status: ResultMessageApi.Error,
                                                            message: ResultMessageApi.GetError));


            var data = await _ProductCommentService.FirstOrDefaultAsync(s => s.IdentityCode == gid);
            if (data == null)
                return BadRequest(new ResponseApiEntity<ResultProductComment>
                                                           (entity: new ResultProductComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<ResultProductComment>(data);
            if (result != null)
                return Ok(new ResponseApiEntity<ResultProductComment>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<ResultProductComment>
                                                           (entity: new ResultProductComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("ProductComments")]
        public async Task<IActionResult> GetProductComments([FromQuery] PaginationParams @params, int? ProductID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultProductComment>
                                                           (entity: new ResultProductComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            Expression<Func<ProductComment, bool>> predicate = x => true;
            if (!string.IsNullOrWhiteSpace(@params.SearchText))
            {
                var search = @params.SearchText;
                predicate = x =>
                    x.AuthorName.Contains(search);
            }
            if (ProductID != null && ProductID > 0)
            {
                predicate = x =>
                    x.ProductID == ProductID;
            }
            var count = await _ProductCommentService.GetCountAllAsync(predicate);
            var ProductComments = await _ProductCommentService
                                   .GetAllAsync(predicate
                                   , page: @params.Page, take: @params.Take);


            var mappedProductComments = _mapper.Map<ICollection<ResultProductComment>>(ProductComments);

            return Ok(new ResponseApiEntities<ResultProductComment>
                                                            (entities: mappedProductComments,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [HttpGet("ProductComments/Public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductCommentsPublic([FromQuery] PaginationParams @params, int? ProductID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultProductComment>
                                                           (entity: new ResultProductComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            Expression<Func<ProductComment, bool>> predicate = x => true;
            if (!string.IsNullOrWhiteSpace(@params.SearchText))
            {
                var search = @params.SearchText;
                predicate = x =>
               
                    x.AuthorName.Contains(search);
            }
            if (ProductID != null && ProductID > 0)
            {
                predicate = x =>
                    x.ProductID == ProductID;
            }
            var count = await _ProductCommentService.GetCountAllAsync(predicate);
            var ProductComments = await _ProductCommentService
                                   .GetAllAsync(predicate
                                   , page: @params.Page, take: @params.Take);

            var mappedProductComments = _mapper.Map<ICollection<ResultProductComment>>(ProductComments);

            return Ok(new ResponseApiEntities<ResultProductComment>
                                                            (entities: mappedProductComments,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [HttpGet("ProductComments/Public/All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductCommentsPublicAll([FromQuery] int? ProductID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultProductComment>
                                                           (entity: new ResultProductComment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            Expression<Func<ProductComment, bool>> predicate = x => true;

            predicate = x => x.ProductID == ProductID;
            if (ProductID != null && ProductID > 0)
            {
                predicate = x =>
                    x.ProductID == ProductID && x.Status == EnumConstant.CommentStatus.Approved;
            }
            var count = await _ProductCommentService.GetCountAllAsync(predicate);
            var ProductComments = await _ProductCommentService
                                   .GetAllAsync(predicate);

            var mappedProductComments = _mapper.Map<ICollection<ResultProductComment>>(ProductComments);

            return Ok(new ResponseApiEntities<ResultProductComment>
                                                            (entities: mappedProductComments,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }

    }
}
