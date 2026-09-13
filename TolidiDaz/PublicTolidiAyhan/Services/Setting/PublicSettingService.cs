
using AutoMapper;
using Dto.Models.DtoFavoritUserProduct;
using Dto.Models.DtoProductFeature;
using Dto.Models.DtoSetting;
using Dto.Models.ResponseApi;
using PublicTolidiAyhan.Services;
using RestSharp;
using ServicesLibrary.Services.SettingSrv;

namespace PublicTolidiAyhan.Services
{
    public class PublicSettingService
    {
      private readonly ISettingService _SettingService;
        private readonly IMapper _mapper;

        public PublicSettingService(ISettingService SettingService, IMapper mapper)
        {
            _SettingService = SettingService;
            _mapper = mapper;
        }
        public ResultPublicSetting? ResultPublicSetting  =new ResultPublicSetting();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();
        private bool loaded = false;
        
        public async Task GetData()
        {
            if(!loaded)
            {
                var data = await _SettingService.GetAllAsync();
                var mappedSettings = _mapper.Map<ICollection<ResultPublicSetting>>(data);
                if (mappedSettings.Count() > 0)
                {
                    ResultPublicSetting = mappedSettings.FirstOrDefault();
                }
                else
                {
                    ResultPublicSetting = new();
                }
                loaded = true;
            }
            
            NotifyStateChanged();

        }
    }
}
