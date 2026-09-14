using Domain;
using Dto.Models.Constant;
using Dto.Models.DtoAccount;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary.Services.AuthenticationManagerSrv;
using ServicesLibrary.Services.RefreshTokenEntitySrv;
using Utility;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    public class AuthenticationController(
        IAuthenticationManager _authenticationManager,
        UserManager<Account> _userManager,
        //UserManager<Account> _userManager,
        IRefreshTokenEntityService _refreshTokenEntity
        ) 
        : ControllerBase
    {
        [HttpPost("auth/Login")]
        public async Task<IActionResult> Login([FromBody] LoginAccount credentials)
        {
            if (!ModelState.IsValid) return  BadRequest(new ResponseApiEntity<ResultLoginAccount>
                                                         (entity: new ResultLoginAccount(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.LoginError));

            var Result = await _authenticationManager.ValidateCredentials(credentials);
            if (Result==null)
            return BadRequest(new ResponseApiEntity<ResultLoginAccount>
                                                         (entity: new ResultLoginAccount(),
                                                         statusCode: ResultMessageApi.ErrorCode,
                                                         status: ResultMessageApi.Error,
                                                         message: ResultMessageApi.LoginError));


            var Token = await _authenticationManager.CreateToken(Result,isRefreshToken: false);
            var RefreshToken = await _authenticationManager.CreateToken(Result, isRefreshToken:true);
            var deviceId = Guid.NewGuid().ToString();
            ResultLoginAccount resultLoginAccount = new ResultLoginAccount
            {
                Token = Token,
                RefreshToken=RefreshToken,
                DeviceID= deviceId,
                TokenExpired = DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds(),
            };
            var newRefreshEntity = new RefreshTokenEntity
            {
                Token = RefreshToken,
                UserId = Result.Id,
                IsRevoked= false,
                DeviceId= deviceId,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),
                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(DateFunctions.GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(DateFunctions.GetNewDate()),
            };

            var users = await _refreshTokenEntity.GetAllAsync(s => s.UserId== Result.Id);
            foreach(var item in  users)
            {
                if(item.ExpiryDate < DateTime.UtcNow )
                {
                    await _refreshTokenEntity.DeleteAsync(item.ID);
                }
                else
                {
                    item.IsRevoked = true;
                    await _refreshTokenEntity.UpdateAsync(item);
                }
            }
            await _refreshTokenEntity.AddAsync(newRefreshEntity);
            return Ok(new ResponseApiEntity<ResultLoginAccount>
                                                        (entity: resultLoginAccount,
                                                        statusCode: ResultMessageApi.SuccessCode,
                                                        status: ResultMessageApi.Success,
                                                        message: ResultMessageApi.LoginOk));


        }
        [HttpGet("SecurityStamp/{SecurityStamp}")]
        public async Task<IActionResult> Login([FromRoute] string SecurityStamp)
        {
            var Account = _userManager.Users.Where(s => s.SecurityStamp == SecurityStamp).FirstOrDefault();

            if(Account==null)
                return NotFound(new ResponseApiEntity<SecurityStampAccount>
                                                        (entity: new SecurityStampAccount(),
                                                        statusCode: ResultMessageApi.ErrorCode,
                                                        status: ResultMessageApi.Error,
                                                        message: ResultMessageApi.GetError));
            else
            {
                SecurityStampAccount securityStampAccount = new SecurityStampAccount
                {
                    SecurityStamp = Account.SecurityStamp,
                    CustomerID = Account.CustomerID,
                };
                return Ok(new ResponseApiEntity<SecurityStampAccount>
                                                          (entity: securityStampAccount,
                                                          statusCode: ResultMessageApi.SuccessCode,
                                                          status: ResultMessageApi.Success,
                                                          message: ResultMessageApi.GetOk));
            }
               
        }
        [HttpGet("auth/CheckExpiredToken")]
        [AllowAnonymous]
        public async Task<IActionResult> CheckExpiredToken([FromQuery] string Token)
        {
            var validate = await _authenticationManager.CheckTokenIsValid(Token);
            var model = new ResultExpiredToken
            {
                Valid=validate,
            };
            if (!validate)
                return NotFound(new ResponseApiEntity<ResultExpiredToken>
                                                        (entity: new ResultExpiredToken(),
                                                        statusCode: ResultMessageApi.ErrorCode,
                                                        status: ResultMessageApi.Error,
                                                        message: ResultMessageApi.ErrorExpiredToken));
            
                return Ok(new ResponseApiEntity<ResultExpiredToken>
                                                          (entity: model,
                                                          statusCode: ResultMessageApi.SuccessCode,
                                                          status: ResultMessageApi.Success,
                                                          message: ResultMessageApi.OKExpiredToken));

        }
        [HttpGet("auth/RefreshToken")]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken([FromQuery] string refreshToken,string DeviceID)
        {
            if (string.IsNullOrEmpty(refreshToken) || string.IsNullOrEmpty(DeviceID))
                return NotFound(new ResponseApiEntity<ResultLoginAccount>
                                                        (entity: new ResultLoginAccount(),
                                                        statusCode: ResultMessageApi.ErrorCode,
                                                        status: ResultMessageApi.Error,
                                                        message: ResultMessageApi.ErrorBadRequest));

            var refreshTokenModel=await _refreshTokenEntity.FirstOrDefaultAsync(s => s.Token == refreshToken && s.DeviceId== DeviceID && s.IsRevoked==false);

            if (refreshTokenModel==null)
                return NotFound(new ResponseApiEntity<ResultLoginAccount>
                                                       (entity: new ResultLoginAccount(),
                                                       statusCode: ResultMessageApi.ErrorCode,
                                                       status: ResultMessageApi.Error,
                                                       message: ResultMessageApi.ErrorBadRequest));
            if (refreshTokenModel.ExpiryDate < DateTime.UtcNow)
            {
                await _refreshTokenEntity.DeleteAsync(refreshTokenModel.ID);
                return NotFound(new ResponseApiEntity<ResultLoginAccount>
                                                          (entity: new ResultLoginAccount(),
                                                          statusCode: ResultMessageApi.ErrorCode,
                                                          status: ResultMessageApi.Error,
                                                          message: ResultMessageApi.ErrorTokenExpired));
            }

            var account = await _userManager.FindByIdAsync(refreshTokenModel.UserId);
            var newToken = await _authenticationManager.CreateToken(account, isRefreshToken: false);
            var newRefreshToken = await _authenticationManager.CreateToken(account, isRefreshToken: true);

            var newRefreshEntity = new RefreshTokenEntity
            {
                Token = newRefreshToken,
                ReplacedByToken = refreshTokenModel.Token,
                IsRevoked = false,
                DeviceId = refreshTokenModel.DeviceId,
                UserId = refreshTokenModel.Account.Id,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                CreatedAt = DateTime.UtcNow,
                RemoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress?.ToString(),

                RegisterTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                EditTime = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second),
                RegisterDate = DateFunctions.ConvertDateStringToInt(DateFunctions.GetNewDate()),
                EditDate = DateFunctions.ConvertDateStringToInt(DateFunctions.GetNewDate()),
            };

            ResultLoginAccount resultLoginAccount = new ResultLoginAccount
            {
                DeviceID = refreshTokenModel.DeviceId,
                Token = newToken,
                RefreshToken = newRefreshToken,
                TokenExpired = DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds(),
            };
            await _refreshTokenEntity.AddAsync(newRefreshEntity);

            var id= await _refreshTokenEntity.DeleteAsync(refreshTokenModel.ID);

            return Ok(new ResponseApiEntity<ResultLoginAccount>
                                                         (entity: resultLoginAccount,
                                                         statusCode: ResultMessageApi.SuccessCode,
                                                         status: ResultMessageApi.Success,
                                                         message: ResultMessageApi.OKExpiredTokenRefresh));
        }

    }
}
