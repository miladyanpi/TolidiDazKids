using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoTrait;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.TraitSrv;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TraitController(
        ITraitService _TraitService,
        IMapper _mapper
        )
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Traits")]
        public async Task<IActionResult> Add([FromBody] AddTrait model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddTrait>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var Trait = _mapper.Map<AddTrait, Trait>(model);
            int id = await _TraitService.AddAsync(Trait);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddTrait>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddTrait>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Traits")]
        public async Task<IActionResult> Update([FromBody] UpdateTrait model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateTrait>
                                                           (entity: new UpdateTrait(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var Trait = _mapper.Map<UpdateTrait, Trait>(model);
            var upd = await _TraitService.UpdateAsync(Trait);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateTrait>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateTrait>
                                                           (entity: new UpdateTrait(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("Traits/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _TraitService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultTrait>
                                                               (entity: new ResultTrait(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultTrait>
                                                           (entity: new ResultTrait(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Traits/Data")]
        public async Task<IActionResult> GetTraits([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultTrait>
                                                                    (entities: new List<ResultTrait>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<Trait, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Title ?? "").Contains(search);
                }

                var count = await _TraitService.GetCountAllAsync(predicate);

                var Traits = await _TraitService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedTraits = _mapper.Map<ICollection<ResultTrait>>(Traits);

                return Ok(new ResponseApiEntities<ResultTrait>
                                                                (entities: mappedTraits,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultTrait>
                                                                    (entities: new List<ResultTrait>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Traits")]
        public async Task<IActionResult> GetTraits2([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultTrait>
                                                           (entity: new ResultTrait(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var count = await _TraitService.GetCountAllAsync(s=>s.Title.Contains(@params.SearchText) );
            var Traits = await _TraitService
                                .GetAllAsync(s =>s.Title.Contains(@params.SearchText) 
                                , page: @params.Page, take: @params.Take);

            var mappedTraits = _mapper.Map<ICollection<ResultTrait>>(Traits);
     
            return Ok(new ResponseApiEntities<ResultTrait>
                                                            (entities: mappedTraits,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Traits/All")]
        public async Task<IActionResult> GetTraits3()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultTrait>
                                                           (entity: new ResultTrait(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var Traits = await _TraitService
                                .GetAllAsync();

            var mappedTraits = _mapper.Map<ICollection<ResultTrait>>(Traits);

            return Ok(new ResponseApiEntities<ResultTrait>
                                                            (entities: mappedTraits,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Traits.Count()));

        }
        [HttpGet("Traits/{id}")]
        public async Task<IActionResult> GetTraitById([FromRoute] int id)
        {

            var data = await _TraitService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateTrait>
                                                           (entity: new UpdateTrait(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateTrait>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateTrait>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateTrait>
                                                           (entity: new UpdateTrait(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName )]
        [HttpGet("Traits/Public/All")]
        public async Task<IActionResult> GetTraitsAll()
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultTrait>
                                                           (entities: new List<ResultTrait>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var Traits = await _TraitService
                                .GetAllAsync();

            var mappedTraits = _mapper.Map<ICollection<ResultTrait>>(Traits);

            return Ok(new ResponseApiEntities<ResultTrait>
                                                            (entities: mappedTraits,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Traits.Count()));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Traits/ByGuid/{guid}")]
        public async Task<IActionResult> GetTraitById([FromRoute] string guid)
        {
            try
            {
                var Trait = await _TraitService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (Trait == null)
                    return BadRequest(new ResponseApiEntity<UpdateTrait>
                                                                              (entity: new UpdateTrait(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateTrait>(Trait);
                return Ok(new ResponseApiEntity<UpdateTrait>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateTrait>
                                                                             (entity: new UpdateTrait(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }
    }
}
