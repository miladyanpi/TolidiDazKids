

using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.JSInterop;

namespace TolidiAyhan.Code
{
    public class Methods
    {
      
        //public List<SelectListItem> GetYears(ResponseApiEntities<ResultYear> responseApiEntities, int? ID = -1)
        //{
        //    List<SelectListItem> ListYears = new List<SelectListItem>();
        //    foreach (var item in responseApiEntities.Entities)
        //    {
        //        var sel = new SelectListItem(item.Title, item.ID.ToString());
        //        sel.Selected = item.ID == ID ? true : false;
        //        ListYears.Add(sel);
        //    }
        //    return ListYears;
        //}
       
       
        public static string CheckStatusMeliCode(string Mcode = "0")
        {
            if (Mcode != null && Mcode.Length == 10)
            {
                if (Mcode == "1111111111" || Mcode == "2222222222" ||
                    Mcode == "3333333333" || Mcode == "4444444444" ||
                    Mcode == "5555555555" || Mcode == "6666666666" ||
                    Mcode == "7777777777" || Mcode == "8888888888" ||
                    Mcode == "9999999999")
                {
                    return "error";
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
                        return "success";
                    }
                    else
                    {
                        return "error";
                    }
                }
            }
            else
                return "error";
        }
        public static string GetSearchText(string? txtSearch)
        {
            return txtSearch == null || txtSearch.Trim() == string.Empty ? string.Empty : $"&SearchText={txtSearch.Trim()}";
        }
        public Int64 GetRoundPrice(double Price)
        {
            Int64 T = 1000;
            Price = double.Parse(Price.ToString("N0"));
            string _Number = Price.ToString("N0");
            switch (_Number.Length)
            {
                case 3:
                    T = 100;
                    break;
                case 2:
                    T = 10;
                    break;
                case 1:
                    T = 1;
                    break;
            }
            var result = ((Int64)(Price / T)) * T;
            return result;
        }

        //////////////////////////////////////////////////////////////////////
        public static int GetDateNow()
        {
            var date = Utility.PersianDateExtensionMethods.GetMiladiToPersianDate(DateTime.Now);
            var dt = int.Parse(date.Substring(0, 4) + date.Substring(5, 2) + date.Substring(8, 2));
            return dt;
        }
        public string GetDateString(int Date)
        {
            var dt = Date.ToString();
            return $"{dt.Substring(0, 4)}/{dt.Substring(4, 2)}/{dt.Substring(6, 2)}";
        }
        public static int GetDateStringToInt(string Date)
        {
            return int.Parse(Date.Substring(0, 4) + Date.Substring(5, 2) + Date.Substring(8, 2));
        }
        //#region CallApi
        //protected async Task<ResponseApiEntity<ResultGroupRenovation>> GetGroupRenovation(int GroupRenovationID)
        //{
        //    var token = GetToken();
        //    return await RootApi<ResponseApiEntity<ResultGroupRenovation>>.RunMethodApi($"GroupRenovations/GetForView/{GroupRenovationID}", null, method: Method.Get, token);

        //}
        //private async Task<ResponseApiEntity<UpdateYear>> GetYear(int YearID)
        //{
        //    var token = GetToken();
        //    return await RootApi<ResponseApiEntity<UpdateYear>>.RunMethodApi($"Years/{YearID}", null, method: Method.Get, token);
        //}
        //private async Task<ResponseApiEntity<UpdateBasicInformationPerYear>> GetBasicInformationPerYear(int YearID, int typeOfBuildingID, int PropertyTypeID)
        //{
        //    var token = GetToken();
        //    return await RootApi<ResponseApiEntity<UpdateBasicInformationPerYear>>
        //                                        .RunMethodApi($"BasicInformationPerYears/GetBy_Year_TypeOfBuilding_PropertyType?" +
        //                                                      $"YearID={YearID}&" +
        //                                                      $"TypeOfBuildingID={typeOfBuildingID}&" +
        //                                                      $"PropertyTypeID={PropertyTypeID}",
        //                                         null, method: Method.Get, token);
        //}
        //#endregion
    }
}
