using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoCategoryTrait;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.CategoryTraitSrv;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CategoryTraitController(
        ICategoryTraitService _CategoryTraitService,
        ICustomerService _CustomerService,
        IMapper _mapper,
         UserManager<Account> userManager
        )
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpPost("CategoryTraits")]
        public async Task<IActionResult> Add([FromQuery] int? CategoryID)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddCategoryTrait>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
            var TraitID = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

            var UniqCode = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<AddCategoryTrait>
                                                                 (entity: new AddCategoryTrait(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(TraitID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<AddCategoryTrait>
                                                                 (entity: new AddCategoryTrait(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var CheckExists = await _CategoryTraitService
                            .FirstOrDefaultAsync(s=>s.TraitID== customer.ID&&s.CategoryID==CategoryID);
            int id = 0;
            bool f = true;
            var model = new AddCategoryTrait
            {
                CategoryID = CategoryID,
                TraitID = customer.ID,
            };
            var CategoryTrait = _mapper.Map<AddCategoryTrait,CategoryTrait>(model);

            if (CheckExists==null)
            {

                id = await _CategoryTraitService.AddAsync(CategoryTrait);
                f = true;
            }
            else
            {
                id = await _CategoryTraitService.DeleteAsync(CheckExists.ID);
                f = false;
            }

            if (id > 0)
                return Ok(new ResponseApiEntity<AddCategoryTrait>
                                                               (entity: new AddCategoryTrait() { CategoryID = CategoryID },
                                                               id: id,
                                                               statusCode: f ? ResultMessageApi.SuccessAddCode : ResultMessageApi.SuccessDeleteCode,
                                                               status: ResultMessageApi.Success,
                                                               message:f? ResultMessageApi.AddFavoritOk: ResultMessageApi.DeleteFavoritOk));
            else
                return BadRequest(new ResponseApiEntity<AddCategoryTrait>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        //[Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        //[HttpPatch("CategoryTraits")]
        //public async Task<IActionResult> Update([FromBody] UpdateCategoryTrait model)
        //{
        //    if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateCategoryTrait>
        //                                                   (entity: new UpdateCategoryTrait(),
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.UpdateError));
            
       
        //    var CategoryTrait = _mapper.Map<UpdateCategoryTrait, CategoryTrait>(model);
        //    var upd = await _CategoryTraitService.UpdateAsync(CategoryTrait);
        //    if (upd > 0)
        //    {
        //        return Ok(new ResponseApiEntity<UpdateCategoryTrait>
        //                                                     (entity: model,
        //                                                     statusCode: ResultMessageApi.SuccessCode,
        //                                                     status: ResultMessageApi.Success,
        //                                                     message: ResultMessageApi.UpdateOk));
        //    }
              
        //    else
        //        return BadRequest(new ResponseApiEntity<UpdateCategoryTrait>
        //                                                   (entity: new UpdateCategoryTrait(),
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.UpdateError));
        //}
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("CategoryTraits/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _CategoryTraitService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultCategoryTrait>
                                                               (entity: new ResultCategoryTrait(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultCategoryTrait>
                                                           (entity: new ResultCategoryTrait(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("CategoryTraits/Data")]
        public async Task<IActionResult> GetCategoryTraits([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultCategoryTrait>
                                                                    (entities: new List<ResultCategoryTrait>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                //Expression<Func<CategoryTrait, bool>> predicate = x => true;

                //if (!string.IsNullOrWhiteSpace(@params.SearchText))
                //{
                //    var search = @params.SearchText;

                //    predicate = x =>
                //        (x.Name ?? "").Contains(search);
                //}

                var count = await _CategoryTraitService.GetCountAllAsync();

                var CategoryTraits = await _CategoryTraitService
                                    .GetAllAsync( page: @params.Page, take: @params.Take);


                var mappedCategoryTraits = _mapper.Map<ICollection<ResultCategoryTrait>>(CategoryTraits);

                return Ok(new ResponseApiEntities<ResultCategoryTrait>
                                                                (entities: mappedCategoryTraits,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultCategoryTrait>
                                                                    (entities: new List<ResultCategoryTrait>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("CategoryTraits")]
        public async Task<IActionResult> GetCategoryTraits([FromQuery] PaginationParams @params,int? TraitID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultCategoryTrait>
                                                           (entity: new ResultCategoryTrait(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var CategoryTraits = await _CategoryTraitService
                                .GetAllAsync(page: @params.Page, take: @params.Take);

            var mappedCategoryTraits = _mapper.Map<ICollection<ResultCategoryTrait>>(CategoryTraits);

            return Ok(new ResponseApiEntities<ResultCategoryTrait>
                                                            (entities: mappedCategoryTraits,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: CategoryTraits.Count()));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("CategoryTraits/AllDataForCurrentUser")]
        public async Task<IActionResult> GetCategoryTraits()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultCategoryTrait>
                                                           (entity: new ResultCategoryTrait(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            var TraitID = User.Claims
                    .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

            var UniqCode = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<AddCategoryTrait>
                                                                 (entity: new AddCategoryTrait(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(TraitID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<AddCategoryTrait>
                                                                 (entity: new AddCategoryTrait(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var CategoryTraits = await _CategoryTraitService
                                .GetAllAsync(s=>s.TraitID==customer.ID);

            var mappedCategoryTraits = _mapper.Map<ICollection<ResultCategoryTrait>>(CategoryTraits);

            return Ok(new ResponseApiEntities<ResultCategoryTrait>
                                                            (entities: mappedCategoryTraits,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: CategoryTraits.Count()));

        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpGet("CategoryTraits2/Public")]
        public async Task<IActionResult> GetCategoryTraits2([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultCategoryTrait>
                                                           (entity: new ResultCategoryTrait(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));
            var TraitID = User.Claims
                        .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<AddCategoryTrait>
                                                                 (entity: new AddCategoryTrait(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.ID == int.Parse(TraitID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<AddCategoryTrait>
                                                                 (entity: new AddCategoryTrait(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var count = await _CategoryTraitService
                               .GetCountAllAsync(s => s.TraitID == int.Parse(TraitID));
            var CategoryTraits = await _CategoryTraitService
                                .GetAllAsync(s=>s.TraitID== int.Parse(TraitID),page: @params.Page, take: @params.Take);

            var mappedCategoryTraits = _mapper.Map<ICollection<ResultCategoryTrait>>(CategoryTraits);

            return Ok(new ResponseApiEntities<ResultCategoryTrait>
                                                            (entities: mappedCategoryTraits,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("CategoryTraits/{id}")]
        public async Task<IActionResult> GetCategoryTraitById([FromRoute] int id)
        {

            var data = await _CategoryTraitService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateCategoryTrait>
                                                           (entity: new UpdateCategoryTrait(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateCategoryTrait>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateCategoryTrait>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateCategoryTrait>
                                                           (entity: new UpdateCategoryTrait(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("CategoryTraits/ByGuid/{guid}")]
        public async Task<IActionResult> GetCategoryTraitById([FromRoute] string guid)
        {
            try
            {
                var CategoryTrait = await _CategoryTraitService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (CategoryTrait == null)
                    return BadRequest(new ResponseApiEntity<UpdateCategoryTrait>
                                                                              (entity: new UpdateCategoryTrait(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateCategoryTrait>(CategoryTrait);
                return Ok(new ResponseApiEntity<UpdateCategoryTrait>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateCategoryTrait>
                                                                             (entity: new UpdateCategoryTrait(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }

    }
}
