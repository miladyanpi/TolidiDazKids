using Dto.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dto.Models
{
    public class PagingModel
    {
        public PagingModel()
        {
            Take = Pagination.Take;
        }

        public decimal CountPage { get; set; }
        public decimal Page { get; set; } = 1;
        public decimal Take { get; set; }
        public decimal CountPagePer10 { get; set; } = 1;
        public decimal PagePer10 { get; set; } = 1;
        public decimal PagePer10Counter { get; set; } = 1;
        public string? Active { get; set; }
        public string? DisablePrev { get; set; }
        public string? DisableNext { get; set; }


        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged(string propertyName)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
