using Domain;
using Dto.Models.DtoAccount;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServicesLibrary.Services.AuthenticationManagerSrv
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private UserManager<Account> _userManager;
        private readonly IConfiguration _configuration;
        //private readonly IRefreshTokenEntityService _refreshTokenEntity;
        //private Account _user;


        public AuthenticationManager(
            UserManager<Account> userManager,
            //IRefreshTokenEntityService refreshTokenEntity,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
            //_refreshTokenEntity= refreshTokenEntity;
        }

        public async Task<string> CreateToken(Account user,bool isRefreshToken)
        {
            var jwtSettings = _configuration.GetSection("jwtSettings");
            var claims = await GetClaims(user);
            var secret=isRefreshToken? jwtSettings.GetSection("RefreshTokenSecret").Value:jwtSettings.GetSection("secretKey").Value ;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("validIssure").Value,
                audience: jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires: DateTime.Now.AddHours(isRefreshToken?24:2),
                signingCredentials: signingCredentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<List<Claim>> GetClaims(Account _user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.Customer!=null? _user.Customer.Name+" "+_user.Customer.LastName:"مدیر سایت"),
                new Claim(ClaimTypes.UserData, _user.UserName),
                new Claim(ClaimTypes.PrimarySid,_user.CustomerID.ToString()),
                new Claim(ClaimTypes.NameIdentifier,_user.Customer.IdentityCode.ToString()),
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        public async Task<Account?> ValidateCredentials(LoginAccount credentials)
        {
            var _user = await _userManager.FindByNameAsync(credentials.UserName);
            var result = _user != null && await _userManager.CheckPasswordAsync(_user, credentials.Password);
         
            if (result)
            {
                return _user;
            }

            return null;
        }
        public async Task<bool> CheckTokenIsValid(string token)
        {
            JwtSecurityToken jwtSecurityToken;
            try
            {
                jwtSecurityToken = new JwtSecurityToken(token);
            }
            catch (Exception)
            {
                return false;
            }

            return jwtSecurityToken.ValidTo > DateTime.UtcNow;
        }

        
    }
}
