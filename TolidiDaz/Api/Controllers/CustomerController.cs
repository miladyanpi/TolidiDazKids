using ServicesLibrary.Services.CartSrv;
using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.OrderSrv;
using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoCustomer;
using Dto.Models.DtoUploadFile;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace Api.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CustomerController(
        ICustomerService _customerService,
        ICartService _cartService,
        IOrderService _OrderService,
        UserManager<Account> _userManager,
        IMapper _mapper
        )
        : ControllerBase
    {
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPost("Customers")]
        public async Task<IActionResult> Add([FromBody] AddCustomer model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user1 = _userManager.Users.Where(s => s.PhoneNumber == model.Mobile).FirstOrDefault();
            if (user1 != null)
                return BadRequest(new ResponseApiEntity<AddCustomer>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UserNameMobileExistsError));

            var user2 = _userManager.Users.Where(s => s.UserName == model.UserName).FirstOrDefault();
            if (user2 != null)
                return BadRequest(new ResponseApiEntity<AddCustomer>
                                                          (entity: null,
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.UserNameExistsError));
            var customer = _mapper.Map<AddCustomer, Customer>(model);
            int id = await _customerService.AddAsync(customer);

            if (id > 0)
                return Ok(new ResponseApiEntity<AddCustomer>
                                                               (entity: model,
                                                               id: id,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddCustomer>
                                                           (entity: null,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpPatch("Customers")]
        public async Task<IActionResult> Update([FromBody] UpdateCustomer model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateCustomer>
                                                           (entity: new UpdateCustomer(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));

            var user = _userManager.Users.Where(s => s.CustomerID == model.ID).FirstOrDefault();
            if (user.UserName != model.UserName)
            {
                var user2 = _userManager.Users.Where(s => s.CustomerID != model.ID && s.UserName == model.UserName).FirstOrDefault();
                if (user2 != null)
                    return BadRequest(new ResponseApiEntity<UpdateCustomer>
                                                          (entity: new UpdateCustomer(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.UserNameExistsError));
                else
                    user.UserName = model.UserName;


            }
            if (user.PhoneNumber != model.Mobile)
            {
                var user3 = _userManager.Users.Where(s => s.CustomerID != model.ID && s.PhoneNumber == model.Mobile).FirstOrDefault();
                if (user3 != null)
                    return BadRequest(new ResponseApiEntity<UpdateCustomer>
                                                          (entity: new UpdateCustomer(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.UserNameMobileReapetly));
                else
                    user.PhoneNumber = model.Mobile;

            }
            if (user.Email != model.Email)
            {
                var user4 = _userManager.Users.Where(s => s.CustomerID != model.ID && s.Email == model.Email).FirstOrDefault();
                if (user4 != null)
                    return BadRequest(new ResponseApiEntity<UpdateCustomer>
                                                          (entity: new UpdateCustomer(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.UserEmailReapetly));
                else
                    user.Email = model.Email;

            }
            var customer = _mapper.Map<UpdateCustomer, Customer>(model);
            customer.IdentityCode = model.IdentityCode;
            var upd = await _customerService.UpdateAsync(customer);
            if (upd > 0)
            {
                _ = await _userManager.UpdateAsync(user);
                return Ok(new ResponseApiEntity<UpdateCustomer>
                                                             (entity: model,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             status: ResultMessageApi.Success,
                                                             message: ResultMessageApi.UpdateOk));
            }

            else
                return BadRequest(new ResponseApiEntity<UpdateCustomer>
                                                           (entity: new UpdateCustomer(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }

        [HttpPatch("Customers/UpdateJsonFile")]
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        public async Task<IActionResult> UpdateUpdateCustomerJsonFile([FromBody] UpdateJsonFile model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var q = await _customerService.GetByIdAsync(model.ID);
            if (q == null)
                return BadRequest(new ResponseApiEntity<UpdateJsonFile>
                                                         (entity: new UpdateJsonFile(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.UpdateError));
            q.JsonPicture = model.JsonPicture;
            // var customer = _mapper.Map<UpdateCustomer, Customer>(q);
            var upd = await _customerService.UpdateAsync(q);
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

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpDelete("Customers/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {

            var user = await _customerService.GetByIdAsync(id);
            if (user != null && user.Account != null)
                _ = await _userManager.DeleteAsync(user.Account);

            var countCart = await _cartService.GetCountAllAsync(s => s.CustomerID == id);
            if (countCart > 0)
                return BadRequest(new ResponseApiEntity<ResultCustomer>
                                                          (entity: new ResultCustomer(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.ErrorCartExists));



            var countOrderService = await _OrderService.GetCountAllAsync(s => s.CustomerID == id);
            if (countOrderService > 0)
                return BadRequest(new ResponseApiEntity<ResultCustomer>
                                                          (entity: new ResultCustomer(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.NotAllowDeleteError));

            var del = await _customerService.DeleteAsync(id);
            if (del > 0)
                return Ok(new ResponseApiEntity<ResultCustomer>
                                                               (entity: new ResultCustomer(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultCustomer>
                                                           (entity: new ResultCustomer(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
        }

        [HttpPost("Customers/Data")]
        public async Task<IActionResult> GetCustomers([FromBody] PaginationParams @params)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultCustomer>
                                                                    (entities: new List<ResultCustomer>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ResultMessageApi.GetError,
                                                                    countAllRecordTable: 0
                                                                   ));

                Expression<Func<Customer, bool>> predicate = x => true;

                if (!string.IsNullOrWhiteSpace(@params.SearchText))
                {
                    var search = @params.SearchText;

                    predicate = x =>
                        (x.Name ?? "").Contains(search) ||
                         x.LastName.Contains(search) ||
                         x.Mobile.Contains(search);
                }

                var count = await _customerService.GetCountAllAsync(predicate);

                var Customers = await _customerService
                                    .GetAllAsync(predicate, page: @params.Page, take: @params.Take);


                var mappedCustomers = _mapper.Map<ICollection<ResultCustomer>>(Customers);

                return Ok(new ResponseApiEntities<ResultCustomer>
                                                                (entities: mappedCustomers,
                                                                status: ResultMessageApi.Success,
                                                                statusCode: ResultMessageApi.SuccessCode,
                                                                message: ResultMessageApi.GetOk,
                                                                countAllRecordTable: count));

            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntities<ResultCustomer>
                                                                    (entities: new List<ResultCustomer>(),
                                                                    statusCode: ResultMessageApi.ErrorCode,
                                                                    status: ResultMessageApi.Error,
                                                                    message: ex.Message,
                                                                    countAllRecordTable: 0
                                                                   ));
            }

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Customers2")]
        public async Task<IActionResult> GetCustomers2([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntities<ResultCustomer>
                                                            (entities: new List<ResultCustomer>(),
                                                            status: ResultMessageApi.Error,
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            message: ResultMessageApi.GetError,
                                                            countAllRecordTable: 0));

            Expression<Func<Customer, bool>> predicate = x => true;

            if (!string.IsNullOrWhiteSpace(@params.SearchText))
            {
                var search = @params.SearchText;

                predicate = x =>
                    x.Name.Contains(search) ||
                    x.LastName.Contains(search) ||
                      x.Mobile.Contains(search);
            }

            int count;
            IEnumerable<Customer> customers;

            if (!string.IsNullOrWhiteSpace(@params.SearchText))
            {
                count = await _customerService.GetCountAllAsync(predicate);
                customers = await _customerService
                                    .GetAllAsync(predicate,
                                                 page: @params.Page,
                                                 take: @params.Take);
            }
            else
            {
                count = await _customerService.GetCountAllAsync();
                customers = await _customerService
                                    .GetAllAsync(page: @params.Page,
                                                 take: @params.Take);
            }


            var mappedCustomers = _mapper.Map<ICollection<ResultCustomer>>(customers);
            var ListCustomers = mappedCustomers.ToList();
            int i = -1;
            foreach (var mappedCustomer in customers.ToList())
            {
                i++;
                var user = _userManager.Users.Where(s => s.CustomerID == mappedCustomer.ID).FirstOrDefault();
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    var TempRoles = new List<(string? RoleName, string? RoleTitle)>(); ;
                    foreach (var role in roles)
                    {
                        TempRoles.Add((role, ConstantRoles.GetRoleTitle(role)));
                    }
                    ListCustomers[i].Roles = TempRoles;
                    ListCustomers[i].ResultCustomerUserInfo = new ResultCustomerUserInfo
                    {
                        Email = user.Email,
                        UserName = user.UserName,
                        Mobile = user.PhoneNumber,
                    };
                }



            }
            return Ok(new ResponseApiEntities<ResultCustomer>
                                                            (entities: mappedCustomers,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: count));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Customers/{id}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] int id)
        {

            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<UpdateCustomer>
                                                           (entity: new UpdateCustomer(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

            var user = _userManager.Users.Where(s => s.CustomerID == id).FirstOrDefault();
            if (user == null)
                return BadRequest(new ResponseApiEntity<UpdateCustomer>
                                                         (entity: new UpdateCustomer(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.GetError));

            var result = _mapper.Map<UpdateCustomer>(customer);
            result.Email = user.Email;
            result.UserName = user.UserName;
            result.Mobile = user.PhoneNumber;
            if (result != null)
                return Ok(new ResponseApiEntity<UpdateCustomer>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateCustomer>
                                                           (entity: new UpdateCustomer(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]
        [HttpGet("Customers/BranchStores")]
        public async Task<IActionResult> GetDesignerCustomers()
        {
            if (!ModelState.IsValid) return BadRequest();
            var UserDesigns = await _userManager.GetUsersInRoleAsync(ConstantRoles.BranchStoreName);
            var customerIds = UserDesigns.Select(s => s.CustomerID).ToList();
            var Customers = await _customerService
                                .GetAllAsync(s => customerIds.Contains(s.ID));

            var mappedCustomers = _mapper.Map<ICollection<ResultCustomer>>(Customers);
            var ListCustomers = mappedCustomers.ToList();
            int i = -1;

            return Ok(new ResponseApiEntities<ResultCustomer>
                                                            (entities: mappedCustomers,
                                                            status: ResultMessageApi.Success,
                                                            statusCode: ResultMessageApi.SuccessCode,
                                                            message: ResultMessageApi.GetOk,
                                                            countAllRecordTable: mappedCustomers.Count()));

        }

        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpGet("Customers/GetInfo/Public")]
        public async Task<IActionResult> GetCustomerInfo()
        {
            var CustomerID = User.Claims
                  .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<UpdateCustomer>
                                                                 (entity: new UpdateCustomer(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customer = await _customerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<UpdateCustomer>
                                                                 (entity: new UpdateCustomer(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var result = _mapper.Map<UpdateCustomer>(customer);
            if (result != null)
                return Ok(new ResponseApiEntity<UpdateCustomer>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return NotFound(new ResponseApiEntity<UpdateCustomer>
                                                           (entity: new UpdateCustomer(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }
        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpPatch("Customers/UpdateCustomerInfo/Public")]
        public async Task<IActionResult> UpdateInfo([FromBody] UpdateCustomerInfo model)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<UpdateCustomerInfo>
                                                         (entity: new UpdateCustomerInfo(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.GetError));

            string pattern = @"^\d{4}/\d{2}/\d{2}$";
            Regex regex = new Regex(pattern);

            // بررسی تطابق رشته با الگو
            if (!regex.IsMatch(model.BirthDate))
                return BadRequest(new ResponseApiEntities<UpdateCustomerInfo>
                                                           (entities: new List<UpdateCustomerInfo>(),
                                                           status: ResultMessageApi.Error,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           message: ResultMessageApi.DateFormatError,
                                                           countAllRecordTable: 0));


            var CustomerID = User.Claims
                  .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<UpdateCustomerInfo>
                                                                 (entity: new UpdateCustomerInfo(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customerinfo = await _customerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customerinfo == null)
                return BadRequest(new ResponseApiEntity<UpdateCustomerInfo>
                                                                 (entity: new UpdateCustomerInfo(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var customer = _mapper.Map<UpdateCustomerInfo, Customer>(model);

            var mobile = await _customerService.FirstOrDefaultAsync(s => s.Mobile == model.Mobile);
            if (mobile != null && mobile.ID != customerinfo.ID)
            {
                return NotFound(new ResponseApiEntity<UpdateCustomerInfo>
                                                    (entity: new UpdateCustomerInfo(),
                                                    statusCode: ResultMessageApi.ErrorCode,
                                                    status: ResultMessageApi.Error,
                                                    message: ResultMessageApi.UserNameMobileReapetly));
            }
            customer.IdentityCode = customerinfo.IdentityCode;
            customer.Mobile = customerinfo.Mobile;
            var upd = await _customerService.UpdateAsync(customer);
            if (upd > 0)
            {
                var username = User.Claims
                        .FirstOrDefault(s => s.Type == ClaimTypes.UserData)?.Value;
                var user = await _userManager.FindByNameAsync(username);


                user.Email = model.Email;
                _ = await _userManager.UpdateAsync(user);
                return Ok(new ResponseApiEntity<UpdateCustomerInfo>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            }
            else
                return BadRequest(new ResponseApiEntity<UpdateCustomerInfo>
                                                           (entity: new UpdateCustomerInfo(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }

        [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpGet("Customers/UpdateCustomerInfo/Public")]
        public async Task<IActionResult> GetUpdateCustomerInfo()
        {
            var CustomerID = User.Claims
                  .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var UniqCode = User.Claims
                      .FirstOrDefault(s => s.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!Guid.TryParse(UniqCode, out Guid guid))
                return BadRequest(new ResponseApiEntity<UpdateCustomerInfo>
                                                                 (entity: new UpdateCustomerInfo(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));
            var customer = await _customerService.FirstOrDefaultAsync(s => s.ID == int.Parse(CustomerID) && s.IdentityCode == guid);
            if (customer == null)
                return BadRequest(new ResponseApiEntity<UpdateCustomerInfo>
                                                                 (entity: new UpdateCustomerInfo(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));

            var result = _mapper.Map<UpdateCustomerInfo>(customer);
            if (result != null)
                return Ok(new ResponseApiEntity<UpdateCustomerInfo>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            else
                return NotFound(new ResponseApiEntity<UpdateCustomerInfo>
                                                           (entity: new UpdateCustomerInfo(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetError));

        }

        [HttpGet("Customers/ByGuid/{guid}")]
        public async Task<IActionResult> GetCustomerById([FromRoute] string guid)
        {
            try
            {
                var Customer = await _customerService.FirstOrDefaultAsync(s => s.IdentityCode.ToString() == guid);
                if (Customer == null)
                    return BadRequest(new ResponseApiEntity<UpdateCustomer>
                                                                              (entity: new UpdateCustomer(),
                                                                              statusCode: ResultMessageApi.ErrorCode,
                                                                              status: ResultMessageApi.Error,
                                                                              message: ResultMessageApi.GetError));
                var result = _mapper.Map<UpdateCustomer>(Customer);
                return Ok(new ResponseApiEntity<UpdateCustomer>
                                                               (entity: result,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.GetOk));
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseApiEntity<UpdateCustomer>
                                                                             (entity: new UpdateCustomer(),
                                                                             statusCode: ResultMessageApi.ErrorCode,
                                                                             status: ResultMessageApi.Error,
                                                                             message: ex.Message));
            }

        }
    }
}
