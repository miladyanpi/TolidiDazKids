using DAL.Context;
using Domain;
using Dto.Models.DtoProduct;

namespace ServicesLibrary.Services.PricingRuleSrv
{
    public interface IPricingRuleService : IRepository<PricingRule>
    {
        Int64 CalculatePrice(ResultProduct product, int ProductCountInCount, List<string> Roles, bool isLogged);
        List<(Int64 Price, string Message)> GetAllPrice(ResultProduct product, int ProductCountInCount, List<string> Roles, bool isLogged);
    }
}
