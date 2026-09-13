namespace AdminPanel.Services
{
    public interface IDeleteFileService
    {
        public  Task DeleteFile(int ID, string endPoint);
    }
}
