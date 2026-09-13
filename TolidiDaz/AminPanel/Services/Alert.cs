using CurrieTechnologies.Razor.SweetAlert2;

namespace AdminPanel.Services
{
    public class Alert : IAlert
    {
        private readonly SweetAlertService Swal;
        public Alert(SweetAlertService sweetAlertService)
        {
            Swal = sweetAlertService;
        }
        public async Task ShowAlert(string Title="پیام",string message="", string statuse="",bool showConfirmButton=false)
        {
            var result2 = await Swal.FireAsync(new SweetAlertOptions
            {
                Title = Title,
                Text = message,
                Icon = statuse,
                ShowConfirmButton = showConfirmButton,
            });
        }

       
    }
}
