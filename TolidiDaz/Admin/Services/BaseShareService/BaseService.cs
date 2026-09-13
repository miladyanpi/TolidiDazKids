using Admin.Base;
using Dto.DtoPaginagion;
using Dto.Models;

namespace Admin.Services.BaseShareService
{
    public class BaseService
    {
        public event Action? OnChange = null;
        public void NotifyStateChanged() => OnChange?.Invoke();
        public PagingModel pagingModel { get; set; } = new();
        public PaginationParams SetPaging(string? SearchText = "")
        {
            return new PaginationParams()
            {
                Page = (int)pagingModel.Page,
                SearchText = SearchText,
                Take = (int)pagingModel.Take,
            };
        }
        public void SetPerPage(int count)
        {
            pagingModel.CountPage = Math.Ceiling(count / pagingModel.Take);
            pagingModel.CountPagePer10 = Math.Ceiling(pagingModel.CountPage / 10);
        }
    }
}
