using CurrieTechnologies.Razor.SweetAlert2;
using Dto.Enum;
using Dto.Models.Constant;
using Dto.Models.DtoAccount;
using Dto.Models.DtoPricingRule;
using Dto.Models.ResponseApi;
using Microsoft.AspNetCore.Components;
using RestSharp;
using static Dto.Enum.EnumConstant;

namespace Admin.Services.PricingRule
{
    public class PricingRuleService(IRootApi<ResponseApiEntity<AddPricingRule>> RootApiAddPricingRule)
    {


        public AddPricingRule? addPricingRule { get; set; } = new() { Visible = true };
        public List<(int? ID, string Title)> RoleTypes = new List<(int?, string)>();
        public List<ConstantRolesModel> ResultRoles = new List<ConstantRolesModel>();

        public event Action? OnChange = null;
        private void NotifyStateChanged() => OnChange?.Invoke();
        public void GetDate()
        {
            RoleTypes = EnumConstant.GetListRuleType();
            addPricingRule.RuleType = RoleTypes[0].ID;

            ResultRoles = ConstantRoles.GetConstantCustomerRoles();
            addPricingRule.RoleID = ResultRoles.Count > 0 ? ResultRoles[0].ID : null;
        }
        public async Task<ResponseApiEntity<AddPricingRule>> Add()
        {
            switch (addPricingRule.RuleType)
            {
                case (int)RuleType.Quantity:
                    addPricingRule.RoleID = null;
                    addPricingRule.FromDate = null;
                    addPricingRule.ToDate = null;
                    
                    break;
                case (int)RuleType.Group:
                    addPricingRule.FromDate = null;
                    addPricingRule.ToDate = null;
                    break;
                case (int)RuleType.QuantityAndGroup:
                    addPricingRule.FromDate = null;
                    addPricingRule.ToDate = null;
                    break;
                case (int)RuleType.Date:
                    addPricingRule.RoleID = null;
                    addPricingRule.MinQuantity = 1;
                    break;
                default:
                    break;
            }
            return await RootApiAddPricingRule.RunMethodApi("PricingRules", addPricingRule, method: Method.Post);
        }
    }
}
