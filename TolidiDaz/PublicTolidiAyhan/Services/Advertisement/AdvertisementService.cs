using Dto.Models.Constant;
using Dto.Models.DtoAbout;
using Dto.Models.DtoAdvertisement;
using Dto.Models.ResponseApi;
using RestSharp;

namespace PublicTolidiAyhan.Services
{
    public class AdvertisementService
    {

        private readonly IRootApi<ResponseApiEntities<ResultAdvertisement>> _RootApiGet;
        public AdvertisementService(IRootApi<ResponseApiEntities<ResultAdvertisement>> RootApiGet)
        {
            _RootApiGet = RootApiGet;
        }
        public List<ResultAdvertisement>? ResultAdvertisements { get; set; } = new List<ResultAdvertisement>();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task GetListDate()
        {
            var resdata = await _RootApiGet.RunMethodApi($"Advertisements/Public/All", null, method: Method.Get);
            if (resdata != null && resdata.Status == ResultMessageApi.Success)
            {
                ResultAdvertisements = resdata.Entities!.ToList();

            }
            NotifyStateChanged();
        }

    }
}
