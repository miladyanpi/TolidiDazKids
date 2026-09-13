namespace AdminPanel.Code
{
    public interface IToastMessage
    {
        Task CallAlertToast(string message);
    }
}
