using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoPosition;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.PositionSrv;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]

    public class PositionController : ControllerBase
    {
        private readonly IPositionService _PositionService;
        private readonly IMapper _mapper;
        public PositionController(
            IPositionService PositionService,
            IMapper mapper,
        UserManager<Account> userManager)
        {
            _PositionService = PositionService;
            _mapper = mapper;   
        }
        [HttpPost("Positions")]
        public async Task<IActionResult> Add([FromBody] AddPosition model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddPosition>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var Position = _mapper.Map<AddPosition, Position>(model);
            int id = await _PositionService.AddAsync(Position);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddPosition>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddPosition>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [HttpPatch("Positions")]
        public async Task<IActionResult> Update([FromBody] UpdatePosition model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdatePosition>
                                                           (entity: new UpdatePosition(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var Position = _mapper.Map<UpdatePosition, Position>(model);
            var upd = await _PositionService.UpdateAsync(Position);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdatePosition>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdatePosition>
                                                           (entity: new UpdatePosition(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        
        [HttpDelete("Positions/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _PositionService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultPosition>
                                                               (entity: new ResultPosition(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultPosition>
                                                           (entity: new ResultPosition(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }
        [HttpGet("Positions")]
        public async Task<IActionResult> GetPositions([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultPosition>
                                                           (entity: new ResultPosition(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var count = await _PositionService.GetCountAllAsync(s => s.Title.Contains(@params.SearchText) );
            var Positions = await _PositionService
                                .GetAllAsync(s =>s.Title.Contains(@params.SearchText) 
                                , page: @params.Page, take: @params.Take);

            var mappedPositions = _mapper.Map<ICollection<ResultPosition>>(Positions);
     
            return Ok(new ResponseApiEntities<ResultPosition>
                                                            (entities: mappedPositions,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [HttpGet("Positions/{id}")]
        public async Task<IActionResult> GetPositionById([FromRoute] int id)
        {

            var data = await _PositionService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdatePosition>
                                                           (entity: new UpdatePosition(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdatePosition>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdatePosition>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdatePosition>
                                                           (entity: new UpdatePosition(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

        [HttpGet("Positions/All")]
        public async Task<IActionResult> GetPositionsAll()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultPosition>
                                                           (entities: new List<ResultPosition>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var Positions = await _PositionService.GetAllAsync();

            var mappedPositions = _mapper.Map<ICollection<ResultPosition>>(Positions);

            return Ok(new ResponseApiEntities<ResultPosition>
                                                            (entities: mappedPositions,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Positions.Count()));

        }


    }
}
