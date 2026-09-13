using System.Collections.Concurrent;

namespace ServicesLibrary.Services.ViewCounter
{
    public class InMemoryViewCounterService : IViewCounterService
    {
        private readonly ConcurrentDictionary<int, int> _viewProductCounts = new();
        private readonly ConcurrentDictionary<int, int> _viewBlogCounts = new();
        private readonly ConcurrentDictionary<string, DateTime> _userViewProductTracker = new();
        private readonly ConcurrentDictionary<string, DateTime> _userViewBlogTracker = new();
        private DateTime _lastProductCleanup = DateTime.UtcNow;
        private DateTime _lastBlogCleanup = DateTime.UtcNow;
        public void RecordViewProduct(int productId, string userIdentifier)
        {
            string key = $"{productId}:{userIdentifier}";

            var now = DateTime.UtcNow;

            if (_userViewProductTracker.TryAdd(key, now))
            {
                _viewProductCounts.AddOrUpdate(productId, 1, (_, count) => count + 1);
            }
            else
            {
                if ((now - _userViewProductTracker[key]).TotalMinutes >= 10)
                {
                    _userViewProductTracker[key] = now;
                    _viewProductCounts.AddOrUpdate(productId, 1, (_, count) => count + 1);
                }
            }

            if ((DateTime.UtcNow - _lastProductCleanup).TotalMinutes > 5)
            {
                CleanupOldProductTrackers();
                _lastProductCleanup = DateTime.UtcNow;
            }
        }
        public void RecordViewBlog(int blogId, string userIdentifier)
        {
            string key = $"{blogId}:{userIdentifier}";

            var now = DateTime.UtcNow;

            if (_userViewBlogTracker.TryAdd(key, now))
            {
                _viewBlogCounts.AddOrUpdate(blogId, 1, (_, count) => count + 1);
            }
            else
            {
                if ((now - _userViewBlogTracker[key]).TotalMinutes >= 10)
                {
                    _userViewBlogTracker[key] = now;
                    _viewBlogCounts.AddOrUpdate(blogId, 1, (_, count) => count + 1);
                }
            }

            if ((DateTime.UtcNow - _lastBlogCleanup).TotalMinutes > 5)
            {
                CleanupOldBlogTrackers();
                _lastBlogCleanup = DateTime.UtcNow;
            }
        }

        private void CleanupOldProductTrackers()
        {
            var now = DateTime.UtcNow;
            var oldKeys = _userViewProductTracker.Where(x => (now - x.Value).TotalMinutes > 30)
                                         .Select(x => x.Key)
                                         .ToList();

            foreach (var key in oldKeys)
                _userViewProductTracker.TryRemove(key, out _);
        }
        private void CleanupOldBlogTrackers()
        {
            var now = DateTime.UtcNow;
            var oldKeys = _userViewBlogTracker.Where(x => (now - x.Value).TotalMinutes > 30)
                                         .Select(x => x.Key)
                                         .ToList();

            foreach (var key in oldKeys)
                _userViewBlogTracker.TryRemove(key, out _);
        }
        public Dictionary<int, int> GetAllPendingProductViews()
        {
            return _viewProductCounts.ToDictionary(x => x.Key, x => x.Value);
        }

        public void ClearPendingProductViews(IEnumerable<int> productIds)
        {
            foreach (var id in productIds)
            {
                _viewProductCounts.TryRemove(id, out _);
            }
        }

        public Dictionary<int, int> GetAllPendingBlogViews()
        {
            return _viewBlogCounts.ToDictionary(x => x.Key, x => x.Value);
        }

        public void ClearPendingBlogViews(IEnumerable<int> blogIds)
        {
            foreach (var id in blogIds)
            {
                _viewBlogCounts.TryRemove(id, out _);
            }
        }
    }
}
