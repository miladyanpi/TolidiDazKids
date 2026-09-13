using AutoMapper;
using DAL.Context;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoCategory;
using Dto.Models.DtoProduct;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.CategorySrv;
using ServicesLibrary.Services.ProductSrv;
using ServicesLibrary.Services.SettingSrv;
using System.Data.Entity;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]

    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _CategoryService;
        private readonly IProductService _ProductService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _ApplicationDbContext;


        public CategoryController(
            ICategoryService CategoryService,
            IProductService ProductService,
            ApplicationDbContext ApplicationDbContext,
            IMapper mapper
            )
        {
            _CategoryService = CategoryService;
            _ProductService = ProductService;
            _ApplicationDbContext = ApplicationDbContext;
            _mapper = mapper;
        }
        [HttpPost("Categorys")]
        public async Task<IActionResult> Add([FromBody] AddCategory model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddCategory>
                                                               (entity: model,
                                                               id: 0,
                                                               statusCode: ResultMessageApi.ErrorCode,
                                                               status: ResultMessageApi.Error,
                                                               message: ResultMessageApi.BadRequestError));
            if (model.ParentID1 == 0 || model.ParentID1 == null)
                model.ParentID1 = null;
            else if (model.ParentID1 > 0 && (model.ParentID2 == 0 || model.ParentID1 == null))
            {
                model.ParentID1 = model.ParentID1;
            }
            else if (model.ParentID1 > 0 && model.ParentID2 > 0)
            {
                model.ParentID1 = model.ParentID2;
            }
            var Category = _mapper.Map<AddCategory, Category>(model);
            int id = await _CategoryService.AddAsync(Category);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddCategory>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddCategory>
                                                           (entity: new AddCategory(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }

        [HttpPatch("Categorys")]
        public async Task<IActionResult> Update([FromBody] UpdateCategory model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateCategory>
                                                               (entity: new UpdateCategory(),
                                                               id: 0,
                                                               statusCode: ResultMessageApi.ErrorCode,
                                                               status: ResultMessageApi.Error,
                                                               message: ResultMessageApi.BadRequestError));

            var Category = _mapper.Map<UpdateCategory, Category>(model);
            var upd = await _CategoryService.UpdateAsync(Category);
            if (upd > 0)
                return Ok(new ResponseApiEntity<UpdateCategory>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateCategory>
                                                           (entity: new UpdateCategory(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [HttpDelete("Categorys/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var c1 = await _CategoryService.GetCountAllAsync(s => s.ParentID == id);
            if (c1 > 0)
                return BadRequest(new ResponseApiEntity<ResultCategory>
                                                         (entity: new ResultCategory(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.NotAllowDeleteError));

            var del = await _CategoryService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultCategory>
                                                               (entity: new ResultCategory(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultCategory>
                                                           (entity: new ResultCategory(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [HttpGet("Categorys")]
        public async Task<IActionResult> GetCategorys([FromQuery] PaginationParams @params, int? ParentID = null)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultCategory>
                                                            (entities: new List<ResultCategory>(),
                                                            status: ResultMessageApi.Error,
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            message: ResultMessageApi.BadRequestError,
                                                            countAllRecordTable: 0));


            var count = await _CategoryService
                           .GetCountAllAsync(s =>
                           s.Title.Contains(@params.SearchText) && s.ParentID == ParentID);

            var Categorys = await _CategoryService
                                 .GetAllAsync(s =>
                                 (s.Title.Contains(@params.SearchText) && s.ParentID == ParentID)
                                 , page: @params.Page, take: @params.Take);


            var mappedCategorys = _mapper.Map<ICollection<ResultCategory>>(Categorys);

            return Ok(new ResponseApiEntities<ResultCategory>
                                                            (entities: mappedCategorys,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));
        }
        [HttpGet("Categorys/{id}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] int id)
        {
            var Category = await _CategoryService.GetByIdAsync(id);

            var result = _mapper.Map<UpdateCategory>(Category);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateCategory>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateCategory>
                                                           (entity: new UpdateCategory(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpPatch("Categorys/UpdateJsonFile")]
        public async Task<IActionResult> UpdateUpdateCategoryJsonFile([FromBody] UpdateJsonFile model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var q = await _CategoryService.GetByIdAsync(model.ID);
            if (q == null)
                return BadRequest(new ResponseApiEntity<UpdateJsonFile>
                                                         (entity: new UpdateJsonFile(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.UpdateError));
            q.JsonPicture = model.JsonPicture;
            // var Category = _mapper.Map<UpdateCustomer, Customer>(q);
            var upd = await _CategoryService.UpdateAsync(q);
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
        [HttpGet("Categorys/All")]
        public async Task<IActionResult> GetCategorysAll([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultCategory>
                                                            (entities: new List<ResultCategory>(),
                                                            status: ResultMessageApi.Error,
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            message: ResultMessageApi.BadRequestError,
                                                            countAllRecordTable: 0));

            var count = await _CategoryService
                           .GetCountAllAsync(s =>
                           s.Title.Contains(@params.SearchText));

            var Categorys = await _CategoryService
                                 .GetAllAsync(s =>
                                 s.Title.Contains(@params.SearchText)
                                 , page: @params.Page, take: @params.Take);

            var mappedCategorys = _mapper.Map<ICollection<ResultCategory>>(Categorys);

            return Ok(new ResponseApiEntities<ResultCategory>
                                                            (entities: mappedCategorys,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));
        }
        [HttpGet("Categorys/ParentID")]
        public async Task<IActionResult> GetCategorysByParentID([FromQuery] int? ParentID = null)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultCategory>
                                                            (entities: new List<ResultCategory>(),
                                                            status: ResultMessageApi.Error,
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            message: ResultMessageApi.BadRequestError,
                                                            countAllRecordTable: 0));





            var count = await _CategoryService
                           .GetCountAllAsync(s => s.Visible == true && s.ParentID == ParentID);
            var Categorys = await _CategoryService
                                 .GetAllAsync(s => s.Visible == true && s.ParentID == ParentID);
            var mappedCategorys = _mapper.Map<ICollection<ResultCategory>>(Categorys);
            return Ok(new ResponseApiEntities<ResultCategory>
                                                            (entities: mappedCategorys,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));
        }
        [HttpGet("Categorys/Public")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategorys([FromQuery] int? ParentID = null)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultCategory>
                                                            (entities: new List<ResultCategory>(),
                                                            status: ResultMessageApi.Error,
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            message: ResultMessageApi.BadRequestError,
                                                            countAllRecordTable: 0));
 
            var Categorys = await _CategoryService
                     .GetAllAsync(s => s.Visible == true && s.ParentID == ParentID);
            var mappedCategorys = _mapper.Map<ICollection<ResultCategory>>(Categorys);
            return Ok(new ResponseApiEntities<ResultCategory>
                                                            (entities: mappedCategorys,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Categorys.Count()));
        }

        [HttpGet("Categorys/Public/UniqCode")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCategorys([FromQuery] string? UniqCode = null)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultCategory>
                                                            (entity: new ResultCategory(),
                                                            status: ResultMessageApi.Error,
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            message: ResultMessageApi.BadRequestError));
            Guid code;
            var isValid = Guid.TryParse(UniqCode, out code);
            if (!isValid)
            {
                return BadRequest(new ResponseApiEntity<ResultProduct>
                                                         (entity: new ResultProduct(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.GetError));
            }
            var Categorys = await _CategoryService
                                 .FirstOrDefaultAsync(s => s.Visible == true && s.IdentityCode == code);
            var mappedCategorys = _mapper.Map<ResultCategory>(Categorys);
            return Ok(new ResponseApiEntity<ResultCategory>
                                                            (entity: mappedCategorys,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk));
        }
    }
}
