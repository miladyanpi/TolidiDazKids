using DAL.Context;
using Microsoft.EntityFrameworkCore;
using ServicesLibrary.Services.BlogSrv;
using ServicesLibrary.Services.ProductSrv;
using System;

namespace ServicesLibrary.Services.ViewCounter
{
    public class SyncProductViewsJob
    {
        private readonly IViewCounterService _viewCounterService;
        private readonly IProductService _ProductService;
        private readonly IBlogService _BlogService;

        public SyncProductViewsJob(
            IViewCounterService viewCounterService,
            IProductService ProductService,
            IBlogService BlogService)
        {
            _viewCounterService = viewCounterService;
            _ProductService = ProductService;
            _BlogService= BlogService;
        }

        public async Task ExecuteProductAsync()
        {
            var pending = _viewCounterService.GetAllPendingProductViews();
            if (!pending.Any()) return;

            try
            {
                // یک Query واحد برای همه محصولات
                var list = (await _ProductService.GetAllAsync(p => pending.Keys.Contains(p.ID))).ToList();
                for (int i= 0; i < list.Count();i++)
                {
                    list[i].ViewCount = list[i].ViewCount + pending[list[i].ID];
                }

                if (list.Any())
                   await _ProductService.UpdateRangeAsync(list);

                _viewCounterService.ClearPendingProductViews(pending.Keys);
            }
            catch (Exception ex)
            {
                // در صورت خطا، بازدیدها را پاک نکنید تا دفع بعدی امتحان شود
                Console.WriteLine($"Error syncing Product views: {ex.Message}");
            }
        }
        public async Task ExecuteBlogAsync()
        {
            var pending = _viewCounterService.GetAllPendingBlogViews();
            if (!pending.Any()) return;

            try
            {
                // یک Query واحد برای همه محصولات
                var list = (await _BlogService.GetAllAsync(p => pending.Keys.Contains(p.ID))).ToList();
                for (int i = 0; i < list.Count(); i++)
                {
                    list[i].NumberOfVisits = list[i].NumberOfVisits + pending[list[i].ID];
                }

                if (list.Any())
                    await _BlogService.UpdateRangeAsync(list);

                _viewCounterService.ClearPendingBlogViews(pending.Keys);
            }
            catch (Exception ex)
            {
                // در صورت خطا، بازدیدها را پاک نکنید تا دفع بعدی امتحان شود
                Console.WriteLine($"Error syncing Blog views: {ex.Message}");
            }
        }
    }
}
