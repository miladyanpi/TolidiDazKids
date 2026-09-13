
//using AdminPanel.Services;

namespace PublicTolidiAyhan.Base
{
    public class BaseCode
    {

        //private IHttpContextAccessor _httpContextAccessor;
        public BaseCode()
        {

        }

        public  static int GetCountShowRecord()
        {
            // var settings = await new IRootApi<ResponseApiEntities<ResultSetting>>.RunMethodApi($"Settings/All", null, method: Method.Get);
            return 15; //settings != null && settings.Entities.Count() > 0 ? settings.Entities.ToList()[0].CountShowRecord : 15;
        }
        public static string GetSearchText(string? txtSearch)
        {
            return txtSearch == null || txtSearch.Trim() == string.Empty ? string.Empty : $"&SearchText={txtSearch.Trim()}";
        }
        public static bool CheckStatusMeliCode(string? Mcode )
        {
            if(string.IsNullOrEmpty(Mcode)) return true;
            if ( Mcode.Length == 10)
            {
                if (Mcode == "1111111111" || Mcode == "2222222222" ||
                    Mcode == "3333333333" || Mcode == "4444444444" ||
                    Mcode == "5555555555" || Mcode == "6666666666" ||
                    Mcode == "7777777777" || Mcode == "8888888888" ||
                    Mcode == "9999999999")
                {
                    return false;
                }
                else
                {
                    var c = int.Parse(Mcode.Substring(9, 1));
                    var n = int.Parse(Mcode.Substring(0, 1)) * 10 +
                            int.Parse(Mcode.Substring(1, 1)) * 9 +
                            int.Parse(Mcode.Substring(2, 1)) * 8 +
                            int.Parse(Mcode.Substring(3, 1)) * 7 +
                            int.Parse(Mcode.Substring(4, 1)) * 6 +
                            int.Parse(Mcode.Substring(5, 1)) * 5 +
                            int.Parse(Mcode.Substring(6, 1)) * 4 +
                            int.Parse(Mcode.Substring(7, 1)) * 3 +
                            int.Parse(Mcode.Substring(8, 1)) * 2;
                    var r = n - (n / 11) * 11;
                    if ((r == 0 && r == c) || (r == 1 && c == 1) || (r > 1 && c == 11 - r))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
                return false;
        }
     
    }
}
