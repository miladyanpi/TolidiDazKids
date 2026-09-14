using ServicesLibrary.Services.AuthenticationManagerSrv;
using ServicesLibrary.Services.CustomerSrv;
using ServicesLibrary.Services.SettingSrv;
using ServicesLibrary.Services.SmsOtpCodeSrv;
using AutoMapper;
using Domain;
using Dto.Enum;
using Dto.Models;
using Dto.Models.Constant;
using Dto.Models.DtoAccount;
using Dto.Models.DtoCustomer;
using Dto.Models.DtoSmsModel;
using Dto.Models.DtoSmsOtpCode;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceReference1;
using System.Text.RegularExpressions;
using Utility;
using static Dto.Models.SmsViewModel;

namespace Api.Controllers
{
    [Route("Api/")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(Roles = ConstantRoles.CustomerName + "," + ConstantRoles.BranchStoreName)]

    public class SmsOtpCodeController(
        ISmsOtpCodeService _SmsOtpCodeService,
        IMapper _mapper,
        ISettingService _SettingService,
        ICustomerService _CustomerService,
        UserManager<Account> _userManager,
        RoleManager<IdentityRole> _roleManager,
        IAuthenticationManager _authenticationManager
        )
        : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("SmsOtpCodes/SendCode/Public")]
        public async Task<IActionResult> SendSms([FromBody] SendSmsOtpCode smsModel)
        {
            string pattern = @"^09(0[1-9]|1[0-9]|2[0-9]|3[0-9]|9[0-9])\d{7}$";
            Regex regex = new Regex(pattern);

            if (!regex.IsMatch(smsModel.PhoneNumber))
                return BadRequest(new ResponseApiEntities<UpdateCustomer>(
                    entities: new List<UpdateCustomer>(),
                    status: ResultMessageApi.Error,
                    statusCode: ResultMessageApi.ErrorCode,
                    message: "فرمت شماره موبایل نادرست است.",
                    countAllRecordTable: 0));


            var result = await _SmsOtpCodeService.FirstOrDefaultAsync(s=>s.PhoneNumber== smsModel.PhoneNumber);
            if(result!=null)
            {
                var dt=PersianDateExtensionMethods.GetMiladiToPersianDate(DateTime.Now);
                var dateNow = DateFunctions.ConvertDateStringToInt(dt);
                var timeNow = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                if(dateNow == result.DateExpired && timeNow < result.TimeExpired)
                {
                    return NotFound(new ResponseApiEntity<AddSmsOtpCode>
                                            (entity: new AddSmsOtpCode(),
                                                id: 0,
                                            statusCode: ResultMessageApi.ErrorCode,
                                            status: ResultMessageApi.ErrorShowBoxCode,
                                            message: ResultMessageApi.ErrorSmsCodeSendAfter));
                }
                if (dateNow > result.DateExpired || timeNow > result.TimeExpired)
                {
                    _ = await _SmsOtpCodeService.DeleteAsync(result.ID);
                    //return NotFound(new ResponseApiEntity<AddSmsOtpCode>
                    //                        (entity: new AddSmsOtpCode(),
                    //                            id: 0,
                    //                        statusCode: ResultMessageApi.ErrorCode,
                    //                        status: ResultMessageApi.Error,
                    //                        message: ResultMessageApi.ErrorSmsCodeExpired));
                }
            }

            List<string> phonNumbers = new List<string>();
            phonNumbers.Add(smsModel.PhoneNumber);

            Random random = new Random();   
            var RandomValue=random.Next(111111,999999);
            AddSmsOtpCode addSmsOtpCode = new AddSmsOtpCode();
            var set = await _SettingService.GetAllAsync();
            if (set.Count() > 0)
            {
                var settings = set.ToList();
                SendSMSRequest smsRequest =
                new(settings[0].UserName,
                settings[0].Password,
                settings[0].PhoneSender,
                phonNumbers.ToArray(),
                $"محرمانه\r\n کد ورود: {RandomValue.ToString()}\r\n {settings[0].Name}",
                smsModel.IsFlash,
                smsModel.RecId,
                smsModel.Status);
                SendServiceClient client = new SendServiceClient();
                var sendSMSResponse = await client.SendSMSAsync(smsRequest);

                var SmsResponseMessage = SmsViewModel.GetSmsResponseMessage((SendSmsReturnType)sendSMSResponse.SendSMSResult);
                client.Close();

                addSmsOtpCode.Code = RandomValue.ToString();
                addSmsOtpCode.PhoneNumber = smsModel.PhoneNumber;   
                
                var SmsOtpCode = _mapper.Map<AddSmsOtpCode, SmsOtpCode>(addSmsOtpCode);
                int id = await _SmsOtpCodeService.AddAsync(SmsOtpCode);
                addSmsOtpCode.Code = string.Empty;
                return Ok(new ResponseApiEntity<AddSmsOtpCode>
                                                    (entity: addSmsOtpCode,
                                                    id: 0,
                                                    statusCode: ResultMessageApi.SuccessCode,
                                                    status: ResultMessageApi.Success,
                                                    message: SmsResponseMessage));
            }

            return NotFound(new ResponseApiEntity<AddSmsOtpCode>
                                                (entity: addSmsOtpCode,
                                                    id: 0,
                                                statusCode: ResultMessageApi.ErrorCode,
                                                status: ResultMessageApi.Error,
                                                message: ResultMessageApi.ErrorSetSetting));
        }
        [AllowAnonymous]
        [HttpPost("SmsOtpCodes/Verify/Public")]
        public async Task<IActionResult> VerifySms([FromBody] VerifySendSmsOtpCode smsModel)
        {
           
            var result = await _SmsOtpCodeService.FirstOrDefaultAsync(s => s.PhoneNumber == smsModel.PhoneNumber && s.Code == smsModel.Code);
            if (result == null)
                return NotFound(new ResponseApiEntity<ResultLoginAccount>
                                          (entity: new ResultLoginAccount(),
                                              id: 0,
                                          statusCode: ResultMessageApi.ErrorCode,
                                          status: ResultMessageApi.Error,
                                          message: ResultMessageApi.ErrorSmsCodeNotExists));
            if (result != null)
            {
                var dt = PersianDateExtensionMethods.GetMiladiToPersianDate(DateTime.Now);
                var dateNow = DateFunctions.ConvertDateStringToInt(dt);
                var timeNow = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                if (dateNow > result.DateExpired || timeNow > result.TimeExpired)
                {
                    _ = await _SmsOtpCodeService.DeleteAsync(result.ID);
                    return NotFound(new ResponseApiEntity<ResultLoginAccount>
                                            (entity: new ResultLoginAccount(),
                                                id: 0,
                                            statusCode: ResultMessageApi.ErrorCode,
                                            status: ResultMessageApi.Error,
                                            message: ResultMessageApi.ErrorSmsCodeExpired));
                }
            }
            string username=string.Empty;   
            string password=string.Empty;
            Random random = new Random();
            var RandomValue = random.Next(11111111, 99999999);
            var customer = await _CustomerService.FirstOrDefaultAsync(s => s.Mobile == smsModel.PhoneNumber);
            if (customer == null)
            {
                AddCustomer addCustomer = new AddCustomer
                {
                    Name = null,
                    LastName = null,
                    Email = null,
                    Gender = EnumConstant.Gender.NotDefine,
                    Mcode = null,
                    Mobile = smsModel.PhoneNumber,
                    RegisterDate = PersianDateExtensionMethods.GetMiladiToPersianDate(DateTime.Now),
                    EditDate = PersianDateExtensionMethods.GetMiladiToPersianDate(DateTime.Now),
                    Visible= true,
                };
                var newcustomer = _mapper.Map<AddCustomer, Customer>(addCustomer);
                int customerid = await _CustomerService.AddAsync(newcustomer);

                var ph = new PasswordHasher<Account>();

                var user1 = new Account
                {
                    UserName = result.PhoneNumber,
                    NormalizedUserName = result.PhoneNumber.ToUpper(),
                    Email =string.Empty,
                    NormalizedEmail = string.Empty,
                    EmailConfirmed = true,
                    CustomerID = customerid,
                    PhoneNumber = smsModel.PhoneNumber,
                    PhoneNumberConfirmed = true,

                };
                username = result.PhoneNumber;
                password = RandomValue.ToString();

                user1.PasswordHash = password = ph.HashPassword(user1, RandomValue.ToString());
                var result1 = await _userManager.CreateAsync(user1);
                var ResultUserRole = await _userManager.AddToRoleAsync(user1, ConstantRoles.CustomerName);
                var SmsOtpCodeServices = await _SmsOtpCodeService.GetAllAsync(s => s.PhoneNumber == smsModel.PhoneNumber);
                foreach (var s in SmsOtpCodeServices)
                    _ = await _SmsOtpCodeService.DeleteAsync(s.ID);

                customer = newcustomer;

            }
            else 
            {

                var user = await _userManager.Users.FirstOrDefaultAsync(s => s.CustomerID == customer.ID);
                username = user.UserName; 
                var ph = new PasswordHasher<Account>();
                user.PasswordHash = ph.HashPassword(user, RandomValue.ToString());
                var changeState = await _userManager.UpdateAsync(user);
                password = user.PasswordHash;

            }
          
            LoginAccount credentials = new LoginAccount
                {
                    UserName = username,
                    Password= RandomValue.ToString(),
                    SecurityStamp=Guid.NewGuid().ToString(),   
                };
            var Result = await _authenticationManager.ValidateCredentials(credentials);
            if (Result==null)
                return NotFound(new ResponseApiEntity<ResultLoginAccount>
                                                             (entity: new ResultLoginAccount(),
                                                             statusCode: ResultMessageApi.ErrorCode,
                                                             status: ResultMessageApi.Error,
                                                             message: ResultMessageApi.LoginError));
            Result.Customer = customer;
            var Token = await _authenticationManager.CreateToken(Result,isRefreshToken: false);
            var RefreshToken = await _authenticationManager.CreateToken(Result,isRefreshToken: true);

            /// بخش ارسال رمز از طریق پیامک 
            //List<string> phonNumbers = new List<string>();
            //phonNumbers.Add(smsModel.PhoneNumber);

            //AddSmsOtpCode addSmsOtpCode = new AddSmsOtpCode();
            //var set = await _SettingService.GetAllAsync();
            //if (set.Count() > 0)
            //{
            //    var settings = set.ToList();
            //    SendSMSRequest smsRequest =
            //    new(settings[0].UserName,
            //    settings[0].Password,
            //    settings[0].PhoneSender,
            //    phonNumbers.ToArray(),
            //    $"محرمانه\r\nنام کاربری:{smsModel.PhoneNumber}\r\nرمز ورود:{RandomValue.ToString()}\r\n{settings[0].Name}",
            //    smsModel.IsFlash,
            //    smsModel.RecId,
            //    smsModel.Status);
            //    SendServiceClient client = new SendServiceClient();
            //    var sendSMSResponse = await client.SendSMSAsync(smsRequest);

            //    var SmsResponseMessage = SmsViewModel.GetSmsResponseMessage((SendSmsReturnType)sendSMSResponse.SendSMSResult);
            //    client.Close();
            //}

            ResultLoginAccount resultLoginAccount = new ResultLoginAccount
            {
                Token = Token,
                RefreshToken = RefreshToken,
                TokenExpired = DateTimeOffset.UtcNow.AddHours(2).ToUnixTimeSeconds(),
            };
            return Ok(new ResponseApiEntity<ResultLoginAccount>
                                                        (entity: resultLoginAccount,
                                                        statusCode: ResultMessageApi.SuccessCode,
                                                        status: ResultMessageApi.Success,
                                                        message: ResultMessageApi.LoginOk));
        }
  

    }
}
