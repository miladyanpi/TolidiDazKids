using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoPricingRule;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.PricingRuleSrv;
using System.Linq.Expressions;
using Utility;
using static Dto.Enum.EnumConstant;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PricingRuleController(
        IPricingRuleService _PricingRuleService,
        IMapper _mapper 
        ) 
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("PricingRules")]
        public async Task<IActionResult> Add([FromBody] AddPricingRule model)
        {

            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddPricingRule>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            //var ResPricingRule = await _PricingRuleService.GetAllAsync(s=> model.MinQuantity<=s.MinQuantity && s.RuleType==model.RuleType && s.RoleID==model.RoleID);
            //if (ResPricingRule.Count()>0)  return BadRequest(new ResponseApiEntity<AddPricingRule>
            //                                               (entity: null,
            //                                               statusCode: ResultMessageApi.ErrorCode,
            //                                               status: ResultMessageApi.Error,
            //                                               message: "عدد بزرگتری وارد نمایید. این عدد برای این نوع قبلا ثبت شده است"));

            if (model.RuleType== (int)RuleType.Date)
            {
                var q = await _PricingRuleService.FirstOrDefaultAsync(s =>  s.RuleType ==(int)RuleType.Date && DateFunctions.ConvertDateStringToInt(model.FromDate)<=s.ToDate);
                //if (ResPricingRule.Count() > 0)
                //{
                //    //if (model.MinQuantity <= q.MinQuantity)
                //    //    return BadRequest(new ResponseApiEntity<AddPricingRule>
                //    //                                         (entity: null,
                //    //                                         statusCode: ResultMessageApi.ErrorCode,
                //    //                                         status: ResultMessageApi.Error,
                //    //                                         message: "عدد بزرگتری وارد نمایید. این عدد برای این نوع قبلا ثبت شده است"));

                  
                //}
                if (DateFunctions.ConvertDateStringToInt(model.FromDate) <= q.ToDate)
                    return BadRequest(new ResponseApiEntity<AddPricingRule>
                                                         (entity: null,
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: "این محدوده تاریخ قبلا قبت شده است"));

            }

            var PricingRule = _mapper.Map<AddPricingRule, PricingRule>(model);
            int id = await _PricingRuleService.AddAsync(PricingRule);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddPricingRule>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddPricingRule>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("PricingRules")]
        public async Task<IActionResult> Update([FromBody] UpdatePricingRule model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdatePricingRule>
                                                           (entity: new UpdatePricingRule(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            
       
            var PricingRule = _mapper.Map<UpdatePricingRule, PricingRule>(model);
            var upd = await _PricingRuleService.UpdateAsync(PricingRule);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdatePricingRule>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdatePricingRule>
                                                           (entity: new UpdatePricingRule(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
     
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("PricingRules/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _PricingRuleService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultPricingRule>
                                                               (entity: new ResultPricingRule(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultPricingRule>
                                                           (entity: new ResultPricingRule(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("PricingRules/Data")]
        public async Task<IActionResult> GetPricingRules([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultPricingRule>
                                                                    (entities: new List<ResultPricingRule>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<PricingRule, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Title ?? "").Contains(search);
                }

                var count = await _PricingRuleService.GetCountAllAsync(predicate);

                var PricingRules = await _PricingRuleService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedPricingRules = _mapper.Map<ICollection<ResultPricingRule>>(PricingRules);

                return Ok(new ResponseApiEntities<ResultPricingRule>
                                                                (entities: mappedPricingRules,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultPricingRule>
                                                                    (entities: new List<ResultPricingRule>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("PricingRules")]
        public async Task<IActionResult> GetPricingRules([FromQuery] PaginationParams @params,int ProductID)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _PricingRuleService.GetCountAllAsync(s =>s.ProductID== ProductID);
            var PricingRules = await _PricingRuleService
                                .GetAllAsync(s => s.ProductID == ProductID
                                , page: @params.Page, take: @params.Take);

            var mappedPricingRules = _mapper.Map<ICollection<ResultPricingRule>>(PricingRules);
     
            return Ok(new ResponseApiEntities<ResultPricingRule>
                                                            (entities: mappedPricingRules,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
       
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("PricingRules/{id}")]
        public async Task<IActionResult> GetPricingRuleById([FromRoute] int id)
        {

            var PricingRule = await _PricingRuleService.GetByIdAsync(id);
            if(PricingRule==null)
                return BadRequest(new ResponseApiEntity<UpdatePricingRule>
                                                           (entity: new UpdatePricingRule(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdatePricingRule>(PricingRule);
      
            if (result != null)
                return Ok(new ResponseApiEntity<UpdatePricingRule>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdatePricingRule>
                                                           (entity: new UpdatePricingRule(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("PricingRules/ByGuid/{guid}")]
        public async Task<IActionResult> GetPricingRuleById([FromRoute] string guid)
        {
            try
            {
                var PricingRule = await _PricingRuleService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (PricingRule == null)
                    return BadRequest(new ResponseApiEntity<UpdatePricingRule>
                                                                              (entity: new UpdatePricingRule(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdatePricingRule>(PricingRule);
                return Ok(new ResponseApiEntity<UpdatePricingRule>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdatePricingRule>
                                                                             (entity: new UpdatePricingRule(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }

    }
}
