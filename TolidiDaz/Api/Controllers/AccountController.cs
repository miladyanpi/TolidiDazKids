using AutoMapper;
using DAL.Paginagion;
using Domain;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoAccount;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class AccountController : ControllerBase
    {
        private readonly UserManager<Account> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
                                 UserManager<Account> userManager,
                                 RoleManager<IdentityRole> roleManager
                                 )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpPost("Accounts/User")]
        public async Task<IActionResult> AddUser([FromBody] AddUser model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var ph = new PasswordHasher<Account>();

            var user1 = new Account
            {
                UserName = model.UserName,
                NormalizedUserName = model.UserName.ToUpper(),
                Email = model.Email,
                NormalizedEmail = model.Email.ToUpper(),
                EmailConfirmed = true,
                CustomerID = model.CustomerID,
                PhoneNumber = model.Mobile,

            };
            user1.PasswordHash = ph.HashPassword(user1, model.Password);
            var role = await _roleManager.FindByIdAsync(model.RoleName);
            if (role == null)
                return
                          NotFound(new ResponseApiEntity<ResultRole>
                          (entity: new ResultRole(),
                          statusCode: ResultMessageApi.ErrorCode,
                          status: ResultMessageApi.Error,
                          message: ResultMessageApi.NotFoundRoleError));

            var userFind = await _userManager.FindByNameAsync(user1.UserName);
            if (userFind != null)
                return
                          NotFound(new ResponseApiEntity<AddUser>
                          (entity: new AddUser(),
                          statusCode: ResultMessageApi.ErrorCode,
                          status: ResultMessageApi.Error,
                          message: ResultMessageApi.UserNameExistsError));

            var result1 = await _userManager.CreateAsync(user1);
            var ResultUserRole = await _userManager.AddToRoleAsync(user1, role.Name);
            if (result1.Succeeded)
                return Ok(new ResponseApiEntity<AddUser>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddUser>
                                                           (entity: new AddUser(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpPost("Accounts/Role")]
        public async Task<IActionResult> AddRole([FromBody] AddRole addRole)
        {
            if (!ModelState.IsValid) return BadRequest();

            var role = new IdentityRole(addRole.RoleName);
            role.ConcurrencyStamp = Guid.NewGuid().ToString();
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
                return Ok(new ResponseApiEntity<AddRole>
                                                               (entity: addRole,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<AddRole>
                                                           (entity: addRole,
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.AddError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpPost("Accounts/UserRole")]
        public async Task<IActionResult> AddUserToRole([FromBody] AddUserRole model)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = _userManager.Users.Where(s => s.CustomerID == model.CusomerID).FirstOrDefault();
            if (user == null) return NotFound(new ResponseApiEntity<ResultUserRole>
                                                           (entity: new ResultUserRole(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.NotFoundUserError));


            if (model.ListResultRoles.Count == 0)
                return NotFound(new ResponseApiEntity<ResultUserRole>
                                                           (entity: new ResultUserRole(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.NotFoundRoleError));
            var roleIDs = model.ListResultRoles.Select(s => s.ID).ToList();
            var roles = _roleManager.Roles.Where(s => roleIDs.Contains(s.Id)).Select(s => s.Name).ToList();



            var ResultUserRole = await _userManager.AddToRolesAsync(user, roles);

            if (ResultUserRole.Succeeded)
                return Ok(new ResponseApiEntity<ResultUserRole>
                                                               (entity: new ResultUserRole(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.AddOk));
            else
                return BadRequest(new ResponseApiEntity<ResultUserRole>
                                                      (entity: new ResultUserRole(),
                                                      statusCode: ResultMessageApi.ErrorCode,
                                                      status: ResultMessageApi.Error,
                                                      message: ResultMessageApi.AddError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpPost("Accounts/AddUserInRole/{CustomerID}/{RoleName}")]
        public async Task<IActionResult> AddUserInRole([FromRoute] int CustomerID, string RoleName)
        {
            if (!ModelState.IsValid) return BadRequest();
            var user = _userManager.Users.Where(s => s.CustomerID == CustomerID).FirstOrDefault();
            if (user == null) return NotFound(new ResponseApiEntity<ResultUserRole>
                                                           (entity: new ResultUserRole(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.NotFoundUserError));
            var ResultUserRole = await _userManager.AddToRoleAsync(user, RoleName);

            if (ResultUserRole.Succeeded)
                return Ok(new ResponseApiEntity<ResultUserRole>
                                                               (entity: new ResultUserRole(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.RoleAddForUser));
            else
                return BadRequest(new ResponseApiEntity<ResultUserRole>
                                                      (entity: new ResultUserRole(),
                                                      statusCode: ResultMessageApi.ErrorCode,
                                                      status: ResultMessageApi.Error,
                                                      message: ResultMessageApi.RoleIsExistsInUser));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpPatch("Accounts/User")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUser model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = await _userManager.FindByIdAsync(model.ID);
            if (user == null)
                return
                    NotFound(new ResponseApiEntity<UpdateUser>
                                                           (entity: new UpdateUser(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            if (model.UserName != model.OldUserName)
            {
                var username = await _userManager.FindByNameAsync(model.UserName);
                if (username != null)
                {
                    return BadRequest(new ResponseApiEntity<UpdateUser>
                               (entity: new UpdateUser(),
                               statusCode: ResultMessageApi.ErrorCode,
                               status: ResultMessageApi.Error,
                               message: ResultMessageApi.UserNameExistsError));
                }

            }
            // user.UserName = model.UserName;
            user.Email = model.Email;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
                return Ok(new ResponseApiEntity<UpdateUser>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateUser>
                                                           (entity: new UpdateUser(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpPatch("Accounts/Role")]
        public async Task<IActionResult> UpdateRole([FromBody] UpdateRole model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var role = await _roleManager.FindByIdAsync(model.ID);
            if (role == null)
                return
                    NotFound(new ResponseApiEntity<UpdateRole>
                                                           (entity: new UpdateRole(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
            role.Name = model.RoleName;

            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
                return Ok(new ResponseApiEntity<UpdateRole>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.UpdateOk));
            else
                return BadRequest(new ResponseApiEntity<UpdateRole>
                                                           (entity: new UpdateRole(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.UpdateError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpDelete("Accounts/User/{id}")]
        public async Task<IActionResult> DeleteUser([FromRoute] string id)
        {

            if (string.IsNullOrEmpty(id))
                return NotFound(new ResponseApiEntity<ResultUser>
                                                           (entity: new ResultUser(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return
                    NotFound(new ResponseApiEntity<ResultUser>
                                                           (entity: new ResultUser(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));

            //var statuse=await _userManager.IsInRoleAsync(user, "admin");
            // var count=_userManager.GetRolesAsync.Count();
            // if (statuse == true && count==1)
            // {
            //     return
            //      BadRequest(new ResponseApiEntity<ResultUser>
            //                                             (entity: new ResultUser(),
            //                                             statusCode: ResultMessageApi.ErrorCode,
            //                                             status: ResultMessageApi.Error,
            //                                             message: ResultMessageApi.OneRoleExistsError));
            // }
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
                return Ok(new ResponseApiEntity<ResultUser>
                                                               (entity: new ResultUser(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultUser>
                                                      (entity: new ResultUser(),
                                                      statusCode: ResultMessageApi.ErrorCode,
                                                      status: ResultMessageApi.Error,
                                                      message: ResultMessageApi.DeleteError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpDelete("Accounts/Role/{id}")]
        public async Task<IActionResult> DeleteRole([FromRoute] string id)
        {

            if (string.IsNullOrEmpty(id))
                return NotFound(new ResponseApiEntity<ResultRole>
                                                           (entity: new ResultRole(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return
                    NotFound(new ResponseApiEntity<ResultRole>
                                                           (entity: new ResultRole(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.DeleteError));
            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return Ok(new ResponseApiEntity<ResultRole>
                                                               (entity: new ResultRole(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultRole>
                                                      (entity: new ResultRole(),
                                                      statusCode: ResultMessageApi.ErrorCode,
                                                      status: ResultMessageApi.Error,
                                                      message: ResultMessageApi.DeleteError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpDelete("Accounts/UserRole/{CustomerID}/{RoleName}")]
        public async Task<IActionResult> DeleteUserRole([FromRoute] int CustomerID, string RoleName)
        {
            var user = _userManager.Users.Where(x => x.CustomerID == CustomerID).FirstOrDefault();
            if (user == null) return NotFound(new ResponseApiEntity<ResultUserRole>
                                                            (entity: new ResultUserRole(),
                                                            statusCode: ResultMessageApi.ErrorCode,
                                                            status: ResultMessageApi.Error,
                                                            message: ResultMessageApi.NotFoundUserError));
            var result = await _userManager.RemoveFromRoleAsync(user, RoleName);
            if (result.Succeeded)
                return Ok(new ResponseApiEntity<ResultUserRole>
                                                               (entity: new ResultUserRole(),
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.DeleteOk));
            else
                return BadRequest(new ResponseApiEntity<ResultUserRole>
                                                      (entity: new ResultUserRole(),
                                                      statusCode: ResultMessageApi.ErrorCode,
                                                      status: ResultMessageApi.Error,
                                                      message: ResultMessageApi.DeleteError));

        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpGet("Accounts/Users")]
        public async Task<IActionResult> GetUsers([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = _userManager.Users.Count(s =>
                                        s.UserName.Contains(@params.SearchText) ||
                                        s.Email.Contains(@params.SearchText));
            int Skip = (@params.Take * @params.Take) - @params.Take;
            var Accounts = _userManager.Users
                            .Where(s =>
                                        s.UserName.Contains(@params.SearchText) ||
                                        s.Email.Contains(@params.SearchText))
                            .Skip(Skip)
                            .Take(@params.Take)
                            .ToList();

            List<ResultUser> resultAccounts = new List<ResultUser>();

            foreach (var item in Accounts)
            {
                var RoleName = await _userManager.GetRolesAsync(item);

                var resultUser = new ResultUser
                {
                    ID = item.Id,
                    UserName = item.UserName,
                    Email = item.Email,

                };
                resultUser.RoleName = ConstantRoles.GetRoleTitle(RoleName[0].ToString());
                resultAccounts.Add(resultUser);

            }
            return Ok(new ResponseApiEntities<ResultUser>
                                                             (entities: resultAccounts,
                                                             status: ResultMessageApi.Success,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             message: ResultMessageApi.GetOk,
                                                             countAllRecordTable: count));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpGet("Accounts/Role")]
        public IActionResult GetRoles([FromQuery] PaginationParams @params)
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = _roleManager.Roles.Count(s => s.Name.Contains(@params.SearchText));
            int Skip = (@params.Page * @params.Take) - @params.Take;
            var Accounts = _roleManager.Roles
                            .Where(s => s.Name.Contains(@params.SearchText))
                            .Take(@params.Take)
                            .Skip(Skip)
                            .ToList();

            List<ResultRole> resultRoles = new List<ResultRole>();
            foreach (var item in Accounts)
            {
                resultRoles.Add(new ResultRole
                {
                    ID = item.Id,
                    RoleName = item.Name,
                });
            }
            return Ok(new ResponseApiEntities<ResultRole>
                                                             (entities: resultRoles,
                                                             status: ResultMessageApi.Success,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             message: ResultMessageApi.GetOk,
                                                             countAllRecordTable: count));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpGet("Accounts/Roles/All")]
        public IActionResult GetRolesAll()
        {
            if (!ModelState.IsValid) return BadRequest();

            var count = _roleManager.Roles.Count();
            var Accounts = _roleManager.Roles.ToList();

            List<ResultRole> resultRoles = new List<ResultRole>();
            foreach (var item in Accounts)
            {
                resultRoles.Add(new ResultRole
                {
                    ID = item.Id,
                    RoleName = item.Name,
                    RoleTitle = ConstantRoles.GetRoleTitle(item.Name),
                });
            }
            return Ok(new ResponseApiEntities<ResultRole>
                                                             (entities: resultRoles,
                                                             status: ResultMessageApi.Success,
                                                             statusCode: ResultMessageApi.SuccessCode,
                                                             message: ResultMessageApi.GetOk,
                                                             countAllRecordTable: count));
        }

        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpGet("Accounts/GetByUserName/{UserName}")]
        public async Task<IActionResult> GetUserByUserName([FromRoute] string UserName)
        {
            if (string.IsNullOrEmpty(UserName))
                return NotFound(new ResponseApiEntity<ResultUser>
                                                           (entity: new ResultUser(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.NotFoundUserError));
            var user = await _userManager.FindByNameAsync(UserName);
            if (user == null)
                return
                    NotFound(new ResponseApiEntity<ResultUser>
                                                           (entity: new ResultUser(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetOk));

            var model = new ResultUser
            {
                Email = user.Email,
                ID = user.Id,
                UserName = user.UserName,
            };

            return Ok(new ResponseApiEntity<ResultUser>
                                                           (entity: model,
                                                           statusCode: ResultMessageApi.SuccessCode,
                                                           status: ResultMessageApi.Success,
                                                           message: ResultMessageApi.GetOk));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpGet("Accounts/User/{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound(new ResponseApiEntity<UpdateUser>
                                                           (entity: new UpdateUser(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.NotFoundUserError));
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return
                    NotFound(new ResponseApiEntity<UpdateUser>
                                                           (entity: new UpdateUser(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.GetOk));

            var model = new UpdateUser
            {
                Email = user.Email,
                ID = user.Id,
                UserName = user.UserName,
                OldUserName = user.UserName,
            };

            return Ok(new ResponseApiEntity<UpdateUser>
                                                           (entity: model,
                                                           statusCode: ResultMessageApi.SuccessCode,
                                                           status: ResultMessageApi.Success,
                                                           message: ResultMessageApi.GetOk));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpGet("Accounts/Role/{id}")]
        public async Task<IActionResult> GetRoleById([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound(new ResponseApiEntity<UpdateRole>
                                                           (entity: new UpdateRole(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.NotFoundRoleError));
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
                return
                    NotFound(new ResponseApiEntity<UpdateRole>
                                                           (entity: new UpdateRole(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.NotFoundRoleError));
            var model = new UpdateRole
            {
                ID = role.Id,
                RoleName = role.Name,
            };

            return Ok(new ResponseApiEntity<UpdateRole>
                                                           (entity: model,
                                                           statusCode: ResultMessageApi.SuccessCode,
                                                           status: ResultMessageApi.Success,
                                                           message: ResultMessageApi.GetOk));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpGet("Accounts/User/{id}/Roles")]
        public async Task<IActionResult> GetUserRolesById([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound(new ResponseApiEntity<ResultRole>
                                                           (entity: new ResultRole(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.NotFoundUserError));
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
                return
                    NotFound(new ResponseApiEntity<ResultRole>
                                                           (entity: new ResultRole(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.NotFoundUserError));
            var userRoles = await _userManager.GetRolesAsync(user);

            var modelRoles = new List<ResultRole>();
            foreach (var roleName in userRoles)
            {
                modelRoles.Add(new ResultRole() { RoleName = roleName });
            }
            return Ok(new ResponseApiEntities<ResultRole>
                                                           (entities: modelRoles,
                                                           statusCode: ResultMessageApi.SuccessCode,
                                                           status: ResultMessageApi.Success,
                                                           message: ResultMessageApi.GetOk,
                                                           countAllRecordTable: modelRoles.Count()));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName)]
        [HttpGet("Accounts/Customer/{CustomerId}/Roles")]
        public async Task<IActionResult> GetUserRolesById([FromRoute] int? CustomerId)
        {

            var user = _userManager.Users.Where(s => s.CustomerID == CustomerId).FirstOrDefault();
            if (user == null)
                return
                    NotFound(new ResponseApiEntities<ResultRole>
                                                           (entities: new List<ResultRole>(),
                                                           statusCode: ResultMessageApi.ErrorCode,
                                                           status: ResultMessageApi.Error,
                                                           message: ResultMessageApi.NotFoundUserError,
                                                           countAllRecordTable: 0));
            var userRoles = await _userManager.GetRolesAsync(user);

            var modelRoles = new List<ResultRole>();
            foreach (var roleName in userRoles)
            {
                modelRoles.Add(new ResultRole() { RoleName = roleName, RoleTitle = ConstantRoles.GetRoleTitle(roleName) });
            }
            return Ok(new ResponseApiEntities<ResultRole>
                                                           (entities: modelRoles,
                                                           statusCode: ResultMessageApi.SuccessCode,
                                                           status: ResultMessageApi.Success,
                                                           message: ResultMessageApi.GetOk,
                                                           countAllRecordTable: modelRoles.Count()));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName + "," + ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpPost("Accounts/ChangePasswordAccountCustomer")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordAccountCustomer changePasswordAccount)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ChangePasswordAccountCustomer>
                                                              (entity: new ChangePasswordAccountCustomer(),
                                                              statusCode: ResultMessageApi.ErrorCode,
                                                              status: ResultMessageApi.Error,
                                                              message: ResultMessageApi.ErrorNotAllowRequest));
            var CustomerID = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var username = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.UserData)?.Value; 
            if(username!= changePasswordAccount.UserName)
                return BadRequest(new ResponseApiEntity<ChangePasswordAccountCustomer>
                                                              (entity: new ChangePasswordAccountCustomer(),
                                                              statusCode: ResultMessageApi.ErrorCode,
                                                              status: ResultMessageApi.Error,
                                                              message: ResultMessageApi.ErrorNotAllowRequest));

            var _user = _userManager.Users.Where(s => s.CustomerID == int.Parse(CustomerID) && s.UserName == changePasswordAccount.UserName).FirstOrDefault();
            if (_user == null)
                return BadRequest(new ResponseApiEntity<ChangePasswordAccountCustomer>
                                                                 (entity: new ChangePasswordAccountCustomer(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            
                var ph = new PasswordHasher<Account>();
                _user.PasswordHash = ph.HashPassword(_user, changePasswordAccount.NewPassword);
                var changeState = await _userManager.UpdateAsync(_user);
                if (changeState.Succeeded)
                    return Ok(new ResponseApiEntity<ChangePasswordAccountCustomer>
                                                                   (entity: new ChangePasswordAccountCustomer(),
                                                                   statusCode: ResultMessageApi.SuccessCode,
                                                                   status: ResultMessageApi.Success,
                                                                   message: ResultMessageApi.ChangePasswordOk));
                return BadRequest(new ResponseApiEntity<ChangePasswordAccountCustomer>
                                                                   (entity: new ChangePasswordAccountCustomer(),
                                                                   statusCode: ResultMessageApi.ErrorCode,
                                                                   status: ResultMessageApi.Error,
                                                                   message: ResultMessageApi.WrongPasswordError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName + "," + ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]
        [HttpPost("Accounts/ChangePasswordAccount")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordAccount changePasswordAccount)
        {
            if (!ModelState.IsValid) return BadRequest(new ResponseApiEntity<ChangePasswordAccount>
                                                              (entity: new ChangePasswordAccount(),
                                                              statusCode: ResultMessageApi.ErrorCode,
                                                              status: ResultMessageApi.Error,
                                                              message: ResultMessageApi.ErrorNotAllowRequest));
            var CustomerID = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.PrimarySid)?.Value;
            var username = User.Claims
                       .FirstOrDefault(s => s.Type == ClaimTypes.UserData)?.Value;
            if (username != changePasswordAccount.UserName)
                return BadRequest(new ResponseApiEntity<ChangePasswordAccount>
                                                              (entity: new ChangePasswordAccount(),
                                                              statusCode: ResultMessageApi.ErrorCode,
                                                              status: ResultMessageApi.Error,
                                                              message: ResultMessageApi.ErrorNotAllowRequest));

            var _user = _userManager.Users.Where(s => s.CustomerID == int.Parse(CustomerID) && s.UserName == changePasswordAccount.UserName).FirstOrDefault();
            if (_user == null)
                return BadRequest(new ResponseApiEntity<ChangePasswordAccount>
                                                                 (entity: new ChangePasswordAccount(),
                                                                 statusCode: ResultMessageApi.ErrorCode,
                                                                 status: ResultMessageApi.Error,
                                                                 message: ResultMessageApi.ErrorNotAllowRequest));


            var result = _user != null && await _userManager.CheckPasswordAsync(_user, changePasswordAccount.OldPassword);
            if (result)
            {
                var ph = new PasswordHasher<Account>();

                _user.PasswordHash = ph.HashPassword(_user, changePasswordAccount.NewPassword);
                var changeState = await _userManager.UpdateAsync(_user);
                if (changeState.Succeeded)
                    return Ok(new ResponseApiEntity<ChangePasswordAccount>
                                                                   (entity: new ChangePasswordAccount(),
                                                                   statusCode: ResultMessageApi.SuccessCode,
                                                                   status: ResultMessageApi.Success,
                                                                   message: ResultMessageApi.ChangePasswordOk));
            }
            return BadRequest(new ResponseApiEntity<ChangePasswordAccount>
                                                                   (entity: new ChangePasswordAccount(),
                                                                   statusCode: ResultMessageApi.ErrorCode,
                                                                   status: ResultMessageApi.Error,
                                                                   message: ResultMessageApi.WrongPasswordError));
        }
        [Authorize(Roles = ConstantRoles.SuperAdminName + "," + ConstantRoles.AdminName)]

        [HttpPost("Accounts/ResetPasswordAccount")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordAccount model)
        {
            if (!ModelState.IsValid) return BadRequest();

            var _user = await _userManager.FindByNameAsync(model.UserName);
            if (_user == null)
                return BadRequest(new ResponseApiEntity<ResetPasswordAccount>
                                                                (entity: new ResetPasswordAccount(),
                                                                statusCode: ResultMessageApi.ErrorCode,
                                                                status: ResultMessageApi.Error,
                                                                message: ResultMessageApi.UserNotFound));
            var ph = new PasswordHasher<Account>();

            _user.PasswordHash = ph.HashPassword(_user, "123");
            var changeState = await _userManager.UpdateAsync(_user);
            if (changeState.Succeeded)
                return Ok(new ResponseApiEntity<ResetPasswordAccount>
                                                               (entity: model,
                                                               statusCode: ResultMessageApi.SuccessCode,
                                                               status: ResultMessageApi.Success,
                                                               message: ResultMessageApi.ResetPasswordOk));

            return BadRequest(new ResponseApiEntity<ResetPasswordAccount>
                                                                   (entity: new ResetPasswordAccount(),
                                                                   statusCode: ResultMessageApi.ErrorCode,
                                                                   status: ResultMessageApi.Error,
                                                                   message: ResultMessageApi.WrongPasswordError));
        }

    }


}
