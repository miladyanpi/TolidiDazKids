namespace Admin.Services.Search
{
    public class SearchService
    {
        public string? SearchText { get; set; }

        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();
        
        public void ClearSearchText()
        {
            SearchText=string.Empty;
        }
    }
}
