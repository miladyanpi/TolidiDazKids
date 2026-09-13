

namespace Dto.Models.DtoPagination
{
    public class PaginationParamsModel
    {
        public string? SearchText { get; set; } = string.Empty;
        public int Page { get; set; } = 1;
        public int Take { get; set; } = 15;
    }
    public class PaginationParamsIDs
    {
        public List<int>? IDs { get; set; }
        public PaginationParamsModel Pagination { get; set; } 
    }
}
