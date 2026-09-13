using Admin.Base;
using Admin.Services;
using Admin.Services.BaseShareService;
using Admin.Services.Category;
using Dto.Models.DtoProductFeature;
using Dto.Models.DtoPersonel;
using Dto.Models.ResponseApi;
using RestSharp;

namespace Admin.Services.Personel
{
    public class PersonelService(
        IRootApi<ResponseApiEntities<ResultPersonel>> _RootApiPersonel) : BaseService
    {

        public List<ResultPersonel>? ResultPersonels { get; set; } = new List<ResultPersonel>();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();
        
        public async Task GetData(int? PositionID)
        {
            var resdata = await _RootApiPersonel.RunMethodApi($"Personels/ByPositionID?PositionID={PositionID}", null, method: Method.Get);
            if (resdata != null)
            {
                ResultPersonels = resdata.Entities.ToList();
            }
            else
            {
                ResultPersonels = new();
            }
            NotifyStateChanged();

        }

    }
}
