using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Models
{
    public class MenuModel
    {
        public MenuModel(int id,int? parentId,string title)
        {
            ID = id;
            ParentID = parentId;    
            Title = title;
        }
        public int ID { get; set; }
        public int? ParentID { get; set; }
        public string? Title { get; set; }
    }
}
