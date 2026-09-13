using ServicesLibrary.Services.OrderPaymentTempSrv;
using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoOrderPaymentTemp;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
    public class OrderPaymentTempController : ControllerBase
    {
        private readonly IOrderPaymentTempService _OrderPaymentTempService;
        private readonly IMapper _mapper;
        public OrderPaymentTempController(
            IOrderPaymentTempService storyService,
            IMapper mapper,
        UserManager<Account> userManager)
        {
            _OrderPaymentTempService = storyService;
            _mapper = mapper;   
        }
   
        [HttpPost("OrderPaymentTemps")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrderPaymentTemps([FromBody] SearchOrderPaymentTemp @params)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<SearchOrderPaymentTemp>
                                                            (entity: @params,
                                                            status: ResultMessageApi.Error,
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            message: ResultMessageApi.GetError));

            var OrderPaymentTemps = await _OrderPaymentTempService
                                .FirstOrDefaultAsync(s => s.Amount == @params.Amount && s.ResNum == @params.ResNum);

            if (OrderPaymentTemps == null)
                return NotFound(new ResponseApiEntity<SearchOrderPaymentTemp>
                                                            (entity: @params,
                                                            status: ResultMessageApi.Error,
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            message: ResultMessageApi.GetError));
            var mappedOrderPaymentTemps = _mapper.Map<SearchOrderPaymentTemp>(OrderPaymentTemps);

            return Ok(new ResponseApiEntity<SearchOrderPaymentTemp>
                                                            (entity: mappedOrderPaymentTemps,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk));

        }



    }
}
