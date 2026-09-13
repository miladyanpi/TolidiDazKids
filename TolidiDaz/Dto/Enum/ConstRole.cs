namespace Dto.Enum
{
    //public class AccountClaimTypes
    //{
    //    public static string NameAndLastName = "NameAndLastName";
    //    public static string CustomerID = "CustomerID";
    //    public static string UserName = "UserName";
    //    public static string Role = "Role";
    //}
    public  class ConstantRolesModel
    {
        public string? ID { get; set; }
        public string? Name { get; set; }
        public string? Title { get; set; }
    }
    
    public static class ConstantRoles
    {
        public static List<ConstantRolesModel> GetConstantAllRoles()
        {
            var list=new List<ConstantRolesModel>();
            list.Add(new ConstantRolesModel
            {
                ID= SuperAdminId,
                Name= SuperAdminName,
                Title= SuperAdminTitle,

            });
            list.Add(new ConstantRolesModel
            {
                ID = AdminId,
                Name = AdminName,
                Title= AdminTitle,
            });
            list.Add(new ConstantRolesModel
            {
                ID = BranchStoreId,
                Name = BranchStoreName,
                Title= BranchStoreTitle,
            });
            list.Add(new ConstantRolesModel
            {
                ID = CustomerId,
                Name = CustomerName,
                Title= CustomerTitle,
            });
            return list;
        }
        public static List<ConstantRolesModel> GetConstantSuperRoles()
        {
            var list = new List<ConstantRolesModel>();
            list.Add(new ConstantRolesModel
            {
                ID = SuperAdminId,
                Name = SuperAdminName,
                Title = SuperAdminTitle,

            });
            list.Add(new ConstantRolesModel
            {
                ID = AdminId,
                Name = AdminName,
                Title = AdminTitle,
            });
            return list;
        }
        public static List<ConstantRolesModel> GetConstantCustomerRoles()
        {
            var list = new List<ConstantRolesModel>();

            list.Add(new ConstantRolesModel
            {
                ID = BranchStoreId,
                Name = BranchStoreName,
                Title = BranchStoreTitle,
            });
            list.Add(new ConstantRolesModel
            {
                ID = CustomerId,
                Name = CustomerName,
                Title = CustomerTitle,
            });

            return list;
        }

        public static string GetRoleTitle(string roleName)
        {
            switch (roleName)
            {
                case SuperAdminName: return SuperAdminTitle;
                case AdminName: return AdminTitle;
                case BranchStoreName: return BranchStoreTitle;
                case CustomerName: return CustomerTitle;
                default:
                    return "-";

            }
        }
        public static string GetRoleIdByRoleName(string roleName)
        {
            switch (roleName)
            {
                case SuperAdminName: return SuperAdminId;
                case AdminName: return AdminId;
                case BranchStoreName: return BranchStoreId;
                case CustomerName: return CustomerId;
                default:
                    return "-";

            }
        }
        public static string GetRoleTitleByRoleId(string roleId)
        {
            switch (roleId)
            {
                case SuperAdminId: return SuperAdminTitle;
                case AdminId: return AdminTitle;
                case BranchStoreId: return BranchStoreTitle;
                case CustomerId: return CustomerTitle;
                default:
                    return "-";

            }
        }
        public const string SuperAdminTitle = "مدیر ارشد";
        public const string AdminTitle = "مدیر";
        public const string BranchStoreTitle = "نماینده فروش";
        public const string CustomerTitle = "مشتری عادی";


        public const string SuperAdminName = "SuperAdmin";
        public const string AdminName = "Admin";
        public const string BranchStoreName = "BranchStore";
        public const string CustomerName = "Customer";

        public const string SuperAdminId = "E5E4F4CC-935A-4074-8EDC-B3192CE7E455";
        public const string AdminId = "35FD6F25-D91F-4616-B9B9-D874A361D694";
        public const string BranchStoreId = "B9A2C137-0F4B-496E-8421-24D48FC6BEF0";
        public const string CustomerId = "425B21D0-42CF-4E75-A3F7-BE0AF587060B";
    }
}
