using Dto.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dto.Enum.EnumConstant;

namespace Dto.Models
{
    public class AlertToast
    {
        public AlertToast( MessageType? messageType=null,string messageAlert="")
        {
            MessageAlert = messageAlert;
            switch (messageType)
            {
                case EnumConstant.MessageType.success:
                    btnStatus = "success-toast";
                    icon = "fa fa-check";
                    icon_color = "#27ae60";
                    animation = "slide-in-slide-out";


                    break;
                case EnumConstant.MessageType.info:
                    btnStatus = "info-toast";
                    icon = "fa fa-info";
                    icon_color = "#2980b9";
                    animation = "slide-in-slide-out";
                    break;
                case EnumConstant.MessageType.danger:
                    btnStatus = "danger-toast";
                    icon = "fa fa-xmark";
                    icon_color = "#c0392b";
                    animation = "slide-in-fade-out";
                    break;
                case EnumConstant.MessageType.warning:
                    btnStatus = "warning-toast";
                    icon = "fa fa-triangle-exclamation";
                    icon_color = "#f39c12";
                    animation = "slide-in-fade-out";
                    break;
                default:
                    btnStatus = string.Empty;
                    icon = string.Empty;
                    icon_color = string.Empty;
                    animation = string.Empty;

                    break;
            }
        }
        public string? MessageAlert { get; set ; }
        public string? btnStatus { get; set; }
        public string? icon { get; set; }
        public string? icon_color { get; set; }
        public string? animation { get; set; }
    }
}
