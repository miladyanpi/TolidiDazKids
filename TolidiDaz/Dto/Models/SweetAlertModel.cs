using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Dto.Models
{
    public class SweetAlertType
    {
        public const string Default = "default";
        public const string Success = "success";
        public const string Error = "error";
    }
    public class SweetAlertModel
    {
        public SweetAlertModel()
        {
            Title = "پیام";
            Type = SweetAlertType.Default ;
            Text = "";
            Timer = 1500;
            ShowConfirmButton = false;

        }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? Type { get; set; }
        public string? Icon { get; set; }
        public bool ShowConfirmButton { get; set; }
        public int Timer { get; set; } 
    }
}
