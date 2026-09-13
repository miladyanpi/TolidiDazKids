using Microsoft.JSInterop;

namespace PublicTolidiAyhan.Code
{
    public class ToastMessage: IToastMessage
    {
        private readonly IJSRuntime js;
        public ToastMessage(IJSRuntime jSRuntime)
        {
            js = jSRuntime;
        }
        public async Task CallAlertToast(string message)
        {
            try
            {
                GC.SuppressFinalize(this);
                await js.InvokeVoidAsync("CallAlertToast", message);
        }
            catch { }
           
        }
    }
}
