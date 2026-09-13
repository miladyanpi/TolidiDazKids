//using Blazored.LocalStorage;
//using Microsoft.Extensions.Caching.Distributed;
//using System;


//using Blazored.LocalStorage;

//namespace TolidiAyhan.Services
//{
//    public class TokenService
//    {
//        private readonly ILocalStorageService _localStorage;
//        private const string TokenKey = "jwt_token_public";

//        public TokenService(ILocalStorageService localStorage)
//        {
//            _localStorage = localStorage;
//        }

//        public async Task SetToken(string token)
//        {
//            await _localStorage.SetItemAsStringAsync(TokenKey, token);
//        }

//        public async Task<string?> GetToken()
//        {
//            try
//            {
//                bool exists = await _localStorage.ContainKeyAsync(TokenKey);

//                if (exists)
//                {
//                    return await _localStorage.GetItemAsStringAsync(TokenKey);
//                }
//            }
//            catch
//            {
//                // اجرای سمت سرور → JSInterop در دسترس نیست
//                return string.Empty;
//            }

//            return string.Empty;
//        }

//        public async Task ClearToken()
//        {
//            await _localStorage.RemoveItemAsync(TokenKey);
//        }
//    }
//}
