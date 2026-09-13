namespace Admin.Services.Search
{
    public class DisableService
    {
        public bool Disabled = false;

        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();
        public void SetDisabled(bool statuse = false)
        {
            Disabled = statuse;
            NotifyStateChanged();
        }
    }
}
