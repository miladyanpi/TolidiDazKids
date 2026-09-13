using Admin.Base;
using Admin.Services;
using Admin.Services.BaseShareService;
using Dto.Models.DtoProduct_CountAction_CostType;
using Dto.Models.ResponseApi;
using RestSharp;

namespace Admin.Services.Product_CountAction_CostType
{
    public class Product_CountAction_CostTypeService(
        IRootApi<ResponseApiEntity<ResultProduct_CountAction_CostType>> _RootApiProduct_CountAction_CostType) : BaseService
    {

        public ResultProduct_CountAction_CostType? ResultProduct_CountAction_CostType { get; set; } = new ResultProduct_CountAction_CostType();
        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();
        
        public async Task GetData(int? PositionID, int? ProductID)
        {
            var resdata = await _RootApiProduct_CountAction_CostType.RunMethodApi($"Product_CountAction_CostTypes/By?PositionID={PositionID}&ProductID={ProductID}", null, method: Method.Get);
            if (resdata != null)
            {
                ResultProduct_CountAction_CostType = resdata.Entity;
            }
            else
            {
                ResultProduct_CountAction_CostType = new();
            }
            NotifyStateChanged();

        }

    }
}
