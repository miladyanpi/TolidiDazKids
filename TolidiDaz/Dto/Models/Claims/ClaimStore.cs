using System.Security.Claims;

namespace Dto.Models.Claims
{
    public static class ClaimStore
    {
        public static List<Claim> AllClaims=new List<Claim>()
        {
            new Claim(ClaimTypeStore.Add,true.ToString()),
            new Claim(ClaimTypeStore.Delete,true.ToString()),
            new Claim(ClaimTypeStore.Update,true.ToString()),
            new Claim(ClaimTypeStore.List,true.ToString()),

        };
    }
    public class ClaimTypeStore
    {
        public const string Add = "Add";
        public const string List = "List";
        public const string Delete = "Delete";
        public const string Update = "Update";

    }
}
