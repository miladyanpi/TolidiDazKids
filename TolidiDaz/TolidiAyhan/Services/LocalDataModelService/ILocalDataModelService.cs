namespace TolidiAyhan.Services.LocalDataModelService
{
    public interface ILocalDataModelService<TEntity> where TEntity : class
    {
        Task StoreLocalDataAsync(string Name, TEntity entity);
        Task<TEntity> GetLocalDataAsync(string Key);
        Task RemoveLocalDataAsync(string Key);
    }
}
