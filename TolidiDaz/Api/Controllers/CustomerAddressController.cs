using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoCustomerAddress;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.CustomerAddressSrv;
using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.OrderSrv;
using System.Linq.Expressions;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomerAddressController(
        ICustomerAddressService _CustomerAddressService,
        IMapper _mapper,
        ICustomerService _customerService,
        IOrderService _orderService
        )
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("CustomerAddresss")]
        public async Task<IActionResult> Add([FromBody] AddCustomerAddress model)
        {
            if (!ModelState.IsValid)  return BadRequest(new ResponseApiEntity<AddCustomerAddress>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
            if(model.Default==true)
            {
                var customerAddresses = await _CustomerAddressService.GetAllAsync(s => s.CustomerID == model.CustomerID);
                var x = customerAddresses.ToList();
                for (int i=0;i< x.Count();i++)
                {
                    x[i].Default = false;
                }
                _ = await _CustomerAddressService.UpdateRangeAsync(x);

            }

            var CustomerAddress = _mapper.Map<AddCustomerAddress, CustomerAddress>(model);
            int id = await _CustomerAddressService.AddAsync(CustomerAddress);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddCustomerAddress>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddCustomerAddress>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("CustomerAddresss")]
        public async Task<IActionResult> Update([FromBody] UpdateCustomerAddress model)
        {
            if (!ModelState.IsValid)    return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                           (entity: new UpdateCustomerAddress(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));

            if (model.Default == true)
            {
                var customerAddresses = await _CustomerAddressService.GetAllAsync(s => s.CustomerID == model.CustomerID);
                var x = customerAddresses.ToList();
                for (int i = 0; i < x.Count(); i++)
                {
                    x[i].Default = false;
                }
                _ = await _CustomerAddressService.UpdateRangeAsync(x);

            }
            var CustomerAddress = _mapper.Map<UpdateCustomerAddress, CustomerAddress>(model);
            var upd = await _CustomerAddressService.UpdateAsync(CustomerAddress);
            if (upd > 0)
            {
                return Ok(new ResponseApiEntity<UpdateCustomerAddress>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }
              
            else
                return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                           (entity: new UpdateCustomerAddress(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("CustomerAddresss/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var del = await _CustomerAddressService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultCustomerAddress>
                                                               (entity: new ResultCustomerAddress(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultCustomerAddress>
                                                           (entity: new ResultCustomerAddress(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("CustomerAddresss/Data")]
        public async Task<IActionResult> GetCustomerAddresss([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultCustomerAddress>
                                                                    (entities: new List<ResultCustomerAddress>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<CustomerAddress, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.CityID.ToString() ?? "").Contains(search);
                }

                var count = await _CustomerAddressService.GetCountAllAsync(predicate);

                var CustomerAddresss = await _CustomerAddressService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedCustomerAddresss = _mapper.Map<ICollection<ResultCustomerAddress>>(CustomerAddresss);

                return Ok(new ResponseApiEntities<ResultCustomerAddress>
                                                                (entities: mappedCustomerAddresss,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultCustomerAddress>
                                                                    (entities: new List<ResultCustomerAddress>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("CustomerAddresss")]
        public async Task<IActionResult> GetCustomerAddresss([FromQuery] PaginationParams @params,int? CustomerID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultCustomerAddress>
                                                           (entity: new ResultCustomerAddress(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var count = await _CustomerAddressService.GetCountAllAsync(s=>s.CustomerID== CustomerID);
            var CustomerAddresss = await _CustomerAddressService
                                .GetAllAsync(s => s.CustomerID == CustomerID, page: @params.Page, take: @params.Take);

            var mappedCustomerAddresss = _mapper.Map<ICollection<ResultCustomerAddress>>(CustomerAddresss);
     
            return Ok(new ResponseApiEntities<ResultCustomerAddress>
                                                            (entities: mappedCustomerAddresss,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("CustomerAddresss/{id}")]
        public async Task<IActionResult> GetCustomerAddressById([FromRoute] int id)
        {
            var data = await _CustomerAddressService.GetByIdAsync(id);
            if (data == null)
                return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                           (entity: new UpdateCustomerAddress(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateCustomerAddress>(data);

            if (result != null)
                return Ok(new ResponseApiEntity<UpdateCustomerAddress>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                           (entity: new UpdateCustomerAddress(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpGet("CustomerAddresss/Public/{identityCode}")]
        public async Task<IActionResult> GetAddressInfo([FromRoute] string? identityCode)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                         (entity: new UpdateCustomerAddress(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.ErrorNotAllowRequest));
            if (!Guid.TryParse(identityCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                                 (entity: new UpdateCustomerAddress(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var data = await _CustomerAddressService.FirstOrDefaultAsync(s => s.IdentityCode == guid);
            var result = _mapper.Map<UpdateCustomerAddress>(data);

            if (data == null)
            {
                return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>(
                    entity: new UpdateCustomerAddress(),
                    statusCode: ResultMessageApi.ErrorCode,
                    status: ResultMessageApi.Error,
                    message: ResultMessageApi.ErrorNotAllowRequest));
            }

           
                return Ok(new ResponseApiEntity<UpdateCustomerAddress>(
                    entity: result,
                    statusCode: ResultMessageApi.SuccessCode,
                    status: ResultMessageApi.Success,
                    message: ResultMessageApi.UpdateOk));
           


        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpGet("CustomerAddresss/Public")]
        public async Task<IActionResult> GetCustomerAddressByCode()
        {
            var CustomerID = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;

            var UniqCode = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guidCustimer))
                return BadRequest(new ResponseApiEntities<UpdateCustomerAddress>
                                                                 (entities: new List<UpdateCustomerAddress>(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var customer = await _customerService.FirstOrDefaultAsync(s => s.IdentityCode == guidCustimer);
            if (customer == null)
                return BadRequest(new ResponseApiEntities<UpdateCustomerAddress>
                                                           (entities: new List<UpdateCustomerAddress>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var data = await _CustomerAddressService.GetAllAsync(s=>s.CustomerID== customer.ID);
            if (data == null)
                return BadRequest(new ResponseApiEntities<UpdateCustomerAddress>
                                                           (entities: new List<UpdateCustomerAddress>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var result = _mapper.Map<ICollection<UpdateCustomerAddress>>(data);

            if (result != null)
                return Ok(new ResponseApiEntities<UpdateCustomerAddress>
                                                               (entities: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntities<UpdateCustomerAddress>
                                                          (entities: new List<UpdateCustomerAddress>(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpPatch("CustomerAddresss/Update/Public")]
        public async Task<IActionResult> UpdateInfo([FromBody] UpdateCustomerAddress model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                         (entity: new UpdateCustomerAddress(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.ErrorNotAllowRequest));
        


            var CustomerID = User.Claims
                  .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                                 (entity: new UpdateCustomerAddress(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customerinfo = await _customerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) &&s.ID== model.CustomerID && s.IdentityCode == guid);
            if (customerinfo == null)
                return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                                 (entity: new UpdateCustomerAddress(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

           
            var records=await _CustomerAddressService.GetAllAsync(s=>s.CustomerID==int.Parse(CustomerID));
            records.ToList().ForEach(x => x.Default = false);
            await _CustomerAddressService.UpdateRangeAsync(records.ToList());

            var customerAddress = _mapper.Map<UpdateCustomerAddress, CustomerAddress>(model);

            customerAddress.Default = true;
            var upd = await _CustomerAddressService.UpdateAsync(customerAddress);
            if (upd > 0)
            {
               
                return Ok(new ResponseApiEntity<UpdateCustomerAddress>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            }
            else
                return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                           (entity: new UpdateCustomerAddress(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.ErrorNotAllowRequest));
        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpPatch("CustomerAddresss/Update/Public/SetDefault/{ID}")]
        public async Task<IActionResult> UpdateInfo([FromRoute] int ID)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                         (entity: new UpdateCustomerAddress(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.ErrorNotAllowRequest));



            var CustomerID = User.Claims
                  .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                                 (entity: new UpdateCustomerAddress(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customerinfo = await _customerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) &&  s.IdentityCode == guid);
            if (customerinfo == null)
                return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                                 (entity: new UpdateCustomerAddress(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var records = await _CustomerAddressService.GetAllAsync(s => s.CustomerID == int.Parse(CustomerID));
            var itesm = records.ToList();
            for (int i=0;i< records.Count();i++)
            {
                if (itesm[i].ID == ID)
                    itesm[i].Default = true;
                else
                    itesm[i].Default = false;
            }
            var model = await _CustomerAddressService.UpdateRangeAsync(itesm);

            if (model > 0)
            {

                return Ok(new ResponseApiEntity<UpdateCustomerAddress>
                                                               (entity: new UpdateCustomerAddress(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            }
            else
                return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                           (entity: new UpdateCustomerAddress(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.ErrorNotAllowRequest));
        }

        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpPost("CustomerAddresss/Add/Public")]
        public async Task<IActionResult> AddInfo([FromBody] AddCustomerAddress model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<AddCustomerAddress>
                                                         (entity: new AddCustomerAddress(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.ErrorNotAllowRequest));



            var CustomerID = User.Claims
                  .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<AddCustomerAddress>
                                                                 (entity: new AddCustomerAddress(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customerinfo = await _customerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) &&  s.IdentityCode == guid);
            if (customerinfo == null)
                return BadRequest(new ResponseApiEntity<AddCustomerAddress>
                                                                 (entity: new AddCustomerAddress(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));



            var customerAddress = _mapper.Map<AddCustomerAddress, CustomerAddress>(model);

            customerAddress.Default = true;
            customerAddress.CustomerID = int.Parse(CustomerID);

            var records = await _CustomerAddressService.GetAllAsync(s => s.CustomerID == int.Parse(CustomerID));
            records.ToList().ForEach(x => x.Default = false);
            await _CustomerAddressService.UpdateRangeAsync(records.ToList());
            var add = await _CustomerAddressService.AddAsync(customerAddress);
            if (add > 0)
            {

                return Ok(new ResponseApiEntity<AddCustomerAddress>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            }
            else
                return BadRequest(new ResponseApiEntity<AddCustomerAddress>
                                                           (entity: new AddCustomerAddress(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.ErrorNotAllowRequest));
        }

        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpDelete("CustomerAddresss/Delete/Public/{identityCode}")]
        public async Task<IActionResult> DeleteInfo([FromRoute] string? identityCode)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultCustomerAddress>
                                                         (entity: new ResultCustomerAddress(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.ErrorNotAllowRequest));

            var CustomerID = User.Claims
                  .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<ResultCustomerAddress>
                                                                 (entity: new ResultCustomerAddress(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customerinfo = await _customerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) &&  s.IdentityCode == guid);
            if (customerinfo == null)
                return BadRequest(new ResponseApiEntity<ResultCustomerAddress>
                                                                 (entity: new ResultCustomerAddress(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            if (!Guid.TryParse(identityCode, out Guid CustomerAddressIdentityCode))
                return BadRequest(new ResponseApiEntity<ResultCustomerAddress>
                                                                 (entity: new ResultCustomerAddress(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
           


            if (customerinfo.CustomerAddresss == null)
            {
                return BadRequest(new ResponseApiEntity<ResultCustomerAddress>(
                    entity: new ResultCustomerAddress(),
                    statusCode: ResultMessageApi.ErrorCode,
                    status: ResultMessageApi.Error,
                    message: ResultMessageApi.ErrorNotAllowRequest));
            }

            var recordToDelete= customerinfo.CustomerAddresss.Where(s => s.IdentityCode == CustomerAddressIdentityCode).FirstOrDefault();
            if (recordToDelete == null)
            {
                return BadRequest(new ResponseApiEntity<ResultCustomerAddress>(
                    entity: new ResultCustomerAddress(),
                    statusCode: ResultMessageApi.ErrorCode,
                    status: ResultMessageApi.Error,
                    message: ResultMessageApi.ErrorNotAllowRequest));
            }
           var address= await _orderService.FirstOrDefaultAsync(s => s.SendProductMethodID == recordToDelete.ID);
            if(address!=null)
            {
                return BadRequest(new ResponseApiEntity<ResultCustomerAddress>(
                    entity: new ResultCustomerAddress(),
                    statusCode: ResultMessageApi.ErrorCode,
                    status: ResultMessageApi.Error,
                    message: ResultMessageApi.ErrorNotAllowDeleteAddress));
            }
            var CustomerAddressRecords = customerinfo.CustomerAddresss.Where(s => s.ID != recordToDelete.ID);
            // وضعیت پیش‌فرض بودن رکورد حذف‌شده
            bool wasDefault = recordToDelete.Default;
            
            // حذف رکورد
            var del = await _CustomerAddressService.DeleteAsync(s => s.IdentityCode == CustomerAddressIdentityCode);

            if (del > 0)
            {
                var list = CustomerAddressRecords.ToList();

                // همه false شوند
                list.ForEach(x => x.Default = false);

                // اگر رکورد حذف‌شده پیش‌فرض بود، یکی از باقی‌مانده‌ها را true کنیم
                if (wasDefault && list.Any())
                {
                    list.First().Default = true;
                }

                await _CustomerAddressService.UpdateRangeAsync(list);

                return Ok(new ResponseApiEntity<ResultCustomerAddress>(
                    entity: new ResultCustomerAddress(),
                    statusCode: ResultMessageApi.SuccessCode,
                    status: ResultMessageApi.Success,
                    message: ResultMessageApi.UpdateOk));
            }
            else
            {
                return BadRequest(new ResponseApiEntity<ResultCustomerAddress>(
                    entity: new ResultCustomerAddress(),
                    statusCode: ResultMessageApi.ErrorCode,
                    status: ResultMessageApi.Error,
                    message: ResultMessageApi.ErrorNotAllowRequest));
            }


        }
        //[HttpGet("CustomerAddresss/Public/All")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetCustomerAddresssAll()
        //{
        //    if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ResultCustomerAddress>
        //                                                   (entity: new ResultCustomerAddress(),
        //                                                   statusCode: ResultMessageApi.ErrorCode,
        //                                                   status: ResultMessageApi.Error,
        //                                                   message: ResultMessageApi.GetError));

        //    var CustomerAddresss = await _CustomerAddressService
        //                        .GetAllAsync(page: 1, take: 15);

        //    var mappedCustomerAddresss = _mapper.Map<ICollection<ResultCustomerAddress>>(CustomerAddresss);

        //    return Ok(new ResponseApiEntities<ResultCustomerAddress>
        //                                                    (entities: mappedCustomerAddresss,
        //                                                    status: ResultMessageApi.Success,
        //                                                    statusCode: ResultMessageApi.SuccessCode,
        //                                                    message: ResultMessageApi.GetOk,
        //                                                    countAllRecordTable: CustomerAddresss.Count()));

        //}

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("CustomerAddresss/ByGuid/{guid}")]
        public async Task<IActionResult> GetCustomerAddressById([FromRoute] string guid)
        {
            try
            {
                var CustomerAddress = await _CustomerAddressService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (CustomerAddress == null)
                    return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                                              (entity: new UpdateCustomerAddress(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateCustomerAddress>(CustomerAddress);
                return Ok(new ResponseApiEntity<UpdateCustomerAddress>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateCustomerAddress>
                                                                             (entity: new UpdateCustomerAddress(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }
    }
}
