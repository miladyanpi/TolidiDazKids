using Admin.Base;
using Admin.Services;
using Admin.Services.BaseShareService;
using Admin.Services.Category;
using CurrieTechnologies.Razor.SweetAlert2;
using Dto.Models.Constant;
using Dto.Models.DtoPersonel;
using Dto.Models.DtoProductFeature;
using Dto.Models.ResponseApi;
using RestSharp;

namespace Admin.Services.Personel
{
    public class PersonelService(
        IRootApi<ResponseApiEntities<ResultPersonel>> _RootApiPersonel, SweetAlertService Swal) : BaseService
    {

        public List<ResultPersonel>? ResultPersonels { get; set; } = new List<ResultPersonel>();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();
        
        public async Task GetData(int? PositionID)
        {
            try
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
            catch (Exception ex)
            {
                var result2 = await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "پیام",
                    Text = ex.ToString(),
                    Icon = ResultMessageApi.Error,
                    ShowConfirmButton = true,
                });
            }
        }

    }
}
