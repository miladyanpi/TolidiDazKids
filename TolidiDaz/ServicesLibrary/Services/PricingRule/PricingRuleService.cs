using DAL.Context;
using Domain;
using Dto.Models.DtoProduct;
using Utility;
using static Dto.Enum.EnumConstant;

namespace ServicesLibrary.Services.PricingRuleSrv
{
    public sealed class PricingRuleService : Repository<PricingRule>, IPricingRuleService
    {
        public PricingRuleService(IUnitOfWork unitOfWork) : base(unitOfWork)
        {

        }
        public event Action? OnChange;
        private void Notify() => OnChange?.Invoke();

        //public Int64 Price { get; set; } = 0;

        public Int64 CalculatePrice(ResultProduct product,int ProductCountInCount,List<string> Roles,bool isLogged)
        {
            List<(Int64 Price, string Message)> Results = new List<(Int64, string)>();
            int todayDate = DateFunctions.GetDateNow();
            var rules = product.ResultPricingRules;
            //if (isLogged)
            //{
            //    // 1️⃣ اولویت: تعداد + نقش
            //    var rule1 = rules
            //        .Where(r => r.RuleType2 == (int)RuleType.QuantityAndGroup
            //                 && r.RoleID == ConstantRoles.GetRoleIdByRoleName(RoleName))
            //        .OrderByDescending(r => r.MinQuantity)
            //        .FirstOrDefault();

            //    if (rule1 != null)
            //        return rule1.Price;

            //    // 2️⃣ نقش کاربر
            //    var rule2 = rules
            //        .FirstOrDefault(r =>
            //            r.RuleType2 == (int)RuleType.Group &&
            //            r.RoleID == ConstantRoles.GetRoleIdByRoleName(RoleName));

            //    if (rule2 != null)
            //        return rule2.Price;
            //}

            // 3️⃣ تعداد خرید
            if(rules == null)
                    return product.Price;
            var ResulrRuls1 = rules
                .Where(r =>
                    r.RuleType2 == (int)RuleType.Quantity &&
                    ProductCountInCount >= r.MinQuantity)
                .OrderByDescending(r => r.MinQuantity)
                .FirstOrDefault();


            if (ResulrRuls1 != null)
                return ResulrRuls1.Price;
            //// 4️⃣ تاریخ
            //rule = rules
            //    .FirstOrDefault(r =>
            //        r.RuleType2 == (int)RuleType.Date &&
            //        DateFunctions.ConvertDateStringToInt(r.FromDate) <= todayDate &&
            //        DateFunctions.ConvertDateStringToInt(r.ToDate) >= todayDate);

            //if (rule != null)
            //    return rule.Price;
            //// 5️⃣ قیمت پایه
            Notify();
            return product.Price;
        }
        public List<(Int64 Price, string Message)> GetAllPrice(ResultProduct product,int ProductCountInCount, List<string> Roles, bool isLogged)
        {
            List<(Int64 Price, string Message)> Results = new List<(Int64, string)>();
            int todayDate = DateFunctions.GetDateNow();
            var rules = product.ResultPricingRules;
            //if (isLogged)
            //{
            //    // 1️⃣ اولویت: تعداد + نقش
            //    var rule1 = rules
            //        .Where(r => r.RuleType2 == (int)RuleType.QuantityAndGroup
            //                 && r.RoleID == ConstantRoles.GetRoleIdByRoleName(RoleName))
            //        .OrderByDescending(r => r.MinQuantity)
            //        .FirstOrDefault();

            //    if (rule1 != null)
            //        return rule1.Price;

            //    // 2️⃣ نقش کاربر
            //    var rule2 = rules
            //        .FirstOrDefault(r =>
            //            r.RuleType2 == (int)RuleType.Group &&
            //            r.RoleID == ConstantRoles.GetRoleIdByRoleName(RoleName));

            //    if (rule2 != null)
            //        return rule2.Price;
            //}

            // 3️⃣ تعداد خرید
            var ResulrRuls1 = rules
                 .Where(r =>
                     r.RuleType2 == (int)RuleType.Quantity)
                 .OrderByDescending(r => r.MinQuantity)
                 .ToList();



            foreach (var item in ResulrRuls1)
            {
                Results.Add((item.Price, $"خرید حداقل: {item.MinQuantity} عدد"));
            }
            if (Results.Count > 0)
                return Results;

            //// 4️⃣ تاریخ
            //rule = rules
            //    .FirstOrDefault(r =>
            //        r.RuleType2 == (int)RuleType.Date &&
            //        DateFunctions.ConvertDateStringToInt(r.FromDate) <= todayDate &&
            //        DateFunctions.ConvertDateStringToInt(r.ToDate) >= todayDate);

            //if (rule != null)
            //    return rule.Price;
            //// 5️⃣ قیمت پایه
            Notify();
            return Results;
        }

    }
}