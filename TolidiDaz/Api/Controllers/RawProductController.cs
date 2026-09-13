using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoRawProduct;
using Dto.Models.DtoRawProduct;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.RawProductSrv;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]

    public class RawProductController : ControllerBase
    {
        private readonly IRawProductService _RawProductService;
        private readonly IMapper _mapper;
        public RawProductController(
            IRawProductService RawProductService,
            IMapper mapper,
        UserManager<Account> userManager)
        {
            _RawProductService = RawProductService;
            _mapper = mapper;   
        }
        [HttpPost("RawProducts")]
        public async Task<IActionResult> Add([FromBody] AddRawProduct model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddRawProduct>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var RawProduct = _mapper.Map<AddRawProduct, RawProduct>(model);
            int id = await _RawProductService.AddAsync(RawProduct);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddRawProduct>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddRawProduct>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [HttpPatch("RawProducts")]
        public async Task<IActionResult> Update([FromBody] UpdateRawProduct model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateRawProduct>
                                                           (entity: new UpdateRawProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var RawProduct = _mapper.Map<UpdateRawProduct, RawProduct>(model);
            var upd = await _RawProductService.UpdateAsync(RawProduct);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateRawProduct>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateRawProduct>
                                                           (entity: new UpdateRawProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        
        [HttpDelete("RawProducts/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _RawProductService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultRawProduct>
                                                               (entity: new ResultRawProduct(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultRawProduct>
                                                           (entity: new ResultRawProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [HttpGet("RawProducts")]
        public async Task<IActionResult> GetRawProducts([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultRawProduct>
                                                           (entity: new ResultRawProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var count = await _RawProductService.GetCountAllAsync(s => s.Title.Contains(@params.SearchText) );
            var RawProducts = await _RawProductService
                                .GetAllAsync(s =>s.Title.Contains(@params.SearchText) 
                                , page: @params.Page, take: @params.Take);

            var mappedRawProducts = _mapper.Map<ICollection<ResultRawProduct>>(RawProducts);
     
            return Ok(new ResponseApiEntities<ResultRawProduct>
                                                            (entities: mappedRawProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [HttpGet("RawProducts/{id}")]
        public async Task<IActionResult> GetRawProductById([FromRoute] int id)
        {

            var data = await _RawProductService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateRawProduct>
                                                           (entity: new UpdateRawProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateRawProduct>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateRawProduct>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateRawProduct>
                                                           (entity: new UpdateRawProduct(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

        [HttpGet("RawProducts/All")]
        public async Task<IActionResult> GetRawProductsAll()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultRawProduct>
                                                           (entities: new List<ResultRawProduct>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var RawProducts = await _RawProductService.GetAllAsync();

            var mappedRawProducts = _mapper.Map<ICollection<ResultRawProduct>>(RawProducts);

            return Ok(new ResponseApiEntities<ResultRawProduct>
                                                            (entities: mappedRawProducts,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: RawProducts.Count()));

        }


    }
}
