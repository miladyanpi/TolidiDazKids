using Admin.Services;
using Dto.Models.DtoPosition;
using Dto.Models.DtoProduct;
using Dto.Models.DtoProductFeature;
using Dto.Models.ResponseApi;
using RestSharp;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Admin.Services.Position
{
    public class PositionService
        (IRootApi<ResponseApiEntities<ResultPosition>> _RootApiResultPositions)
    {

        public AddPosition? addPosition { get; set; } = new() { Visible = true };
        public List<ResultPosition> ResultPositions { get; set; } = new();
        public event Action? OnChange = null;

        private void NotifyStateChanged() => OnChange?.Invoke();
      
        public async Task GetListDate(int? parentId = null)
        {
            var data = await _RootApiResultPositions.RunMethodApi($"Positions/All", null, method: Method.Get);
            ResultPositions= data.Entities.ToList();
            NotifyStateChanged();
        }

    }
}
