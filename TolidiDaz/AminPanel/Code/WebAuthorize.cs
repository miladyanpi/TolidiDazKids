//using AdminPanel.Models.Const;
//using AdminPanel.Services;
//using Blazored.LocalStorage;
//using Dto.Models.Constant;
//using Dto.Models.DtoAccount;
//using Dto.Models.ResponseApi;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Mvc.Filters;
//using RestSharp;
//using System.Security.Authentication;

//namespace AdminPanel.Code
//{
//    public class WebAuthorize : IWebAuthorize
//    {
//       readonly private ILocalStorageService _;
//       readonly private NavigationManager _NavManager;

//        public  WebAuthorize(ILocalStorageService , NavigationManager NavManager)
//        {
//            _ = ;
//            _NavManager = NavManager;
//        }
//       public async Task Checktoken()
//        {
//            string stringToken = await _.GetItemAsStringAsync("token");
//            var Login = await (new IRootApi<ResponseApiEntity<ResultExpiredToken>>(_,httpContextAccessor)).RunMethodApi($"auth/CheckExpiredToken?Token={stringToken}", null, method: Method.Get);
//            if (Login.Status == ResultMessageApi.Error)
//            {
//                _NavManager.NavigateTo($"/login");
               
//            }
//        }

      
//    }
//}
