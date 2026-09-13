using Dto.Models.Constant;
using Dto.Models.DtoTeam;
using Dto.Models.ResponseApi;
using RestSharp;

namespace PublicTolidiAyhan.Services
{
    public class TeamService
    {
        IRootApi<ResponseApiEntities<ResultTeam>> _RootApiResultTeams;
        public TeamService(IRootApi<ResponseApiEntities<ResultTeam>> RootApiUpdateTeams)
        {
            _RootApiResultTeams = RootApiUpdateTeams;
        }
        public List<ResultTeam> ResultTeams { get; set; } = new();

        public event Action? OnChange = null;

        private void NotifyStateChanged() => OnChange?.Invoke();

        public async Task GetListDate()
        {
            var resdata = await _RootApiResultTeams.RunMethodApi($"Teams/All", null, method: Method.Get);
            if (resdata != null && resdata.Status == ResultMessageApi.Success)
            {
                ResultTeams = resdata.Entities.ToList();
            }
            NotifyStateChanged();
        }
    }
    }
