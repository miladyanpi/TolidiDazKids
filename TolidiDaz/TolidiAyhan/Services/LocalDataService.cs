//using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

//namespace TolidiAyhan.Services
//{
//    public class LocalDataService : ILocalDataService
//    {
//        private readonly ProtectedLocalStorage _protectedLocalStorage;

//        public LocalDataService(ProtectedLocalStorage protectedLocalStorage)
//        {
//            _protectedLocalStorage = protectedLocalStorage;
//        }

//        public async Task StoreLocalDataAsync(string data)
//        {
//            await _protectedLocalStorage.SetAsync("token", data);

//        }

//        public async Task<string> GetLocalDataAsync()
//        {
//            var result = await _protectedLocalStorage.GetAsync<string>("token");
//            return result.Success ? result.Value : null;
//        }

//        public async Task RemoveLocalDataAsync(string key)
//        {
//            await _protectedLocalStorage.DeleteAsync(key);
//        }
//    }
//}
