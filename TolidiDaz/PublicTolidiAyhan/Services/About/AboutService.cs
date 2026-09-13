using Dto.Models.Constant;
using Dto.Models.DtoAbout;
using Dto.Models.DtoPosition;
using Dto.Models.ResponseApi;
using RestSharp;

namespace PublicTolidiAyhan.Services
{
    public class AboutService
    {
       private readonly  IRootApi<ResponseApiEntities<UpdateAbout>> _RootApiUpdateAbouts;
        public AboutService(IRootApi<ResponseApiEntities<UpdateAbout>> RootApiUpdateAbouts)
        {
            _RootApiUpdateAbouts = RootApiUpdateAbouts;
        }
        private List<UpdateAbout> UpdateAbouts { get; set; } = new();
        public UpdateAbout? updateAbout { get; set; } = new()
        {
            ResultUploadFiles=new List<Dto.Models.DtoUploadFile.ResultUploadFile>()
        };
        public event Action? OnChange = null;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task GetListDate()
        {
            var resdata = await _RootApiUpdateAbouts.RunMethodApi($"Abouts/All", null, method: Method.Get);
            if (resdata != null && resdata.Status == ResultMessageApi.Success)
            {
                UpdateAbouts = resdata.Entities!.ToList();
                if (resdata.CountAllRecordTable > 0)
                {
                    updateAbout = UpdateAbouts[0];
                }
            }
            NotifyStateChanged();
        }

    }
}
