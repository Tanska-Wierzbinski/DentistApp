using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace DentistApp.Application.ViewModels
{
    public class VisitInfoForIndexListVM
    {
        public List<VisitInfoForIndexVM> Visits { get; set; }
        public PaginatedList<DateTime> PaginatedDates { get; set; }
        public DateTime DateMin { get; set; }
        public DateTime DateMax { get; set; }

        [DisplayName("Dentist")]
        public int DentistId { get; set; }

        [DisplayName("Visits in future")]
        public bool InFuture { get; set; }

        public string SortOrder { get; set; }
        public List<SelectListItem> Dentists { get; set; }

    }
}
