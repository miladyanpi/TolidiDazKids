using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoRawProduct;
using Dto.Models.DtoRawProductStore;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.RawProductStoreSrv;
using Utility;
using static Dto.Enum.EnumConstant;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RawProductStoreController : ControllerBase
    {
        private readonly IRawProductStoreService _RawProductStoreService;
        private readonly IMapper _mapper;
        public RawProductStoreController(
            IRawProductStoreService RawProductStoreService,
            IMapper mapper,
        UserManager<Account> userManager)
        {
            _RawProductStoreService = RawProductStoreService;
            _mapper = mapper;
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("RawProductStores")]
        public async Task<IActionResult> Add([FromBody] AddRawProductStore model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddRawProductStore>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var RawProductStore = _mapper.Map<AddRawProductStore, RawProductStore>(model);
            int id = await _RawProductStoreService.AddAsync(RawProductStore);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddRawProductStore>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddRawProductStore>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("RawProductStores")]
        public async Task<IActionResult> Update([FromBody] UpdateRawProductStore model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateRawProductStore>
                                                           (entity: new UpdateRawProductStore(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));


            var RawProductStore = _mapper.Map<UpdateRawProductStore, RawProductStore>(model);
            var upd = await _RawProductStoreService.UpdateAsync(RawProductStore);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateRawProductStore>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }

            else
                return BadRequest(new ResponseApiEntity<UpdateRawProductStore>
                                                           (entity: new UpdateRawProductStore(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("RawProductStores/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _RawProductStoreService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultRawProductStore>
                                                               (entity: new ResultRawProductStore(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultRawProductStore>
                                                           (entity: new ResultRawProductStore(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("RawProductStores")]
        public async Task<IActionResult> GetRawProductStores([FromQuery] PaginationParams @params, int? RawProductID=null)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultRawProductStore>
                                                           (entity: new ResultRawProductStore(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            IEnumerable<ResultRawProductStore> RawProductStores = new List<ResultRawProductStore>();
            var count = RawProductID != null ?
                await _RawProductStoreService.GetCountAllAsync(s => s.RawProductID == RawProductID)
                :
                await _RawProductStoreService.GetCountAllAsync();

            //var RawProductStores = RawProductID != null ?
            //    await _RawProductStoreService
            //                    .GetAllAsync(s => s.RawProductID == RawProductID
            //                    , page: @params.Page, take: @params.Take)
            //                    :
            //                    await _RawProductStoreService
            //                    .GetAllAsync(page: @params.Page,                            
            //                    take: @params.Take);
            if (RawProductID != null)
            {
                RawProductStores = await _RawProductStoreService.GetAllAsync(
               predicate: s => s.RawProductID == RawProductID,

                   selector: r => new ResultRawProductStore
                   {
                       ID = r.ID,
                       RawProductID = r.RawProductID,
                       MessurmentType = GetTitleMessurmentType((MessurmentType)r.MessurmentType),
                       MessurmentType2 = (MessurmentType)r.MessurmentType,
                       Amount = r.Amount,
                       Price = r.Price,
                       SumPrice = r.Amount * r.Price,

                       // محاسبه مجموع هزینه‌های ثبت شده (Cast Raw)
                       SumPriceCastRaw = r.RawProductStore_Products
                           .SelectMany(p => p.RegisterCostRawProductStores)
                           .Sum(rc => rc.Price * (long)rc.Count),
                       SliceCount = r.RawProductStore_Products.Sum(rc => (int)rc.Count),
                       Visible = r.Visible,
                       BuyDate = DateFunctions.ConvertDateIntToString(r.BuyDate),

                       ResultRawProduct = r.RawProduct != null ? new ResultRawProduct
                       {
                           ID = r.RawProduct.ID,
                           Title = r.RawProduct.Title,
                           Description = r.RawProduct.Description,
                           Visible = r.RawProduct.Visible,
                       } : null,

                       IdentityCode = r.IdentityCode,
                       RegisterDate = DateFunctions.ConvertDateIntToString(r.RegisterDate),
                       RegisterTime = r.RegisterTime,
                       EditDate = DateFunctions.ConvertDateIntToString(r.EditDate),
                       EditTime = r.EditTime,
                   },

                   page: @params.Page,
                   take: @params.Take
               );
            }
            else
            {
                 RawProductStores = await _RawProductStoreService.GetAllAsync(

                selector: r => new ResultRawProductStore
                {
                    ID = r.ID,
                    RawProductID = r.RawProductID,
                    MessurmentType = Dto.Enum.EnumConstant.GetTitleMessurmentType((MessurmentType)r.MessurmentType),
                    MessurmentType2 = (MessurmentType)r.MessurmentType,
                    Amount = r.Amount,
                    Price = r.Price,
                    SumPrice = r.Amount * r.Price,

                    // محاسبه مجموع هزینه‌های ثبت شده (Cast Raw)
                    SumPriceCastRaw = r.RawProductStore_Products
                        .SelectMany(p => p.RegisterCostRawProductStores)
                        .Sum(rc => rc.Price * (long)rc.Count),
                    SliceCount=r.RawProductStore_Products.Sum(rc=>(int)rc.Count),
                    Visible = r.Visible,
                    BuyDate = DateFunctions.ConvertDateIntToString(r.BuyDate),

                    ResultRawProduct = r.RawProduct != null ? new ResultRawProduct
                    {
                        ID = r.RawProduct.ID,
                        Title = r.RawProduct.Title,
                        Description = r.RawProduct.Description,
                        Visible = r.RawProduct.Visible,
                    } : null,

                    IdentityCode = r.IdentityCode,
                    RegisterDate = DateFunctions.ConvertDateIntToString(r.RegisterDate),
                    RegisterTime = r.RegisterTime,
                    EditDate = DateFunctions.ConvertDateIntToString(r.EditDate),
                    EditTime = r.EditTime,
                },

                page: @params.Page,
                take: @params.Take
            );
            }
            //var mappedRawProductStores = _mapper.Map<ICollection<ResultRawProductStore>>(RawProductStores);
            var list = RawProductStores.ToList();
            int i = -1;
            foreach (var item in list)
            {
                i++;
                list[i].FinishPriceForAnyProduct = item.SliceCount>0? (item.SumPrice + item.SumPriceCastRaw) / item.SliceCount:0;
            }
            return Ok(new ResponseApiEntities<ResultRawProductStore>
                                                            (entities: RawProductStores,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));
        }
        [HttpGet("RawProductStores/{id}")]
        public async Task<IActionResult> GetRawProductStoreById([FromRoute] int id)
        {

            var data = await _RawProductStoreService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateRawProductStore>
                                                           (entity: new UpdateRawProductStore(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateRawProductStore>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateRawProductStore>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateRawProductStore>
                                                           (entity: new UpdateRawProductStore(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

        //[HttpGet("RawProductStores/Public/All")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetRawProductStoresAll()
        //{
        //    if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultRawProductStore>
        //                                                   (entity: new ResultRawProductStore(),
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.GetError));

        //    var RawProductStores = await _RawProductStoreService
        //                        .GetAllAsync(page: 1, take: 15);

        //    var mappedRawProductStores = _mapper.Map<ICollection<ResultRawProductStore>>(RawProductStores);

        //    return Ok(new ResponseApiEntities<ResultRawProductStore>
        //                                                    (entities: mappedRawProductStores,
        //                                                    status: ResultMessageApi.Success,
        //                                                    statusCode: ResultMessageApi.SuccessCode,
        //                                                    message: ResultMessageApi.GetOk,
        //                                                    countAllRecordTable: RawProductStores.Count()));

        //}


    }
}
