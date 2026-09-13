namespace Dto.Models.DtoPaginagion
{
    public class PaginationMetaData
    {
        public PaginationMetaData(int totalCount,int currentPage, int itemsPerPage)
        {
            TotalCount = totalCount;
            CurrentPage = currentPage;
            TotalPages = (int)Math.Ceiling(totalCount / (double)itemsPerPage);
        }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPreviuos => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

    }
}
