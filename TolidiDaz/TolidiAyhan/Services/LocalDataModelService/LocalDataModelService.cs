
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Newtonsoft.Json;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TolidiAyhan.Services.LocalDataModelService
{
    public class LocalDataModelService<TEntity> : ILocalDataModelService<TEntity> where TEntity : class
    {
        private readonly ProtectedLocalStorage _protectedLocalStorage;

        public LocalDataModelService(ProtectedLocalStorage protectedLocalStorage)
        {
            _protectedLocalStorage = protectedLocalStorage;
        }
        public async Task<TEntity> GetLocalDataAsync(string Key)
        {
            var content = await _protectedLocalStorage.GetAsync<string>(Key);
            var data= content.Success ? content.Value : null;
            return JsonConvert.DeserializeObject<TEntity>(data)!;
        }

        public async Task RemoveLocalDataAsync(string Key)
        {
            await _protectedLocalStorage.DeleteAsync(Key);
        }
        public async Task StoreLocalDataAsync(string Name, TEntity entity)
        {
             await _protectedLocalStorage.SetAsync(Name, entity);
        }
    }
}
