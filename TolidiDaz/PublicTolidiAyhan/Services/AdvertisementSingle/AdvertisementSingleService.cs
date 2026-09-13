using Dto.Models.Constant;
using Dto.Models.DtoAbout;
using Dto.Models.DtoAdvertisementSingle;
using Dto.Models.ResponseApi;
using RestSharp;

namespace PublicTolidiAyhan.Services
{
    public class AdvertisementSingleService
    {

        private readonly IRootApi<ResponseApiEntities<ResultAdvertisementSingle>> _RootApiGet;
        public AdvertisementSingleService(IRootApi<ResponseApiEntities<ResultAdvertisementSingle>> RootApiGet)
        {
            _RootApiGet = RootApiGet;
        }
        public List<ResultAdvertisementSingle>? ResultAdvertisementSingles { get; set; } = new List<ResultAdvertisementSingle>();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task GetListDate()
        {
            var resdata = await _RootApiGet.RunMethodApi($"AdvertisementSingles/Public/All", null, method: Method.Get);
            if (resdata != null && resdata.Status == ResultMessageApi.Success)
            {
                ResultAdvertisementSingles = resdata.Entities!.ToList();

            }
            NotifyStateChanged();
        }

    }
}
