using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoRawProduct;
using Dto.Models.DtoRawProductStore_Product;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.RawProductStore_ProductSrv;
using ServicesLibrary.Services.RegisterCostRawProductStoreSrv;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RawProductStore_ProductController : ControllerBase
    {
        private readonly IRawProductStore_ProductService _RawProductStore_ProductService;
        private readonly IRegisterCostRawProductStoreService _RegisterCostRawProductStoreService;
        private readonly IMapper _mapper;
        public RawProductStore_ProductController(
            IRawProductStore_ProductService RawProductStore_ProductService,
            IMapper mapper,
            IRegisterCostRawProductStoreService RegisterCostRawProductStoreService,
        UserManager<Account> userManager)
        {
            _RawProductStore_ProductService = RawProductStore_ProductService;
            _mapper = mapper;
            _RegisterCostRawProductStoreService= RegisterCostRawProductStoreService;
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("RawProductStore_Products")]
        public async Task<IActionResult> Add([FromBody] AddRawProductStore_Product model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddRawProductStore_Product>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var RawProductStore_Product = _mapper.Map<AddRawProductStore_Product, RawProductStore_Product>(model);
            int id = await _RawProductStore_ProductService.AddAsync(RawProductStore_Product);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddRawProductStore_Product>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddRawProductStore_Product>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("RawProductStore_Products")]
        public async Task<IActionResult> Update([FromBody] UpdateRawProductStore_Product model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateRawProductStore_Product>
                                                           (entity: new UpdateRawProductStore_Product(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var RawProductStore_Product = _mapper.Map<UpdateRawProductStore_Product, RawProductStore_Product>(model);
            var upd = await _RawProductStore_ProductService.UpdateAsync(RawProductStore_Product);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateRawProductStore_Product>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateRawProductStore_Product>
                                                           (entity: new UpdateRawProductStore_Product(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("RawProductStore_Products/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var count= await _RegisterCostRawProductStoreService.GetCountAllAsync(s => s.RawProductStore_ProductID == id);
            if(count > 0)
                return BadRequest(new ResponseApiEntity<ResultRawProductStore_Product>
                                                           (entity: new ResultRawProductStore_Product(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.NotAllowDeleteError));


            var del = await _RawProductStore_ProductService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultRawProductStore_Product>
                                                               (entity: new ResultRawProductStore_Product(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultRawProductStore_Product>
                                                           (entity: new ResultRawProductStore_Product(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("RawProductStore_Products")]
        public async Task<IActionResult> GetRawProductStore_Products([FromQuery] PaginationParams @params,int? RawProductStoreID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultRawProductStore_Product>
                                                           (entity: new ResultRawProductStore_Product(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var count = await _RawProductStore_ProductService.GetCountAllAsync(s =>s.RawProductStoreID == RawProductStoreID  );
            var RawProductStore_Products = await _RawProductStore_ProductService
                                .GetAllAsync(s => s.RawProductStoreID == RawProductStoreID 
                                , page: @params.Page, take: @params.Take);

            var mappedRawProductStore_Products = _mapper.Map<ICollection<ResultRawProductStore_Product>>(RawProductStore_Products);
     
            return Ok(new ResponseApiEntities<ResultRawProductStore_Product>
                                                            (entities: mappedRawProductStore_Products,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }

        [HttpGet("RawProductStore_Products/{id}")]
        public async Task<IActionResult> GetRawProductById([FromRoute] int id)
        {

            var data = await _RawProductStore_ProductService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateRawProductStore_Product>
                                                           (entity: new UpdateRawProductStore_Product(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateRawProductStore_Product>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateRawProductStore_Product>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateRawProductStore_Product>
                                                           (entity: new UpdateRawProductStore_Product(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("RawProductStore_Products/ByRawProductStoreID")]
        public async Task<IActionResult> GetRawProductStore_Products([FromQuery] int? RawProductStoreID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultRawProductStore_Product>
                                                           (entities: new List<ResultRawProductStore_Product>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var RawProductStore_Products = await _RawProductStore_ProductService
                                .GetAllAsync(s => s.RawProductStoreID == RawProductStoreID);

            var mappedRawProductStore_Products = _mapper.Map<ICollection<ResultRawProductStore_Product>>(RawProductStore_Products);

            return Ok(new ResponseApiEntities<ResultRawProductStore_Product>
                                                            (entities: mappedRawProductStore_Products,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: mappedRawProductStore_Products.Count()));

        }
    }
}
