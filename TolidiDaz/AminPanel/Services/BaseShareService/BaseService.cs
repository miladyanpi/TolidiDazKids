using AdminPanel.Base;
using AminPanel.Services.ProductFeature;
using Dto.Models;

namespace AminPanel.Services.BaseShareService
{
    public class BaseService
    {
        public PagingModel pagingModel { get; set; } = new();
        public int Count = 0;
        public BaseService()
        {

            pagingModel.Page = 1;
            pagingModel.Take = Base.GetCountShowRecord();
        }
    }
}
