using System;
using System.Collections.Generic;
using System.Text;

namespace DentistApp.Application.ViewModels
{
    public class VisitInfoForIndexListVM
    {
        public List<VisitInfoForIndexVM> Visits { get; set; }
        public List<DateTime> Dates { get; set; }
        
    }
}
