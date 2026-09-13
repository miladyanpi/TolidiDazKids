namespace DAL.Paginagion
{

    public class PaginationParams
    {
        public string? SearchText { get; set; } = string.Empty;
        private const int _maxTake = 50;
        private int _take=15;
        public int Page { get; set; } = 1;
        public int Take
        {
            get => _take;
            set => _take = value > _maxTake ? _maxTake : value;
        }

    }
    public class IDParams
    {
        public List<int>? IDS { get; set; }
    }

   
}
