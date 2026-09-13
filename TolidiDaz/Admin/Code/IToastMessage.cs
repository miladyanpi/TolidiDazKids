namespace Admin.Code
{
    public interface IToastMessage
    {
        Task CallAlertToast(string message);
    }
}
