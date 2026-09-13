namespace AdminPanel.Services
{
    public interface IAlert
    {
         Task ShowAlert(string Tilte="پیام" ,string message="",string statuse="", bool showConfirmButton = false);
    }
}
