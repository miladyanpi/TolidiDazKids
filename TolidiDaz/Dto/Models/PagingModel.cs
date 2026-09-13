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
        //private decimal _CountPage;
        //public decimal CountPage
        //{
        //    get => _CountPage;
        //    set
        //    {
        //        _CountPage = value;
        //        OnPropertyChanged(nameof(CountPage));
        //    }
        //}
        //private decimal _Page;
        //public decimal Page
        //{
        //    get => _Page;
        //    set
        //    {
        //        _Page = value;
        //        OnPropertyChanged(nameof(Page));
        //    }
        //}
        //private decimal _Take;
        //public decimal Take
        //{
        //    get => _Take;
        //    set
        //    {
        //        _Take = value;
        //        OnPropertyChanged(nameof(Take));
        //    }
        //}
        //private decimal _CountPagePer10;
        //public decimal CountPagePer10
        //{
        //    get => _CountPagePer10;
        //    set
        //    {
        //        _CountPagePer10 = value;
        //        OnPropertyChanged(nameof(CountPagePer10));
        //    }
        //}
        //private decimal _PagePer10;
        //public decimal PagePer10
        //{
        //    get => _PagePer10;
        //    set
        //    {
        //        _PagePer10 = value;
        //        OnPropertyChanged(nameof(PagePer10));
        //    }
        //}
        //private decimal _PagePer10Counter;
        //public decimal PagePer10Counter
        //{
        //    get => _PagePer10Counter;
        //    set
        //    {
        //        _PagePer10Counter = value;
        //        OnPropertyChanged(nameof(PagePer10Counter));
        //    }
        //}
        //private string? _Active;
        //public string? Active
        //{
        //    get => _Active;
        //    set
        //    {
        //        _Active = value;
        //        OnPropertyChanged(nameof(Active));
        //    }
        //}
        //private string? _DisablePrev;
        //public string? DisablePrev
        //{
        //    get => _DisablePrev;
        //    set
        //    {
        //        _DisablePrev = value;
        //        OnPropertyChanged(nameof(DisablePrev));
        //    }
        //}
        //private string? _DisableNext;
        //public string? DisableNext
        //{
        //    get => _DisableNext;
        //    set
        //    {
        //        _DisableNext = value;
        //        OnPropertyChanged(nameof(DisableNext));
        //    }
        //}
        public decimal CountPage { get; set; }
        public decimal Page { get; set; }
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
