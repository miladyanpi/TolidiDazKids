using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoDepartment;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.DepartmentSrv;
using System.Linq.Expressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DepartmentController(
        IDepartmentService _DepartmentService,
        IMapper _mapper
        )
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Departments")]
        public async Task<IActionResult> Add([FromBody] AddDepartment model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddDepartment>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));

            var data = _mapper.Map<AddDepartment, Department>(model);
            int id = await _DepartmentService.AddAsync(data);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddDepartment>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddDepartment>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Departments")]
        public async Task<IActionResult> Update([FromBody] UpdateDepartment model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateDepartment>
                                                           (entity: new UpdateDepartment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));


            var data = _mapper.Map<UpdateDepartment, Department>(model);
            var upd = await _DepartmentService.UpdateAsync(data);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateDepartment>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }

            else
                return BadRequest(new ResponseApiEntity<UpdateDepartment>
                                                           (entity: new UpdateDepartment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("Departments/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _DepartmentService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultDepartment>
                                                               (entity: new ResultDepartment(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultDepartment>
                                                           (entity: new ResultDepartment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [HttpPost("Departments/Data")]
        public async Task<IActionResult> GetDepartments([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultDepartment>
                                                                    (entities: new List<ResultDepartment>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<Department, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Title ?? "").Contains(search);
                }

                var count = await _DepartmentService.GetCountAllAsync(predicate);

                var Departments = await _DepartmentService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedDepartments = _mapper.Map<ICollection<ResultDepartment>>(Departments);

                return Ok(new ResponseApiEntities<ResultDepartment>
                                                                (entities: mappedDepartments,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultDepartment>
                                                                    (entities: new List<ResultDepartment>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Departments2")]
        public async Task<IActionResult> GetDepartments2([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = await _DepartmentService.GetCountAllAsync(s =>
                                s.Title.Contains(@params.SearchText));
            var Departments = await _DepartmentService
                                .GetAllAsync(s =>
                                s.Title.Contains(@params.SearchText)
                                , page: @params.Page, take: @params.Take);

            var mappedDepartments = _mapper.Map<ICollection<ResultDepartment>>(Departments);

            return Ok(new ResponseApiEntities<ResultDepartment>
                                                            (entities: mappedDepartments,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Departments/{id}")]
        public async Task<IActionResult> GetDepartmentById([FromRoute] int id)
        {

            var data = await _DepartmentService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateDepartment>
                                                           (entity: new UpdateDepartment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateDepartment>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateDepartment>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateDepartment>
                                                           (entity: new UpdateDepartment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("Departments/Public/IdentityCode/{Code}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDepartmentByIdentityCode([FromRoute] string? Code)
        {

            var data = await _DepartmentService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == Code);
            if (data == null)
                return BadRequest(new ResponseApiEntity<ResultDepartment>
                                                           (entity: new ResultDepartment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<ResultDepartment>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<ResultDepartment>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<ResultDepartment>
                                                           (entity: new ResultDepartment(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [HttpGet("Departments/All")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDepartmentsAll()
        {
            if (!ModelState.IsValid) return BadRequest();

            var Departments = await _DepartmentService
                                .GetAllAsync(s => s.Visible == true);
            var mappedDepartments = _mapper.Map<ICollection<ResultDepartment>>(Departments);

            return Ok(new ResponseApiEntities<ResultDepartment>
                                                            (entities: mappedDepartments,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: Departments.Count()));

        }
        [HttpGet("Departments/ByGuid/{guid}")]
        public async Task<IActionResult> GetDepartmentById([FromRoute] string guid)
        {
            try
            {
                var Department = await _DepartmentService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (Department == null)
                    return BadRequest(new ResponseApiEntity<UpdateDepartment>
                                                                              (entity: new UpdateDepartment(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateDepartment>(Department);
                return Ok(new ResponseApiEntity<UpdateDepartment>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateDepartment>
                                                                             (entity: new UpdateDepartment(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }

    }
}
