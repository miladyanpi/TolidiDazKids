using CurrieTechnologies.Razor.SweetAlert2;
using Dto.Models.Constant;
using Dto.Models.DtoCustomer;
using Dto.Models.ResponseApi;
using RestSharp;
namespace PublicTolidiAyhan.Services.Customer
{
    public class CustomerPublicService(IRootApi<ResponseApiEntity<UpdateCustomerInfo>> _RootApiGet, SweetAlertService Swal)
    {
        public UpdateCustomerInfo? updateCustomerInfo { get; set; } = new ();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task GetDateInfo()
        {
            var resdata = await _RootApiGet.RunMethodApi($"Customers/UpdateCustomerInfo/Public", null, method: Method.Get);
            if (resdata != null && resdata.Status == ResultMessageApi.Success)
            {
                updateCustomerInfo = resdata.Entity;
            }
            NotifyStateChanged();
        }
        public async Task Update()
        {
            var resdata = await _RootApiGet.RunMethodApi("Customers/UpdateCustomerInfo/Public", updateCustomerInfo, method: Method.Patch);
            if (resdata.Status == ResultMessageApi.Success)
            {
                var result2 = await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "پیام",
                    Text = resdata.Message,
                    Icon = resdata.Status,
                    ShowConfirmButton = true,
                });

            }
            else
            {
                var result2 = await Swal.FireAsync(new SweetAlertOptions
                {
                    Title = "پیام",
                    Text = resdata.Message,
                    Icon = resdata.Status,
                    ShowConfirmButton = true,
                });
            }


        }
    }
}
