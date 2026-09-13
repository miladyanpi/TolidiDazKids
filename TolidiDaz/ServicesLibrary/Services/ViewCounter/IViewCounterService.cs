namespace ServicesLibrary.Services.ViewCounter
{
    public interface IViewCounterService
    {
        void RecordViewProduct(int productId, string userIdentifier);
        void RecordViewBlog(int blogId, string userIdentifier);
        Dictionary<int, int> GetAllPendingProductViews();
        Dictionary<int, int> GetAllPendingBlogViews();
        void ClearPendingProductViews(IEnumerable<int> productIds);
        void ClearPendingBlogViews(IEnumerable<int> blogIds);
    }
}
